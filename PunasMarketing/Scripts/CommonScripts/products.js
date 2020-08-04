$('#btnSubmit').click(function (e) {
    e.preventDefault();
    if ($("#UploadFrom").valid()) {
        var dataString;
        var action = $("#UploadFrom").attr("action");
        if ($("#UploadFrom").attr("enctype") == "multipart/form-data") {
            //this only works in some browsers.
            //purpose? to submit files over ajax. because screw iframes.
            //also, we need to call .get(0) on the jQuery element to turn it into a regular DOM element so that FormData can use it.
            dataString = new FormData($("#UploadFrom").get(0));
            contentType = false;
            processData = false;
        } else {
            // regular form, do your own thing if you need it
        }
        $.ajax({
            type: "POST",
            url: action,
            data: dataString,
            dataType: "json", //change to your own, else read my note above on enabling the JsonValueProviderFactory in MVC
            contentType: contentType,
            processData: processData,
            success: function (data) {
                //BTW, data is one of the worst names you can make for a variable
                //handleSuccessFunctionHERE(data);

                createsuccess(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //do your own thing
                //alert("fail");
            }
        });
    }
});


function createsuccess(data) {
    if (data.isvalid == true) {
        $('#divInfo').slideUp("slow", function () { });
        $("#divUploadPic").slideDown("slow", function () { });
        $("#ProductId").val(data.ProductId);
    }
    if (data.Message) {
        Toast(data.Message);
    }
}

function ShowInvoiceDetails(factorId) {
    $.ajax({
        url: "/Invoice/InvoiceDetailsModal/" + factorId,
        type: "Get",
        success: function (res) {
            $('#invoiceModal').modal();
            $('#invoiceModalBody').html(res);
        }
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
                var submitButton = document.querySelector("#send");
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
                        window.location.href = '/Product/Products';
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

                var submitButton = document.querySelector("#send");
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
                        window.location.href = '/Product/Products'
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

$('#removeCatalog').click(function () {
    $('#catalogFileName').html('&nbsp;');
    $('#ImageFile').val('');
    $('#hiddenCatalogFileName').val('');
});