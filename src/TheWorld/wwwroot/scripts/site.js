$(document).ready(function () {

    /*var $main = $("#main");
    $main.on("mouseenter", function () {
        $main.css("background-color", "#d9d9d9");
    });

    $main.on("mouseleave", () => {
        $main.css("background-color", "");
    });*/

    var $btnHide = $("#sidebarToggle");
    var $iconHide = $("#sidebarToggle i.fa");
    $btnHide.on("click", function () {
        $("#sidebar,#wrapper").toggleClass("hide-sidebar");
        if ($("#sidebar,#wrapper").hasClass("hide-sidebar")) {
            $iconHide.removeClass("fa-angle-left");
            $iconHide.addClass("fa-angle-right");
        }
        else {
            $iconHide.removeClass("fa-angle-right");
            $iconHide.addClass("fa-angle-left");
        }
    })
});