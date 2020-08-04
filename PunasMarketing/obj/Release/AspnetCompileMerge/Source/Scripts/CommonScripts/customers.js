function addCustomer() {
    $(document).on("change",
        "#Customer_ProvinceId",
        function () {

            $.ajax({
                url: "/Customer/GetCities",
                data: { id: $("#Customer_ProvinceId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpCities").html(result.Html);
                    if (result.Success) {

                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });

    $(document).on("change",
        "#Customer_CityId",
        function () {

            $.ajax({
                url: "/Customer/GetRegions",
                data: { id: $("#Customer_CityId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpRegions").html(result.Html);
                    if (result.Success) {

                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });
}

function updateCustomer() {
    $(document).on("change",
        "#Customer_ProvinceId",
        function () {
            $.ajax({
                url: "/Customer/GetCities",
                data: { id: $("#Customer_ProvinceId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpCities").html(result.Html);
                    if (result.Success) {

                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });

    $(document).on("change",
        "#Customer_CityId",
        function () {
            $.ajax({
                url: "/Customer/GetRegions",
                data: { id: $("#Customer_CityId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpRegions").html(result.Html);
                    if (result.Success) {

                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });
}

function changeCustomerPassword() {
    $(document).ready(function () {

        var txtPassword1 = $('#txtPassword1');
        var txtPassword2 = $('#txtPassword2');

        $('#btnSubmit').click(function () {
            if ($('#myForm').valid()) {
                if (txtPassword1.val() !== txtPassword2.val()) {
                    Toast("کلمه عبور و تکرار آن باید یکسان باشند.,info");
                }
                else {
                    var token = $("input[name = __RequestVerificationToken]").val();

                    $.ajax({
                        url: '/Customer/ChangePassword',
                        data:
                        {
                            __RequestVerificationToken: token,
                            CustomerId: $("#customerId").val(),
                            NewPassword: txtPassword1.val()
                        },
                        type: "Post",
                        dataType: "Json",
                        success: function (result) {
                            if (result.Success) {
                                $('#modal').modal('toggle');

                                swal("کلمه عبور با موفقیت تغییر یافت.",
                                    {
                                        icon: "success",
                                        button: "تایید"
                                    });
                            } else {
                                $('#modal').modal('toggle');

                                swal("خطا در تغییر کلمه عبور!", {
                                    icon: "error",
                                    button: "تایید"
                                });
                            }
                        }

                    });
                }
            }

            return false;
        });
    });
}

function customerManagement() {
    var customerId = $("#customerId").val();

    $(document).on("click", "a#SanadDetail", function () {
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

    $(document).on("click", "#btnActivate", function () {
        swal({
            title: "فعالسازی حساب کاربری",
            text: "آیا می خواهید حساب کاربری را فعال کنید؟",
            icon: "warning",
            buttons: true,
            dangerMode: true,
            buttons: ['انصراف', 'تأیید']
        })
            .then((willDelete) => {
                if (willDelete) {
                    var token = $("input[name = __RequestVerificationToken]").val();

                    $.post("/Customer/Activate/" + customerId,
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

                    $.post("/Customer/Disable/" + customerId,
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
        $.get("/Customer/ChangePassword/" + customerId,
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