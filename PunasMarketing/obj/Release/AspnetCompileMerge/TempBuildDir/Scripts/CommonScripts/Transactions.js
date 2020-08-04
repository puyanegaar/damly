function Addpayment() {

    var cashCount = -1;
    var CardCount = -1;
    var ChequeCount = -1;
    var PayCheque = [];
    $(document).on("keyup", ".Cashprice", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });
    });
    //CardPart

    $(document).on("change", ".CardBankAccount", function () {
        var Row = $(this).closest('.card').attr("Row");
        var value = $("#Card_" + Row + "__BankAccountId").find(":selected").val();
        $.ajax({
            url: "/Transaction/GetAccountInfo",
            data: { BookId: value },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#Card_" + Row + "__CardAccountNumber").val(result.Html + "/" + result.Option);
            },
            error: function () {
                alert("خطا!");
            }
        });
    });
    $(document).on("keyup", ".Cardprice", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });
    });


    $(document).on("click", "#CashAdd", function (e) {
        e.preventDefault();
        cashCount = cashCount + 1;
        $.ajax({
            url: "/Transaction/GenerateForm",
            data: { Row: cashCount, Formtype: 1 },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#CashPart").append(result.Html);
                var form = $("#PayForm")
                    .removeData("validator") /* added by the raw jquery.validate plugin */
                    .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/
                $.validator.unobtrusive.parse(form);
                $(form).data('unobtrusiveValidation');
            },
            error: function () {
                alert("خطا!");
            }
        });

    });
    $(document).on("click", "#BankJarAdd", function (e) {
        e.preventDefault();
        CardCount = CardCount + 1;
        $.ajax({
            url: "/Transaction/GenerateForm",
            data: { Row: CardCount, Formtype: 2, ptaype: false },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#CardPart").append(result.Html);
                var form = $("#PayForm")
                    .removeData("validator") /* added by the raw jquery.validate plugin */
                    .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/
                $.validator.unobtrusive.parse(form);
                $(form).data('unobtrusiveValidation');
            },
            error: function () {
                alert("خطا!");
            }
        });

    });
    $(document).on("click", "#ChequeAdd", function (e) {
        e.preventDefault();
        ChequeCount = ChequeCount + 1;
        $.ajax({
            url: "/Transaction/GenerateForm",
            data: { Row: ChequeCount, Formtype: 4 },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#ChequePart").append(result.Html);
                var form = $("#PayForm")
                    .removeData("validator") /* added by the raw jquery.validate plugin */
                    .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/
                $.validator.unobtrusive.parse(form);
                $(form).data('unobtrusiveValidation');
                $(".DatePicker").datepicker({
                    beforeShow: function (input, inst) {
                        var cal = inst.dpDiv;
                        var left = $(this).offset().left;
                        setTimeout(function () {
                            cal.css({
                                'left': left
                            });
                        }, 10);
                    }
                });
            },
            error: function () {
                alert("خطا!");
            }
        });

    });
    $(document).on("click", "#PayCheque", function (e) {
        e.preventDefault();
        $.ajax({
            url: "/Transaction/GetAvailableCheque",
            data: {},
            type: "Post",
            dataType: "Json",
            success: function (result) {

                $("#AvailableChequeList").html(result.Html);
                var dataTable = $("#AvailableChequeItems").dataTable({
                    "bInfo": false,
                    "bLengthChange": false,
                    "bSort": false,
                    "language": {
                        "paginate": {
                            "previous": "قبلی",
                            "next": "بعدی"
                        }
                    },
                    "iDisplayLength": 6
                });
                $("#Chequesearchbox").keyup(function () {
                    dataTable.fnFilter(this.value);
                });
                $("#modalAvailableCheque").modal({ backdrop: true });
            },
            error: function () {
                alert("خطا!");
            }
        });
    });
    $(document).on("click", "#AvailableChequeItems tr td a#SelectAvailableCheque", function () {
        var Row = $(this).closest('tr');
        var ChequeId = $(this).attr("value");

        if (jQuery.inArray(ChequeId, PayCheque) !== -1) {

            Toast("چک با این شماره به لیست اضافه شده,info");
        }
        else {
            $.ajax({
                url: "/Transaction/GenerateForm",
                data: { Row: ChequeId, Formtype: 5, ptaype: false },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#PayChequePart").append(result.Html);
                    var Amoutn = Row.find('td:eq(2)').text().trim().replace(/,/g, '');
                    var TotalAmount = $("#pay_TotalAmount").val();
                    TotalAmount = +TotalAmount + +Amoutn;
                    $("#pay_TotalAmount").val(TotalAmount);
                    var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    $("#TAmount").text(T);
                    PayCheque.push(ChequeId);
                    $("#PayChequeIds").val(PayCheque);
                    $("#modalAvailableCheque").modal('hide');
                },
                error: function () {
                    alert("خطا!");
                }
            });

        }




    });
    $(document).on("click", ".RemoveCashPart", function () {
        var Row = $(this).closest('.card').attr('Row');
        if (cashCount == Row) {

            cashCount = cashCount - 1;
            var Amount = $("#cash_" + Row + "__Amount").val().replace(/,/g, '');
            var TotalAmount = $("#pay_TotalAmount").val();
            TotalAmount = +TotalAmount - +Amount;
            $("#pay_TotalAmount").val(TotalAmount);
            var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#TAmount").text(T);
            $(this).closest('.card').remove();
        }

    });
    $(document).on("click", ".RemoveCardPart", function () {
        var Row = $(this).closest('.card').attr('Row');
        if (CardCount == Row) {

            CardCount = CardCount - 1;
            var Amount = $("#Card_" + Row + "__Amount").val().replace(/,/g, '');
            var TotalAmount = $("#pay_TotalAmount").val();
            TotalAmount = +TotalAmount - +Amount;
            $("#pay_TotalAmount").val(TotalAmount);
            var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#TAmount").text(T);
            $(this).closest('.card').remove();

        }

    });
    $(document).on("click", ".RemoveChequePart", function () {
        var Row = $(this).closest('.card').attr('Row');

        if (ChequeCount == Row) {

            ChequeCount = ChequeCount - 1;
            var Amount = $("#cheques_" + Row + "__Price").val().replace(/,/g, '');
            var TotalAmount = $("#pay_TotalAmount").val();
            TotalAmount = +TotalAmount - +Amount;
            $("#pay_TotalAmount").val(TotalAmount);
            var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#TAmount").text(T);
            $(this).closest('.card').remove();
        }

    });
    $(document).on("click", ".RemovePayChequePart", function () {

        var ChequeId = $(this).closest('.card').attr('ChequeId');
        var Amount = $(this).closest('.card').attr('Amount');
        var TotalAmount = $("#pay_TotalAmount").val();
        TotalAmount = +TotalAmount - +Amount;
        $("#pay_TotalAmount").val(TotalAmount);
        PayCheque = jQuery.grep(PayCheque, function (value) {
            return value != ChequeId;
        });
        $("#PayChequeIds").val(PayCheque);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
        $(this).closest('.card').remove()


    });

    //select2
    $(document).ready(function () {
        $("#pay_PersonType").select2();
        $("#pay_Persons").select2();
        $("#pay_sarfasl").select2();
        $("#pay_Tafsili").select2();
    });

    //personTypeChange
    $(document).on("change", "#pay_PersonType", function () {

        var value = $("#pay_PersonType").find(":selected").val();
        if (value != 0) {
            $.ajax({
                url: "/Transaction/GetPesronsInfo",
                data: { PersonType: value },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpPerson").html(result.Html);
                    $("#pay_Persons").select2();

                },
                error: function () {
                    alert("خطا!");
                }
            });
        }

    });
    $(document).on("keyup", ".Chequeprice", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });

    });

    $(document).on("focusout", ".Chequeprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount + +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });
    $(document).on("focusin", ".Chequeprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount - +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });

    $(document).on("focusin", ".Cashprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount - +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });
    $(document).on("focusout", ".Cashprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount + +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });

    $(document).on("focusout", ".Cardprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount + +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });
    $(document).on("focusin", ".Cardprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount - +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });



    //GetBalance
    $(document).on("change", "#pay_Persons", function () {

        var pt = $("#pay_PersonType").find(":selected").val();
        var p = $("#pay_Persons").find(":selected").val();
        $.ajax({
            url: "/Transaction/GetPersonBalance",
            data: { PersonId: p.split('/')[0], PersonType: pt },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#Blance").html(result.Html);
                var TotalAmount = $("#pay_TotalAmount").val();
                var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                $("#TAmount").text(T);

            },
            error: function () {
                alert("خطا!");
            }
        });

    });

    //radio
    $(document).on("click", ".radioactive", function () {
        var val = $('input[name=PayType]:checked').val();
        if (val == 0) {
            $("#pay_PersonType").removeAttr("disabled");
            $("#pay_Persons").removeAttr("disabled");
            setTimeout(function () {
                $("#PersonSelect").slideDown();
            }, 50);
            $("#HesabSelect").slideUp();
            setTimeout(function () {
                $("#pay_sarfasl").attr("disabled", "disabled");
                $("#pay_Tafsili").attr("disabled", "disabled");
            }, 250);


        }
        else {
            $("#pay_sarfasl").removeAttr("disabled");
            $("#pay_Tafsili").removeAttr("disabled");
            setTimeout(function () {
                $("#HesabSelect").slideDown();
            }, 50);
            $("#PersonSelect").slideUp();
            setTimeout(function () {
                $("#pay_PersonType").attr("disabled", "disabled");
                $("#pay_Persons").attr("disabled", "disabled");
            }, 250);



        }

    });

    //HesabChange
    $(document).on("change", "#pay_sarfasl", function () {
        var value = $("#pay_sarfasl").find(":selected").val();
        if (value != 0) {
            $.ajax({
                url: "/Transaction/GetTafsiliInfo",
                data: { SarfaslId: value },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpTafsili").html(result.Html);
                    $("#pay_Tafsili").select2();
                    var form = $("#PayForm")
                        .removeData("validator") /* added by the raw jquery.validate plugin */
                        .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/
                    $.validator.unobtrusive.parse(form);
                    $(form).data('unobtrusiveValidation');

                },
                error: function () {
                    alert("خطا!");
                }
            });
        }

    });


}

function AddReceive() {

    var cashCount = -1;
    var CardCount = -1;
    var ChequeCount = -1;
    $(document).on("keyup", ".Cashprice", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });
    });

    //CardPart
    $(document).on("change", ".CardBankAccount", function () {
        var Row = $(this).closest('.card').attr("Row");
        var value = $("#Card_" + Row + "__BankAccountId").find(":selected").val();
        $.ajax({
            url: "/Transaction/GetAccountInfo",
            data: { BookId: value },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#Card_" + Row + "__CardAccountNumber").val(result.Html + "/" + result.Option);
            },
            error: function () {
                alert("خطا!");
            }
        });
    });
    $(document).on("keyup", ".Cardprice", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });
    });


    $(document).on("click", "#CashAdd", function (e) {
        e.preventDefault();
        cashCount = cashCount + 1;
        $.ajax({
            url: "/Transaction/GenerateForm",
            data: { Row: cashCount, Formtype: 1 },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#CashPart").append(result.Html);
                var form = $("#ReceiveForm")
                    .removeData("validator") /* added by the raw jquery.validate plugin */
                    .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/
                $.validator.unobtrusive.parse(form);
                $(form).data('unobtrusiveValidation');
            },
            error: function () {
                alert("خطا!");
            }
        });

    });
    $(document).on("click", "#BankJarAdd", function (e) {
        e.preventDefault();
        CardCount = CardCount + 1;
        $.ajax({
            url: "/Transaction/GenerateForm",
            data: { Row: CardCount, Formtype: 2 },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#CardPart").append(result.Html);
                var form = $("#ReceiveForm")
                    .removeData("validator") /* added by the raw jquery.validate plugin */
                    .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/
                $.validator.unobtrusive.parse(form);
                $(form).data('unobtrusiveValidation');
            },
            error: function () {
                alert("خطا!");
            }
        });

    });
    $(document).on("click", "#ChequeAdd", function (e) {
        e.preventDefault();
        ChequeCount = ChequeCount + 1;
        $.ajax({
            url: "/Transaction/GenerateForm",
            data: { Row: ChequeCount, Formtype: 3 },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#ChequePart").append(result.Html);
                var form = $("#ReceiveForm")
                    .removeData("validator") /* added by the raw jquery.validate plugin */
                    .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/
                $.validator.unobtrusive.parse(form);
                $(form).data('unobtrusiveValidation');
                $(".DatePicker").datepicker({
                    beforeShow: function (input, inst) {
                        var cal = inst.dpDiv;
                        var left = $(this).offset().left;
                        setTimeout(function () {
                            cal.css({
                                'left': left
                            });
                        }, 10);
                    }
                });
            },
            error: function () {
                alert("خطا!");
            }
        });

    });
    $(document).on("click", ".RemoveCashPart", function () {
        var Row = $(this).closest('.card').attr('Row');
        if (cashCount == Row) {
            cashCount = cashCount - 1;
            var Amount = $("#cash_" + Row + "__Amount").val().replace(/,/g, '');
            var TotalAmount = $("#pay_TotalAmount").val();
            TotalAmount = +TotalAmount - +Amount;
            $("#pay_TotalAmount").val(TotalAmount);
            var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#TAmount").text(T);
            $(this).closest('.card').remove();
        }

    });
    $(document).on("click", ".RemoveCardPart", function () {
        var Row = $(this).closest('.card').attr('Row');
        if (CardCount == Row) {
            CardCount = CardCount - 1;
            var Amount = $("#Card_" + Row + "__Amount").val().replace(/,/g, '');
            var TotalAmount = $("#pay_TotalAmount").val();
            TotalAmount = +TotalAmount - +Amount;
            $("#pay_TotalAmount").val(TotalAmount);
            var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#TAmount").text(T);
            $(this).closest('.card').remove();
        }

    });
    $(document).on("click", ".RemoveChequePart", function () {
        var Row = $(this).closest('.card').attr('Row');

        if (ChequeCount == Row) {
            var Amount = $("#cheques_" + Row + "__Price").val().replace(/,/g, '');
            var TotalAmount = $("#pay_TotalAmount").val();
            TotalAmount = +TotalAmount - +Amount;
            $("#pay_TotalAmount").val(TotalAmount);
            var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#TAmount").text(T);
            $(this).closest('.card').remove();
            ChequeCount = ChequeCount - 1;
        }

    });

    //select2
    $(document).ready(function () {
        $("#pay_PersonType").select2();
        $("#pay_Persons").select2();
        $("#pay_sarfasl").select2();
    });

    //personTypeChange
    $(document).on("change", "#pay_PersonType", function () {

        var value = $("#pay_PersonType").find(":selected").val();
        if (value != 0) {
            $.ajax({
                url: "/Transaction/GetPesronsInfo",
                data: { PersonType: value },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpPerson").html(result.Html);
                    $("#pay_Persons").select2();

                },
                error: function () {
                    alert("خطا!");
                }
            });
        }

    });

    $(document).on("keyup", ".Chequeprice", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });

    });

    $(document).on("focusout", ".Chequeprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount + +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });
    $(document).on("focusin", ".Chequeprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount - +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });

    $(document).on("focusin", ".Cashprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount - +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });
    $(document).on("focusout", ".Cashprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount + +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });

    $(document).on("focusout", ".Cardprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount + +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });
    $(document).on("focusin", ".Cardprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount - +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });

  

    //GetBalance
    $(document).on("change", "#pay_Persons", function () {

        var pt = $("#pay_PersonType").find(":selected").val();
        var p = $("#pay_Persons").find(":selected").val();
        $.ajax({
            url: "/Transaction/GetPersonBalance",
            data: { PersonId: p.split('/')[0], PersonType: pt },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#Blance").html(result.Html);


            },
            error: function () {
                alert("خطا!");
            }
        });

    });

}

function AddTransfer() {


    //select2
    $(document).ready(function () {
        $("#FromCashDesk").select2();
        $("#ToCashDesk").select2();
        $("#FromBank").select2();
        $("#ToBank").select2();
    });

    //radio
    $(document).on("click", ".SourceRadioactive", function () {
        var val = $('input[name=SourceType]:checked').val();
        if (val == 0) {
            $("#FromCashDesk").removeAttr("disabled");
            setTimeout(function () {
                $("#DrpSourceCashDesk").slideDown();
            }, 50);
            $("#DrpSourceBank").slideUp();
            setTimeout(function () {
                $("#FromBank").attr("disabled", "disabled");
            }, 250);


        }
        else {

            $("#FromBank").removeAttr("disabled");
            setTimeout(function () {
                $("#DrpSourceBank").slideDown();
            }, 50);
            $("#DrpSourceCashDesk").slideUp();
            setTimeout(function () {
                $("#FromCashDesk").attr("disabled", "disabled");
            }, 250);
        }
    });

    $(document).on("click", ".DestRadioactive", function () {
        var val = $('input[name=DestType]:checked').val();
        if (val == 0) {

            $("#ToCashDesk").removeAttr("disabled");
            setTimeout(function () {
                $("#DrpDestCashDesk").slideDown();
            }, 50);
            $("#DrpDestBank").slideUp();
            setTimeout(function () {
                $("#ToBank").attr("disabled", "disabled");
            }, 250);
        }
        else {

            $("#ToBank").removeAttr("disabled");
            setTimeout(function () {
                $("#DrpDestBank").slideDown();
            }, 50);
            $("#DrpDestCashDesk").slideUp();
            setTimeout(function () {
                $("#ToCashDesk").attr("disabled", "disabled");
            }, 250);
        }

    });

    $(document).on("keyup", "#Amount", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });
    });

    $("#Date").datepicker({
        beforeShow: function (input, inst) {
            var cal = inst.dpDiv;
            var left = $(this).offset().left;
            setTimeout(function () {
                cal.css({
                    'left': left
                });
            }, 10);
        }
    });

   

}

function payments() {
    var CurrentDetail = 0;

    $(document).ready(function () {
        var dataTable = $('#sanadsList').dataTable({
            "bInfo": false,
            "language": {
                "paginate": {
                    "previous": "قبلی",
                    "next": "بعدی"
                }
            }
        });

        $("#searchbox").keyup(function () {
            dataTable.fnFilter(this.value);
        });
    });

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
}

function Recevies() {
    var CurrentDetail = 0;

    $(document).ready(function () {
        var dataTable = $('#sanadsList').dataTable({
            "bInfo": false,
            "language": {
                "paginate": {
                    "previous": "قبلی",
                    "next": "بعدی"
                }
            }
        });

        $("#searchbox").keyup(function () {
            dataTable.fnFilter(this.value);
        });
    });

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
}

function Transfers() {
    var CurrentDetail = 0;

    $(document).ready(function () {
        var dataTable = $('#sanadsList').dataTable({
            "bInfo": false,
            "language": {
                "paginate": {
                    "previous": "قبلی",
                    "next": "بعدی"
                }
            }
        });

        $("#searchbox").keyup(function () {
            dataTable.fnFilter(this.value);
        });
    });

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
}

function updatePayment() {

    var cashCount = -1;
    var CardCount = -1;
    var ChequeCount = -1;
    var Delete = [];
    var DeleteCheque = [];
    var DeletePayCheque = [];
    var PayCheque = [];

    $(document).on("keyup", ".Cashprice", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });
    });

    //CardPart
    $(document).on("change", ".CardBankAccount", function () {

        var EditRow = $(this).closest('.card').attr('EditRow');
        if (typeof EditRow !== typeof undefined && EditRow !== false) {
            var value = $("#EditCard_" + EditRow + "__BankAccountId").find(":selected").val();
            $.ajax({
                url: "/Transaction/GetAccountInfo",
                data: { BookId: value },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#EditCard_" + EditRow + "__CardAccountNumber").val(result.Html + "/" + result.Option);
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }
        else {
            var Row = $(this).closest('.card').attr("Row");
            var value = $("#Card_" + Row + "__BankAccountId").find(":selected").val();

            $.ajax({
                url: "/Transaction/GetAccountInfo",
                data: { BookId: value },
                type: "Post",
                dataType: "Json",
                success: function (result) {

                    $("#Card_" + Row + "__CardAccountNumber").val(result.Html + "/" + result.Option);
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }
    });
    $(document).on("keyup", ".Cardprice", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });
    });

    $(document).on("click", "#CashAdd", function (e) {
        e.preventDefault();
        cashCount = cashCount + 1;
        $.ajax({
            url: "/Transaction/GenerateForm",
            data: { Row: cashCount, Formtype: 1 },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#CashPart").append(result.Html);
                var form = $("#ReceiveForm")
                    .removeData("validator") /* added by the raw jquery.validate plugin */
                    .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/
                $.validator.unobtrusive.parse(form);
                $(form).data('unobtrusiveValidation');
            },
            error: function () {
                alert("خطا!");
            }
        });

    });
    $(document).on("click", "#BankJarAdd", function (e) {
        e.preventDefault();
        CardCount = CardCount + 1;
        $.ajax({
            url: "/Transaction/GenerateForm",
            data: { Row: CardCount, Formtype: 2 },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#CardPart").append(result.Html);
                var form = $("#ReceiveForm")
                    .removeData("validator") /* added by the raw jquery.validate plugin */
                    .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/
                $.validator.unobtrusive.parse(form);
                $(form).data('unobtrusiveValidation');
            },
            error: function () {
                alert("خطا!");
            }
        });

    });
    $(document).on("click", "#ChequeAdd", function (e) {
        e.preventDefault();
        ChequeCount = ChequeCount + 1;
        $.ajax({
            url: "/Transaction/GenerateForm",
            data: { Row: ChequeCount, Formtype: 4 },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#ChequePart").append(result.Html);
                var form = $("#PayForm")
                    .removeData("validator") /* added by the raw jquery.validate plugin */
                    .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/
                $.validator.unobtrusive.parse(form);
                $(form).data('unobtrusiveValidation');
                $(".DatePicker").datepicker({
                    beforeShow: function (input, inst) {
                        var cal = inst.dpDiv;
                        var left = $(this).offset().left;
                        setTimeout(function () {
                            cal.css({
                                'left': left
                            });
                        }, 10);
                    }
                });
            },
            error: function () {
                alert("خطا!");
            }
        });

    });
    $(document).on("click", "#PayCheque", function (e) {
        e.preventDefault();
        $.ajax({
            url: "/Transaction/GetAvailableCheque",
            data: {},
            type: "Post",
            dataType: "Json",
            success: function (result) {

                $("#AvailableChequeList").html(result.Html);
                var dataTable = $("#AvailableChequeItems").dataTable({
                    "bInfo": false,
                    "bLengthChange": false,
                    "bSort": false,
                    "language": {
                        "paginate": {
                            "previous": "قبلی",
                            "next": "بعدی"
                        }
                    },
                    "iDisplayLength": 6
                });
                $("#Chequesearchbox").keyup(function () {
                    dataTable.fnFilter(this.value);
                });
                $("#modalAvailableCheque").modal({ backdrop: true });
            },
            error: function () {
                alert("خطا!");
            }
        });
    });
    $(document).on("click", "#AvailableChequeItems tr td a#SelectAvailableCheque", function () {
        var Row = $(this).closest('tr');
        var ChequeId = $(this).attr("value");

        if (jQuery.inArray(ChequeId, PayCheque) !== -1) {

            Toast("چک با این شماره به لیست اضافه شده,info");
        }
        else {
            $.ajax({
                url: "/Transaction/GenerateForm",
                data: { Row: ChequeId, Formtype: 5, ptaype: false },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#PayChequePart").append(result.Html);
                    var Amoutn = Row.find('td:eq(2)').text().trim().replace(/,/g, '');
                    var TotalAmount = $("#pay_TotalAmount").val();
                    TotalAmount = +TotalAmount + +Amoutn;
                    $("#pay_TotalAmount").val(TotalAmount);
                    PayCheque.push(ChequeId);
                    $("#PayChequeIds").val(PayCheque);
                    $("#modalAvailableCheque").modal('hide');
                },
                error: function () {
                    alert("خطا!");
                }
            });

        }




    });

    $(document).on("click", ".RemoveCashPart", function () {

        var EditRow = $(this).closest('.card').attr('EditRow');
        if (typeof EditRow !== typeof undefined && EditRow !== false) {
            $(this).closest('.card').hide();
            var EditCashId = $("#EditCash_" + EditRow + "__id").val();
            var EditCashSecondId = $("#EditCash_" + EditRow + "__SecondId").val();
            var TotalAmount = $("#pay_TotalAmount").val();
            var Amount = $("#EditCash_" + EditRow + "__Amount").val().replace(/,/g, '');
            var re = +TotalAmount - +Amount;
            $("#pay_TotalAmount").val(re);
            Delete.push(EditCashId);
            Delete.push(EditCashSecondId);
            $("#deletedItem").val(Delete);
            var T = re.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#TAmount").text(T);
        }
        else {
            var Row = $(this).closest('.card').attr('Row');
            if (cashCount == Row) {
                cashCount = cashCount - 1;
                var Amount = $("#cash_" + Row + "__Amount").val().replace(/,/g, '');
                var TotalAmount = $("#pay_TotalAmount").val();
                TotalAmount = +TotalAmount - +Amount;
                $("#pay_TotalAmount").val(TotalAmount);
                var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                $("#TAmount").text(T);
                $(this).closest('.card').remove();
            }

        }


    });
    $(document).on("click", ".RemoveCardPart", function () {

        var EditRow = $(this).closest('.card').attr('EditRow');
        if (typeof EditRow !== typeof undefined && EditRow !== false) {
            $(this).closest('.card').hide();
            var EditCardId = $("#EditCard_" + EditRow + "__id").val();
            var EditCardSecondId = $("#EditCard_" + EditRow + "__SecondId").val();
            var TotalAmount = $("#pay_TotalAmount").val();
            var Amount = $("#EditCard_" + EditRow + "__Amount").val().replace(/,/g, '');
            var re = +TotalAmount - +Amount;
            $("#pay_TotalAmount").val(re);
            Delete.push(EditCardId);
            Delete.push(EditCardSecondId);
            $("#deletedItem").val(Delete);
            var T = re.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#TAmount").text(T);
        }
        else {
            var Row = $(this).closest('.card').attr('Row');
            if (CardCount == Row) {
                CardCount = CardCount - 1;
                var Amount = $("#Card_" + Row + "__Amount").val().replace(/,/g, '');
                var TotalAmount = $("#pay_TotalAmount").val();
                TotalAmount = +TotalAmount - +Amount;
                $("#pay_TotalAmount").val(TotalAmount);
                var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                $("#TAmount").text(T);
                $(this).closest('.card').remove();
            }
        }




    });
    $(document).on("click", ".RemoveChequePart", function () {
        var EditRow = $(this).closest('.card').attr('EditRow');
        if (typeof EditRow !== typeof undefined && EditRow !== false) {
            $(this).closest('.card').hide();
            var EditChequeId = $("#Editcheques_" + EditRow + "__ChequeSanadId").val();

            var EditChequeSecondId = $("#Editcheques_" + EditRow + "__ChequeSanadSecondId").val();
            var ChequeId = $("#Editcheques_" + EditRow + "__Id").val();

            var TotalAmount = $("#pay_TotalAmount").val();
            var Amount = $("#Editcheques_" + EditRow + "__Price").val().replace(/,/g, '');
            var re = +TotalAmount - +Amount;
            $("#pay_TotalAmount").val(re);
            Delete.push(EditChequeId);
            Delete.push(EditChequeSecondId);
            DeleteCheque.push(ChequeId);
            $("#deletedItem").val(Delete);
            $("#deteledCheque").val(DeleteCheque);
            var T = re.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#TAmount").text(T);

        }
        else {

            var Row = $(this).closest('.card').attr('Row');
            if (ChequeCount == Row) {

                ChequeCount = ChequeCount - 1;
                var Amount = $("#cheques_" + Row + "__Price").val().replace(/,/g, '');
                var TotalAmount = $("#pay_TotalAmount").val();
                TotalAmount = +TotalAmount - +Amount;
                $("#pay_TotalAmount").val(TotalAmount);
                var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                $("#TAmount").text(T);
                $(this).closest('.card').remove();
            }
        }

    });
    $(document).on("click", ".RemovePayChequePart", function () {
        var EditRow = $(this).closest('.card').attr('EditRow');

        if (typeof EditRow !== typeof undefined && EditRow !== false) {
            var EditPaySanadChequeId = $("#EditPaycheques_" + EditRow + "__ChequeSanadId").val();

            var EditPaySanadChequeSecondId = $("#EditPaycheques_" + EditRow + "__ChequeSanadSecondId").val();

            var PayChequeId = $(this).closest('.card').attr('ChequeId');

            var Amount = $(this).closest('.card').attr('Amount');
            var TotalAmount = $("#pay_TotalAmount").val();
            TotalAmount = +TotalAmount - +Amount;
            $("#pay_TotalAmount").val(TotalAmount);
            Delete.push(EditPaySanadChequeId);
            Delete.push(EditPaySanadChequeSecondId);
            DeletePayCheque.push(PayChequeId);
            $("#deletedItem").val(Delete);
            $("#deteledPayCheque").val(DeletePayCheque);
            var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#TAmount").text(T);
            $(this).closest('.card').hide();

        }
        else {
            var ChequeId = $(this).closest('.card').attr('ChequeId');
            var Amount = $(this).closest('.card').attr('Amount');
            var TotalAmount = $("#pay_TotalAmount").val();
            TotalAmount = +TotalAmount - +Amount;
            $("#pay_TotalAmount").val(TotalAmount);
            PayCheque = jQuery.grep(PayCheque, function (value) {
                return value != ChequeId;
            });
            $("#PayChequeIds").val(PayCheque);
            var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#TAmount").text(T);
            $(this).closest('.card').remove()
        }

    });

    $(document).on("keyup", ".Chequeprice", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });

    });

    $(document).on("focusout", ".Chequeprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount + +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);

    });
    $(document).on("focusin", ".Chequeprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount - +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);

    });

    $(document).on("focusin", ".Cashprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount - +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);


    });
    $(document).on("focusout", ".Cashprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount + +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);

    });

    $(document).on("focusout", ".Cardprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount + +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });
    $(document).on("focusin", ".Cardprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount - +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });

    //GetBalance

    $(".DatePicker").datepicker({
        beforeShow: function (input, inst) {
            var cal = inst.dpDiv;
            var left = $(this).offset().left;
            setTimeout(function () {
                cal.css({
                    'left': left
                });
            }, 10);
        }
    });

   

}

function updateReceive() {

    var cashCount = -1;
    var CardCount = -1;
    var ChequeCount = -1;
    var Delete = [];
    var DeleteCheque = [];
    $(document).on("keyup", ".Cashprice", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });
    });

    //CardPart
    $(document).on("change", ".CardBankAccount", function () {

        var EditRow = $(this).closest('.card').attr('EditRow');
        if (typeof EditRow !== typeof undefined && EditRow !== false) {
            var value = $("#EditCard_" + EditRow + "__BankAccountId").find(":selected").val();
            $.ajax({
                url: "/Transaction/GetAccountInfo",
                data: { BookId: value },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#EditCard_" + EditRow + "__CardAccountNumber").val(result.Html + "/" + result.Option);
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }
        else {
            var Row = $(this).closest('.card').attr("Row");
            var value = $("#Card_" + Row + "__BankAccountId").find(":selected").val();
            $.ajax({
                url: "/Transaction/GetAccountInfo",
                data: { BookId: value },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#Card_" + Row + "__CardAccountNumber").val(result.Html + "/" + result.Option);
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }
    });
    $(document).on("keyup", ".Cardprice", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });
    });


    $(document).on("click", "#CashAdd", function (e) {
        e.preventDefault();
        cashCount = cashCount + 1;
        $.ajax({
            url: "/Transaction/GenerateForm",
            data: { Row: cashCount, Formtype: 1 },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#CashPart").append(result.Html);
                var form = $("#ReceiveForm")
                    .removeData("validator") /* added by the raw jquery.validate plugin */
                    .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/
                $.validator.unobtrusive.parse(form);
                $(form).data('unobtrusiveValidation');
            },
            error: function () {
                alert("خطا!");
            }
        });

    });
    $(document).on("click", "#BankJarAdd", function (e) {
        e.preventDefault();
        CardCount = CardCount + 1;
        $.ajax({
            url: "/Transaction/GenerateForm",
            data: { Row: CardCount, Formtype: 2 },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#CardPart").append(result.Html);
                var form = $("#ReceiveForm")
                    .removeData("validator") /* added by the raw jquery.validate plugin */
                    .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/
                $.validator.unobtrusive.parse(form);
                $(form).data('unobtrusiveValidation');
            },
            error: function () {
                alert("خطا!");
            }
        });

    });
    $(document).on("click", "#ChequeAdd", function (e) {
        e.preventDefault();
        ChequeCount = ChequeCount + 1;
        $.ajax({
            url: "/Transaction/GenerateForm",
            data: { Row: ChequeCount, Formtype: 3 },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                $("#ChequePart").append(result.Html);
                var form = $("#ReceiveForm")
                    .removeData("validator") /* added by the raw jquery.validate plugin */
                    .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/
                $.validator.unobtrusive.parse(form);
                $(form).data('unobtrusiveValidation');
                $(".DatePicker").datepicker({
                    beforeShow: function (input, inst) {
                        var cal = inst.dpDiv;
                        var left = $(this).offset().left;
                        setTimeout(function () {
                            cal.css({
                                'left': left
                            });
                        }, 10);
                    }
                });
            },
            error: function () {
                alert("خطا!");
            }
        });

    });

    $(document).on("click", ".RemoveCashPart", function () {

        var EditRow = $(this).closest('.card').attr('EditRow');
        if (typeof EditRow !== typeof undefined && EditRow !== false) {
            $(this).closest('.card').hide();
            var EditCashId = $("#EditCash_" + EditRow + "__id").val();
            var EditCashSecondId = $("#EditCash_" + EditRow + "__SecondId").val();
            var TotalAmout = $("#pay_TotalAmount").val();
            var Amount = $("#EditCash_" + EditRow + "__Amount").val().replace(/,/g, '');
            var re = +TotalAmout - +Amount;
            $("#pay_TotalAmount").val(re);
            Delete.push(EditCashId);
            Delete.push(EditCashSecondId);
            $("#deletedItem").val(Delete);
            var T = re.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#TAmount").text(T);
        }
        else {
            var Row = $(this).closest('.card').attr('Row');
            if (cashCount == Row) {
                cashCount = cashCount - 1;
                var Amount = $("#cheques_" + Row + "__Price").val().replace(/,/g, '');
                var TotalAmount = $("#pay_TotalAmount").val();
                TotalAmount = +TotalAmount - +Amount;
                $("#pay_TotalAmount").val(TotalAmount);
                var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                $("#TAmount").text(T);
                $(this).closest('.card').remove();
            }

        }


    });
    $(document).on("click", ".RemoveCardPart", function () {

        var EditRow = $(this).closest('.card').attr('EditRow');
        if (typeof EditRow !== typeof undefined && EditRow !== false) {
            $(this).closest('.card').hide();
            var EditCashId = $("#EditCard_" + EditRow + "__id").val();
            var EditCashSecondId = $("#EditCard_" + EditRow + "__SecondId").val();
            var TotalAmout = $("#pay_TotalAmount").val();
            var Amount = $("#EditCard_" + EditRow + "__Amount").val().replace(/,/g, '');
            var re = +TotalAmout - +Amount;
            $("#pay_TotalAmount").val(re);
            Delete.push(EditCashId);
            Delete.push(EditCashSecondId);
            $("#deletedItem").val(Delete);
            var T = re.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#TAmount").text(T);
        }
        else {
            var Row = $(this).closest('.card').attr('Row');
            if (CardCount == Row) {
                CardCount = CardCount - 1;
                var Amount = $("#Card_" + Row + "__Amount").val().replace(/,/g, '');
                var TotalAmount = $("#pay_TotalAmount").val();
                TotalAmount = +TotalAmount - +Amount;
                $("#pay_TotalAmount").val(TotalAmount);
                var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                $("#TAmount").text(T);
                $(this).closest('.card').remove();
            }
        }




    });
    $(document).on("click", ".RemoveChequePart", function () {

        var EditRow = $(this).closest('.card').attr('EditRow');
        if (typeof EditRow !== typeof undefined && EditRow !== false) {
            $(this).closest('.card').hide();
            var EditCashId = $("#Editcheques_" + EditRow + "__ChequeSanadId").val();
            var EditCashSecondId = $("#Editcheques_" + EditRow + "__ChequeSanadSecondId").val();
            var ChequeId = $("#Editcheques_" + EditRow + "__Id").val();
            var TotalAmout = $("#pay_TotalAmount").val();
            var Amount = $("#Editcheques_" + EditRow + "__Price").val().replace(/,/g, '');
            var re = +TotalAmout - +Amount;
            $("#pay_TotalAmount").val(re);
            Delete.push(EditCashId);
            Delete.push(EditCashSecondId);
            DeleteCheque.push(ChequeId);
            $("#deletedItem").val(Delete);
            $("#deteledCheque").val(DeleteCheque);
            var T = re.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#TAmount").text(T);

        }
        else {
            var Row = $(this).closest('.card').attr('Row');
            if (ChequeCount == Row) {

                ChequeCount = ChequeCount - 1;
                var Amount = $("#cheques_" + Row + "__Price").val().replace(/,/g, '');
                var TotalAmount = $("#pay_TotalAmount").val();
                TotalAmount = +TotalAmount - +Amount;
                $("#pay_TotalAmount").val(TotalAmount);
                var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                $("#TAmount").text(T);
                $(this).closest('.card').remove();
            }
        }

    });





    $(document).on("keyup", ".Chequeprice", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });

    });

    $(document).on("focusout", ".Chequeprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount + +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });
    $(document).on("focusin", ".Chequeprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount - +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });

    $(document).on("focusin", ".Cashprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount - +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });
    $(document).on("focusout", ".Cashprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount + +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });

    $(document).on("focusout", ".Cardprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount + +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });
    $(document).on("focusin", ".Cardprice", function () {
        var TotalAmount = $("#pay_TotalAmount").val();
        var cashAmout = $(this).val().replace(/,/g, '');
        TotalAmount = +TotalAmount - +cashAmout;
        $("#pay_TotalAmount").val(TotalAmount);
        var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        $("#TAmount").text(T);
    });
    $(".DatePicker").datepicker({
        beforeShow: function (input, inst) {
            var cal = inst.dpDiv;
            var left = $(this).offset().left;
            setTimeout(function () {
                cal.css({
                    'left': left
                });
            }, 10);
        }
    });

}

function updateTransfer() {


    //select2
    $(document).ready(function () {
        $("#FromCashDesk").select2();
        $("#ToCashDesk").select2();
        $("#FromBank").select2();
        $("#ToBank").select2();
    });

    //radio
    $(document).on("click", ".SourceRadioactive", function () {
        var val = $('input[name=SourceType]:checked').val();
        if (val == 0) {
            $("#FromCashDesk").removeAttr("disabled");
            setTimeout(function () {
                $("#DrpSourceCashDesk").slideDown();
            }, 50);
            $("#DrpSourceBank").slideUp();
            setTimeout(function () {
                $("#FromBank").attr("disabled", "disabled");
            }, 250);


        }
        else {

            $("#FromBank").removeAttr("disabled");
            setTimeout(function () {
                $("#DrpSourceBank").slideDown();
            }, 50);
            $("#DrpSourceCashDesk").slideUp();
            setTimeout(function () {
                $("#FromCashDesk").attr("disabled", "disabled");
            }, 250);


        }

    });

    $(document).on("click", ".DestRadioactive", function () {
        var val = $('input[name=DestType]:checked').val();
        if (val == 0) {

            $("#ToCashDesk").removeAttr("disabled");
            setTimeout(function () {
                $("#DrpDestCashDesk").slideDown();
            }, 50);
            $("#DrpDestBank").slideUp();
            setTimeout(function () {
                $("#ToBank").attr("disabled", "disabled");
            }, 250);
        }
        else {

            $("#ToBank").removeAttr("disabled");
            setTimeout(function () {
                $("#DrpDestBank").slideDown();
            }, 50);
            $("#DrpDestCashDesk").slideUp();
            setTimeout(function () {
                $("#ToCashDesk").attr("disabled", "disabled");
            }, 250);
        }

    });

    $(document).on("keyup", "#Amount", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });
    });

    $("#Date").datepicker({
        beforeShow: function (input, inst) {
            var cal = inst.dpDiv;
            var left = $(this).offset().left;
            setTimeout(function () {
                cal.css({
                    'left': left
                });
            }, 10);
        }
    });

    

    $(document).on("focusin", "#Description", function () {
        $("#Description").val("");
        $("#IsChanged").val("true");
    });

}