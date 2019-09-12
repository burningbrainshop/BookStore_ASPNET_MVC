$(document).ready(function () {
    $("span#dockBack").css({ "float": "left" });

    showDock();

    $.ajax({
        url: "/BookProduct/GetCategory",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data) {
                var options;
                var $bookType = $("select#bookType");

                $bookType.empty();
                for (var i in data) {
                    options += "<option value=" + data[i].CategoryId + ">" + data[i].CategoryName + "</option>";
                }
                $bookType.append(options);
                $("div#listTitle").html("圖書類別 > " + $("select#bookType option:selected").text());
                show_booklist(data[0].CategoryId);
            }
        },
        error: function (data, textStatus) {
            alert("存取錯誤:" + textStatus + " - " + data);
        }
    });

    $("span#news").singlesticker();
    $("input#searchBtn").click(function (event) {
        event.preventDefault();
        $.ajax({
            type: "GET",
            url: "/Product/SearchProducts",
            data: { "term": $("input#searchTxt").val() },
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data) {
                    $("div#listTitle").empty().html("搜尋結果：").css({
                        "position": "absolute"
                    });
                    listBooks(data);
                }
            },
            error: function (data, textStatus) {
                alert("存取錯誤:" + textStatus + " - " + data);
            }
        });
    });

});

$("select#bookType").change(function () {
    $obj = $(this);

    $("div#listTitle").empty().html("圖書類別 > " + $("select#bookType option:selected").text());
    show_booklist($obj.val());
});


$("input#searchTxt").autocomplete({
    source: function (request, callback) {
        var data = { "term": request.term };
        $.ajax({
            type: "GET",
            url: "/Product/ProductByKey",
            data: data,
            open: function (event) {
                var $ul = $(this).autocomplete("widget");
                $ul.css({
                    "width": "200px",
                    "text-align": "left",
                    "z-index": "99999"
                });
            },
            complete: function (xhr, result) {
                if (result != "success") return;
                var response = xhr.responseText.substr(1, xhr.responseText.length - 3).split(",");
                var books = [];
                
                for (var i = 0; i < response.length; i++)
                {
                    response[i] = response[i].substr(1, response[i].length - 2);
                }
                
                /*
                $(response).filter("li").each(function () {
                    books.push($(this).text());
                });
                */
                callback(response);
            }
        });
    }
});

$("div#bookListContainer").mousemove(function (event) {
    var $obj = $(this);
    var $objList = $("div#bookList");
    var containerHeight = $obj.height();
    var objListHeight = $objList.height();

    var distance = (event.pageY - $obj.offset().top) * (objListHeight - containerHeight) / containerHeight;
    $obj.scrollTop(distance);
});

$("div#bookContainer").droppable({
    hoverClass: "hoverBookContainer",
    drop: function (event, ui) {
        var bookid = ui.draggable.attr("val");
        $(this).pagerpoint({ BookId: bookid });
    }
});

$("input#addCart").click(function (event) {
    event.preventDefault();
    if ($("span#no").text() == null || $("span#no").text() == "") {
        alert("您尚未選擇任何商品!");
        return;
    }

    var prodNo = $("span#no").text() * 1;
    $.ajax({
        type: "GET",
        url: "/Cart/Add",
        data: { "productId": prodNo },
        cache: false,
        success: function () {
            alert("商品已放入購物車!");
        },
        error: function (data, textStatus) {
            alert("存取錯誤:" + textStatus + " - " + data);
        }
    });
});

function show_booklist(typeid) {
    $.ajax({
        type: "GET",
        url: "/BookProduct/ProductsListByCategory",
        data: { "categoryId": typeid },
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
    var $obj = $("div#bookListContainer");
    var objWidth = $obj.width();
    var objHeight = $obj.height();

    $("<div id='waiting' style='width:" + objWidth + "px; height:" + objHeight + "px;z-index:20;' class='loadingWhite'></div>").appendTo($("div#bookListContainer"));
    $("div#bookList").empty().css("height", "0px");
    for (var i in data) {
        $("<div id='eachProduct' style='display:line-block; position:absolute; width:auto; border:1px solid #336699; color:#FFFFCC; background-color:#336699; padding:2px; margin-bottom:1px; cursor:pointer; z-index:1; text-align:left;' val=" + data[i].ProductId + ">" + data[i].Name + "</div>")
            .draggable({
                helper: "clone",
                revert: "invalid",
                cursor: "pointer",
                appendTo: "body"
            })
            .bind("dragstor", f = function (event, ui) {
                ui.helper.clone()
                         .draggable({ helper: "clone" })
                         .bind("dragstop", f);
                $(this).remove();
            })
            .appendTo($("div#bookList"));
    }
    
    var pcPos = $("div#bookList").position();
    var topPos = pcPos.top - 25;
    var items = 0;
    var itemHeight = 26;
    var totalHeight = 0;
    $("div#bookList").hide().delay(2000).show("fast", function () { $("div#waiting").remove(); }).find("div#eachProduct").each(function () {
        items++;
        
        totalHeight += itemHeight;
        $(this).css({ "top": topPos += itemHeight });

        if ($(this).text().length > 41) {
            itemHeight = 45;
        }
        else {
            itemHeight = 26;
        }
    });

    if ($("div#bookList").height() < (objHeight - 5)) {
        $("div#bookList").css({
            "height": (objHeight - 5) + "px",
            "background-color": "#0b202e"
        });
    }
    else {
        $("div#bookList").css("height", (totalHeight));
    }
    //$("div#bookList").css("height", 27 * items);
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

function tips(msg) {
    alert(msg);
}