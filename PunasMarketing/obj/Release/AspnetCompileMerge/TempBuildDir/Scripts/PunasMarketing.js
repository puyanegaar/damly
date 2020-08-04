
function Toast(msg) {

    var m = msg.split(',');
    var title = m[0];
    var type = m[1];

    toastr.options =
        {
            'closeButton': 'true',
            'debug': false,
            'newestOnTop': true,
            'progressBar': true,
            'positionClass': 'toast-top-center',
            'preventDuplicates': false,
            'onclick': null,
            'showDuration': '300',
            'hideDuration': '1000',
            'timeOut': '5000',
            'extendedTimeOut': '1000',
            'showEasing': 'swing',
            'hideEasing': 'linear',
            'showMethod': 'fadeIn',
            'hideMethod': 'fadeOut'
        }; toastr[type](title, '');
}


function Suppliers() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
        var Row = $(this);
        if (confirm("آیا برای حذف  مطمئن هستید؟") == true) {
            $.ajax({
                url: "/WareHouse/DeleteSupplier",
                data: { Code: $(this).attr('value') },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    if (result.Success) {
                        $(Row).fadeTo(400, 0, function () {
                            Row.closest('tr').remove();
                        });
                    }
                    Toast(result.Script);
                },
                erorr: function () {
                    alert('خطا');
                },
                beforeSend: function () {
                    showLoading();
                },

                complete: function () {
                    hideLoading();
                },

            });
        }
    });
}

function showLoading() {
    $('.layout-content-body').addClass("blurredElement");
    $("#divLoading").css('display', 'block');
}

function hideLoading() {
    $('.layout-content-body').removeClass("blurredElement");
    $("#divLoading").css('display', 'none');
}



function JobTitle() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/JobTitle/DeleteJobTitle/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}


function Category() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/ProductCategory/DeleteCategory/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}


function Unit() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/Unit/DeleteUnit/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}

function Delete(controller, action) {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
        var Row = $(this);
        var id = $(this).attr('value');

        swal({
            title: "آیا از حذف این آیتم مطمئن هستید؟",
            text: "بعد از حذف، امکان بازیابی اطلاعات وجود ندارد!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
            buttons: ['انصراف', 'حذف']
        })
            .then((willDelete) => {
                if (willDelete) {
                    var token = $("input[name = __RequestVerificationToken]").val();
                    $.post('/' + controller + '/' + action + '/' + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });
    });
}

function Personnel() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/Personnel/DeletePersonnel/" + id,
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



function PriceType() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/PriceType/DeletePriceType/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}

function Product() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/Product/DeleteProduct/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}


function Warehouse() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/Warehouse/DeleteWarehouse/" + id,
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


function Supplier() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/Supplier/DeleteSupplier/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}


function ProductPrice() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/ProductPrice/DeleteProductPrice/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}


function Bank() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/Bank/DeleteBank/" + id,
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

function Order() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/Order/DeleteOrder/" + id,
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



function InventoryChecking() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/InventoryChecking/DeleteInventoryChecking/" + id,
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


function BankAccount() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/BankAccount/DeleteBankAccount/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}


function CostCategory() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/CostCategory/DeleteCostCategory/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}


function Cost() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/Cost/DeleteCost/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}

function Cheque() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/Cheque/DeleteCheque/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}

function Region() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/Region/DeleteRegion/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}



function OtherTafsili() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/OtherTafsili/DeleteOtherTafsili/" + id,
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


function CashDesk() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/CashDesk/DeleteCashDesk/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}

function Transaction() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}

function CustomerCategory() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/CustomerCategory/DeleteCustomerCategory/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}

function Customer() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/Customer/DeleteCustomer/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}

function AddTransaction() {
    //Pay
    $("#TransactionModel_IssueDate").kendoDatePicker();



    $("#TransactionModel_PaySupCusId").change(function () {
        CheckPaySupCusId();
    });


    $("#TransactionModel_PayBankAccountId").change(function () {
        PayBankAccountId();
    });

    $("#TransactionModel_PayCashDeskId").change(function () {
        CheckPayCashDeskId();
    });

    //Received

    $("#TransactionModel_ReceiveSupCusId").change(function () {
        CheckReceiveSupCusId();
    });

    $("#TransactionModel_ReceiveBankAccountId").change(function () {
        CheckReceiveBankAccountId();
    });

    $("#TransactionModel_ReceiveCashDeskId").change(function () {
        CheckReceiveCashDeskId();
    });

    $("#TransactionModel_CostId").change(function () {
        CheckCostId();
    });
}

function CheckPaySupCusId() {
    var paySupCusId = $("#TransactionModel_PaySupCusId").val();

    if (paySupCusId > "0") {
        $("#PayBankAccountId").hide();
        $("#PayCashDeskId").hide();
    } else {

        $("#PayBankAccountId").show();
        $("#PayCashDeskId").show();
    }
}

