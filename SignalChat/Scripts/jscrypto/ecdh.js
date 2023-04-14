
var chat = $.connection.chatHub;
let ciphertext;
let alicesKeyPair;
let SenderpublicKeyJwk = "";
let alicesSecretKey = "";
var myusername = "";
var currentchatuser = "";
const arrdecryptmsg = [];
var syncmsg = 0;

// Get the user name and store it to prepend to messages.
$('#chatbox').hide();
//while (myusername == '' || myusername == null)
//    myusername = prompt('Enter your name:', '');
$('#message').focus();
//$('#ContentPlaceHolder1_lblusername').text(myusername);
// Start the connection.
$.connection.hub.start().done(function () {
    myusername = $('#ContentPlaceHolder1_lblusername').text();
    chat.server.setuser(myusername);
});

function AddUser(Contactname) {
    $('#username').append(Contactname);
}

/*
Fetch the contents of the "message" textbox, and encode it
in a form we can use for the encrypt operation.
*/
function getMessageEncoding(message) {

    let enc = new TextEncoder();
    return enc.encode(message);
}

/*
Encrypt the message using the secret key.
Update the "ciphertextValue" box with a representation of part of
the ciphertext.
*/
async function encrypt(text) {

    let iv = new TextEncoder().encode("Initialization Vector");
    let encoded = getMessageEncoding(text);

    ciphertext = await window.crypto.subtle.encrypt(
        {
            name: "AES-GCM",
            iv: iv
        },
        alicesSecretKey,
        encoded
    );

    const uintArray = new Uint8Array(ciphertext);
    const string = String.fromCharCode.apply(null, uintArray);

    return btoa(string);



}

/*
Decrypt the message using the secret key.
If the ciphertext was decrypted successfully,
update the "decryptedValue" box with the decrypted value.
If there was an error decrypting,
update the "decryptedValue" box with an error message.
*/
async function decrypt(message) {
    let iv = new TextEncoder().encode("Initialization Vector");
    const string = atob(message);

    const uintArray = new Uint8Array(
        [...string].map((char) => char.charCodeAt(0))
    );
    let decryptedData = await window.crypto.subtle.decrypt(
        {
            name: "AES-GCM",
            iv: iv
        },
        alicesSecretKey,
        uintArray.buffer
    );
    return new TextDecoder().decode(decryptedData);
}

/*
Derive an AES key, given:
- our ECDH private key
- their ECDH public key
*/
async function deriveSecretKey(privateKey, publicKey) {
    return await window.crypto.subtle.deriveKey(
        {
            name: "ECDH",
            namedCurve: "P-256",
            public: publicKey
        },
        privateKey,
        {
            name: "AES-GCM",
            length: 256
        },
        true,
        ["encrypt", "decrypt"]
    );
}

async function agreeSharedSecretKey(sendername) {
    // Generate 2 ECDH key pairs: one for Alice and one for Bob
    // In more normal usage, they would generate their key pairs
    // separately and exchange public keys securely
    alicesKeyPair = await window.crypto.subtle.generateKey(
        {
            name: "ECDH",
            namedCurve: "P-256"
        },
        true,
        ["deriveKey", "deriveBits"]
    );
    var publicKeyJwk = await window.crypto.subtle.exportKey(
        "jwk",
        alicesKeyPair.publicKey
    );
    var PrivateKeyJwk = await window.crypto.subtle.exportKey(
        "jwk",
        alicesKeyPair.privateKey
    );
    localStorage.setItem("PrivateKey_" + sendername.toLowerCase(), JSON.stringify(PrivateKeyJwk));
    localStorage.setItem("PublicKey_" + sendername.toLowerCase(), JSON.stringify(publicKeyJwk));

}
async function SendMessage() {
    var messagetosend = $('#message').val();
    if (messagetosend.trim() != "") {
        let base64Data = await encrypt($('#message').val());
        if ($.connection.hub && $.connection.hub.state === $.signalR.connectionState.disconnected) {
            $.connection.hub.start()
        }
        chat.server.send(myusername, base64Data, currentchatuser, JSON.parse(localStorage.getItem("PublicKey_" + currentchatuser.toLowerCase())));
    }
    setsendmessage(currentchatuser.toLowerCase(), messagetosend);
    $('#message').val('').focus();
}

