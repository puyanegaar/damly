function orderDetails() {
    $(document).ready(function () {
        var orderId = $('#orderId').val();

        //var btnVerify = $('#btnVerify');
        var theRedTr = $('.wrongTd').closest('tr');

        theRedTr.addClass('deleted');

        if (theRedTr.length > 0) {
            $('#countError').show();
            //btnVerify.attr("disabled", true);
        }

        $('#btnFactor').click(function () {
            $.ajax({
                url: "/Factor/FactorForOrder/" + orderId,
                type: "Get",
                success: function (res) {
                    $('#orderModal').modal('toggle'); // close current order modal

                    $('#autoFactorModal').modal();
                    $('#autoFactorModalBody').html(res);
                }
            });
        });

        $('#btnVerify').click(function () {
            swal({
                title: "آیا از تأیید این سفارش مطمئن هستید؟",
                icon: "warning",
                buttons: true,
                dangerMode: true,
                buttons: ['انصراف', 'تأیید']
            }).then((willDelete) => {
                if (willDelete) {
                    var itemsDic = [];

                    var $inputs = jQuery('#OrderList :input[type=number]');
                    $inputs.each(function () {
                        var item = { Key: $(this).attr('key'), Value: $(this).val() };
                        itemsDic.push(item);
                    });

                    var token = $("input[name = __RequestVerificationToken]").val();
                    $.post("/Order/Verify/",
                        {
                            __RequestVerificationToken: token,
                            id: orderId,
                            items: itemsDic
                        },
                        function (result) {
                            if (result.Success) {
                                swal("سفارش با موفقیت تأیید شد.",
                                    {
                                        icon: "success",
                                        button: "تایید"
                                    });

                                $('#verifyStatus_' + orderId).html('<td id="verifyStatus_' + orderId + '" style="color: green">تأیید شده</td>');
                                $('#orderModal').modal('toggle');

                            } else {
                                var message = result.Message;
                                swal(message,
                                    {
                                        icon: "error",
                                        button: "تایید"
                                    });
                            }
                        });
                }
            });
        });

        $('#btnUnverify').click(function () {
            swal({
                title: "آیا از عدم تأیید این سفارش مطمئن هستید؟",
                icon: "warning",
                buttons: true,
                dangerMode: true,
                buttons: ['انصراف', 'تأیید']
            })
                .then((willDelete) => {
                    if (willDelete) {
                        var token = $("input[name = __RequestVerificationToken]").val();
                        $.post("/Order/Unverify/" + orderId,
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

                                    $('#verifyStatus_' + orderId).html('<td id="verifyStatus_' + orderId + '" style="color: darkorange">تأیید نشده</td>');
                                    $('#orderModal').modal('toggle');

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

function allOrders() {
    $(document).on("click",
        "#orderDetails",
        function () {
            var value = $(this).attr("value");

            $.get("/Order/OrderDetail/" + value,
                function (res) {
                    $("#orderModal").modal();
                    $("#orderNum").text(value);
                    $("#modalBody").html(res);
                });
        });
}

function ShowFactor(factorId, periodId) {
    $.ajax({
        url: "/Factor/ShowFactorDetails",
        type: "Get",
        data: { id: factorId, periodId: periodId },
        success: function (res) {
            if (($("#orderModal").data('bs.modal') || {}).isShown) {
                $('#orderModal').modal('toggle'); // close current order modal
            }

            $('#ShowFactorModal').modal();
            $('#ShowFactorModalBody').html(res);
        }
    });
}

function DeleteOrderItem() {
    $(document).on("click", "#OrderList tr td a#deleteOrderItem", function () {
        var Row = $(this);
        var id = $(this).attr('value');

        swal({
            title: "آیا می خواهید این آیتم را از سفارش حذف کنید؟",
            text: "بعد از حذف، امکان بازیابی اطلاعات وجود ندارد!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
            buttons: ['انصراف', 'حذف']
        })
            .then((willDelete) => {
                if (willDelete) {
                    var token = $("input[name = __RequestVerificationToken]").val();
                    $.post('/Order/DeleteItem/' + id,
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
                                var title = "";
                                var text = "";

                                if (result.IsUsed == true) {
                                    title = "این آیتم قابل حذف نیست!";
                                    text = "اطلاعات آیتم مورد نظر در جای دیگری استفاده شده است.";
                                }
                                else {
                                    title = "خطا در انجام عملیات!";
                                    text = "لطفا دوباره تلاش کنید.";
                                }

                                swal({
                                    title: title,
                                    text: text,
                                    icon: "error",
                                    button: "تایید"
                                });
                            }
                        });

                }
            });
    });
}