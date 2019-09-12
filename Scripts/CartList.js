$(document).ready(function () {
    $("span#dockBack").css({ "float": "left" });
    $("span#news").singlesticker();
    showDock();
    show_booklist();
    show_nextbuy();
});

$("div#bookListContainer").mousemove(function (event) {
    var $obj = $(this);
    var $objList = $("div#bookList");
    var containerHeight = $obj.height();
    var objListHeight = $objList.height();

    var distance = (event.pageY - $obj.offset().top) * (objListHeight - containerHeight) / containerHeight;
    $obj.scrollTop(distance);
}).droppable({
    drop: function (event, ui) {        
        var bookid = ui.draggable.attr("name");
        $.ajax({
            type: "GET",
            url: "/BookProduct/DeleteNextBuy",
            data: { "productId": bookid },
            cache: false,
            success: function () {
                addCart(bookid);
            },
            error: function (data, textStatus) {
                alert("存取錯誤:" + textStatus + " - " + data);
            }
        });
    }
});

$("div#nextbuyContainer").droppable({
    hoverClass: "hoverNextbuyContainer",
    drop: function (event, ui) {
        var bookid = ui.draggable.attr("val");
        addNextbuy(bookid);
    }
});

$("div#nextbuyContainer").mousemove(function (event) {
    var $obj = $(this);
    var $objList = $("div#nextbuyList");
    var containerHeight = $obj.height();
    var objListHeight = $objList.height();

    var distance = (event.pageY - $obj.offset().top) * (objListHeight - containerHeight) / containerHeight;
    $obj.scrollTop(distance);
});

function show_nextbuy()
{
    $.ajax({
        type: "GET",
        url: "/BookProduct/GetNextBuyProducts",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data != null) {
                listNextBuy(data)
            }
        }
    });
}

function listNextBuy(data)
{
    $("div#nextbuyList").empty();
    for (var i in data) {
        $("<div id='eachProduct' name='" + data[i].productId + "' style='display:line-block; position:absolute; width:353px; border:1px solid #336699; color:#FFFFCC; background-color:#336699; padding:2px; margin-bottom:1px; z-index:99999; cursor:pointer; text-align:left;'>" + data[i].productName + "</div>").appendTo($("div#nextbuyList"));
        
        var pcPos = $("div#nextbuyList").position();
        var topPos = pcPos.top - 25;
        var items = 0;
        var itemHeight = 26;
        var totalHeight = 0;
        $("div#nextbuyList").find("div#eachProduct").each(function () {
            items++;

            totalHeight += itemHeight;
            $(this).css({ "top": topPos += itemHeight });

            if ($(this).text().length > 34) {
                itemHeight = 44;
            }
            else {
                itemHeight = 26;
            }
        });
        $("div#nextbuyList").css("height", (totalHeight));
    }

    $("div#eachProduct")
        .draggable({
            helper: "clone",
            revert: "invalid",
            cursor: "pointer",
            appendTo: "body"
        })
        .bind("dragstor", f = function (event, ui) {
            $(this).remove();
        });
}

function addNextbuy(prodNo) {
    $.ajax({
        type: "GET",
        url: "/Member/CheckUserLogin",
        dataType: "json",
        cache: false,
        success: function (data, textStatus) {
            if (textStatus == "success") {
                $.ajax({
                    type: "GET",
                    url: "/BookProduct/AddNextBuy",
                    data: { "productId": prodNo },
                    cache: false,
                    success: function () {
                        alert("商品已放入下次買清單!");
                        delCart(prodNo, 0);
                        show_nextbuy();
                        show_booklist();
                    }
                });
            }
            else {
                alert("您尚未登入,請先登入網站!");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("您尚未登入,請先登入網站!");
        }
    });    
}

function show_booklist() {
    $.ajax({
        type: "GET",
        url: "/Cart/GetCart",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data) {
                listBooks(data);
            }
        },
        error: function (data, textStatus) {
            alert("存取錯誤:" + textStatus + " - " + data);
        }
    });
}

function listBooks(data) {
    var items = 0;
    var moneyCount = 0;
    var cartContent = "<table id='cartList' style='width:100%;margin-top:-1px;'><tr align='center' style='color:#FFF; background-color:#303a4a;'><td style='border:1px solid #414c91; padding:3px;'>書名</td><td style='border:1px solid #414c91; padding:3px;'>數量</td><td style='border:1px solid #414c91; padding:3px;'>金額</td></tr>";
    
    $("div#bookList").empty();    
    for (var i in data) {
        items++;

        var eachCount = data[i].quantity * data[i].productCart.Price;
        moneyCount += eachCount;

        var qtySelector = "<select id='changeQty' name='" + data[i].productCart.ProductId + "' style='width:60px;'>";
        for (var j = 0; j <= data[i].productCart.Stock; j++) {
            if (j == data[i].quantity)
                qtySelector += "<option value='" + j + "' selected>" + j + "</option>";
            else
                qtySelector += "<option value='" + j + "'>" + j + "</option>";
        }
        qtySelector += "</select>";
        cartContent += "<tr style='color:#ababab;'><td style='border:1px solid #363f7a; background-color:#3a4763; padding:3px; text-align:left;'><div id='nextbuy' val='" + data[i].productCart.ProductId + "' style='border:1px solid #3a4763; background-color:#3a4763; padding:3px; cursor:pointer;'>" + data[i].productCart.Name + "</div></td><td style='border:1px solid #363f7a; background-color:#3a4763; padding:3px; text-align:center;'>" + qtySelector + "</td><td style='border:1px solid #363f7a;background-color:#3a4763; padding:3px;text-align:right;font-weight:bold;'>" + eachCount + "</td></tr>";
    }
    cartContent += "<tr style='color:#FFF; background-color:#4b5668;'><td style='border:1px solid #363f7a; padding:5px; text-align:left; color:#abd58d;'>應付金額</td><td style='border:1px solid #363f7a; padding:5px;'></td><td style='border:1px solid #363f7a;padding:5px;text-align:right;font-weight:bold;'>" + moneyCount + "</td></tr></table>";
    $("div#bookList").append(cartContent);
    $("div#bookList").css("height", 33 * items);

    $("select#changeQty").each(function () {
        $(this).change(function () {
            delCart($(this).attr("name"), $(this).val());
        });
    });

    $("div#nextbuy")
        .draggable({
            helper: "clone",
            revert: "invalid",
            cursor: "pointer",
            appendTo: "body"
        })
        .bind("dragstor", f = function (event, ui) {
            $(this).remove();
        });
}

function addCart(prodNo)
{
    $.ajax({
        type: "GET",
        url: "/Cart/Add",
        data: { "productId": prodNo },
        cache: false,
        success: function () {
            alert("商品已經放入購物車!");
            show_nextbuy();
            show_booklist();
        },
        error: function (data, textStatus) {
            alert("存取錯誤:" + textStatus + " - " + data);
        }
    });
}

function delCart(prodNo, prodQty) {
    $.ajax({
        type: "GET",
        url: "/Cart/Update",
        data: { "productId": prodNo, "quantity": prodQty },
        async: false,
        cache: false,
        success: function () {
            show_booklist();
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