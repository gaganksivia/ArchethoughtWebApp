(() => {
    var chat = $.connection.chatHub;
    let iv;
    let ciphertext;
    let alicesKeyPair;
    let publicKeyJwk;
    let alicesSecretKey;
    let encryptmsg;
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
    async function encrypt(secretKey, text,chat) {

        iv = window.crypto.getRandomValues(new Uint8Array(12));
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

        chat.server.send($('#displayname').val(), base64Data, alicesKeyPair.publicKey, 1);
       // return base64Data.toString();
    }

    /*
    Decrypt the message using the secret key.
    If the ciphertext was decrypted successfully,
    update the "decryptedValue" box with the decrypted value.
    If there was an error decrypting,
    update the "decryptedValue" box with an error message.
    */
    async function decrypt(secretKey) {
        const decryptedValue = document.querySelector(".ecdh .decrypted-value");
        decryptedValue.textContent = "";
        decryptedValue.classList.remove("error");

        try {
            let decrypted = await window.crypto.subtle.decrypt(
                {
                    name: "AES-GCM",
                    iv: iv
                },
                secretKey,
                ciphertext
            );

            let dec = new TextDecoder();
            decryptedValue.classList.add("fade-in");
            decryptedValue.addEventListener("animationend", () => {
                decryptedValue.classList.remove("fade-in");
            });
            decryptedValue.textContent = dec.decode(decrypted);
        } catch (e) {
            decryptedValue.classList.add("error");
            decryptedValue.textContent = "*** Decryption error ***";
        }
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
                public: publicKey
            },
            privateKey,
            {
                name: "AES-GCM",
                length: 256
            },
            false,
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
                namedCurve: "P-384"
            },
            true,
            ["deriveKey", "deriveBits"]
        );
        publicKeyJwk = await window.crypto.subtle.exportKey(
            "jwk",
            alicesKeyPair.publicKey
        );
        privateKeyJwk = await window.crypto.subtle.exportKey(
            "jwk",
            alicesKeyPair.privateKey
        );
    }
    function SendMessage(chat) {

        encrypt(alicesSecretKey, $('#message').val(), chat);

    }
    function SendK(chat) {

        chat.server.send($('#displayname').val(), "", publicKeyJwk, 0);
    }

    //agreeSharedSecretKey();
    $(function () {
        // Declare a proxy to reference the hub.


        chat.client.messageReceived = function (userName, message) {
            alert("s");
        }
        // Create a function that the hub can call to broadcast messages.
        chat.client.broadcastMessage = function (name, message, publicKey) {
            // Html encode display name and message.
            var encodedName = $('<div />').text(name).html();
            var encodedMsg = $('<div />').text(message).html();
            // Add the message to the page.
            $('#discussion').append('<li><strong>' + encodedName
                + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
        };
        chat.client.getkey = async function (name, SenderpublicKeyJwk) {

            const publicKey = await window.crypto.subtle.importKey(
                "jwk",
                SenderpublicKeyJwk,
                {
                    name: "ECDH",
                    namedCurve: "P-384"
                },
                true,
                []
            );


            alicesSecretKey = await deriveSecretKey(alicesKeyPair.privateKey, publicKey);

        };
        // Get the user name and store it to prepend to messages.
        $('#displayname').val(prompt('Enter your name:', ''));
        agreeSharedSecretKey();
        // Set initial focus to message input box.
        $('#message').focus();
        // Start the connection.
        $.connection.hub.start().done(function () {
            SendK(chat);
            $('#sendmessage').click(function () {
                // Call the Send method on the hub.
                SendMessage(chat);

                // Clear text box and reset focus for next comment.
                $('#message').val('').focus();
            });
        });
    });
})();
