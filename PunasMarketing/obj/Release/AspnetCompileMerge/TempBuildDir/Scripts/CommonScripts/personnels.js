function addPersonnel() {
    $("#birthDate").datepicker({
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
        changeMonth: true,
    });

    $("#hireDate").datepicker({
        beforeShow: function (input, inst) {
            var cal = inst.dpDiv;
            var left = $(this).offset().left;
            setTimeout(function () {
                cal.css({
                    'left': left
                });
            },
                10);
        }
    });

    $(document).on("change",
        "#personnel_BirthStateId",
        function () {
            $.ajax({
                url: "/Personnel/GetBirthCities",
                data: { id: $("#personnel_BirthStateId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpBirthCities").html(result.Html);
                    if (result.Success) {
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });

    $(document).on("change",
        "#personnel_HomeStateId",
        function () {
            $.ajax({
                url: "/Personnel/GetHomeCities",
                data: { id: $("#personnel_HomeStateId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpHomeCities").html(result.Html);
                    if (result.Success) {
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });

    $(document).on("change",
        "#personnel_SectionId",
        function () {
            $.ajax({
                url: "/Personnel/GetJobTilte",
                data: { id: $("#personnel_SectionId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpJobTitleId").html(result.Html);
                    if (result.Success) {
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });

    $(document).ready(function () {

        var drpMaritialStatus = $('#personnel_IsMarried');
        var drpGender = $('#personnel_IsMale');
        var maritialStatusError = $('#maritialStatusError');
        var genderError = $('#genderError');

        $('#btnSubmit').on('click',
            function () {
                if (drpGender.prop('selectedIndex') === 0) {
                    genderError.show();

                    return false;
                } else {
                    genderError.hide();
                }

                if (drpMaritialStatus.prop('selectedIndex') === 0) {
                    maritialStatusError.show();

                    return false;
                } else {
                    maritialStatusError.hide();
                }
            });

        $(document).on("change",
            "#personnel_IsMarried",
            function () {
                if (drpMaritialStatus.prop('selectedIndex') === 0) {
                    maritialStatusError.show();
                } else {
                    maritialStatusError.hide();
                }
            });

        $(document).on("change",
            "#personnel_IsMale",
            function () {
                if (drpGender.prop('selectedIndex') === 0) {
                    genderError.show();
                } else {
                    genderError.hide();
                }
            });
    });
}

function updatePersonnel() {
    $('#btnClear').click(function (e) {
        e.preventDefault();

        $('#fireDate').val('');
    });


    $("#birthDate").datepicker({
        beforeShow: function (input, inst) {
            var cal = inst.dpDiv;
            var left = $(this).offset().left;
            setTimeout(function () {
                cal.css({
                    'left': left
                });
            }, 10);
        },
        changeYear: true,
        yearRange: '1350:1420',
        changeMonth: true,
    });

    $("#hireDate").datepicker({
        beforeShow: function (input, inst) {
            var cal = inst.dpDiv;
            var left = $(this).offset().left;
            setTimeout(function () {
                cal.css({
                    'left': left
                });
            }, 10);
        },
        changeYear: true,
        yearRange: '1350:1420',
        changeMonth: true,
    });

    $("#fireDate").datepicker({
        beforeShow: function (input, inst) {
            var cal = inst.dpDiv;
            var left = $(this).offset().left;
            setTimeout(function () {
                cal.css({
                    'left': left
                });
            }, 10);
        },
        changeYear: true,
        yearRange: '1350:1420',
        changeMonth: true,
    });

    $(document).on("change",
        "#personnel_BirthStateId",
        function () {
            $.ajax({
                url: "/Personnel/GetBirthCities",
                data: { id: $("#personnel_BirthStateId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpBirthCities").html(result.Html);
                    if (result.Success) {
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });

    $(document).on("change",
        "#personnel_HomeStateId",
        function () {
            $.ajax({
                url: "/Personnel/GetHomeCities",
                data: { id: $("#personnel_HomeStateId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpHomeCities").html(result.Html);
                    if (result.Success) {
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });

    $(document).on("change",
        "#personnel_SectionId",
        function () {
            $.ajax({
                url: "/Personnel/GetJobTilte",
                data: { id: $("#personnel_SectionId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpJobTitleId").html(result.Html);
                    if (result.Success) {
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });

    $(document).ready(function () {

        var drpMaritialStatus = $('#personnel_IsMarried');
        var drpGender = $('#personnel_IsMale');
        var maritialStatusError = $('#maritialStatusError');
        var genderError = $('#genderError');

        $('#btnSubmit').on('click',
            function () {
                if (drpGender.prop('selectedIndex') === 0) {
                    genderError.show();

                    return false;
                } else {
                    genderError.hide();
                }

                if (drpMaritialStatus.prop('selectedIndex') === 0) {
                    maritialStatusError.show();

                    return false;
                } else {
                    maritialStatusError.hide();
                }
            });

        $(document).on("change",
            "#personnel_IsMarried",
            function () {
                if (drpMaritialStatus.prop('selectedIndex') === 0) {
                    maritialStatusError.show();
                } else {
                    maritialStatusError.hide();
                }
            });

        $(document).on("change",
            "#personnel_IsMale",
            function () {
                if (drpGender.prop('selectedIndex') === 0) {
                    genderError.show();
                } else {
                    genderError.hide();
                }
            });
    });

}

function personnelManagement() {
	var personnelId = $('#personnelId').val();

    $(document).on("change",
        "#ProvinceId",
        function () {
            $.ajax({
                url: "/Customer/GetCities",
                data: { id: $("#ProvinceId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpCities").html(result.Html);
                    if (result.Success) {

                    }
                },
                error: function () {
                    
                }
            });
        });

    $(document).on("change",
        "#Customer_CityId",
        function () {

            $.ajax({
                url: "/Marketer/GetRegions",
                data: { cityId: $("#Customer_CityId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("select#demo-select2-2").html(result.Html);
                    if (result.Success) {

                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });


    $(document).on("click",
        "a#SanadDetail",
        function () {
            var value = $(this).attr("value");
            $.ajax({
                url: "/Sanad/SanadDetail",
                data: { SanadId: value },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#SanadNum").text(value);
                    $("#SanadDetailList").html(result.Html);
                    $("#modalSanadDetail").modal({ backdrop: true });
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });

    var Items = [];
    $(document).on("change",
        "#SectionId",
        function () {
            $.ajax({
                url: "/Personnel/GetActionGroup",
                data: { id: $("#SectionId").find(":selected").val(), UserId: $("#UserId").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#ActionGroup").html(result.Html);
                    if (result.Success) {
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });

    $(document).on("click",
        "#AddAccess",
        function (e) {
            e.preventDefault();
            Items.push(0);
            $("[Identity$='AccessItem'").each(function () {
                var value = $(this).val();
                if ($(this).is(":checked")) {
                    Items.push(value);
                }
            });
            $.ajax({
                url: "/Personnel/AccessDefine",
                data: { UserId: $("#UserId").val(), AccessProperty: Items, Section: $("#SectionGroup").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    Toast(result.Script);
                    Items = [];
                },
            });
        });

    var RegionsName = [];
    $(document).on("click",
        "#btnAssignRegions",
        function (e) {
            e.preventDefault();
            $("li.select2-selection__choice").each(function () {
                var title = $(this).attr('title');
                RegionsName.push(title);
            });
            var token = $("input[name = __RequestVerificationToken]").val();
            $.ajax({
                url: '/Marketer/AssignRegionsForMarketer/',
                data: {
                    __RequestVerificationToken: token,
                    MarketerId: personnelId,
                    RegionsName: RegionsName
                },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    Toast(result.Script);
                    RegionsName = [];
                }
            });
        });

    $(document).on("click",
        "#btnActivate",
        function () {
            swal({
                title: "فعالسازی حساب کاربری",
                text: "آیا می خواهید حساب کاربری را فعال کنید؟",
                icon: "warning",
                buttons: true,
                dangerMode: true,
                buttons: ['انصراف', 'تأیید']
            }).then((willDelete) => {
                if (willDelete) {
                    var token = $("input[name = __RequestVerificationToken]").val();
                    $.post("/Personnel/Activate/" + personnelId,
                        {
                            __RequestVerificationToken: token
                        },
                        function (result) {
                            if (result.Success) {
                                swal("حساب کاربری با موفقیت فعال شد.",
                                    {
                                        icon: "success",
                                        button: "تایید"
                                    });

                                $('#divActivateDisable').html("<input id='btnDisable' class='btn btn-outline-primary' type='button' value='غیرفعالسازی حساب کاربری' />");
                                $('#lblActivateDisable').html("فعال");
                            }
                            else {
                                swal("خطا در فعالسازی حساب کاربری!", {
                                    icon: "error",
                                    button: "تایید"
                                });
                            }
                        });

                }
            });
        });

    $(document).on("click", "#btnDisable", function () {
        swal({
            title: "غیرفعالسازی حساب کاربری",
            text: "آیا می خواهید حساب کاربری را غیرفعال کنید؟",
            icon: "warning",
            buttons: true,
            dangerMode: true,
            buttons: ['انصراف', 'تأیید']
        })
            .then((willDelete) => {
                if (willDelete) {
                    var token = $("input[name = __RequestVerificationToken]").val();
                    $.post("/Personnel/Disable/" + personnelId,
                        {
                            __RequestVerificationToken: token
                        },
                        function (result) {
                            if (result.Success) {
                                swal("حساب کاربری با موفقیت غیرفعال شد.",
                                    {
                                        icon: "success",
                                        button: "تایید"
                                    });

                                $('#divActivateDisable').html("<input id='btnActivate' class='btn btn-outline-success' type='button' value='فعالسازی حساب کاربری' />");
                                $('#lblActivateDisable').html("غیرفعال");
                            }
                            else {
                                swal("خطا در غیرفعالسازی حساب کاربری!", {
                                    icon: "error",
                                    button: "تایید"
                                });
                            }
                        });
                }
            });
    });

    $("#btnChangePassword").click(function () {
	    $.get("/Personnel/ChangePassword/" + personnelId,
		    function (res) {
			    $("#modal").modal();
			    $("#modalBody").html(res);
		    });
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