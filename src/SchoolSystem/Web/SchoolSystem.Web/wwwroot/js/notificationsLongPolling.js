var connection = new signalR.HubConnectionBuilder().withUrl("/notificationsHub").build();
connection.start();

connection.on("SendNotifications", function (newNotifications) {
    console.log(newNotifications)
    for (const notification of newNotifications) {
        $(".notifications-container ul").prepend(`<li>${notification.message}</li>`);
    }
    $(".notifications-display .notifications-count").text(newNotifications.length);
});

$(".notifications-display").on("click", function () {
    $.ajax({
        url: "api/notifications/MarkNotificationsAsRead",
        type: 'PUT',
        dataType: 'json',
        headers: {
            Accept: "application/json"
        }
    });

    $(this).find(".nav-link .notifications-count").text("0");
});




