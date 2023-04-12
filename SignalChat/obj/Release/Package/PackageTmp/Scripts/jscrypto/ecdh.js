
var chat = $.connection.chatHub;
let ciphertext;
let alicesKeyPair;
let publicKeyJwk;
let alicesSecretKey = "";

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
async function encrypt(secretKey, text, chat) {

    let iv = new TextEncoder().encode("Initialization Vector");
    let encoded = getMessageEncoding(text);

    ciphertext = await window.crypto.subtle.encrypt(
        {
            name: "AES-GCM",
            iv: iv
        },
        secretKey,
        encoded
    );

    const uintArray = new Uint8Array(ciphertext);
    const string = String.fromCharCode.apply(null, uintArray);

    const base64Data = btoa(string);

    chat.server.send($('#displayname').val(), base64Data);

}

/*
Decrypt the message using the secret key.
If the ciphertext was decrypted successfully,
update the "decryptedValue" box with the decrypted value.
If there was an error decrypting,
update the "decryptedValue" box with an error message.
*/
async function decrypt(secretKey, message, username) {
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
        secretKey,
        uintArray.buffer
    );
    //const algorithm = {
    //    name: "AES-GCM",
    //    iv: iv,
    //};
    //const decryptedData = await window.crypto.subtle.decrypt(
    //    algorithm,
    //    secretKey,
    //    uintArray
    //);
    let encodemsg = new TextDecoder().decode(decryptedData);
    var encodedName = $('<div />').text(username).html();
    var encodedMsg = $('<div />').text(message).html();
    // Add the message to the page.
    $('#discussion').append('<li><strong>Decrypt Message: ' + encodedName
        + '</strong>:&nbsp;&nbsp;' + encodemsg + '</li>');
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

async function agreeSharedSecretKey(chat) {
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
    publicKeyJwk = await window.crypto.subtle.exportKey(
        "jwk",
        alicesKeyPair.publicKey
    );
    chat.server.sendKey($('#displayname').val(), publicKeyJwk);
}
function SendMessage(chat) {
    encrypt(alicesSecretKey, $('#message').val(), chat);
}

function SendKeyToServer(chat) {
    agreeSharedSecretKey(chat);

}




// Create a function that the hub can call to broadcast messages.
chat.client.broadcastMessage = async function (name, message, SenderpublicKeyJwk) {
    if (alicesSecretKey == "") {
        const publicKey = await window.crypto.subtle.importKey(
            "jwk",
            SenderpublicKeyJwk,
            {
                name: "ECDH",
                namedCurve: "P-256"
            },
            true,
            []
        );

        alicesSecretKey = await deriveSecretKey(alicesKeyPair.privateKey, publicKey);
        decrypt(alicesSecretKey, message, name);
    }
    else {
        decrypt(alicesSecretKey, message, name);
    }
    // Html encode display name and message.
    var encodedName = $('<div />').text(name).html();
    var encodedMsg = $('<div />').text(message).html();
    // Add the message to the page.
    $('#discussion').append('<li><strong>Encrypt Message: ' + encodedName
        + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
};
chat.client.getkey = async function (SenderpublicKeyJwk) {

    const publicKey = await window.crypto.subtle.importKey(
        "jwk",
        SenderpublicKeyJwk,
        {
            name: "ECDH",
            namedCurve: "P-256"
        },
        true,
        []
    );

    alicesSecretKey = await deriveSecretKey(alicesKeyPair.privateKey, publicKey);
};
chat.client.refreshpage = async function () {
    ciphertext = "";
    alicesKeyPair = "";
    publicKeyJwk = "";
    alicesSecretKey = "";
    location.reload();
};
// Get the user name and store it to prepend to messages.
$('#displayname').val(prompt('Enter your name:', ''));

// Set initial focus to message input box.
$('#message').focus();
// Start the connection.
$.connection.hub.start().done(function () {
    $('#lblusername').text($('#displayname').val());
    SendKeyToServer(chat);
    $('#sendmessage').click(function () {
        // Call the Send method onthe hub.
        SendMessage(chat);
        // Clear text box and reset focus for next comment.
        $('#message').val('').focus();
    });

    $('#sharekeys').click(function () {
        if (alicesSecretKey == "") {
            chat.server.askUserPublicKey($('#displayname').val());
        }
    });
    $('#clearusers').click(function () {
        chat.server.clearUsersData();
    });
});


