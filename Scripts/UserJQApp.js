$(document).ready(function () {
    $("div#choices > input[type='radio']").button();
    $("div#choices").buttonset();
    $("span#dockBack").css({ "float": "left" });
    $("span#news").singlesticker();
    showDock();
    
    $("input#idmember").click(function (event) { event.preventDefault(); location.href = "/Member/Index"; });
    $("input#idorder").click(function (event) { event.preventDefault(); location.href = "/Member/MemberOrderIndex"; });
    $("input#idnextbuy").click(function (event) { event.preventDefault(); location.href = "/Member/NextBuyIndex"; });
});

$("input#chgpwdBtn").click(function (event) {
    event.preventDefault();
    $.ajax({
        type: "Get",
        url: "/Member/GetPasswordChangeHtml",
        dataType: "json",
        success: function (data) {            
            $("body").append(data);
            var $dialog = $("div#changePwd").dialog({
                autoOpen: true,
                width: 400,
                height: 300,
                show: "blind",
                modal: true,
                resizable: false,
                buttons: {
                    "確定": function () {
                        var valid = true;

                        $("input.required", this).each(function () {
                            if (validateElement(this) == false)
                                valid = false;
                        });

                        var oldPwd = $("input#olduserpwd").val();
                        var newPwd = $("input#newuserpwd").val();
                        var vrfPwd = $("input#varifypwd").val();

                        if (newPwd !== vrfPwd) {
                            valid = false;
                            alert("新密碼與確認密碼不相同");
                            $("input#varifypwd").focus();
                        }
                        
                        if (valid) {
                            $.ajax({
                                type: "Get",
                                url: "/Member/ChangePassword",
                                data: { "newpwd": newPwd },
                                success: function () {
                                    alert("密碼已變更，下次請以新密碼登入!");
                                },
                                error: function () {
                                    alert("無法變更密碼，請重新操作!");
                                }
                            });
                            $dialog.dialog("close").remove();
                        }
                    },
                    "取消": function () {
                        $dialog.dialog("close").remove();
                    }
                },
                close: function () {
                    $dialog.remove();
                }
            }).dialog("refresh");
        }
    });
});

$("a#delNextBuy").click(function (event) {
    event.preventDefault();
    var urlstr = $(this).attr("href");
    var urlarray = urlstr.split("?");
    var urlpath = urlarray[0];
    var urlparam = urlarray[1];
    var corfm = confirm("您確定要刪除此筆資料?");

    if(corfm)
    {
        $.ajax({
            type: "Get",
            url: urlpath,
            data: urlparam,
            cache: false,
            success: function () {
                alert("資料刪除成功");
                location.reload();
            },
            error: function () {
                alert("資料刪除失敗");
            }
        });
    }
});

$("a#putCart").click(function (event) {
    event.preventDefault();
    var urlstr = $(this).attr("href");
    var urlarray = urlstr.split("?");
    var urlpath = urlarray[0];
    var urlparam = urlarray[1];

    var paramarray = urlparam.split("=");
    $.ajax({
        type: "Get",
        url: urlpath,
        data: urlparam,
        cache: false,
        success: function () {
            addCart(paramarray[1]);
        },
        error: function () {
            alert("資料刪除失敗");
        }
    });
});

function addCart(prodNo) {
    $.ajax({
        type: "GET",
        url: "/Cart/Add",
        data: { "productId": prodNo },
        cache: false,
        success: function () {
            alert("商品已經放入購物車!");
            location.reload();
        },
        error: function (data, textStatus) {
            alert("存取錯誤:" + textStatus + " - " + data);
        }
    });
}

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

function validateElement(element) {
    var Checked = true;

    var $elem = $(element);
    var name = $elem.attr("name");
    var val = $elem.val();

    var type = $elem[0].type.toLowerCase();

    switch (type) {
        case "text":
        case "textarea":
        case "password":
            if (val.length == 0 || val.replace(/\s/g).length == 0) { Checked = false; }
            break;
        case "checkbox":
            if ($("input[name='" + name + "']:checked").length == 0) { Checked = false; }
            break;
    }

    var method = Checked ? "removeClass" : "addClass";

    $("#errorMessage_" + name)[method]("ui-state-highlight showErrorMessage");

    return Checked;
}