function addInventoryChecking() {

	$(document).ready(function () {

        var index = 0;
        var drpProduct = $('#drpProduct');
        var drpWarehouse = $('#drpWarehouse');
        var txtRealCount = $('#txtRealCount');
        var txtMainUnit = $('#txtMainUnit');
        var lblRealCountError = $('#lblRealCountError');
        var txtSystemCount = $('#txtSystemCount');
        var txtDifference = $('#txtDifference');
        var txtDescription = $('#txtDescription');
        var productReqMessage = $('#productReqMessage');
        var productExistsMessage = $('#productExistsMessage');
        var inventoryCheckingItems = [];

        drpProduct.select2();

        productReqMessage.hide();
        productExistsMessage.hide();
        lblRealCountError.hide();

        $("#createdDate").datepicker({
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

        $(document).on("change",
            "#drpWarehouse",
            function () {
                if (inventoryCheckingItems.length > 0) {
                    swal({
                        title: "با تغییر انبار، کلیه آیتم های افزوده شده به لیست کالاهای شمارش شده، حذف خواهند شد. آیا ادامه می دهید؟",
                        icon: "warning",
                        buttons: true,
                        dangerMode: true,
                        buttons: ['انصراف', 'تأیید']
                    })
                        .then((isAccepted) => {
                            if (isAccepted) {
                                inventoryCheckingItems = [];
                                index = 0;
                                $('#tbodyItems').html('<tr></tr>');
                            }
                        });
                }

                $.ajax({
                    url: "/InventoryChecking/GetProducts",
                    data: { id: drpWarehouse.find(":selected").val() },
                    type: "POST",
                    dataType: "Json",
                    success: function (result) {
                        drpProduct.html(result.Html);
                    },
                    error: function () {
                    }
                });
            });

        $(document).on("change",
            "#drpProduct",
            function () {
                productExistsMessage.hide();

                var selectedOption = $("#drpProduct option:selected");

                if (selectedOption.index() !== 0) {
                    productReqMessage.hide();

                    var systemInventory = selectedOption.attr('data-inventory');
                    txtSystemCount.val(systemInventory);
                    var mainUnitName = selectedOption.attr('data-main-unit');
                    txtMainUnit.val(mainUnitName);
                } else {
                    txtSystemCount.val('0');
                }
            });

        txtRealCount.on('keyup',
            function () {
                var realCount = $(this).val();
                var systemCount = txtSystemCount.val();
                txtDifference.val(realCount - systemCount);
            });

        $('#btnAddItem').click(function () {
            var selectedIndex = $("#drpProduct option:selected").index();
            if (selectedIndex === 0) {
                productReqMessage.show();

                return false;
            }

            var productId = $("#drpProduct option:selected").val();

            for (var i = 0; i < inventoryCheckingItems.length; i++) {
                if (inventoryCheckingItems[i].ProductId == productId) {
                    productExistsMessage.show();

                    return false;
                }
            }

            if (txtRealCount.val().trim() == "") {
                lblRealCountError.show();

                return false;
            } else {
                lblRealCountError.hide();
            }

            index++;
            var item = {
                'index': index,
                'ProductName': drpProduct.find(":selected").text(),
                'ProductId': drpProduct.find(":selected").val(),
                'MainUnit': txtMainUnit.val(),
                'RealMainUnitCount': txtRealCount.val(),
                'SystemMainUnitCount': txtSystemCount.val(),
                'Difference': txtDifference.val(),
                'Description': txtDescription.val()
            };

            var btnDeleteId = "btnDelete" + index;
            inventoryCheckingItems.push(item);

            $('#tblItems tr:last').after('<tr class="columnSize"><td>' +
                index +
                '</td><td>' +
                item.ProductName +
                '</td><td>' +
                item.MainUnit +
                '</td><td class="txtMinusNum">' +
                item.RealMainUnitCount +
                '</td><td class="txtMinusNum">' +
                item.SystemMainUnitCount +
                '</td><td class="txtMinusNum">' +
                item.Difference +
                '</td><td>' +
                item.Description +
                '</td><td><a id="' +
                btnDeleteId +
                '" data-id=' +
                index +
                ' class="icon icon-remove iconColor"></a></td></tr>');

            $('#' + btnDeleteId).on('click',
                function () {
                    swal({
                        title: "آیا می خواهید این آیتم را از جدول حذف کنید؟",
                        icon: "warning",
                        buttons: true,
                        dangerMode: true,
                        buttons: ['انصراف', 'تأیید']
                    })
                        .then((isAccepted) => {
                            if (isAccepted) {
                                $(this).closest('tr').remove();
                                var id = $(this).attr('data-id');
                                var theItem = inventoryCheckingItems.find(x => x.index == id);
                                inventoryCheckingItems.splice(inventoryCheckingItems.indexOf(theItem), 1);
                            }
                        });
                });

            $('div#dropDownProductDiv select').val('0');
            txtRealCount.val('');
            txtSystemCount.val('');
            txtDifference.val('');
            txtDescription.val('');
        });

        $('#btnSubmit').click(function () {
            if ($('#myForm').valid()) {
                if (inventoryCheckingItems.length <= 0) {
                    Toast("لیست کالاهای شمارش شده خالی است!,info");
                    return false;
                }

                $.ajax({
                    url: "/InventoryChecking/AddInventoryCheckingAjax",
                    data: {
                        InventoryChecking: {
                            WarehouseId: $('#drpWarehouse option:selected').val(),
                            Description: $('#txtInventoryCheckingDescription').val()
                            //CreatedDate: $('#createdDate').val()
                        },
                        InventoryCheckingItems: inventoryCheckingItems,
                        StrCreatedDate: $('#createdDate').val()
                    },
                    type: "POST",
                    dataType: "Json",
                    success: function (result) {
                        if (result.Success) {
                            var url = '/InventoryChecking/InventoryCheckings';
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

function updateInventoryChecking(inventoryCheckingItems) {

    var index = 0;
    var drpProduct = $('#drpProduct');
    var drpWarehouse = $('#drpWarehouse');
    var txtRealCount = $('#txtRealCount');
    var txtMainUnit = $('#txtMainUnit');
    var lblRealCountError = $('#lblRealCountError');
    var txtSystemCount = $('#txtSystemCount');
    var txtDifference = $('#txtDifference');
    var txtDescription = $('#txtDescription');
    var productReqMessage = $('#productReqMessage');
    var productExistsMessage = $('#productExistsMessage');

    drpProduct.select2();

    $.ajax({
        url: "/InventoryChecking/GetProducts",
        data: { id: drpWarehouse.find(":selected").val() },
        type: "POST",
        dataType: "Json",
        success: function (result) {
            drpProduct.html(result.Html);
        },
        error: function () {
        }
    });

    for (var i = 0; i < inventoryCheckingItems.length; i++) {
        var item = inventoryCheckingItems[i];

        index++;
        var btnDeleteId = "btnDelete" + index;

        $('#tblItems tr:last').after('<tr class="columnSize"><td>' +
            index +
            '</td><td>' +
            item.ProductName +
            '</td><td>' +
            item.MainUnit +
            '</td><td class="txtMinusNum">' +
            item.RealMainUnitCount +
            '</td><td class="txtMinusNum">' +
            item.SystemMainUnitCount +
            '</td><td class="txtMinusNum">' +
            item.Difference +
            '</td><td>' +
            item.Description +
            '</td><td><a id="' +
            btnDeleteId +
            '" data-id=' +
            index +
            ' class="icon icon-remove iconColor"></a></td></tr>');

        $('#' + btnDeleteId).on('click', function () {
            swal({
                title: "آیا می خواهید این آیتم را از جدول حذف کنید؟",
                icon: "warning",
                buttons: true,
                dangerMode: true,
                buttons: ['انصراف', 'تأیید']
            })
                .then((isAccepted) => {
                    if (isAccepted) {
                        $(this).closest('tr').remove();
                        var id = $(this).attr('data-id');
                        var theItem = inventoryCheckingItems.find(x => x.Index == id);
                        inventoryCheckingItems.splice(inventoryCheckingItems.indexOf(theItem), 1);
                    }
                });
        });
    }

    productReqMessage.hide();
    productExistsMessage.hide();
    lblRealCountError.hide();

    $("#createdDate").datepicker({
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

    $(document).on("change",
        "#drpWarehouse",
        function () {
            if (inventoryCheckingItems.length > 0) {
                swal({
                    title: "با تغییر انبار، کلیه آیتم های افزوده شده به لیست کالاهای شمارش شده، حذف خواهند شد. آیا ادامه می دهید؟",
                    icon: "warning",
                    buttons: true,
                    dangerMode: true,
                    buttons: ['انصراف', 'تأیید']
                })
                    .then((isAccepted) => {
                        if (isAccepted) {
                            inventoryCheckingItems = [];
                            index = 0;
                            $('#tbodyItems').html('<tr></tr>');
                        }
                    });
            }

            $.ajax({
                url: "/InventoryChecking/GetProducts",
                data: { id: drpWarehouse.find(":selected").val() },
                type: "POST",
                dataType: "Json",
                success: function (result) {
                    drpProduct.html(result.Html);
                },
                error: function () {
                }
            });
        });

    $(document).on("change",
        "#drpProduct",
        function () {
            productExistsMessage.hide();

            var selectedOption = $("#drpProduct option:selected");

            if (selectedOption.index() !== 0) {
                productReqMessage.hide();

                var systemInventory = selectedOption.attr('data-inventory');
                txtSystemCount.val(systemInventory);
                var mainUnitName = selectedOption.attr('data-main-unit');
                txtMainUnit.val(mainUnitName);
            } else {
                txtSystemCount.val('0');
            }
        });

    txtRealCount.on('keyup', function () {
        var realCount = $(this).val();
        var systemCount = txtSystemCount.val();
        txtDifference.val(realCount - systemCount);
    });

    $('#btnAddItem').click(function () {
        var selectedIndex = $("#drpProduct option:selected").index();
        if (selectedIndex === 0) {
            productReqMessage.show();

            return false;
        }

        var productId = $("#drpProduct option:selected").val();

        for (var i = 0; i < inventoryCheckingItems.length; i++) {
            if (inventoryCheckingItems[i].ProductId == productId) {
                productExistsMessage.show();

                return false;
            }
        }

        if (txtRealCount.val().trim() == "") {
            lblRealCountError.show();

            return false;
        } else {
            lblRealCountError.hide();
        }

        index++;
        var item = {
            'Index': index,
            'ProductName': drpProduct.find(":selected").text(),
            'ProductId': drpProduct.find(":selected").val(),
            'MainUnit': txtMainUnit.val(),
            'RealMainUnitCount': txtRealCount.val(),
            'SystemMainUnitCount': txtSystemCount.val(),
            'Difference': txtDifference.val(),
            'Description': txtDescription.val()
        };

        var btnDeleteId = "btnButton" + index;
        inventoryCheckingItems.push(item);

        $('#tblItems tr:last').after('<tr class="columnSize"><td>' +
            index +
            '</td><td>' +
            item.ProductName +
            '</td><td>' +
            item.MainUnit +
            '</td><td class="txtMinusNum">' +
            item.RealMainUnitCount +
            '</td><td class="txtMinusNum">' +
            item.SystemMainUnitCount +
            '</td><td class="txtMinusNum">' +
            item.Difference +
            '</td><td>' +
            item.Description +
            '</td><td><a id="' +
            btnDeleteId +
            '" data-id=' +
            index +
            ' class="icon icon-remove iconColor"></a></td></tr>');
        $('#' + btnDeleteId).on('click', function () {
            swal({
                title: "آیا می خواهید این آیتم را از جدول حذف کنید؟",
                //text: "پس از تأیید، امکان بازیابی آیتم وجود ندارد!",
                icon: "warning",
                buttons: true,
                dangerMode: true,
                buttons: ['انصراف', 'تأیید']
            })
                .then((isAccepted) => {
                    if (isAccepted) {
                        $(this).closest('tr').remove();
                        var id = $(this).attr('data-id');
                        var theItem = inventoryCheckingItems.find(x => x.Index == id);
                        inventoryCheckingItems.splice(inventoryCheckingItems.indexOf(theItem), 1);
                    }
                });
        });

        $('div#dropDownProductDiv select').val('0');
        txtRealCount.val('');
        txtSystemCount.val('');
        txtDifference.val('');
        txtDescription.val('');
    });

    $('#btnSubmit').click(function () {
        if (inventoryCheckingItems.length <= 0) {
            Toast("لیست کالاهای شمارش شده خالی است!,info");
            return false;
        }

        $.ajax({
            url: "/InventoryChecking/UpdateInventoryCheckingAjax",
            data: {
                InventoryChecking: {
                    Id: $('#txtInventoryCheckingId').val(),
                    CreatedByUserId: $('#txtCreatedByUserId').val(),
                    WarehouseId: $('#drpWarehouse option:selected').val(),
                    Description: $('#txtInventoryCheckingDescription').val()
                },
                InventoryCheckingItems: inventoryCheckingItems,
                StrCreatedDate: $('#createdDate').val()
            },
            type: "POST",
            dataType: "Json",
            success: function (result) {
                if (result.Success) {
                    var url = '/InventoryChecking/InventoryCheckings';
                    window.location.href = url;
                } else {
                    Toast("خطا در ثبت اطلاعات,info");
                }
            },
            error: function () {
            }
        });
    });
}

function details(inventoryCheckingItems) {
    var index = 0;
    //var inventoryCheckingItems = items;

    var inventoryCheckingId = $('#txtInventoryCheckingId').val();
    var warehouseId = $('#txtWarehouseId').val();

    for (var i = 0; i < inventoryCheckingItems.length; i++) {
        var item = inventoryCheckingItems[i];

        index++;
        var btnTadilId = "btnTadil" + index;
        var btnTadil = "";

        if (item.IsCorrected) {
            btnTadil = "<span class='badge badge-success'>تعدیل شده</span>";
        } else {
            if (item.Difference != 0) {
                var buttonText = "";

                if (item.Difference > 0) {
                    buttonText = "صدور رسید تعدیل";
                } else if (item.Difference < 0) {
                    buttonText = "صدور حواله تعدیل";
                }

                btnTadil = "<input type='button' class='btn btn-outline-info' id='" +
                    btnTadilId +
                    "' data-id='" +
                    index +
                    "' value='" +
                    buttonText +
                    "' />";
            }
        }

        $('#tblItems tr:last').after('<tr class="columnSize"><td>' +
            index +
            '</td><td>' +
            item.ProductName +
            '</td><td>' +
            item.MainUnit +
            '</td><td>' +
            item.RealMainUnitCount +
            '</td><td>' +
            item.SystemMainUnitCount +
            '</td><td style="direction:ltr; text-align:right">' +
            item.Difference +
            '</td><td>' +
            item.Description +
            '</td><td>' +
            btnTadil +
            '</td></tr>');

        $('#' + btnTadilId).on('click',
            { param: item },
            function (event) {
                var theItem = event.data.param;
                var description = "";
                description += theItem.Difference > 0 ? "رسید" : "حواله";
                description += " تعدیل از بابت انبار گردانی شماره ";
                description += inventoryCheckingId;
                description += " جهت به روزرسانی موجودی کالا";
                $.ajax({
                    url: "/InventoryChecking/TadilModal",
                    data: {
                        InventoryCheckingId: inventoryCheckingId,
                        InventoryCheckingItemId: theItem.InventoryCheckingItemId,
                        Invoice: {
                            IsReceive: theItem.Difference > 0 ? true : false,
                            ThisWareHouseId: warehouseId,
                            Description: description
                        },
                        InvoiceItems: [
                            {
                                ProductId: theItem.ProductId,
                                MainUnitCount: Math.abs(theItem.Difference),
                                SubUnitCount: 0,
                                MainUnitName: theItem.MainUnit
                            }
                        ],
                        WarehouseName: $('#txtWarehouseId').html(),
                        ProductName: theItem.ProductName,
                        UnitName: theItem.MainUnit
                    },
                    type: "Post",
                    dataType: "Json",
                    success: function (result) {
                        $("#modal").modal();
                        $("#modalBody").html(result.Html);
                    },
                    error: function () {
                        console.log('Ajax error');
                    }
                });
            });
    }
}