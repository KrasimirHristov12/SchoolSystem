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

    if ($(".notifications-container").hasClass("d-none")) {
        $(".notifications-container").removeClass("d-none");
    }
    else {
        $(".notifications-container").addClass("d-none");
    } 
});


let currentPage = 1;
let observer = new IntersectionObserver(function (entries) {
    let lastLiEntry = entries[0];
    if (lastLiEntry.isIntersecting) {
        console.log("in");
        currentPage++;
        observer.unobserve(lastLiEntry.target);
        $.get(`/api/notifications/GetNotifications?getNewOnesOnly=False&page=${currentPage}&elementsPerPage=10`, function (data, status) {
            if (status == 'success') {


                for (const notification of data) {
                    $(".notifications-container ul").append(`<li>${notification.message}</li>`);

                }

                observer.observe(document.querySelector(".notifications-container li:last-child"));

            }
        })

    }
});

observer.observe(document.querySelector(".notifications-container li:last-child"));



