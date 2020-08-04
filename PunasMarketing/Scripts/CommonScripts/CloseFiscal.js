var step = 1;
//CashDesk
$(document).on("click", "#CloseTable tr td span#CashDesk", function () {
    var Row = $(this).closest('tr');
    var value = $(this).attr("value");
    var status = $(this).attr("status");
    if (status == 0) {
        if (value == step) {
            $.ajax({
                url: "/FiscalYearOp/CashDeskFiscalYearOp",
                data: { FiscalId: $("#CurrentFiscalId").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    if (result.Success) {
                        Row.find('td').find('span#CashDesk').removeClass("label-outline-danger");
                        Row.find('td').find('span#CashDesk').addClass("label-outline-success");
                        var s = Row.find('td').find('span#status');
                        s.removeClass("label-outline-danger");
                        s.addClass("label-outline-success");
                        s.text("حساب بسته شد");
                        step = step + 1;
                        Row.find('td').find('span#CashDesk').attr("status", 1);


                    }
                    Toast(result.Script);
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }
        else {
            Toast("لطفا در بستن حساب ها ترتیب را رعایت فرمایید,info");
        }
    }
    else {

        Toast("این حساب با موفقیت بسته شده است,info");
    }
});
//Bank
$(document).on("click", "#CloseTable tr td span#Banks", function () {
    var Row = $(this).closest('tr');
    var value = $(this).attr("value");
    var status = $(this).attr("status");
    if (status == 0) {
        if (value == step) {
            $.ajax({
                url: "/FiscalYearOp/BankFiscalYearOp",
                data: { FiscalId: $("#CurrentFiscalId").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    if (result.Success) {
                        Row.find('td').find('span#Banks').removeClass("label-outline-danger");
                        Row.find('td').find('span#Banks').addClass("label-outline-success");
                        var s = Row.find('td').find('span#status');
                        s.removeClass("label-outline-danger");
                        s.addClass("label-outline-success");
                        s.text("حساب بسته شد");
                        step = step + 1;
                        Row.find('td').find('span#Banks').attr("status", 1);


                    }
                    Toast(result.Script);
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }
        else {
            Toast("لطفا در بستن حساب ها ترتیب را رعایت فرمایید,info");
        }
    }
    else {

        Toast("این حساب با موفقیت بسته شده است,info");
    }
});
//HesabAshkhas
$(document).on("click", "#CloseTable tr td span#HesabAshkhas", function () {
    var Row = $(this).closest('tr');
    var value = $(this).attr("value");
    var status = $(this).attr("status");
    if (status == 0) {
        if (value == step) {
            $.ajax({
                url: "/FiscalYearOp/HesabAshkhasFiscalYearOp",
                data: { FiscalId: $("#CurrentFiscalId").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    if (result.Success) {
                        Row.find('td').find('span#HesabAshkhas').removeClass("label-outline-danger");
                        Row.find('td').find('span#HesabAshkhas').addClass("label-outline-success");
                        var s = Row.find('td').find('span#status');
                        s.removeClass("label-outline-danger");
                        s.addClass("label-outline-success");
                        s.text("حساب بسته شد");
                        step = step + 1;
                        Row.find('td').find('span#HesabAshkhas').attr("status", 1);


                    }
                    Toast(result.Script);
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }
        else {
            Toast("لطفا در بستن حساب ها ترتیب را رعایت فرمایید,info");
        }
    }
    else {

        Toast("این حساب با موفقیت بسته شده است,info");
    }
});
//AsnadDaryftani
$(document).on("click", "#CloseTable tr td span#ADaryaftani", function () {
    var Row = $(this).closest('tr');
    var value = $(this).attr("value");
    var status = $(this).attr("status");
    if (status == 0) {
        if (value == step) {
            $.ajax({
                url: "/FiscalYearOp/AsnadDaryaftaniFiscalYearOp",
                data: { FiscalId: $("#CurrentFiscalId").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    if (result.Success) {
                        Row.find('td').find('span#ADaryaftani').removeClass("label-outline-danger");
                        Row.find('td').find('span#ADaryaftani').addClass("label-outline-success");
                        var s = Row.find('td').find('span#status');
                        s.removeClass("label-outline-danger");
                        s.addClass("label-outline-success");
                        s.text("حساب بسته شد");
                        step = step + 1;
                        Row.find('td').find('span#ADaryaftani').attr("status", 1);


                    }
                    Toast(result.Script);
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }
        else {
            Toast("لطفا در بستن حساب ها ترتیب را رعایت فرمایید,info");
        }
    }
    else {

        Toast("این حساب با موفقیت بسته شده است,info");
    }
});
//ChequeDarjarayan
$(document).on("click", "#CloseTable tr td span#Cheuqe", function () {
    var Row = $(this).closest('tr');
    var value = $(this).attr("value");
    var status = $(this).attr("status");
    if (status == 0) {
        if (value == step) {
            $.ajax({
                url: "/FiscalYearOp/ChequeDarJarayanFiscalYearOp",
                data: { FiscalId: $("#CurrentFiscalId").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    if (result.Success) {
                        Row.find('td').find('span#Cheuqe').removeClass("label-outline-danger");
                        Row.find('td').find('span#Cheuqe').addClass("label-outline-success");
                        var s = Row.find('td').find('span#status');
                        s.removeClass("label-outline-danger");
                        s.addClass("label-outline-success");
                        s.text("حساب بسته شد");
                        step = step + 1;
                        Row.find('td').find('span#Cheuqe').attr("status", 1);


                    }
                    Toast(result.Script);
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }
        else {
            Toast("لطفا در بستن حساب ها ترتیب را رعایت فرمایید,info");
        }
    }
    else {

        Toast("این حساب با موفقیت بسته شده است,info");
    }
});

//Sarmayeh
$(document).on("click", "#CloseTable tr td span#Sarmayeh", function () {
    var Row = $(this).closest('tr');
    var value = $(this).attr("value");
    var status = $(this).attr("status");
    if (status == 0) {
        if (value == step) {
            $.ajax({
                url: "/FiscalYearOp/HesabSarmayehFiscalYearOp",
                data: { FiscalId: $("#CurrentFiscalId").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    if (result.Success) {
                        Row.find('td').find('span#Sarmayeh').removeClass("label-outline-danger");
                        Row.find('td').find('span#Sarmayeh').addClass("label-outline-success");
                        var s = Row.find('td').find('span#status');
                        s.removeClass("label-outline-danger");
                        s.addClass("label-outline-success");
                        s.text("حساب بسته شد");
                        step = step + 1;
                        Row.find('td').find('span#Sarmayeh').attr("status", 1);


                    }
                    Toast(result.Script);
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }
        else {
            Toast("لطفا در بستن حساب ها ترتیب را رعایت فرمایید,info");
        }
    }
    else {

        Toast("این حساب با موفقیت بسته شده است,info");
    }
});
//OtherDara
$(document).on("click", "#CloseTable tr td span#OtherDara", function () {
    var Row = $(this).closest('tr');
    var value = $(this).attr("value");
    var status = $(this).attr("status");
    if (status == 0) {
        if (value == step) {
            $.ajax({
                url: "/FiscalYearOp/OtherDaraeiFiscaYearOp",
                data: { FiscalId: $("#CurrentFiscalId").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    if (result.Success) {
                        Row.find('td').find('span#OtherDara').removeClass("label-outline-danger");
                        Row.find('td').find('span#OtherDara').addClass("label-outline-success");
                        var s = Row.find('td').find('span#status');
                        s.removeClass("label-outline-danger");
                        s.addClass("label-outline-success");
                        s.text("حساب بسته شد");
                        step = step + 1;
                        Row.find('td').find('span#OtherDara').attr("status", 1);


                    }
                    Toast(result.Script);
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }
        else {
            Toast("لطفا در بستن حساب ها ترتیب را رعایت فرمایید,info");
        }
    }
    else {

        Toast("این حساب با موفقیت بسته شده است,info");
    }
});
//AsnadPardakhtani
$(document).on("click", "#CloseTable tr td span#APardakhtani", function () {
    var Row = $(this).closest('tr');
    var value = $(this).attr("value");
    var status = $(this).attr("status");
    if (status == 0) {
        if (value == step) {
            $.ajax({
                url: "/FiscalYearOp/AsnadPardakhtaniFiscalYearOp",
                data: { FiscalId: $("#CurrentFiscalId").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    if (result.Success) {
                        Row.find('td').find('span#APardakhtani').removeClass("label-outline-danger");
                        Row.find('td').find('span#APardakhtani').addClass("label-outline-success");
                        var s = Row.find('td').find('span#status');
                        s.removeClass("label-outline-danger");
                        s.addClass("label-outline-success");
                        s.text("حساب بسته شد");
                        step = step + 1;
                        Row.find('td').find('span#APardakhtani').attr("status", 1);


                    }
                    Toast(result.Script);
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }
        else {
            Toast("لطفا در بستن حساب ها ترتیب را رعایت فرمایید,info");
        }
    }
    else {

        Toast("این حساب با موفقیت بسته شده است,info");
    }
});
//OtherBedhi
$(document).on("click", "#CloseTable tr td span#OtherBedehi", function () {
    var Row = $(this).closest('tr');
    var value = $(this).attr("value");
    var status = $(this).attr("status");
    if (status == 0) {
        if (value == step) {
            $.ajax({
                url: "/FiscalYearOp/OtherBedhiFiscaYearOp",
                data: { FiscalId: $("#CurrentFiscalId").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    if (result.Success) {
                        Row.find('td').find('span#OtherBedehi').removeClass("label-outline-danger");
                        Row.find('td').find('span#OtherBedehi').addClass("label-outline-success");
                        var s = Row.find('td').find('span#status');
                        s.removeClass("label-outline-danger");
                        s.addClass("label-outline-success");
                        s.text("حساب بسته شد");
                        step = step + 1;
                        Row.find('td').find('span#OtherBedehi').attr("status", 1);
                        if (value == 9) {
                            $("#CloseFinanceYear").removeAttr("disabled");
                        }

                    }
                    Toast(result.Script);
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }
        else {
            Toast("لطفا در بستن حساب ها ترتیب را رعایت فرمایید,info");
        }
    }
    else {

        Toast("این حساب با موفقیت بسته شده است,info");
    }
});

//initiateDate
$(document).on("ready", function () {

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
                $("#NewFiscalYear").val(FiscalName);
            }
            else {
                $("#EndDate").val("");
                $("#NewFiscalYear").val("");
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

//FinallClose
$(document).on("click", "#CloseFinanceYear", function () {

    if ($("#StartDate").val() != "" && $("#EndDate").val() != "" && $("#CurrentFiscalId").val() != "" && $("#NewFiscalYear").val() != "") {
        $.ajax({
            url: "/FiscalYearOp/CloseFinanceYear",
            data: { startDate: $("#StartDate").val(), EndDate: $("#EndDate").val(), FiscalId: $("#CurrentFiscalId").val(), FiscalName: $("#NewFiscalYear").val() },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                if (result.Success) {
                    window.location.href = result.Html;
                }
                Toast(result.Script);
            },
            error: function () {
                alert("خطا!");
            }
        });
    }
    else {
        Toast('اطلاعات سال مالی تازه را واردنمایید,info');
    }

});