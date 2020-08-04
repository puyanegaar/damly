function journalReport() {
    var CurrentDetail = 0;

    $(document).on("ready", function () {
        $("#FromDate").datepicker({
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

        $("#ToDate").datepicker({
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


        var dataTable = $("#JournalList").dataTable({
            "bInfo": false,
            "bLengthChange": false,
            "bSort": false,
            "language": {
                "paginate": {
                    "previous": "قبلی",
                    "next": "بعدی"
                }
            },
            "iDisplayLength": 20
        });
        $("#SearchJurnalReport").keyup(function () {
            dataTable.fnFilter(this.value);
        });

    });

    $(document).on("click", "#JournalList td a#SanadDetail", function () {
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
                    alert("خطا!");
                }
            });
        }
    });
}

function journalTotalAccountsReport() {
    var CurrentDetail = 0;

    $(document).on("ready", function () {
        $("#FromDate").datepicker({
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

        $("#ToDate").datepicker({
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


        var dataTable = $("#JournalTAList").dataTable({
            "bInfo": false,
            "bLengthChange": false,
            "bSort": false,
            "language": {
                "paginate": {
                    "previous": "قبلی",
                    "next": "بعدی"
                }
            },
            "iDisplayLength": 20
        });
        $("#SearchJurnalReport").keyup(function () {
            dataTable.fnFilter(this.value);
        });

    });
    $(document).on("click", "#JournalTAList td a#SanadDetail", function () {
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
                    alert("خطا!");
                }
            });
        }
    });
}

function successJournal() {
	var dataTable = $("#JournalList").dataTable({
		"bInfo": false,
		"bLengthChange": false,
		"bSort": false,
		"language": {
			"paginate": {
				"previous": "قبلی",
				"next": "بعدی"
			}
		},
		"iDisplayLength": 20
	});
	$("#SearchJurnalReport").keyup(function () {
		dataTable.fnFilter(this.value);
	});
}

function ledgerReport() {
    var SarfalName;
    var SarfaslType;
    var ParentId = 0;
    var value = 0;
    var TafsiliValue = 0;
    var CurrentDetail = 0;
    $(document).on("ready", function () {
        $("#StartDate").datepicker({
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
            changeMonth: true,
        });
    });
    $(document).on("click", "#SelectHesab", function (e) {
        e.preventDefault();

        $("#modalSelectHesab").modal({ backdrop: true });
    });
    $(document).on("click", "#selType li span a#SarfaslName", function () {
        SarfalName = $(this).attr("SarfaslName");
        SarfaslType = +$(this).attr("SarfaslType");
        ParentId = +$(this).attr("parentid");
        if (ParentId == 0) {
            Toast("لطفا یک حساب کل یا معین انتخاب نمایید!,info");
        }
        if (ParentId != 0 || SarfaslType == 1) {
            value = $(this).attr("value");
            $("#selectedhesab").text(SarfalName);
        }
        if (SarfaslType == 1) {
            $.ajax({
                url: "/Reports/GetTafsiliList",
                data: { SarfaslId: value, SarfaslName: SarfalName },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#TafsiliPart").html(result.Html);
                    var dataTable = $("#TafsiliResultTable").dataTable({
                        "bInfo": false,
                        "bLengthChange": false,
                        "bSort": false,
                        "language": {
                            "paginate": {
                                "previous": "قبلی",
                                "next": "بعدی"
                            }
                        },
                        "iDisplayLength": 15
                    });
                    $("#SearchTafsili").keyup(function () {
                        dataTable.fnFilter(this.value);
                    });
                },
                error: function () {
                    alert("خطا!");
                }
            });
        }
    });
    $(document).on("click", "#selType li span a#SarfaslName", function () { // when li is clicked
        if (ParentId != 0) {

            $(this).addClass("sel")
            $(this).parents().siblings().find("a").removeClass("sel"); // swap class

        }


    });
    $(document).on("click", "#TafsiliResultTable tr", function () {
        TafsiliValue = $(this).attr("value");
        $("#selectedhesab").text($(this).find('td:eq(1)').text().trim());
        SarfalName = $(this).find('td:eq(1)').text().trim();
    });


    $(document).on("click", "#ConfirmHesab", function () {

        var type = 0;
        if (ParentId != 0 && SarfaslType == 0) {
            type = 1;
        }
        else if (ParentId != 0 && SarfaslType == 1) {
            type = 2;
            if (TafsiliValue != 0) {
                type = 3;
            }
        }

        if (ParentId != 0) {
            $("#type").val(type);
            $("#SarfaslId").val(value);
            $("#TafsiliId").val(TafsiliValue);
            $("#SelectHesab").val(SarfalName);
            $("#modalSelectHesab").modal('hide');
            //ParentId = 0;
            //value = 0;
            //TafsiliValue = 0;
        }
        else {
            Toast("لطفا یک حساب کل معین یا تفضیلی را انتخاب نمایید,info");
        }

    });

    $(document).on("click", "#GetLedgerReport", function (e) {
        e.preventDefault();
       
        if ($("#type").val() != 0 && $("#StartDate").val() != "" && $("#EndDate").val() != "" && ($("#SarfaslId").val() != 0 || $("#TafsiliId").val() != 0)) {
            alert();
            $("#LedgerReportFrom").submit();

        }
        else {
            Toast("لطفا یک حساب کل معین یا تفضیلی را انتخاب نمایید,info");
        }

    });


    $(document).on("click", "#LedgerList td a#SanadDetail", function () {
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
                    alert("خطا!");
                }
            });
        }
    });
}

function successJournalTA() {
	var dataTable = $("#JournalTAList").dataTable({
		"bInfo": false,
		"bLengthChange": false,
		"bSort": false,
		"language": {
			"paginate": {
				"previous": "قبلی",
				"next": "بعدی"
			}
		},
		"iDisplayLength": 20
	});
	$("#SearchJurnalReport").keyup(function () {
		dataTable.fnFilter(this.value);
	});
}

function LedgerReportSuccess(data) {
	var dataTable = $("#LedgerList").dataTable({
		"bInfo": false,
		"bLengthChange": false,
		"bSort": false,
		"language": {
			"paginate": {
				"previous": "قبلی",
				"next": "بعدی"
			}
		},
		"iDisplayLength": 20
	});
	$("#SearchLedgerReport").keyup(function () {
		dataTable.fnFilter(this.value);
    });
    if (data.Message) {
        Toast(data.Message)
    }
}