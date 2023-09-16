var chatConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
chatConnection.start();

chatConnection.on("ClientConnected", function (data) {
    if ($("ul.chat-users").find(`li:contains(${data.username})`).length == 0) {
        $("ul.chat-users").append(`<li role="button">${data.username}</li>`);
        $(".online-users .count").text($("ul.chat-users li").length.toString());
    }
    
});

chatConnection.on("ClientDisconnected", function (data) {
    $("ul.chat-users li").remove(`:contains('${data.username}')`);
    $(".online-users .count").text($("ul.chat-users li").length.toString());
});

chatConnection.on("VisualizeMessage", function (message, usernameOfSender) {
    let isDisplayedOnPage = false;
    $(".chat-box").each(function () {
        let currentUsername = $(this).find(`.username:contains(${usernameOfSender})`);
        if (currentUsername.length > 0) {
            isDisplayedOnPage = true;
            let fullName = currentUsername.closest(".chat-box").find(".full-name").text();
            currentUsername.closest(".chat-box").find(".chat-content").append(`<p>${fullName}<br/>${message}</p>`);
        }
    });

    if (!isDisplayedOnPage) {
        let receiverUsername = getCurrentUserUsername();
        let senderUserId = getUserId(usernameOfSender);
        let receiverUserId = getUserId(receiverUsername);

        let chatBox = constructChatBox(usernameOfSender, senderUserId, receiverUserId);
        let fullName = $(chatBox).find(".full-name").text();
        $(chatBox).find(".chat-content").append(`<p>${fullName}<br/>${message}</p>`);
    }
    
})

$("body").on("click", "ul.chat-users li", function () {
    let receiverUsername = $(this).text();

    let alreadyExisting = $(".chat-box").find(`.username:contains(${receiverUsername})`);

    if (alreadyExisting.length == 0) {
        let senderUsername = getCurrentUserUsername();
        let receiverUserId = getUserId(receiverUsername);
        let senderUserId = getUserId(senderUsername);
        constructChatBox(receiverUsername, senderUserId, receiverUserId);
    }



});

$("body").on("click", ".bi-send", function () {
    let message = $(this).closest(".chat-box").find("textarea").val();
    let sendButton = $(this);
    if (message !== '') {
        let senderUsername = getCurrentUserUsername();
        let senderFullName = getFullName(senderUsername);
        let senderId = getUserId(senderUsername);

        $(this).closest(".chat-box").find(".chat-content").append(`<p>${senderFullName}<br/>${message}</p>`);

        let receiverUsername = $(this).closest(".chat-box").find(".username").text();
        let receiverId = getUserId(receiverUsername);

        $.ajax({
            type: "POST",
            url: `/api/chat`,
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify({ senderId: senderId, receiverId: receiverId, message: message }),
            success: function (data) {
                console.log(data);
                chatConnection.invoke("SendMessage", receiverId, message);
                sendButton.closest(".chat-box").find("textarea").val('');
            }
        });

        

        
    }
});

$("body").on("click", ".close-button", function () {
    $(this).closest(".chat-box").remove();
});

function createChatBox(username, fullName, distanceInRight) {
    let chatDiv = $(`<div class="col-md-2 chat-box border" style="position: fixed; bottom: 70px; right: ${distanceInRight}px; height: 300px; padding: 10px;">
       <div class="chat-header d-flex border-bottom border-dark"><div><div class="username"><b>${username}</b></div><div class="full-name" style="font-size: 14px;">${fullName}</div></div><div><i class="bi bi-x-lg close-button" role="button"></i></div></div>
       <div class="chat-content" style="overflow-y: overlay; max-height: 200px;"></div>
       <div class="chat-footer">
          <textarea rows="2" cols="30" class="chat-text-input"></textarea>
          <i class="bi bi-send" role="button" style="font-size: 20px;"></i>
        </div>
     </div>`);
    chatDiv.insertBefore(".footer");
    return chatDiv;
}

function removeFirstChatBox() {
    $(".chat-box").eq(0).remove();

}

function constructChatBox(username, senderUserId, receiverUserId) {
    let chatElement = null;
    let chatBoxesLength = $(".chat-box").length;
    let fullName = getFullName(username);
    if (chatBoxesLength == 3) {
        removeFirstChatBox();
        returnedChatBox = createChatBox(username, fullName, chatBoxesLength * 280);
        for (let i = 0; i < $(".chat-box").length; i++) {
            $(".chat-box").eq(i).css("right", `${i * 280}px`)
        }
        chatElement = returnedChatBox[0];
    }
    else {
        returnedChatBox = createChatBox(username, fullName, chatBoxesLength * 280);
        chatElement = returnedChatBox[0];
    }
    let chats = getChats(senderUserId, receiverUserId);

    for (const chat of chats) {
        $(chatElement).find(".chat-content").append(`<p>${chat.fullName}<br/>${chat.message}</p>`);
    }


    return chatElement
}

function getFullName(username) {
    let fullName = ''
    $.ajax({
        async: false,
        type: "GET",
        url: `/api/users/GetFullName`,
        data: { username: username },
        success: function (data) {
            fullName = data;
        }
    });

    return fullName;
}

function getUserId(username) {
    let receiverId = "";

    $.ajax({
        async: false,
        type: "GET",
        url: `/api/users/GetUserId`,
        data: { username: username },
        success: function (data) {
            receiverId = data;
        }
    });
    return receiverId;
}

function getCurrentUserUsername() {
    return $(".logged-in-user").text();
}

function getChats(senderUserId, receiverUserId) {
    chats = [];
    $.ajax({
        async: false,
        type: "GET",
        url: `/api/chat`,
        data: { senderId: senderUserId, receiverId: receiverUserId },
        success: function (data) {
            chats = data;
        }
    });

    return chats;
}

