function AddSanad() {

    var index = 0;
    var SanadItem = [];
    $(document).on("ready", function () {
        $("#Sarfasl").select2();
        $("#Tafsili").select2();

    });
    $(document).on("change", "#Sarfasl", function () {
        var value = $("#Sarfasl").find(":selected").val();
        if (value != 0) {
            $.ajax({
                url: "/Sanad/GetTafsili",
                data: { SarfaslId: value },
                type: "Post",
                dataType: "Json",
                success: function (result) {

                    $("#SanadItemTable tr td#DrpTafsili").html(result.Html);
                    $("#Tafsili").select2();
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }

    });

    $(document).on("keyup", "#BedAmount", function () {
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

    $(document).on("focusout", "#BedAmount", function () {
        var value = $(this).val().replace(/,/g, '');
        if (value != "" || value != 0) {
            $("#BesAmount").attr("disabled", "disabled");
        }
        else {
            $("#BesAmount").removeAttr("disabled");
        }
        //var cashAmout = $(this).val().replace(/,/g, '');
        //TotalAmount = +TotalAmount + +cashAmout;
        //$("#pay_TotalAmount").val(TotalAmount);
        //var T = TotalAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        //$("#TAmount").text(T);
    });
    $(document).on("keyup", "#BesAmount", function () {
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

    $(document).on("focusout", "#BesAmount", function () {
        var value = $(this).val().replace(/,/g, '');
        if (value != "" || value != 0) {
            $("#BedAmount").attr("disabled", "disabled");
        }
        else {
            $("#BedAmount").removeAttr("disabled");
        }
    });

    $(document).on("click", "#SanadItemTable tr td a#AddItem", function () {

        var Sarfasl = $("#Sarfasl").find(":selected").val();
        var Tafsili = $("#Tafsili").find(":selected").val();
        var Description = $("#Description").val();
        var BedehAmount = $("#BedAmount").val();
        var BestanAmount = $("#BesAmount").val();
        if (Sarfasl == "") {
            Toast("سرفصل را انتخاب کنید,info");
        }
        else if (Description == "") {
            Toast("توضیح را وارد کنید,info");
        }
        else if (BedehAmount == "" && BestanAmount == "") {
            Toast("مقدار بدهکار یا بستانکار را وارد کنید,info");
        }
        else {

            index++;
            var item = {
                'index': index,
                'ItemId': 0,
                'Sarfasl': Sarfasl,
                'Tafsili': Tafsili,
                'Description': Description,
                'BedehAmount': BedehAmount.replace(/,/g, ''),
                'BestanAmount': BestanAmount.replace(/,/g, '')
            };

            var SarfaslCode = $("#Sarfasl").find(":selected").text().split('-')[0];
            var SarfaslText = $("#Sarfasl").find(":selected").text().split('-')[1];
            var tafsiliText = "";
            if (Tafsili != "")
                var tafsiliText = $("#Tafsili").find(":selected").text();
            SanadItem.push(item);
            $("#SanadItemTable").append('<tr class="columnSize"><td style="width:5%;text-align:center">' + SarfaslCode + '</td><td style="width:5%;text-align:center">' + SarfaslText + '</td> <td style="width:20%;text-align:center">' + tafsiliText + '</td> <td style="width:25%;text-align:center">' + Description + '</td><td style="width:15%;text-align:center">' + BedehAmount + '</td><td style="width:15%;text-align:center">' + BestanAmount + '</td><td style="text-align:center"><a id="deleteItem" style="cursor:pointer" class="icon icon-trash-o iconColor" value="' + index + '"></a></td></tr>');

            $("#Sarfasl").prop('selectedIndex', 1);
            $("#Tafsili").prop('selectedIndex', 0);
            $("#BedAmount").val("");
            $("#BesAmount").val("");
            $("#Description").val("");

            $("#BedAmount").removeAttr("disabled");
            $("#BesAmount").removeAttr("disabled");


            BedehAmount = BedehAmount.replace(/,/g, '');
            BestanAmount = BestanAmount.replace(/,/g, '');
            var TotalBed = $("#TotalBedeh").text().replace(/,/g, '');
            var TotalBes = $("#TotalBestan").text().replace(/,/g, '');
            var TotalDiff = $("#TotalDifference").text().replace(/,/g, '');
            TotalBed = +TotalBed + +BedehAmount;
            TotalBes = +TotalBes + +BestanAmount;
            TotalDiff = +TotalBed - +TotalBes;
            TotalDiff = Math.abs(TotalDiff);
            $("#TotalBedeh").text(TotalBed.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
            $("#TotalBestan").text(TotalBes.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
            $("#TotalDifference").text(TotalDiff.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        }
    });

    $(document).on("click", "#SanadItemTable tr td a#deleteItem", function () {

        var BedehAmount = $(this).closest('tr').find('td:eq(4)').text().replace(/,/g, '');
        var BestanAmount = $(this).closest('tr').find('td:eq(5)').text().replace(/,/g, '');
        var TotalBed = $("#TotalBedeh").text().replace(/,/g, '');
        var TotalBes = $("#TotalBestan").text().replace(/,/g, '');
        var TotalDiff = $("#TotalDifference").text().replace(/,/g, '');
        TotalBed = +TotalBed - +BedehAmount;
        TotalBes = +TotalBes - +BestanAmount;
        TotalDiff = +TotalBed - +TotalBes;
        TotalDiff = Math.abs(TotalDiff);
        $("#TotalBedeh").text(TotalBed.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        $("#TotalBestan").text(TotalBes.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        $("#TotalDifference").text(TotalDiff.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        var ItemId = $(this).attr("value");
        var theItem = SanadItem.find(x => x.index == ItemId);
        SanadItem.splice(SanadItem.indexOf(theItem), 1);
        $(this).closest('tr').remove();







    });
    $(document).on("click", "#AddSanad", function () {
        var SanadDes = $("#SanadDes").val();
        var SanadDate = $("#SanadDate").val();
        var TotalBed = $("#TotalBedeh").text().replace(/,/g, '');

        var TotalBes = $("#TotalBestan").text().replace(/,/g, '');

        if (SanadDes == "") {
            Toast("شرح سند را وارد نمایید,info");
        }
        else if (SanadDate == "") {
            Toast("تاریخ سند را وارد نمایید,info");
        }
        else if (SanadItem.length == 0) {
            Toast("اطلاعاتی برای سند ثبت نشده است,info");
        }
        else if (+TotalBed != +TotalBes) {
            Toast("سند تراز نیست. جمع بدهکار و بستانکار برابر نمی باشد,info");
        }
        else {
            $.ajax({
                url: "/Sanad/AddSanad",
                data: { SanadItem:SanadItem, SanadDes:SanadDes, SanadDate:SanadDate},
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    if (result.Success) {
                        window.location.href = result.Url;
                    }
                    else {
                        Toast(result.Script)
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }

    });

    $("#SanadDate").datepicker({
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

function ModaldeleteSanad() {
    $(document).ready(function () {
        $('#btnDelete').click(function () {

            var id = $(this).attr('value');

            swal({
                title: "آیا از حذف این سند مطمئن هستید؟",
                text: "بعد از حذف، امکان بازیابی اطلاعات وجود ندارد!",
                icon: "warning",
                buttons: true,
                dangerMode: true,
                buttons: ['انصراف', 'حذف']
            })
                .then((willDelete) => {
                    if (willDelete) {
                        var token = $("input[name = __RequestVerificationToken]").val();
                        $.post("/Sanad/DeleteSanad/" + id,
                            {
                                __RequestVerificationToken: token
                            },
                            function (result) {
                                if (result.Success) {
                                    swal("عملیات با موفقیت انجام شد.",
                                        {
                                            icon: "success",
                                            button: "تایید"
                                        });
                                    window.location.href = '/Sanad/Sanads/';

                                } else {
                                    swal("خطایی در انجام عملیات رخ داد . دوباره تلاش کنید.",
                                        {
                                            icon: "error",
                                            button: "تایید"
                                        });
                                }
                            });

                    }

                });
        });
    });
}

function AddsnadDasti() {
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

function DeleteSanad() {
    $(document).on("click", "#sanadsList tr td a#Delete", function () {
        var Row = $(this);
        var id = $(this).attr('value');

        swal({
            title: "آیا مطمئن هستید؟",
            text: "بعد از حذف ، امکان بازیابی اطلاعات وجود ندارد !",
            icon: "warning",
            buttons: true,
            dangerMode: true,
            buttons: ['انصراف', 'حذف']
        })
            .then((willDelete) => {
                if (willDelete) {
                    var token = $("input[name = __RequestVerificationToken]").val();
                    $.post("/Transaction/DeleteTransaction/" + id,
                        {
                            __RequestVerificationToken: token
                        },

                        function (result) {
                            if (result.Success) {
                                swal("عملیات با موفقیت انجام شد.",
                                    {
                                        icon: "success",
                                        button: "تایید"
                                    });
                                $(Row).fadeTo(400, 0, function () {
                                    Row.closest('tr').remove();
                                });

                            }
                            else {
                                swal("خطایی در انجام عملیات رخ داد . دوباره تلاش کنید.", {
                                    icon: "error",
                                    button: "تایید"
                                });
                            }
                        });

                }

            });

    });
}

function sanads() {
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

function updateSanad() {

    var index = 0;
    var SanadItem = [];
    var SanadItemDeleted = [];
    $(document).on("ready", function () {
        $("#Sarfasl").select2();
        $("#Tafsili").select2();

    });
    $(document).on("change", "#Sarfasl", function () {
        var value = $("#Sarfasl").find(":selected").val();
        if (value != 0) {
            $.ajax({
                url: "/Sanad/GetTafsili",
                data: { SarfaslId: value },
                type: "Post",
                dataType: "Json",
                success: function (result) {

                    $("#SanadItemTable tr td#DrpTafsili").html(result.Html);
                    $("#Tafsili").select2();
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }

    });

    $(document).on("keyup", "#BedAmount", function () {
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

    $(document).on("focusout", "#BedAmount", function () {
        var value = $(this).val().replace(/,/g, '');
        if (value != "" || value != 0) {
            $("#BesAmount").attr("disabled", "disabled");
        }
        else {
            $("#BesAmount").removeAttr("disabled");
        }
       
    });
    $(document).on("keyup", "#BesAmount", function () {
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

    $(document).on("focusout", "#BesAmount", function () {
        var value = $(this).val().replace(/,/g, '');
        if (value != "" || value != 0) {
            $("#BedAmount").attr("disabled", "disabled");
        }
        else {
            $("#BedAmount").removeAttr("disabled");
        }
    });

    $(document).on("click", "#SanadItemTable tr td a#AddItem", function () {

        var Sarfasl = $("#Sarfasl").find(":selected").val();
        var Tafsili = $("#Tafsili").find(":selected").val();
        var Description = $("#Description").val();
        var BedehAmount = $("#BedAmount").val();
        var BestanAmount = $("#BesAmount").val();
        if (Sarfasl == "") {
            Toast("سرفصل را انتخاب کنید,info");
        }
        else if (Description == "") {
            Toast("توضیح را وارد کنید,info");
        }
        else if (BedehAmount == "" && BestanAmount == "") {
            Toast("مقدار بدهکار یا بستانکار را وارد کنید,info");
        }
        else {

            index++;
            var item = {
                'index': index,
                'ItemId': 0,
                'Sarfasl': Sarfasl,
                'Tafsili': Tafsili,
                'Description': Description,
                'BedehAmount': BedehAmount.replace(/,/g, ''),
                'BestanAmount': BestanAmount.replace(/,/g, '')
            };

            var SarfaslCode = $("#Sarfasl").find(":selected").text().split('-')[0];
            var SarfaslText = $("#Sarfasl").find(":selected").text().split('-')[1];
            var tafsiliText = "";
            if (Tafsili != "")
                var tafsiliText = $("#Tafsili").find(":selected").text();
            SanadItem.push(item);
            $("#SanadItemTable").append('<tr class="columnSize"><td style="width:5%;text-align:center">' + SarfaslCode + '</td><td style="width:5%;text-align:center">' + SarfaslText + '</td> <td style="width:20%;text-align:center">' + tafsiliText + '</td> <td style="width:25%;text-align:center">' + Description + '</td><td style="width:15%;text-align:center">' + BedehAmount + '</td><td style="width:15%;text-align:center">' + BestanAmount + '</td><td style="text-align:center"><a id="deleteItem" style="cursor:pointer" class="icon icon-trash-o iconColor" value="' + index + '"></a></td></tr>');

            $("#Sarfasl").prop('selectedIndex', 1);
            $("#Tafsili").prop('selectedIndex', 0);
            $("#BedAmount").val("");
            $("#BesAmount").val("");
            $("#Description").val("");

            $("#BedAmount").removeAttr("disabled");
            $("#BesAmount").removeAttr("disabled");


            BedehAmount = BedehAmount.replace(/,/g, '');
            BestanAmount = BestanAmount.replace(/,/g, '');
            var TotalBed = $("#TotalBedeh").text().replace(/,/g, '');
            var TotalBes = $("#TotalBestan").text().replace(/,/g, '');
            var TotalDiff = $("#TotalDifference").text().replace(/,/g, '');
            TotalBed = +TotalBed + +BedehAmount;
            TotalBes = +TotalBes + +BestanAmount;
            TotalDiff = +TotalBed - +TotalBes;
            TotalDiff = Math.abs(TotalDiff);
            $("#TotalBedeh").text(TotalBed.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
            $("#TotalBestan").text(TotalBes.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
            $("#TotalDifference").text(TotalDiff.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        }
    });

    $(document).on("click", "#SanadItemTable tr td a#deleteItem", function () {

        var SanadItemIdRow = $(this).attr('SanadItemId');
        if (typeof SanadItemIdRow !== typeof undefined && SanadItemIdRow !== false) {
            SanadItemDeleted.push(SanadItemIdRow);
            $("#SanadItemDeleted").val(SanadItemDeleted);
        }

        var BedehAmount = $(this).closest('tr').find('td:eq(4)').text().replace(/,/g, '');
        var BestanAmount = $(this).closest('tr').find('td:eq(5)').text().replace(/,/g, '');
        var TotalBed = $("#TotalBedeh").text().replace(/,/g, '');
        var TotalBes = $("#TotalBestan").text().replace(/,/g, '');
        var TotalDiff = $("#TotalDifference").text().replace(/,/g, '');
        TotalBed = +TotalBed - +BedehAmount;
        TotalBes = +TotalBes - +BestanAmount;
        TotalDiff = +TotalBed - +TotalBes;
        TotalDiff = Math.abs(TotalDiff);
        $("#TotalBedeh").text(TotalBed.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        $("#TotalBestan").text(TotalBes.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        $("#TotalDifference").text(TotalDiff.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        var ItemId = $(this).attr("value");
        var theItem = SanadItem.find(x => x.index == ItemId);
        SanadItem.splice(SanadItem.indexOf(theItem), 1);
        $(this).closest('tr').remove();







    });

    $(document).on("click", "#EditSanad", function () {
        var SanadDes = $("#SanadDes").val();
        var SanadDate = $("#SanadDate").val();
        var TotalBed = $("#TotalBedeh").text().replace(/,/g, '');

        var TotalBes = $("#TotalBestan").text().replace(/,/g, '');
        var DeletedItem = $("#SanadItemDeleted").val();
        var sanadId = $("#SanadId").val();
        if (SanadDes == "") {
            Toast("شرح سند را وارد نمایید,info");
        }
        else if (SanadDate == "") {
            Toast("تاریخ سند را وارد نمایید,info");
        }
        else if (SanadItem.length == 0) {
            Toast("اطلاعاتی برای سند ثبت نشده است,info");
        }
        else if (+TotalBed != +TotalBes) {
            Toast("سند تراز نیست. جمع بدهکار و بستانکار برابر نمی باشد,info");
        }
        else {
            $.ajax({
                url: "/Sanad/UpdateSanad",
                data: { SanadItem: SanadItem, SanadDes: SanadDes, SanadDate: SanadDate, SanadItemDeleted: DeletedItem, SanadId: sanadId },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    if (result.Success) {
                        window.location.href = result.Url;
                    }
                    else {
                        Toast(result.Script)
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }

    });

    $("#SanadDate").datepicker({
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

    //---------UpdatePart----------------
    $(document).on("ready", function () {
        $('#SanadItemTable > tbody#CurrentTbody  > tr').each(function () {

            var ind = $(this).find('td:eq(6) a#deleteItem').attr("value");
            var ItemId = $(this).find('td:eq(6) a#deleteItem').attr("SanadItemId");
            var Sarfasl = $(this).find('td:eq(1)').attr("SarfaslId");
            var Tafsili = $(this).find('td:eq(2)').attr("TafisiliId");
            var Des = $(this).find('td:eq(3)').text().trim();
            var bedeh = $(this).find('td:eq(4)').text();
            var bestan = $(this).find('td:eq(5)').text();

            var item = {
                'index': ind,
                'ItemId': ItemId,
                'Sarfasl': Sarfasl,
                'Tafsili': Tafsili,
                'Description': Des,
                'BedehAmount': bedeh.replace(/,/g, ''),
                'BestanAmount': bestan.replace(/,/g, '')
            };
            SanadItem.push(item);
            index = ind
        });

    });


}