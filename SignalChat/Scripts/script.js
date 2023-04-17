$('.friend-drawer--onhover').on('click', function () {

    $('.chat-bubble').hide('fast').show('fast');

});

function Addnewcontact(userlist) {
    for (var i = 0; i < userlist.length; i++) {
        if (!$('#box_' + userlist[i].UserName).length) {
            $('#newuseradd').append('<div class="friend-drawer friend-drawer--onhover" onclick=\'setchatbox("' + userlist[i].UserName.toLowerCase() + '")\'  id=box_' + userlist[i].UserName + ' >' +
                '<img class= "profile-image" src="/img/user_icon.png" alt = "" >' +
                '<div class="text">' +
                '<h6>' + userlist[i].UserName + '</h6>' +
                '<p   id="lastmsg_' + userlist[i].UserName + '" class="text-muted"></p > ' +
                '</div>' +
                '<span id="lastmsgtime_' + userlist[i].UserName + '" class="timtext-muted small"></span>' +
                '</div >' +
                '<hr>');
        }
    }
}

function setchatbox(username) {
    var id = '#discussion_' + currentchatuser;
    $(id).hide();
    var idbox = '#box_' + username;
    $(idbox).css('background-color', '#d0edde');
    idbox = '#box_' + currentchatuser;
    $(idbox).css('background-color', '');
    $('#chatbox').show();
    currentchatuser = username;
    $('#lblchatusername').text(currentchatuser);
    id = '#discussion_' + currentchatuser;
    $(id).show();
    if (!$('#discussion_' + username).length)
        $('#discussions').append('<div id="discussion_' + username + '"></div >');
}

function setsendmessage(username, message) {
    var dt = new Date();
    var time = dt.getHours() + ":" + dt.getMinutes();
    var id = '#discussion_' + username;
    if (!$(id).length) {
        $('#discussions').append('<div id="discussion_' + username + '"></div >');
    }
    $(id).append('<div class="row no-gutters" >' +
        '<div class="col-md-5 offset-md-7">' +
        '<div class="chat-bubble chat-bubble--right">' + message +
        '<br><span class="text-muted small">' + time + '</span>' +
        '</div>' +
        '</div>' +
        '</div >');
}

function setreceivemessage(username, message, time) {
    var datetime = new Date(time);
    var timetoset = datetime.getHours() + ":" + datetime.getMinutes();
    var id = '#discussion_' + username;
    if (!$(id).length) {
        $('#discussions').append('<div id="discussion_' + username + '"></div >');
        $(id).hide();
    }
    $(id).append('<div class="row no-gutters" >' +
        '<div class="col-md-5">' +
        '<div class="chat-bubble chat-bubble--left">' + message +
        '<br><span class="text-muted small">' + timetoset + '</span>' +
        '</div>' +
        '</div>' +
        '</div >');
    var idLastmsg = '#lastmsg_' + username;
    if (message.length > 15)
        $(idLastmsg).text(message.substring(0, 14));
    else
        $(idLastmsg).text(message);
}
