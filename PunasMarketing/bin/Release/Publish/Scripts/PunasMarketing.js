
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
function UploadImage() {
    Dropzone.options.dropzoneForm = {
        acceptedFiles: "image/jpeg,image/png,image/gif",
        autoProcessQueue: false,
        maxFilesize: 10,
        uploadMultiple: true,
        parallelUploads: 11,
        maxFiles: 10,

        init: function () {
            var thisDropzone = this;

            this.on("maxfilesexceeded", function (data) {
                var res = eval('(' + data.xhr.responseText + ')');

            });
            this.on("addedfile", function (file) {

                // Create the remove button   
                var removeButton = Dropzone.createElement("<button class=\"btn btn-primary\"><span class=\"icon icon-remove\"></span></button>");


                // Capture the Dropzone instance as closure.
                var submitButton = document.querySelector("#send")
                dropzoneForm = this; // closure

                submitButton.addEventListener("click", function () {
                    //e.preventDefault();
                    dropzoneForm.processQueue();
                    //var files = dropzoneForm.getQueuedFiles();
                    //files.forEach(function(file) {
                    //    dropzoneForm.processFile(file);
                    //});
                });
                //   dropzoneForm.processQueue(); // Tell Dropzone to process all queued files.

                var _this = this;
                dropzoneForm.on("complete", function () {
                    if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                        dropzoneForm.removeFile(file);

                    }

                });

                dropzoneForm.on("success", function () {
                    if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                        //var a = window.location.href.substr(0, window.location.href.indexOf('/'));
                        window.location.href = 'Products'
                        //$.ajax({
                        //    url: 'Admin/AddRest',
                        //    type: "GET",
                        //    data: { 'name': 'redirect' }
                    };
                })

                //});
                // Listen to the click event
                removeButton.addEventListener("click", function (e) {
                    // Make sure the button click doesn't submit the form:
                    e.preventDefault();
                    e.stopPropagation();
                    // Remove the file preview.
                    _this.removeFile(file);
                    // If you want to the delete the file on the server as well,
                    // you can do the AJAX request here.

                    //$.ajax({
                    //    url: 'Admin/deleteImgRest',
                    //    type: "POST",
                    //    data: { 'name': file.name }
                    //});
                });

                // Add the button to the file preview element.
                file.previewElement.appendChild(removeButton);
            });
        }
    };
}

function UpdateUploadImage() {

    //var url = '@Url.Action("GetImages","Admin")';
    var count = $("#imgCount").val();
    //File Upload response from the server
    Dropzone.options.dropzoneForm = {
        acceptedFiles: "image/jpeg,image/png,image/gif",
        autoProcessQueue: false,
        maxFilesize: 10,
        uploadMultiple: true,
        parallelUploads: 11,
        maxFiles: 10 - count,
        init: function () {
            var thisDropzone = this;

            $.getJSON("/Product/GetImages", { Code: $("#ProductId").val() }).done(function (data) {
                if (data.Data != '') {

                    $.each(data.Data, function (index, item) {

                        //// Create the mock file:
                        var mockFile = {
                            name: item.Name,
                            id: item.id,
                            size: 12345
                        };

                        // Call the default addedfile event handler
                        thisDropzone.emit("addedfile", mockFile);

                        // And optionally show the thumbnail of the file:
                        thisDropzone.emit("thumbnail", mockFile, item.Path);

                        // If you use the maxFiles option, make sure you adjust it to the
                        // correct amount:
                        //var existingFileCount = 1; // The number of files already uploaded
                        //myDropzone.options.maxFiles = myDropzone.options.maxFiles - existingFileCount;
                    });
                }

            });


            this.on("maxfilesexceeded", function (data) {
                var res = eval('(' + data.xhr.responseText + ')');

            });
            this.on("addedfile", function (file) {

                // Create the remove button         <span class="icon icon-remove"></span>
                var removeButton = Dropzone.createElement("<button class=\"btn btn-primary\"><span class=\"icon icon-remove\"></span></button>");


                // Capture the Dropzone instance as closure.

                var submitButton = document.querySelector("#send")
                dropzoneForm = this; // closure

                submitButton.addEventListener("click", function () {
                    //e.preventDefault();
                    dropzoneForm.processQueue();
                    //var files = dropzoneForm.getQueuedFiles();
                    //files.forEach(function(file) {
                    //    dropzoneForm.processFile(file);
                    //});
                });
                //   dropzoneForm.processQueue(); // Tell Dropzone to process all queued files.

                var _this = this;
                dropzoneForm.on("complete", function () {
                    if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                        dropzoneForm.removeFile(file);

                    }

                });

                dropzoneForm.on("success", function () {
                    if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                        window.location.href = 'Products'
                    };
                })

                //});
                // Listen to the click event
                removeButton.addEventListener("click", function (e) {
                    // Make sure the button click doesn't submit the form:
                    e.preventDefault();
                    e.stopPropagation();
                    // Remove the file preview.
                    _this.removeFile(file);
                    // If you want to the delete the file on the server as well,
                    // you can do the AJAX request here.

                    $.ajax({
                        url: '/Product/deleteImg',
                        type: "POST",
                        data: { 'id': file.id }
                    });
                });

                // Add the button to the file preview element.
                file.previewElement.appendChild(removeButton);
            });
        }
    };
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
            title: "آیا مطمئن هستید؟",
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

function Sanad() {
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

function Sarfasl() {
    $(document).on("click", "#sarfaslTable tr td a#Delete", function () {
        var row = $(this);
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
                    $.post("/Sarfasl/DeleteSarfasl/" + id,
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

                                var theFirstRow = $('.btn-delete-sarfasl').first();
                                $(theFirstRow).fadeTo(400, 0, function () {
                                    theFirstRow.closest('tr').remove();
                                });

                                var theLastRow = $('.btn-delete-sarfasl').last();
                                $(theLastRow).fadeTo(400, 0, function () {
                                    theLastRow.closest('tr').remove();
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

function ProductManagement() {
    var prop = [];
    $(document).on("click", "#InputRow #visiableCheckbox", function () {
        var Id = $(this).attr("bPropertyId");
        var element = $("[PropertyId$='" + Id + "'");
        if ($(this).is(":checked")) {
            element.attr("IsVisible", 1);
        }
        else {
            element.attr("IsVisible", 0);
        }


    });
    $(document).on("ready", function () {
        $("[Identity$='PropertyboolValue'").each(function () {
            var val = $(this).val();
            if (val == "True") {
                $(this).attr("checked", "checked");
            }
        });
    });
    $(document).on("click", "#Add", function (e) {
        e.preventDefault();
        $("[Identity$='PropertyValue'").each(function () {
            var value = $(this).val().replace(/,/g, '') + "&" + $(this).attr("PropertyId") + "&" + $(this).attr("IsVisible");
            prop.push(value);
        });
        $.ajax({
            url: "/Product/AddProductPrice",
            data: { ProductId: $("#ProductId").val(), PriceProperty: prop, mood: $("#mood").val() },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                if (result.Success) {
                    $("#mood").val('1');
                }
                Toast(result.Script);
                prop = [];

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


    });
    $(document).on("keyup", "#InputRow input", function () {
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

