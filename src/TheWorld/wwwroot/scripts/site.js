$(document).ready(function () {

    var $main = $("#main");
    $main.on("mouseenter", function () {
        $main.css("background-color", "#888");
    });

    $main.on("mouseleave", () => {
        $main.css("background-color", "");
    });

    var $btnHide = $("#sidebarToggle");
    $btnHide.on("click", function () {
        $("#sidebar,#wrapper").toggleClass("hide-sidebar");
        if ($("#sidebar,#wrapper").hasClass("hide-sidebar")) {
            $(this).text("Show menu");
        }
        else {
            $(this).text("Hide menu");
        }
    })
});