function PayBankAccountId() {
    var bankAccountId = $("#TransactionModel_PayBankAccountId").val();

    if (bankAccountId > "0") {
        $("#PaySupCusId").hide();
        $("#PayCashDeskId").hide();
    } else {
        $("#PaySupCusId").show();
        $("#PayCashDeskId").show();
    }
}

function CheckPayCashDeskId() {
    var cashDeskId = $("#TransactionModel_PayCashDeskId").val();

    if (cashDeskId > "0") {
        $("#PaySupCusId").hide();
        $("#PayBankAccountId").hide();
    } else {
        $("#PaySupCusId").show();
        $("#PayBankAccountId").show();
    }
}

function CheckReceiveSupCusId() {
    var receiveSupCusId = $("#TransactionModel_ReceiveSupCusId").val();

    if (receiveSupCusId > "0") {
        $("#ReceiveBankAccountId").hide();
        $("#ReceiveCashDeskId").hide();
        $("#CostId").hide();
    } else {
        $("#ReceiveBankAccountId").show();
        $("#ReceiveCashDeskId").show();
        $("#CostId").show();
    }
}

function CheckReceiveBankAccountId() {
    var receiveBankAccountId = $("#TransactionModel_ReceiveBankAccountId").val();

    if (receiveBankAccountId > "0") {
        $("#ReceiveSupCusId").hide();
        $("#ReceiveCashDeskId").hide();
        $("#CostId").hide();
    } else {
        $("#ReceiveSupCusId").show();
        $("#ReceiveCashDeskId").show();
        $("#CostId").show();
    }
}

function CheckReceiveCashDeskId() {
    var checkReceiveCashDeskId = $("#TransactionModel_ReceiveCashDeskId").val();

    if (checkReceiveCashDeskId > "0") {
        $("#ReceiveSupCusId").hide();
        $("#ReceiveBankAccountId").hide();
        $("#CostId").hide();
    } else {
        $("#ReceiveSupCusId").show();
        $("#ReceiveBankAccountId").show();
        $("#CostId").show();
    }
}

function CheckCostId() {
    var costId = $("#TransactionModel_CostId").val();

    if (costId > "0") {
        $("#ReceiveSupCusId").hide();
        $("#ReceiveBankAccountId").hide();
        $("#ReceiveCashDeskId").hide();
    } else {
        $("#ReceiveSupCusId").show();
        $("#ReceiveBankAccountId").show();
        $("#ReceiveCashDeskId").show();
    }
}

function SelectImage() {
    $("#ImageSelect").click(function (e) {
        e.preventDefault();
        $("#ImageFile").click();
    });

    document.getElementById("ImageFile").onchange = function () {
        var reader = new FileReader();
        var imgPath = $(this)[0].value;
        var extn = imgPath.substring(imgPath.indexOf('.') + 1).toLowerCase();
        if (extn == "gif" || extn == "png" || extn == "jpg" || extn == "jpeg") {
            if (typeof (FileReader) != "undefined") {
                reader.onload = function (e) {
                    // get loaded data and render thumbnail.
                    document.getElementById("Image").src = e.target.result;
                };
                // read the image file as a data URL.
                reader.readAsDataURL(this.files[0]);
            }
        }
    };
}

function SelectCatalog() {
    $("#ImageSelect").click(function (e) {
        e.preventDefault();
        $("#ImageFile").click();
    });

    document.getElementById("ImageFile").onchange = function () {
        var reader = new FileReader();
        var imgPath = $(this)[0].value;
        var fileName = imgPath.substring(imgPath.lastIndexOf('\\') + 1);
        //if (extn == "gif" || extn == "png" || extn == "jpg" || extn == "jpeg")
        {
            if (typeof (FileReader) != "undefined") {
                reader.onload = function (e) {
                    //get loaded data and show path in a text field.
                    $("#catalogFileName").html(fileName);
                    $('#hiddenCatalogFileName').val(fileName);
                };
                // read the image file as a data URL.
                reader.readAsDataURL(this.files[0]);
            }
        }
    };
}

function DeleteInvoiceItem() {
    $(document).on("click", "#SendInvoiceItems tr td a#Delete", function () {
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
                    $.post("/Invoice/DeleteInvoiceItem/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}

function DeleteInvoice() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/Invoice/DeleteInvoice/" + id,
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}

function Factor() {
    $(document).on("click", "#demo-datatables-1 tr td a#Delete", function () {
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
                    $.post("/Factor/DeleteFactor/" + id,
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
                                    Row.closest('tr').addClass("deleted");
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
                //else {
                //    swal("Your user is safe!");
                //}
            });

    });
}

function start() {
    $("#progress").css("display", "block");
}
function Complete() {
    $("#progress").css("display", "none");
}