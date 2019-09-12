$(document).ready(function () {
    $("select#CategoryId").change(function (event) {
        event.preventDefault();

        var pid = $("input#ProviderId").val();
        var value = $(this).val();
        var $product = $("select#ProductId");
        if(value != null)
        {
            var x = 0;
            var options = "<option value=''>--請選擇商品--</option>";
            $.ajax({
                type: 'GET',
                url: '/Purchase/GetProductsByCategory',
                cache: false,
                data: { 'providerId': pid, 'categoryId': value },
                dataType: 'json',
                success: function (result) {
                    if(result)
                    {                        
                        for(var i in result)
                        {
                            options += "<option value=" + result[i].ProductId + ">" + result[i].Name + "</options>";
                            x++;
                        }
                        $product.html("").append(options);
                    }
                }
            });
        }
    });

    $("select#ProductId").change(function (event) {
        event.preventDefault();

        var value = $(this).val();
        $.ajax({
            type: 'GET',
            url: '/Purchase/GetCost',
            cache: false,
            data: { 'productId': value },
            dataType: 'json',
            success: function (result) {
                $("input#Cost").val(result);
            }
        });
    });

    $("#submitBtn").click(function (event) {
        event.preventDefault();

        $("div#showError").remove();

        var flag = true;
        var category = $("select#CategoryId").val();
        var product = $("select#ProductId").val();
        var cost = $("input#Cost").val();
        var quantity = $("input#Quantity").val();

        if (category == "" || category == null) { $("select#ProductId").after("<div id='showError' class='field-validation-error'>請選擇商品</div>"); flag = false; }
        if (product == "" || product == null) { if (flag == true) { $("select#ProductId").after("<div id='showError' class='field-validation-error'>請選擇商品</div>"); flag = false; } }
        if (cost == "" || cost == null || cost < 0 || checkNumber($("input#Cost"), /[0-9]/g) == false) { $("input#Cost").after("<div id='showError' class='field-validation-error'>請輸入成本金額且必須大(等)於0</div>"); flag = false; }
        if (quantity == "" || quantity == null || quantity < 0 || checkNumber($("input#Quantity"), /[0-9]/g) == false) { $("input#Quantity").after("<div id='showError' class='field-validation-error'>請輸入採購金額且必須大(等)於0</div>"); flag = false; }

        if (flag)
            $("form").submit();
        else
            return;
    });
});

function checkNumber($obj, regexp) {
    if (!regexp.test($obj.val())) {
        return false;
    }
    else
        return true;
}