function PayCheque() {
    // دریافتی
    $(".dropDownItem_0").click(function () {
        var theLi = $(this);

        $.get("/Cheque/ReceivedModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // وصول کردن به حساب
    $(".dropDownItem_1").click(function () {
        var theLi = $(this);

        $.get("/Cheque/ClearChequeModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // عودت داده شده
    $(".dropDownItem_2").click(function () {
        var theLi = $(this);

        $.get("/Cheque/ReturnedModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // برگشت خورده
    $(".dropDownItem_3").click(function () {
        var theLi = $(this);

        $.get("/Cheque/BouncedModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // خرج کردن به مشتری
    $(".dropDownItem_4").click(function () {
        var theLi = $(this);

        $.get("/Cheque/PayForCustomerModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // خرج کردن به تأمین کننده
    $(".dropDownItem_5").click(function () {
        var theLi = $(this);

        $.get("/Cheque/PayForSupplierModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // خرج کردن به هزینه
    $(".dropDownItem_6").click(function () {
        var theLi = $(this);

        $.get("/Cheque/PayForCostModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // خواباندن به حساب
    $(".dropDownItem_7").click(function () {
        var theLi = $(this);

        $.get("/Cheque/DepositModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });
}

function ReceivedCheque() {
    // دریافتی
    $(".dropDownItem_0").click(function () {
        var theLi = $(this);

        $.get("/Cheque/ReceivedModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // وصول کردن به حساب
    $(".dropDownItem_1").click(function () {
        var theLi = $(this);

        $.get("/Cheque/ClearChequeModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // عودت داده شده
    $(".dropDownItem_2").click(function () {
        var theLi = $(this);

        $.get("/Cheque/ReturnedModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // برگشت خورده
    $(".dropDownItem_3").click(function () {
        var theLi = $(this);

        $.get("/Cheque/BouncedModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // خرج کردن به مشتری
    $(".dropDownItem_4").click(function () {
        var theLi = $(this);

        $.get("/Cheque/PayForCustomerModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // خرج کردن به تأمین کننده
    $(".dropDownItem_5").click(function () {
        var theLi = $(this);

        $.get("/Cheque/PayForSupplierModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // خرج کردن به هزینه
    $(".dropDownItem_6").click(function () {
        var theLi = $(this);

        $.get("/Cheque/PayForCostModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // خواباندن به حساب
    $(".dropDownItem_7").click(function () {
        var theLi = $(this);

        $.get("/Cheque/DepositModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });
}

function ClearPayCheque() {
    $(document).ready(function () {
        $("#txtClearDate").datepicker({
            beforeShow: function (input, inst) {
                var cal = inst.dpDiv;
                var left = $(this).offset().left;
                setTimeout(function () {
                    cal.css({
                        'left': left
                    });
                },
                    10);
            },
            changeYear: true,
            yearRange: '1350:1420',
            changeMonth: true
        });
    });
}

function ClearReceiveCheque() {
    $(document).ready(function () {
        var accountDiv = $('#accountDiv');
        var cashDiv = $('#cashDiv');
        var accountError = $('#accountError');
        var cashDeskError = $('#cashDeskError');
        var btnSubmitClearReceiveCheque = $('#btnSubmitClearReceiveCheque');

        accountError.show();
        cashDeskError.show();
        accountDiv.hide();
        cashDiv.show();

        $(document).on("change",
            "#drpAccount",
            function () {
                var selectedOption = $("#drpAccount option:selected");

                if (selectedOption.index() !== 0) {
                    accountError.hide();
                } else {
                    accountError.show();
                }
            });

        $(document).on("change",
            "#drpCashDesk",
            function () {
                var selectedOption = $("#drpCashDesk option:selected");

                if (selectedOption.index() !== 0) {
                    cashDeskError.hide();
                } else {
                    cashDeskError.show();
                }
            });

        $('input[type=radio][name=tranType]').change(function () {
            if (this.value === 'cash') {
                accountDiv.hide();
                cashDiv.show();
            } else if (this.value === 'account') {
                accountDiv.show();
                cashDiv.hide();
            }
        });

        $("#txtClearDate").datepicker({
            beforeShow: function (input, inst) {
                var cal = inst.dpDiv;
                var left = $(this).offset().left;
                setTimeout(function () {
                    cal.css({
                        'left': left
                    });
                },
                    10);
            },
            changeYear: true,
            yearRange: '1350:1420',
            changeMonth: true
        });

        btnSubmitClearReceiveCheque.click(function () {
            if ($('#myForm').valid()) {

                if ($('#rbnCash').is(':checked')) {
                    if ($("#drpCashDesk option:selected").index() == 0) {
                        return false;
                    }
                } else if ($('#rbnAccount').is(':checked')) {
                    if ($("#drpAccount option:selected").index() == 0) {
                        return false;
                    }
                }

                var drpAccVal = $('#drpAccount').val();
                var drpCashDeskVal = $('#drpCashDesk').val();
                var txtClearDateVal = $('#txtClearDate').val();
                var hiddenChequeIdVal = $('#hiddenChequeId').val();

                $.ajax({
                    url: "/Cheque/ClearReceiveModalAjax",
                    data: {
                        ChequeId: hiddenChequeIdVal,
                        ClearDate: txtClearDateVal,
                        BankAccountId: drpAccVal,
                        CashDeskId: drpCashDeskVal
                    },
                    type: "POST",
                    dataType: "Json",
                    success: function (result) {
                        if (result.Success) {
                            var url = '/Cheque/ChequeDetails/' + hiddenChequeIdVal;
                            window.location.href = url;
                        } else {
                            Toast("خطا در ثبت اطلاعات,info");
                        }
                    },
                    error: function () {
                    }
                });
            }
        });
    });
}

function ClearReceiveDepositedCheque() {
    $(document).ready(function() {
        $("#txtDepositDate").datepicker({
            beforeShow: function(input, inst) {
                var cal = inst.dpDiv;
                var left = $(this).offset().left;
                setTimeout(function() {
                        cal.css({
                            'left': left
                        });
                    },
                    10);
            },
            changeYear: true,
            yearRange: '1350:1420',
            changeMonth: true
        });
    });
}

function DepositeReceiveCheque() {
    $(document).ready(function() {
        $("#txtDepositDate").datepicker({
            beforeShow: function(input, inst) {
                var cal = inst.dpDiv;
                var left = $(this).offset().left;
                setTimeout(function() {
                        cal.css({
                            'left': left
                        });
                    },
                    10);
            },
            changeYear: true,
            yearRange: '1350:1420',
            changeMonth: true
        });
    });
}

function ReturnedPayCheque() {
    $(document).ready(function () {
        $("#txtClearDate").datepicker({
            beforeShow: function (input, inst) {
                var cal = inst.dpDiv;
                var left = $(this).offset().left;
                setTimeout(function () {
                        cal.css({
                            'left': left
                        });
                    },
                    10);
            },
            changeYear: true,
            yearRange: '1350:1420',
            changeMonth: true
        });
    })
}

function ReturnedReceiveCheque() {
    $(document).ready(function() {
        $("#txtReturnDate").datepicker({
            beforeShow: function(input, inst) {
                var cal = inst.dpDiv;
                var left = $(this).offset().left;
                setTimeout(function() {
                        cal.css({
                            'left': left
                        });
                    },
                    10);
            },
            changeYear: true,
            yearRange: '1350:1420',
            changeMonth: true
        });
    });
}

function ChequeDetails() {
    var CurrentDetail = 0;

    $(document).on("click", "#SanadDetail", function () {
        var value = $(this).attr("value");
        if (CurrentDetail == value) {
            $("#modalSanadDetail").modal({ backdrop: true });
        }
        else {
            $.ajax({
                url: "/Sanad/SanadDetail",
                data: { SanadId: value },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#SanadNum").text(value);
                    $("#SanadDetailList").html(result.Html);
                    $("#modalSanadDetail").modal({ backdrop: true });
                    CurrentDetail = value;
                },
                error: function () {
                    //alert("خطا!");
                }
            });
        }
    });


    // دریافتی
    $(".dropDownItem_1").click(function () {
        var theLi = $(this);

        $.get("/Cheque/ReceivedModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // به حساب خواباندن دریافتی
    $(".dropDownItem_2").click(function () {
        var theLi = $(this);

        $.get("/Cheque/DepositReceiveModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // وصول کردن چک دریافتی
    $(".dropDownItem_3").click(function () {
        var theLi = $(this);

        $.get("/Cheque/ClearReceiveModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // عودت دادن چک دریافتی
    $(".dropDownItem_4").click(function () {
        var theLi = $(this);

        $.get("/Cheque/ReturnedReceiveModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // برگشت خوردن چک دریافتی
    $(".dropDownItem_5").click(function () {
        var theLi = $(this);

        $.get("/Cheque/BouncedReceiveModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // خرج کردن چک دریافتی
    $(".dropDownItem_6").click(function () {
        var theLi = $(this);

        $.get("/Cheque/SpendReceiveModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // پس دادن چک دریافتی
    $(".dropDownItem_7").click(function () {
        var theLi = $(this);

        $.get("/Cheque/GivenBackReceiveModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });


    //////////////////////////////////////////////////////

    // پرداختی
    $(".dropDownItem_11").click(function () {
        var theLi = $(this);

        $.get("/Cheque/PayedModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // وصول کردن چک پرداختی
    $(".dropDownItem_12").click(function () {
        var theLi = $(this);

        $.get("/Cheque/ClearPayModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // عودت دادن چک پرداختی
    $(".dropDownItem_13").click(function () {
        var theLi = $(this);

        $.get("/Cheque/ReturnedPayModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // برگشت خوردن چک پرداختی
    $(".dropDownItem_14").click(function () {
        var theLi = $(this);

        $.get("/Cheque/BouncedPayModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    // پس دادن چک پرداختی
    $(".dropDownItem_15").click(function () {
        var theLi = $(this);

        $.get("/Cheque/GivenBackPayModal/" + theLi.parent().attr("data-cheque-id"), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

}