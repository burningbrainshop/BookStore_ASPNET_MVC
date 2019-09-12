$(document).ready(function () {
    $("span#dockBack").css({ "float": "left" });
    $("span#news").singlesticker();
    showDock();
});

function showDock() {
    $("ul#dock > li").hover(function () {
        var $obj = $(this).find("> img");
        var imgPos = $obj.offset();

        $obj.animate({
            marginTop: "0px",
            marginLeft: "0px",
            width: "40px",
            height: "40px",
            cursor: "pointer"
        }, 500);

        var key = $obj.attr("alt");
        var desp;
        switch (key) {
            case "HOME":
                desp = "回首頁";
                break;
            case "LOGIN":
                desp = "登入";
                break;
            case "WEBSITE":
                desp = "逛逛書店";
                break;
            case "CART":
                desp = "購物車";
                break;
            case "USER":
                desp = "會員專區";
                break;
            case "MANAGER":
                desp = "後端平台";
                break;
        }

        var $tips = $("<div id='functionTip' style='position:absolute; display:none; z-index:999999; color:#FFF; background-color:#000; border:1px solid white; width:80px; height:20px; text-align:center; line-height:20px;'>" + desp + "</div>").appendTo("body");
        $tips.show().css({
            "top": imgPos.top - 20,
            "left": imgPos.left
        });
    }, function () {
        $(this)
            .find("> img").animate({
                marginTop: "0px",
                marginLeft: "0px",
                width: "35px",
                height: "35px"
            }, 500);

        $("div#functionTip").remove();
    }).click(function () {
        var key = $(this).find("img").attr("alt");

        switch (key) {
            case "HOME":
                location.href = "/BookProduct/Index";
                break;
            case "LOGIN":
                location.href = "/Member/Login";
                break;
            case "WEBSITE":
                location.href = "/BookProduct/Index";
                break;
            case "CART":
                location.href = "/Cart/Index";
                break;
            case "USER":
                location.href = "/Member/Index";
                break;
            case "MANAGER":
                location.href = "/Manager/Login";
                break;
        }
    });
}