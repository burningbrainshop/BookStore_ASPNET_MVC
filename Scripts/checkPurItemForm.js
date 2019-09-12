$(document).ready(function () {
    $("#updPurchDetail").click(function (event) {
        event.preventDefault();
        
        $("div#showError").remove();
        
        var flag = true;
        var cost = $("input#item_Cost").val();
        var quantity = $("input#item_Quantity").val();
        
        if (cost == "" || cost == null || cost < 0 || checkNumber($("input#item_Cost"), /[0-9]/g) == false) { $("#show").append("<div id='showError' class='field-validation-error'><br />請輸入商品成本且必須大(等)於0</div>"); flag = false; }
        if (quantity == "" || quantity == null || quantity < 0 || checkNumber($("input#item_Quantity"), /[0-9]/g) == false) { $("#show").append("<div id='showError' class='field-validation-error' style='clear:both;'><br />請輸入採購數量且必須大(等)於0</div>"); flag = false; }

        if (flag)
            $("form").submit();
        else
            return;
    });

    $("#updStockDetail").click(function (event) {
        event.preventDefault();

        $("div#showError").remove();

        var flag = true;
        var quantity = $("input#item_Quantity").val();
        var returnquantity = $("input#item_ReturnQuantity").val();

        if (quantity == "" || quantity == null || quantity < 0 || checkNumber($("input#item_Quantity"), /[0-9]/g) == false) { $("#show").append("<div id='showError' class='field-validation-error' style='clear:both;'><br />請輸入進貨數量且必須大(等)於0</div>"); flag = false; }
        if (returnquantity == "" || returnquantity == null || returnquantity < 0 || checkNumber($("input#item_ReturnQuantity"), /[0-9]/g) == false) { $("#show").append("<div id='showError' class='field-validation-error' style='clear:both;'><br />請輸入退貨數量且必須大(等)於0</div>"); flag = false; }

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