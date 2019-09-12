$(document).ready(function() {
    var date = $("#date").datepicker({
        dateFormat: "yy/mm/dd",
        //----週六與週日不允許選取----------------
        beforeShowDay: function(date) {
            var dayOfWeek = date.getDay();
            if(dayOfWeek == 0 || dayOfWeek == 6)
                return [false, "", "Lay off"];
            else
                return [true];
        },
        onSelect: function (dateText) {
            $("#StockedOn").val(dateText);
            $("#InStockedOn").val(dateText);
            $(this).toggle();  //輸入完成後,自動關閉日曆
        }
    });

    $(date).hide();

    $("#calendar")
      .button({
          text: false,
          icons: {
              primary: "ui-icon-calendar"
          }
      })
      .click(function (event) {
          event.preventDefault();
          var pos = $("#calendar").position();
          //----日曆顯示的地方在按鈕下方,且左側對齊-----------------------------
          $(date).css({
              position: "absolute",
              top: (pos.top + 45),
              left: pos.left
          });
        
          //---切換日曆開 / 關:按一下按鈕以開啟日曆,再次按按鈕時,則關閉日曆
          $(date).toggle();
      });
});