async function ReceiveMessages(message) {
    for (var i = 0; i < message.length; i++) {
        //currentchatuser = message[i].FromUsername;
        var RestorePrivateKeyJwk = JSON.parse(localStorage.getItem("PrivateKey_" + message[i].FromUsername.toLowerCase()));
        const privateKey = await window.crypto.subtle.importKey(
            "jwk",
            RestorePrivateKeyJwk,
            {
                name: "ECDH",
                namedCurve: "P-256",
            },
            true,
            ["deriveKey", "deriveBits"]
        );

        const publicKey = await window.crypto.subtle.importKey(
            "jwk",
            message[i].UserPublicKey,
            {
                name: "ECDH",
                namedCurve: "P-256"
            },
            true,
            []
        );
        localStorage.setItem("SenderpublicKey_" + message[i].FromUsername.toLowerCase(), JSON.stringify(message[i].UserPublicKey));
        alicesSecretKey = await deriveSecretKey(privateKey, publicKey);
        let encodemsg = await decrypt(message[i].Message);
        // Add the message to the page.
        setreceivemessage(message[i].FromUsername.toLowerCase(), encodemsg, message[i].MessageDateTime);

    }
}
chat.client.receivemsg = async function (message) {
    ReceiveMessages(message);
};

async function createsecretkey(SenderpublicKeyJWK, senderusername) {
    const SenderpublicKey = await window.crypto.subtle.importKey(
        "jwk",
        SenderpublicKeyJWK,
        {
            name: "ECDH",
            namedCurve: "P-256"
        },
        true,
        []
    );
    await agreeSharedSecretKey(senderusername);
    var PrivateKeyJwk = JSON.parse(localStorage.getItem("PrivateKey_" + senderusername.toLowerCase()));
    const privateKey = await window.crypto.subtle.importKey(
        "jwk",
        PrivateKeyJwk,
        {
            name: "ECDH",
            namedCurve: "P-256"
        },
        true,
        ["deriveKey", "deriveBits"]
    );
    alicesSecretKey = await deriveSecretKey(privateKey, SenderpublicKey);
    SendMessage();
}

chat.client.getuserkey = async function (senderusername, receiverusername, name) {
    //Addnewcontact(name);
    await agreeSharedSecretKey(senderusername);
    chat.server.sendKey(senderusername, receiverusername, JSON.parse(localStorage.getItem("PublicKey_" + senderusername.toLowerCase())));
};
chat.client.sendrequestkey = async function (senderpublickey, senderusername) {
    await createsecretkey(senderpublickey, senderusername);
};

// Create a function that the hub can call to broadcast messages.
chat.client.newuseradd = function (name, newmessaged) {
    Addnewcontact(name);
    ReceiveMessages(newmessaged);
};

chat.client.refreshpage = async function () {
    ciphertext = "";
    alicesKeyPair = "";
    alicesSecretKey = "";
    myusername = "";
    location.reload();
};

$('#sendmessage').click(async function () {
    if (currentchatuser != '') {
        var RestoreSenderpublicKey = JSON.parse(localStorage.getItem("SenderpublicKey_" + currentchatuser.toLowerCase()));
        if (RestoreSenderpublicKey != null) {
            createsecretkey(RestoreSenderpublicKey, currentchatuser);
        }
        else {
            chat.server.askUserPublicKey(currentchatuser, myusername);
        }
    }
});

$('#signout').click(function () {
    chat.server.signOut(myusername);
});


$('#clearcache').click(function () {
    localStorage.removeItem("PrivateKey_" + currentchatuser.toLowerCase());
    localStorage.removeItem("PublicKey_" + currentchatuser.toLowerCase());
});
$('#clearusers').click(function () {
    chat.server.clearUsersData();
});


