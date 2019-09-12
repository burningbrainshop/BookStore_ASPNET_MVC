$(document).ready(function () {
    $("a#convertBtn").click(function (event) {
        event.preventDefault();
        var url = $(this).attr("href");
        var path = url.split("?");
        var param = path[1].split("=");
        var flag = confirm("您確定要由此筆採購單資料產生進貨單嗎?");
        if (flag) {
            $.ajax({
                type: "Get",
                url: "/Purchase/GetDetails",
                cache: false,
                data: { "purchaseId": param[1] },
                dataType: "text",
                success: function (data) {
                    if (data <= 0) {
                        alert("此採購單尚未建立採購項目,無法轉成進貨單");
                    }
                    else {
                        $.ajax({
                            type: "Get",
                            url: path[0],
                            cache: false,
                            data: { "purchaseId": param[1] },
                            dataType: "json",
                            success: function (data) {
                                if (data) {
                                    alert("進貨單轉單成功");
                                    location.reload();
                                }
                                else
                                    alert("進貨單轉單失敗");
                            },
                            error: function () {
                                alert("進貨單轉單失敗");
                            }
                        });
                    }
                },
                error: function (xhr) {
                    alert("存取錯誤: " + xhr.status);
                }
            });           
        }
    });
});