
var chat = $.connection.chatHub;
let ciphertext;
let alicesKeyPair;
let SenderpublicKeyJwk = "";
let alicesSecretKey = "";
var myusername = "";
var currentchatuser = "";
const arrdecryptmsg = [];
var syncmsg = 0;
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

async function agreeSharedSecretKey() {
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
    localStorage.setItem("PrivateKey", JSON.stringify(PrivateKeyJwk));
    localStorage.setItem("PublicKey", JSON.stringify(publicKeyJwk));

}
async function SendMessage() {
    var messagetosend = $('#message').val();
    if (messagetosend.trim() != "") {
        let base64Data = await encrypt($('#message').val());
        if ($.connection.hub && $.connection.hub.state === $.signalR.connectionState.disconnected) {
            $.connection.hub.start()
        }
        chat.server.send(myusername, base64Data, $('#UserNameList').find(":selected").text(), JSON.parse(localStorage.getItem("PublicKey")));
    }
    $('#message').val('').focus();
}

//chat.client.receivemsg = async function (message) {
//    var RestorePrivateKeyJwk = JSON.parse(PrivateKeyJwk);
//    const privateKey = await window.crypto.subtle.importKey(
//        "jwk",
//        RestorePrivateKeyJwk,
//        {
//            name: "ECDH",
//            namedCurve: "P-256",
//        },
//        true,
//        ["deriveKey", "deriveBits"]
//    );

//    const publicKey = await window.crypto.subtle.importKey(
//        "jwk",
//        message[0].UserPublicKey,
//        {
//            name: "ECDH",
//            namedCurve: "P-256"
//        },
//        true,
//        []
//    );
//    alicesSecretKey = await deriveSecretKey(privateKey, publicKey);

//    if (alicesSecretKey != "") {
//        if (currentchatuser == sendername) {
//            let encodemsg = await decrypt(message);
//            var encodedName = $('<div />').text(sendername).html();
//            var encodedMsg = $('<div />').text(message).html();
//            // Add the message to the page.
//            $('#discussion').append('<li><strong>From: ' + encodedName
//                + '</strong>:&nbsp;&nbsp;' + encodemsg + '</li>');
//        }
//        else {
//            await createsecretkey(SenderpublicKeyJwk);
//            currentchatuser = sendername;
//            $("#UserNameList").val(currentchatuser);
//            let encodemsg = await decrypt(message);
//            var encodedName = $('<div />').text(sendername).html();
//            var encodedMsg = $('<div />').text(message).html();
//            // Add the message to the page.
//            $('#discussion').append('<li><strong>From: ' + encodedName
//                + '</strong>:&nbsp;&nbsp;' + encodemsg + '</li>');

//        }
//    }
//};

async function setReceiveMessages(message) {
    if (message.length == 0)
        return;
    currentchatuser = message[0].FromUsername;
    $("#UserNameList").val(currentchatuser);
    var RestorePrivateKeyJwk = JSON.parse(localStorage.getItem("PrivateKey"));
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

    for (var i = 0; i < message.length; i++) {
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
        SenderpublicKeyJwk = message[i].UserPublicKey;
        alicesSecretKey = await deriveSecretKey(privateKey, publicKey);
        let encodemsg = await decrypt(message[i].Message);
        var encodedName = $('<div />').text(currentchatuser).html();
        // Add the message to the page.
        $('#discussion').append('<li><strong>From: ' + encodedName
            + '</strong>:&nbsp;&nbsp;' + encodemsg + '</li>');
    }
}
chat.client.receivemsg = async function (message) {
    setReceiveMessages(message);
};

async function createsecretkey(SenderpublicKey) {
    const publicKey = await window.crypto.subtle.importKey(
        "jwk",
        SenderpublicKey,
        {
            name: "ECDH",
            namedCurve: "P-256"
        },
        true,
        []
    );
    await agreeSharedSecretKey();
    var PrivateKeyJwk = JSON.parse(localStorage.getItem("PrivateKey"));
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
    alicesSecretKey = await deriveSecretKey(privateKey, publicKey);
    SendMessage();
}

chat.client.getuserkey = async function (senderusername, name) {
    $("#UserNameList").empty();
    for (var i = 0; i < name.length; i++) {
        if (name[i] != myusername) {
            var newOption = $('<option value="' + name[i] + '">' + name[i] + '</option>');
            $('#UserNameList').append(newOption);
        }
    }
    currentchatuser = senderusername;
    $("#UserNameList").val(currentchatuser);
    await agreeSharedSecretKey();
    chat.server.sendKey(senderusername, JSON.parse(localStorage.getItem("PublicKey")));
    //await createsecretkey(SenderpublicKeyJwk);
};
chat.client.sendrequestkey = async function (senderpublickey) {
    await createsecretkey(senderpublickey);
};


// Create a function that the hub can call to broadcast messages.
chat.client.newuseradd = function (name, newmessaged) {
    $("#UserNameList").empty();
    for (var i = 0; i < name.length; i++) {
        if (name[i] != myusername) {
            var newOption = $('<option value="' + name[i] + '">' + name[i] + '</option>');
            $('#UserNameList').append(newOption);
        }
    }
    setReceiveMessages(newmessaged);
};

chat.client.refreshpage = async function () {
    ciphertext = "";
    alicesKeyPair = "";
    alicesSecretKey = "";
    myusername = "";
    location.reload();
};
// Get the user name and store it to prepend to messages.
$('#displayname').val(prompt('Enter your name:', ''));

// Set initial focus to message input box.
$('#message').focus();
// Start the connection.
myusername = $('#displayname').val();
$('#lblusername').text(myusername);
$.connection.hub.start().done(function () {
    chat.server.setuser(myusername);
});

$('#sendmessage').click(async function () {
    if (SenderpublicKeyJwk == "" || SenderpublicKeyJwk == 'undefined') {
        chat.server.askUserPublicKey($('#UserNameList').find(":selected").text(), myusername);
    }
    else {
        createsecretkey(SenderpublicKeyJwk);
    }
});

$('#signout').click(function () {
    chat.server.signOut(myusername);
});


$('#clearcache').click(function () {
    localStorage.removeItem("PrivateKey");
    localStorage.removeItem("PublicKey");
});
$('#clearusers').click(function () {
    chat.server.clearUsersData();
});


