function defineFiscal() {
    //initiateDate
    $(document).on("ready", function () {
        $(".startdate").datepicker({
            beforeShow: function (input, inst) {
                var cal = inst.dpDiv;
                var left = $(this).offset().left;
                setTimeout(function () {
                    cal.css({
                        'left': left
                    });
                }, 10);
            },
            changeMonth: true,
        });


        $("#EndDate").datepicker({
            beforeShow: function (input, inst) {
                var cal = inst.dpDiv;
                var left = $(this).offset().left;
                setTimeout(function () {
                    cal.css({
                        'left': left
                    });
                }, 10);
            },

            onSelect: function (dateStr) {
                var st = $("#StartDate").val();
                if (CheckDate(st, dateStr)) {
                    var FiscalName = "سال مالی منتهی به" + " " + dateStr;
                    $("#FiscalYearName").val(FiscalName);
                }
                else {
                    $("#EndDate").val("");
                    $("#FiscalYearName").val("");
                }
            },
            changeMonth: true,
        });

        function CheckDate(startDate, EndDate) {
            var d1 = startDate.split('/');
            var d2 = EndDate.split('/');
            if (d2[0] > d1[0]) {
                return true;
            }
            else if (d2[0] == d1[0] && d2[1] > d1[1]) {
                return true;
            }
            else if (d2[0] == d1[0] && d2[1] == d1[1] && d2[2] > d1[1]) {
                return true;
            }
            else {
                return false;
            }
        }
    });
}