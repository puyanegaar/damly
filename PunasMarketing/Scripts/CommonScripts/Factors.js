function FactorForOrder() {
    $(document).ready(function () {
        var lblTotalPrice = $('#lblTotalPrice');
        var lblTotalDiscount = $('#lblTotalDiscount');
        var lblTotalTax = $('#lblTotalTax');
        var lblDiscountOnFactor = $('#lblDiscountOnFactor');
        var lblAdditions = $('#lblAdditions');
        var lblDeductions = $('#lblDeductions');
        var lblFinalPrice = $('#lblFinalPrice');

        var txtDiscountOnFactor = $('#txtDiscountOnFactor');
        var txtAdditions = $('#txtAdditions');
        var txtDeductions = $('#txtDeductions');

        $('#btnVerify').click(function () {
            $(this).addClass('spinner spinner-inverse spinner-sm');
            $(this).attr("disabled", true);
            $('#theForm').submit();
        });

        txtDiscountOnFactor.on('input', function (e) {
            var val = txtDiscountOnFactor.val();
            if (val.trim() == "") {
                val = 0;
                txtDiscountOnFactor.val(0);
            }
            lblDiscountOnFactor.html(addCommas(val));
            lblDiscountOnFactor.attr('number-value', val);

            calculateAndShowFinalPrice();
        });

        txtAdditions.on('input', function (e) {
            var val = txtAdditions.val();
            if (val.trim() == "") {
                val = 0;
                txtAdditions.val(0);
            }
            lblAdditions.html(addCommas(val));
            lblAdditions.attr('number-value', val);

            calculateAndShowFinalPrice();
        });

        txtDeductions.on('input', function (e) {
            var val = txtDeductions.val();
            if (val.trim() == "") {
                val = 0;
                txtDeductions.val(0);
            }
            lblDeductions.html(addCommas(val));
            lblDeductions.attr('number-value', val);

            calculateAndShowFinalPrice();
        });

        function calculateAndShowFinalPrice() {
            var totalPrice = Number(lblTotalPrice.attr('number-value'));
            var totalDiscount = Number(lblTotalDiscount.attr('number-value'));
            var totalTax = Number(lblTotalTax.attr('number-value'));
            var discountOnFactor = Number(lblDiscountOnFactor.attr('number-value'));
            var additions = Number(lblAdditions.attr('number-value'));
            var deductions = Number(lblDeductions.attr('number-value'));

            var finalPrice = totalPrice - totalDiscount + totalTax - discountOnFactor + additions - deductions;
            lblFinalPrice.html(addCommas(finalPrice));
        }

        function addCommas(nStr) {
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }
    });

}

function AddBuyFactor() {
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
        };

    $(document).ready(function () {
        var txtUnitPrice = $('#txtUnitPrice');
        var txtCount = $('#txtCount');
        var txtDiscount = $('#txtDiscount');
        var lblTotalDiscount = $('#lblTotalDiscount');
        var txtTax = $('#txtTax');
        var txtVat = $('#txtVat');
        var lblFinalPrice = $('#lblFinalPrice');
        var lblTotalPrice = $('#lblTotalPrice');
        var btnAddItem = $('#btnAddItem');
        var drpSuppliers = $('#Suppliers');
        var drpProduct = $('#Products');
        var drpUnit = $('#Units');
        var lblSumTotalPrice = $('#lblSumTotalPrice');
        var lblSumDiscount = $('#lblSumDiscount');
        var lblDiscountOnFactor = $('#lblDiscountOnFactor');
        var lblSumTax = $('#lblSumTax');
        var lblAdditions = $('#lblAdditions');
        var lblDeductions = $('#lblDeductions');
        var lblSumFinalPrice = $('#lblSumFinalPrice');
        var txtDiscountOnFactor = $('#txtDiscountOnFactor');
        var txtAdditions = $('#txtAdditions');
        var txtDeductions = $('#txtDeductions');
        var index = 0;
        var factorItems = [];

        drpSuppliers.select2();
        drpProduct.select2();

        function addComma(num) {
            return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        function removeComma(num) {
            return num.toString().replace(/,/g, '');
        }

        txtTax.on('keyup', function (event) {
            if (event.keyCode === 13) {
                event.preventDefault();

                btnAddItem.click();
                $('#Products').focus();
            }
        });

        function calculateAndShowFactorSums() {
            var sumTotalPrice = 0;
            var sumDiscount = 0;
            var sumTax = 0;
            var discountOnFactor = Number(removeComma(txtDiscountOnFactor.val()));
            var additions = Number(removeComma(txtAdditions.val()));
            var deductions = Number(removeComma(txtDeductions.val()));

            if (isNaN(discountOnFactor) || discountOnFactor.toString().trim() == '') {
                discountOnFactor = 0;
            }

            if (isNaN(additions) || additions.toString().trim() == '') {
                additions = 0;
            }

            if (isNaN(deductions) || deductions.toString().trim() == '') {
                deductions = 0;
            }

            for (var i = 0; i < factorItems.length; i++) {
                var theItem = factorItems[i];

                sumTotalPrice += Number(theItem.TotalPrice);
                sumDiscount += Number(theItem.TotalDiscount);
                sumTax += Number(theItem.Tax);
            }

            var sumFinalPrice = sumTotalPrice - discountOnFactor - sumDiscount + sumTax - deductions + additions;

            lblSumTotalPrice.html(addComma(sumTotalPrice));
            lblSumDiscount.html(addComma(sumDiscount));
            lblSumTax.html(addComma(sumTax));
            lblDiscountOnFactor.html(addComma(discountOnFactor));
            lblAdditions.html(addComma(additions));
            lblDeductions.html(addComma(deductions));
            lblSumFinalPrice.html(addComma(sumFinalPrice));
        }

        function calculateItemPrices(shouldCalculateTax = true) {
            var count = removeComma(txtCount.val());
            var unitPrice = removeComma(txtUnitPrice.val());
            var discount = removeComma(txtDiscount.val());
            var tax = removeComma(txtTax.val());

            if (isNaN(count) || count.trim() == '') {
                count = 0;
            }

            if (isNaN(unitPrice) || unitPrice.trim() == '') {
                unitPrice = 0;
            }

            if (isNaN(discount) || discount.trim() == '') {
                discount = 0;
            }

            if (isNaN(tax) || tax.trim() == '') {
                tax = 0;
            }

            //var selectedUnitId = drpUnit.find(":selected").val();
            //var selectedProduct = drpProduct.find(":selected");
            //var productMainUnitId = selectedProduct.attr('data-main-unit-id');
            //var productUnitRate = selectedProduct.attr('data-unit-rate');
            var calculatedCount = count;

            //if (productMainUnitId == selectedUnitId) // main unit is selected
            //{
            //    calculatedCount = count;
            //}
            //else // subunit is selected
            //{
            //    calculatedCount = count * productUnitRate;
            //}

            var totalPrice = calculatedCount * unitPrice;
            if (isNaN(totalPrice)) {
                totalPrice = 0;
            }

            var totalDiscount = calculatedCount * discount;
            if (isNaN(totalDiscount)) {
                totalDiscount = 0;
            }

            lblTotalDiscount.html(addComma(totalDiscount));

            lblTotalPrice.html(addComma(totalPrice.toFixed(0)));
            var calculatedTax;
            if (shouldCalculateTax) {
                calculatedTax = Math.round((totalPrice - totalDiscount) * (txtVat.val() / 100.0));
            } else {
                calculatedTax = tax;
            }

            if (isNaN(calculatedTax)) {
                calculatedTax = 0;
            }
            txtTax.val(addComma(calculatedTax));
            var finalPrice = totalPrice - Number(totalDiscount) + Number(calculatedTax);

            if (isNaN(finalPrice)) {
                finalPrice = 0;
            }

            finalPrice = addComma(finalPrice.toFixed(0));
            lblFinalPrice.html(finalPrice);
        }

        txtUnitPrice.on("change paste keyup", function () {
            calculateItemPrices();
        });

        txtCount.on("change paste keyup", function () {
            calculateItemPrices();
        });

        txtDiscount.on("change paste keyup", function (event) {
            calculateItemPrices();
        });

        txtTax.on("change paste keyup", function () {
            calculateItemPrices(false);
        });

        function clearItemFields() {
            $('#Units').val(0); // select first item as default
            $('#txtCount').val(1);
            $('#txtUnitPrice').val(0);
            $('#txtDiscount').val(0);
            $('#lblTotalDiscount').html(0);
            $('#txtTax').val(0);
            $('#lblFinalPrice').html(0);
            $('#lblTotalPrice').html(0);


            // hide all items except the first one
            $("#Units option").each(function () {
                var thisValue = $(this).val();

                if (thisValue != 0) {
                    $(this).hide();
                }
            });
        }

        function setRowNums() {
            $('.tdIndex').each(function (i) {
                $(this).html(addComma(i + 1));
            });
        }

        function productExists(items, productId) {
            for (var i = 0; i < items.length; i++) {
                if (items[i].ProductId == productId) {
                    return true;
                }
            }

            return false;
        }

        btnAddItem.click(function () {
            var countVal = removeComma(txtCount.val());
            var unitPriceVal = removeComma(txtUnitPrice.val());
            var discountVal = removeComma(txtDiscount.val());
            var totalDiscountVal = removeComma(lblTotalDiscount.html());
            var taxVal = removeComma(txtTax.val());
            var finalPriceVal = removeComma(lblFinalPrice.html());

            if (drpProduct.find(":selected").val() == 0) {
                toastr.error("کالا را انتخاب کنید!");
                return false;
            }

            if (productExists(factorItems, drpProduct.find(":selected").val())) {
                toastr.error("کالای " + drpProduct.find(":selected").text() + "، قبلا به لیست اضافه شده است!");
                return false;
            }

            if (isNaN(countVal) || countVal.trim() == '') {
                toastr.error("تعداد کالا را وارد کنید!");
                return false;
            }

            if (countVal <= 0) {
                toastr.error("تعداد کالا نامعتبر است!");
                return false;
            }

            if (drpUnit.find(":selected").val() == 0) {
                toastr.error("واحد شمارشی را انتخاب کنید!");
                return false;
            }

            // verify product available count:
            //var selectedUnitId = drpUnit.find(":selected").val();
            //var selectedProduct = drpProduct.find(":selected");
            //var productUnitRate = selectedProduct.attr('data-unit-rate');
            //var productMainUnitId = selectedProduct.attr('data-main-unit-id');
            var calculatedCount = countVal;

            //if (productMainUnitId == selectedUnitId) // main unit is selected
            //{
            //    calculatedCount = countVal;
            //}
            //else // subunit is selected
            //{
            //    calculatedCount = countVal * productUnitRate;
            //}

            if (isNaN(unitPriceVal) || unitPriceVal.trim() == '') {
                unitPriceVal = 0;
            }

            if (unitPriceVal < 0) {
                toastr.error("قیمت واحد نامعتبر است!");
                return false;
            }

            var totalPrice = calculatedCount * unitPriceVal;
            if (totalDiscountVal > totalPrice) {
                toastr.error("مبلغ تخفیف کل نباید بیش از قیمت کل باشد!");
                return false;
            }

            if (finalPriceVal < 0) {
                toastr.error("قیمت نهایی نباید کمتر از صفر باشد!");
                return false;
            }

            var rayganAttr = '';
            if (unitPriceVal == 0 || finalPriceVal == 0) {
                rayganAttr = 'data-raygan="data-raygan"';
                swal({
                    text: 'قیمت کالای «' + drpProduct.find(":selected").text() + '»، صفر ثبت خواهد شد!',
                    title: "آیا این کالا رایگان است؟",
                    icon: "warning",
                    buttons: true,
                    dangerMode: true,
                    closeOnClickOutside: false,
                    buttons: ['انصراف', 'تأیید']
                }).then((isAccepted) => {
                    var rayganItemTr = $('#tblItems tr:last[' + rayganAttr + ']');
                    if (isAccepted) {
                        rayganItemTr.removeAttr('data-raygan');
                    } else {
                        rayganItemTr.remove();

                        var id = rayganItemTr.attr('data-id');
                        var theItem = factorItems.find(x => x.index == id);
                        factorItems.splice(factorItems.indexOf(theItem), 1);

                        setRowNums();
                        calculateAndShowFactorSums();

                        return false;
                    }
                });
            }

            if (isNaN(discountVal) || discountVal.trim() == '') {
                discountVal = 0;
            }

            if (isNaN(totalDiscountVal) || totalDiscountVal.trim() == '') {
                totalDiscountVal = 0;
            }

            if (isNaN(taxVal) || taxVal.trim() == '') {
                taxVal = 0;
            }

            var itemTotalPrice = calculatedCount * unitPriceVal;

            index++;
            var item = {
                'index': index,
                'ProductId': drpProduct.find(":selected").val(),
                'Count': calculatedCount,
                'UnitId': drpUnit.find(":selected").val(),
                'UnitPrice': unitPriceVal,
                'TotalPrice': itemTotalPrice,
                'Discount': discountVal,
                'TotalDiscount': totalDiscountVal,
                'Tax': taxVal,
                'FinalPrice': Number(itemTotalPrice) - Number(totalDiscountVal) + Number(taxVal)
            };

            var btnDeleteId = "btnDelete" + index;
            factorItems.push(item);

            $('#tblItems tr:last').after(
                '<tr class="columnSize" style="width:5%; text-align: center; vertical-align: middle" ' +
                rayganAttr +
                '><td class="tdIndex">' +
                addComma(index) +
                '</td><td><a href="/Product/ProductManagement/' + item.ProductId + '">' +
                drpProduct.find(":selected").text() +
                '</a></td><td>' +
                addComma(countVal) +
                '</td><td><a href="/Unit/Units/">' +
                drpUnit.find(":selected").text() +
                '</a></td><td>' +
                addComma(unitPriceVal) +
                '</td><td>' +
                addComma(itemTotalPrice) +
                '<td>' +
                addComma(discountVal) +
                '</td><td>' +
                addComma(totalDiscountVal) +
                '</td><td>' +
                addComma(taxVal) +
                '</td><td>' +
                lblFinalPrice.html() +
                '</td><td><a id="' +
                btnDeleteId +
                '" data-id=' +
                index +
                ' class="icon icon-remove iconColor" style="cursor: pointer"></a></td></tr>');

            setRowNums();
            calculateAndShowFactorSums();
            clearItemFields();
            $('#Products').prop('selectedIndex', 0).change();

            $('#' + btnDeleteId).on('click',
                function () {
                    swal({
                        title: "آیا می خواهید این آیتم را از جدول حذف کنید؟",
                        icon: "warning",
                        buttons: true,
                        dangerMode: true,
                        buttons: ['انصراف', 'تأیید']
                    }).then((isAccepted) => {
                        if (isAccepted) {
                            $(this).closest('tr').remove();
                            var id = $(this).attr('data-id');
                            var theItem = factorItems.find(x => x.index == id);
                            factorItems.splice(factorItems.indexOf(theItem), 1);
                            setRowNums();
                            calculateAndShowFactorSums();
                        }
                    });
                });
        });

        $(document).on("keyup", ".justNumber", function () {
            // skip for arrow keys
            if (event.which >= 37 && event.which <= 40) return;

            // format number
            $(this).val(function (index, value) {

                return value
                    .replace(/\D/g, "")
                    .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                    .replace(/^0+/, '');
            });
        });

        $('#Units').change(function () {
            calculateAndShowUnitPrice();
            calculateItemPrices();
        });

        function calculateAndShowUnitPrice() {
            var selectedProduct = drpProduct.find(":selected");
            var productUnitRate = Number(selectedProduct.attr('data-unit-rate'));
            var selectedUnitId = drpUnit.find(":selected").val();
            var productMainUnitId = selectedProduct.attr('data-main-unit-id');
            var unitPrice = Number(removeComma($('#txtUnitPrice').val()));

            if (selectedUnitId == 0) {
                $('#txtUnitPrice').val(0);
            }
            else if (productUnitRate > 0) {
                if (productMainUnitId == selectedUnitId) // main unit is selected
                {
                    $('#txtUnitPrice').val(addComma(unitPrice / productUnitRate));
                }
                else // subunit is selected
                {
                    $('#txtUnitPrice').val(addComma(unitPrice * productUnitRate));
                }
            }
        }

        $('#Products').change(function () {
            if (productExists(factorItems, drpProduct.find(":selected").val())) {
                toastr.error("کالای " + drpProduct.find(":selected").text() + "، قبلا به لیست اضافه شده است!");
                return false;
            }

            var priceTypeId = $("#priceType").val();
            var supplierId = $('#Suppliers').val();

            clearItemFields();

            var selectedProductVal = $(this).val();
            if (selectedProductVal != 0) {
                if (isNaN(supplierId) || supplierId.toString().trim() == '') {
                    toastr.error('ابتدا تأمین کننده را انتخاب کنید.');
                    return false;
                }

                $.ajax({
                    url: "/Factor/GetBuyProductInfo/",
                    type: "Get",
                    dataType: "Json",
                    data: {
                        productId: selectedProductVal,
                        priceTypeId: priceTypeId
                    },
                    success: function (res) {
                        drpProduct.find(":selected").attr('data-main-unit-name', res.MainUnitName);
                        drpProduct.find(":selected").attr('data-sub-unit-name', res.SubUnitName);

                        $("#Units option").each(function () {
                            var thisValue = $(this).val();

                            if (thisValue == res.MainUnitId || thisValue == res.SubUnitId) {
                                $(this).show();
                            }
                        });

                        $('#Units').val(res.MainUnitId); // select mainunit as default item

                        txtUnitPrice.val(addComma(res.UnitPrice));

                        calculateItemPrices();

                        txtCount.focus();
                        txtCount.select();
                    }
                });
            }
        });

        txtDiscountOnFactor.on("change paste keyup", function () {
            calculateAndShowFactorSums();
        });

        txtAdditions.on("change paste keyup", function () {
            calculateAndShowFactorSums();
        });

        txtDeductions.on("change paste keyup", function () {
            calculateAndShowFactorSums();
        });

        $('#btnSubmit').click(function () {
            var validator = $('#FactorForm').validate();

            var supplierId = $('#Suppliers').val();
            if (isNaN(supplierId) || supplierId.toString().trim() == '') {
                toastr.error('تأمین کننده را انتخاب کنید.');
                return false;
            }

            if (factorItems.length <= 0) {
                toastr.error('فاکتور بدون آیتم قابل ثبت نیست.');
                return false;
            }

            if ($('#FactorForm').valid()) {
                swal({
                    title: "آیا از ثبت فاکتور مطمئن هستید؟",
                    icon: "warning",
                    buttons: true,
                    dangerMode: true,
                    buttons: ['انصراف', 'تأیید']
                }).then((isAccepted) => {
                    if (isAccepted) {
                        $.ajax({
                            type: "POST",
                            dataType: "Json",
                            url: "/Factor/AddBuyFactor",
                            data: {
                                Factor: {
                                    SupplierId: supplierId,
                                    PaperFactorCode: $('#txtPaperFactorCode').val(),
                                    TotalPrice: removeComma($('#lblSumTotalPrice').html()),
                                    DiscountOnFactor: removeComma($('#txtDiscountOnFactor').val()),
                                    TotalDiscount: removeComma($('#lblSumDiscount').html()),
                                    Additions: removeComma($('#txtAdditions').val()),
                                    Deductions: removeComma($('#txtDeductions').val()),
                                    TotalTax: removeComma($('#lblSumTax').html()),
                                    FinalPrice: removeComma($('#lblSumFinalPrice').html()),
                                    Description: $('#txtDescription').val(),
                                    UnitId: drpUnit.find(":selected").val()
                                },
                                FactorItems: factorItems,
                                StrCreateDate: $('#txtCreateDate').val()
                            },
                            success: function (result) {
                                if (result.Success) {
                                    var url = '/Factor/BuyFactors/';
                                    window.location.href = url;
                                    var addedFactorId = result.Id;

                                    swal({
                                        title: "فاکتور جدید با شناسه " + addedFactorId + "، با موفقیت ثبت شد.",
                                        text: 'در حال انتقال به فهرست فاکتورها...',
                                        icon: "success",
                                        button: "تایید"
                                    });
                                } else {
                                    toastr.error(result.Message);
                                }
                            },
                            error: function () {
                                toastr.error('خطای سمت سرور!');
                            }
                        });
                    }
                });
            } else {
                var messages = validator.errorList;
                console.log(messages);
                for (var i = 0; i < messages.length; i++) {
                    toastr.error(messages[i].message);
                }
            }
        });

        $("#txtCreateDate").datepicker({
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
    });
}

function AddSellFactor() {
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
        };

    var txtMarketer = $('#txtMarketer');
    var chkAssignCommission = $('#chkAssignCommission');
    var chkAssignOffer = $('#chkAssignOffer');
    var txtUnitPrice = $('#txtUnitPrice');
    var txtCommission = $('#txtCommission');
    var lblTotalCommission = $('#lblTotalCommission');
    var txtCount = $('#txtCount');
    var txtDiscount = $('#txtDiscount');
    var lblTotalDiscount = $('#lblTotalDiscount');
    var lblOffer = $('#lblOffer');
    var txtTax = $('#txtTax');
    var txtVat = $('#txtVat');
    var lblFinalPrice = $('#lblFinalPrice');
    var lblTotalPrice = $('#lblTotalPrice');
    var btnAddItem = $('#btnAddItem');
    var drpCustomer = $('#Customers');
    var drpProduct = $('#Products');
    var drpUnit = $('#Units');
    var lblSumTotalPrice = $('#lblSumTotalPrice');
    var lblSumDiscount = $('#lblSumDiscount');
    var lblDiscountOnFactor = $('#lblDiscountOnFactor');
    var lblSumTax = $('#lblSumTax');
    var lblAdditions = $('#lblAdditions');
    var lblDeductions = $('#lblDeductions');
    var lblSumFinalPrice = $('#lblSumFinalPrice');
    var lblSumCommission = $('#lblSumCommission');
    var txtDiscountOnFactor = $('#txtDiscountOnFactor');
    var txtAdditions = $('#txtAdditions');
    var txtDeductions = $('#txtDeductions');
    var index = 0;
    var factorItems = [];

    drpCustomer.select2();
    drpProduct.select2();
    $('.col_commission').hide();
    chkAssignCommission.prop('disabled', 'disabled');

    function addComma(num) {
        return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    function removeComma(num) {
        return num.toString().replace(/,/g, '');
    }

    txtTax.on('keyup',
        function (event) {
            if (!chkAssignCommission.is(':checked')) {
                if (event.keyCode === 13) {
                    event.preventDefault();

                    btnAddItem.click();
                    $('#Products').focus();
                }
            }
        });

    txtCommission.on('keyup',
        function (event) {
            if (event.keyCode === 13) {
                event.preventDefault();

                btnAddItem.click();
                $('#Products').focus();
            }
        });

    function calculateAndShowFactorSums() {
        var sumTotalPrice = 0;
        var sumTotalDiscount = 0;
        var sumTax = 0;
        var sumTotalCommission = 0;
        var discountOnFactor = Number(removeComma(txtDiscountOnFactor.val()));
        var additions = Number(removeComma(txtAdditions.val()));
        var deductions = Number(removeComma(txtDeductions.val()));

        if (isNaN(discountOnFactor) || discountOnFactor.toString().trim() == '') {
            discountOnFactor = 0;
        }

        if (isNaN(additions) || additions.toString().trim() == '') {
            additions = 0;
        }

        if (isNaN(deductions) || deductions.toString().trim() == '') {
            deductions = 0;
        }

        for (var i = 0; i < factorItems.length; i++) {
            var theItem = factorItems[i];

            sumTotalPrice += Number(theItem.TotalPrice);
            sumTotalDiscount += Number(theItem.TotalDiscount);
            sumTax += Number(theItem.Tax);
            sumTotalCommission += Number(theItem.TotalCommission);
        }

        var sumFinalPrice = sumTotalPrice - discountOnFactor - sumTotalDiscount + sumTax - deductions + additions;

        if (!chkAssignCommission.is(':checked')) {
            sumTotalCommission = 0;
        }

        lblSumTotalPrice.html(addComma(sumTotalPrice));
        lblSumDiscount.html(addComma(sumTotalDiscount));
        lblSumTax.html(addComma(sumTax));
        lblSumCommission.html(addComma(sumTotalCommission));
        lblDiscountOnFactor.html(addComma(discountOnFactor));
        lblAdditions.html(addComma(additions));
        lblDeductions.html(addComma(deductions));
        lblSumFinalPrice.html(addComma(sumFinalPrice));
    }

    function calculateItemPrices(shouldCalculateTax = true) {
        var count = removeComma(txtCount.val());
        var unitPrice = removeComma(txtUnitPrice.val());
        var discount = removeComma(txtDiscount.val());
        var tax = removeComma(txtTax.val());
        var commission = removeComma(txtCommission.val());

        if (isNaN(count) || count.trim() == '') {
            count = 0;
        }

        if (isNaN(unitPrice) || unitPrice.trim() == '') {
            unitPrice = 0;
        }

        if (isNaN(discount) || discount.trim() == '') {
            discount = 0;
        }

        if (isNaN(tax) || tax.trim() == '') {
            tax = 0;
        }

        if (isNaN(commission) || commission.trim() == '') {
            commission = 0;
        }

        //var selectedUnitId = drpUnit.find(":selected").val();
        var selectedProduct = drpProduct.find(":selected");
        //var productMainUnitId = selectedProduct.attr('data-main-unit-id');
        //var productUnitRate = selectedProduct.attr('data-unit-rate');
        var productDiscountAmount = selectedProduct.attr('data-discount-amount');
        var productDiscountPercent = selectedProduct.attr('data-discount-percent');
        var calculatedCount = count;

        //if (productMainUnitId == selectedUnitId) // main unit is selected
        //{
        //    calculatedCount = count;
        //}
        //else // subunit is selected
        //{
        //    calculatedCount = count * productUnitRate;
        //}

        var totalPrice = calculatedCount * unitPrice;
        if (isNaN(totalPrice)) {
            totalPrice = 0;
        }

        if (chkAssignOffer.is(':checked')) {
            if (productDiscountAmount != null) // تخفیف از نوع ریالی است
            {
                discount = productDiscountAmount;
                txtDiscount.val(addComma(discount));
            }
            else if (productDiscountPercent != null) // تخفیف از نوع درصدی است
            {
                discount = Math.round(productDiscountPercent * unitPrice / 100);
                txtDiscount.val(addComma(discount));
            }
        }

        var totalDiscount = calculatedCount * discount;
        if (isNaN(totalDiscount)) {
            totalDiscount = 0;
        }

        lblTotalDiscount.html(addComma(totalDiscount));

        lblTotalPrice.html(addComma(totalPrice.toFixed(0)));
        var calculatedTax;
        if (shouldCalculateTax) {
            calculatedTax = Math.round((totalPrice - totalDiscount) * (txtVat.val() / 100.0));
        } else {
            calculatedTax = tax;
        }

        if (isNaN(calculatedTax)) {
            calculatedTax = 0;
        }
        txtTax.val(addComma(calculatedTax));

        var totalCommission = calculatedCount * Number(commission);
        if (isNaN(totalCommission)) {
            totalCommission = 0;
        }
        lblTotalCommission.html(addComma(totalCommission));

        var finalPrice = totalPrice - Number(totalDiscount) + Number(calculatedTax);

        if (isNaN(finalPrice)) {
            finalPrice = 0;
        }

        finalPrice = addComma(finalPrice.toFixed(0));
        lblFinalPrice.html(finalPrice);
    }

    txtUnitPrice.on("change paste keyup", function () {
        calculateItemPrices();
    });

    txtCommission.on("change paste keyup", function () {
        calculateItemPrices();
    });

    txtCount.on("change paste keyup", function () {
        calculateItemPrices();
    });

    txtDiscount.on("change paste keyup", function (event) {
        var amount = drpProduct.find(":selected").attr('data-discount-amount');
        var percent = drpProduct.find(":selected").attr('data-discount-percent');

        if (event.keyCode !== 9) {
            if (amount != null || percent != null) {
                drpProduct.find(":selected").removeAttr('data-discount-amount');
                drpProduct.find(":selected").removeAttr('data-discount-percent');
                lblOffer.html('-');
                var diableOfferMessage =
                    'جشنواره تخفیف برای کالای «' +
                    drpProduct.find(":selected").text() +
                    '» غیرفعال شد!' +
                    ' برای اعمال مجدد تخفیفات جشنواره، بایستی کالا را دوباره انتخاب کنید.';
                toastr.warning(diableOfferMessage);
            }
        }

        calculateItemPrices();
    });

    txtTax.on("change paste keyup", function () {
        calculateItemPrices(false);
    });

    var chkAssignCommissionChange = function () {
        if (chkAssignCommission.is(':checked')) {
            $('.col_commission').show();
        } else {
            $('.col_commission').hide();
        }

        calculateAndShowFactorSums();
    };
    chkAssignCommission.change(chkAssignCommissionChange);

    var chkAssignOfferChange = function () {
        if (chkAssignOffer.is(':checked')) {
            $('.col_offer').show();
        } else {
            $('.col_offer').hide();
        }
    };
    chkAssignOffer.change(chkAssignOfferChange);

    function setRowNums() {
        $('.tdIndex').each(function (i) {
            $(this).html(addComma(i + 1));
        });
    }

    function productExists(items, productId) {
        for (var i = 0; i < items.length; i++) {
            if (items[i].ProductId == productId) {
                return true;
            }
        }

        return false;
    }

    btnAddItem.click(function () {
        var countVal = removeComma(txtCount.val());
        var unitPriceVal = removeComma(txtUnitPrice.val());
        var discountVal = removeComma(txtDiscount.val());
        var totalDiscountVal = removeComma(lblTotalDiscount.html());
        var taxVal = removeComma(txtTax.val());
        var commissionVal = removeComma(txtCommission.val());
        var totalCommissionVal = removeComma(lblTotalCommission.html());
        var finalPriceVal = removeComma(lblFinalPrice.html());

        if (drpProduct.find(":selected").val() == 0) {
            toastr.error("کالا را انتخاب کنید!");
            return false;
        }

        if (productExists(factorItems, drpProduct.find(":selected").val())) {
            toastr.error("کالای " + drpProduct.find(":selected").text() + "، قبلا به لیست اضافه شده است!");
            return false;
        }

        if (isNaN(countVal) || countVal.trim() == '') {
            toastr.error("تعداد کالا را وارد کنید!");
            return false;
        }

        if (countVal <= 0) {
            toastr.error("تعداد کالا نامعتبر است!");
            return false;
        }

        if (drpUnit.find(":selected").val() == 0) {
            toastr.error("واحد شمارشی را انتخاب کنید!");
            return false;
        }

        // verify product available count:
        var selectedUnitId = drpUnit.find(":selected").val();
        var selectedProduct = drpProduct.find(":selected");
        var productUnitRate = selectedProduct.attr('data-unit-rate');
        var productMainUnitId = selectedProduct.attr('data-main-unit-id');
        var productAvailableCount = selectedProduct.attr('data-available-count');
        var productMainUnitName = selectedProduct.attr('data-main-unit-name');
        var productSubUnitName = selectedProduct.attr('data-sub-unit-name');
        var calculatedCount = countVal;
        var insufficientMessage =
            "امکان صدور فاکتور برای این تعداد از کالا وجود ندارد! حداکثر تعداد قابل سفارش برای کالای " +
            selectedProduct.text() + ": " +
            addComma(productAvailableCount) + " " +
            productMainUnitName +
            "؛ معادل " + addComma(productAvailableCount / productUnitRate) + " " + productSubUnitName;
        if (productMainUnitId == selectedUnitId) // main unit is selected
        {
            //calculatedCount = countVal;

            if (Number(countVal) > Number(productAvailableCount)) {
                toastr.info(insufficientMessage);
                return false;
            }
        }
        else // subunit is selected
        {
            //calculatedCount = countVal * productUnitRate;

            if (Number(countVal * productUnitRate) > Number(productAvailableCount)) {
                toastr.info(insufficientMessage);
                return false;
            }
        }

        if (isNaN(unitPriceVal) || unitPriceVal.trim() == '') {
            unitPriceVal = 0;
        }


        if (unitPriceVal < 0) {
            toastr.error("قیمت واحد نامعتبر است!");
            return false;
        }

        var totalPrice = calculatedCount * unitPriceVal;
        if (totalDiscountVal > totalPrice) {
            toastr.error("مبلغ تخفیف کل نباید بیش از قیمت کل باشد!");
            return false;
        }

        if (chkAssignCommission.is(':checked')) {
            if (totalCommissionVal > totalPrice) {
                toastr.error("مبلغ کمیسیون کل نباید بیش از قیمت کل باشد!");
                return false;
            }
        }

        if (finalPriceVal < 0) {
            toastr.error("قیمت نهایی نباید کمتر از صفر باشد!");
            return false;
        }

        var rayganAttr = '';
        if (unitPriceVal == 0 || finalPriceVal == 0) {
            rayganAttr = 'data-raygan="data-raygan"';
            swal({
                text: 'قیمت کالای «' + drpProduct.find(":selected").text() + '»، صفر ثبت خواهد شد!',
                title: "آیا این کالا رایگان است؟",
                icon: "warning",
                buttons: true,
                dangerMode: true,
                closeOnClickOutside: false,
                buttons: ['انصراف', 'تأیید']
            }).then((isAccepted) => {
                var rayganItemTr = $('#tblItems tr:last[' + rayganAttr + ']');
                if (isAccepted) {
                    rayganItemTr.removeAttr('data-raygan');
                } else {
                    rayganItemTr.remove();

                    var id = rayganItemTr.attr('data-id');
                    var theItem = factorItems.find(x => x.index == id);
                    factorItems.splice(factorItems.indexOf(theItem), 1);

                    setRowNums();
                    calculateAndShowFactorSums();

                    return false;
                }
            });
        }

        if (isNaN(discountVal) || discountVal.trim() == '') {
            discountVal = 0;
        }

        if (isNaN(totalDiscountVal) || totalDiscountVal.trim() == '') {
            totalDiscountVal = 0;
        }

        if (isNaN(taxVal) || taxVal.trim() == '') {
            taxVal = 0;
        }

        if (isNaN(commissionVal) || commissionVal.trim() == '') {
            commissionVal = 0;
        }

        var itemTotalPrice = calculatedCount * unitPriceVal;

        index++;
        var item = {
            'index': index,
            'ProductId': drpProduct.find(":selected").val(),
            'Count': calculatedCount,
            'UnitId': drpUnit.find(":selected").val(),
            'UnitPrice': unitPriceVal,
            'TotalPrice': itemTotalPrice,
            'Discount': discountVal,
            'TotalDiscount': totalDiscountVal,
            'Tax': taxVal,
            'Commission': commissionVal,
            'TotalCommission': totalCommissionVal,
            'FinalPrice': Number(itemTotalPrice) - Number(totalDiscountVal) + Number(taxVal)
        };

        var btnDeleteId = "btnDelete" + index;
        factorItems.push(item);

        $('#tblItems tr:last').after(
            '<tr class="columnSize" style="width:5%; text-align: center; vertical-align: middle" ' +
            rayganAttr +
            '><td class="tdIndex">' +
            addComma(index) +
            '</td><td><a href="/Product/ProductManagement/' + item.ProductId + '">' +
            drpProduct.find(":selected").text() +
            '</a></td><td>' +
            addComma(countVal) +
            '</td><td><a href="/Unit/Units/">' +
            drpUnit.find(":selected").text() +
            '</a></td><td>' +
            addComma(unitPriceVal) +
            '</td><td>' +
            addComma(itemTotalPrice) +
            '</td><td class="col_offer"><a href="/Offer/Offers/">' +
            lblOffer.html() +
            '</a></td><td>' +
            addComma(discountVal) +
            '</td><td>' +
            addComma(totalDiscountVal) +
            '</td><td>' +
            addComma(taxVal) +
            '</td><td class="col_commission">' +
            addComma(commissionVal) +
            '</td><td class="col_commission">' +
            addComma(totalCommissionVal) +
            '</td><td>' +
            lblFinalPrice.html() +
            '</td><td><a id="' +
            btnDeleteId +
            '" data-id=' +
            index +
            ' class="icon icon-remove iconColor" style="cursor: pointer"></a></td></tr>');

        // اعمال تخفیف کالایی
        var discountGiftProductId = selectedProduct.attr('data-discount-gift-product-id');
        if (discountGiftProductId != null) // تخفیف کالایی دارد
        {
            var discountGiftProductName = selectedProduct.attr('data-discount-gift-product-name');
            var discountMinProductCount = Number(selectedProduct.attr('data-discount-min-product-count'));
            var discountGiftProductCount = Number(selectedProduct.attr('data-discount-gift-product-count'));
            discountGiftProductCount *= Math.floor(calculatedCount / discountMinProductCount);
            var discountGiftMainUnitId = Number(selectedProduct.attr('data-discount-gift-main-unit-id'));
            var discountGiftUnitName = selectedProduct.attr('data-discount-gift-main-unit-name');
            var discountGiftAvailableCount = Number(selectedProduct.attr('data-discount-gift-available-count'));
            var discountOfferId = Number(selectedProduct.attr('data-discount-offer-id'));
            var discountOfferName = selectedProduct.attr('data-discount-offer-name');

            if (discountMinProductCount > discountGiftAvailableCount) {
                toastr.error("امکان اختصاص کالای هدیه وجود ندارد! موجودی کالای هدیه ناکافی است.");
            }
            //else if (productExists(factorItems, discountGiftProductId)) {
            //    toastr.error("امکان اختصاص کالای هدیه وجود ندارد! کالای «" + discountGiftProductName + "»، قبلا به لیست اضافه شده است.");
            //}
            else {
                if (calculatedCount >= discountMinProductCount) {
                    var hadiyeAttr = 'data-hadiye="data-hadiye"';

                    swal({
                        text: 'کالای «' +
                            discountGiftProductName +
                            '»، به عنوان کالای هدیه جشنواره «' +
                            discountOfferName +
                            '»، به لیست اضافه خواهد شد.',
                        title: "آیا با اختصاص کالای هدیه موافق هستید؟",
                        icon: "warning",
                        buttons: true,
                        dangerMode: true,
                        closeOnClickOutside: false,
                        buttons: ['انصراف', 'تأیید']
                    }).then((isAccepted) => {
                        var hadiyeItemTr = $('#tblItems tr:last[' + hadiyeAttr + ']');
                        if (isAccepted) {
                            hadiyeItemTr.removeAttr('data-hadiye');
                        } else {
                            hadiyeItemTr.remove();

                            var id = hadiyeItemTr.attr('data-id');
                            var theItem = factorItems.find(x => x.index == id);
                            factorItems.splice(factorItems.indexOf(theItem), 1);

                            setRowNums();
                            calculateAndShowFactorSums();

                            return false;
                        }
                    });

                    index++;
                    var giftItem = {
                        'index': index,
                        'ProductId': discountGiftProductId,
                        'Count': discountGiftProductCount,
                        'UnitId': discountGiftMainUnitId,
                        'UnitPrice': 0,
                        'TotalPrice': 0,
                        'Discount': 0,
                        'TotalDiscount': 0,
                        'Tax': 0,
                        'Commission': 0,
                        'TotalCommission': 0,
                        'FinalPrice': 0
                    };

                    var btnGiftDeleteId = "btnDelete" + index;
                    factorItems.push(giftItem);


                    $('#tblItems tr:last').after(
                        '<tr class="columnSize" style="width:5%; text-align: center; vertical-align: middle" ' +
                        hadiyeAttr +
                        '><td class="tdIndex">' +
                        addComma(index) +
                        '</td><td><a href="/Product/ProductManagement/' + discountGiftProductId + '">' +
                        discountGiftProductName +
                        '</a></td><td>' +
                        addComma(discountGiftProductCount) +
                        '</td><td><a href="/Unit/Units/">' +
                        discountGiftUnitName +
                        '</a></td><td>' +
                        0 +
                        '</td><td>' +
                        0 +
                        '</td><td class="col_offer"><a href="/Offer/Offers/">جشنواره ' +
                        discountOfferId +
                        '</a></td><td>' +
                        0 +
                        '</td><td>' +
                        0 +
                        '</td><td>' +
                        0 +
                        '</td><td class="col_commission">' +
                        0 +
                        '</td><td class="col_commission">' +
                        0 +
                        '</td><td>' +
                        0 +
                        '</td><td><a id="' +
                        btnGiftDeleteId +
                        '" data-id=' +
                        index +
                        ' class="icon icon-remove iconColor" style="cursor: pointer"></a></td></tr>');

                    $('#' + btnGiftDeleteId).on('click',
                        function () {
                            swal({
                                title: "آیا می خواهید این آیتم را از جدول حذف کنید؟",
                                icon: "warning",
                                buttons: true,
                                dangerMode: true,
                                buttons: ['انصراف', 'تأیید']
                            }).then((isAccepted) => {
                                if (isAccepted) {
                                    $(this).closest('tr').remove();
                                    var id = $(this).attr('data-id');
                                    var theItem = factorItems.find(x => x.index == id);
                                    factorItems.splice(factorItems.indexOf(theItem), 1);
                                    setRowNums();
                                    calculateAndShowFactorSums();
                                }
                            });
                        });
                }
            }
        }


        chkAssignCommissionChange();
        chkAssignOfferChange();
        setRowNums();
        calculateAndShowFactorSums();
        clearItemFields();
        $('#Products').prop('selectedIndex', 0).change();

        $('#' + btnDeleteId).on('click',
            function () {
                swal({
                    title: "آیا می خواهید این آیتم را از جدول حذف کنید؟",
                    icon: "warning",
                    buttons: true,
                    dangerMode: true,
                    buttons: ['انصراف', 'تأیید']
                }).then((isAccepted) => {
                    if (isAccepted) {
                        $(this).closest('tr').remove();
                        var id = $(this).attr('data-id');
                        var theItem = factorItems.find(x => x.index == id);
                        factorItems.splice(factorItems.indexOf(theItem), 1);
                        setRowNums();
                        calculateAndShowFactorSums();
                    }
                });
            });
    });

    $(document).on("keyup", ".justNumber", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {

            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                .replace(/^0+/, '');
        });
    });

    $('#Customers').change(function () {
        var selectedVal = $(this).val();
        if (selectedVal != '') {
            $.ajax({
                url: "/Factor/GetMarketerName",
                type: "Get",
                dataType: "Json",
                data: { customerId: selectedVal },
                success: function (res) {
                    if (res.id != null) {
                        txtMarketer.html(res.name);
                        txtMarketer.attr('href', '/Personnel/PersonnelManagement/' + res.id);
                        chkAssignCommission.prop('checked', true);
                        chkAssignCommission.removeAttr('disabled');
                        $('.col_commission').show();
                    } else {
                        txtMarketer.html(res.name);
                        txtMarketer.removeAttr('href');
                        chkAssignCommission.prop('checked', false);
                        chkAssignCommission.prop('disabled', 'disabled');
                        $('.col_commission').hide();
                    }

                    $('#Products').prop('selectedIndex', 0).change();
                }
            });
        } else {
            txtMarketer.html('-');
            txtMarketer.removeAttr('href');
            chkAssignCommission.prop('checked', false);
            $('.col_commission').hide();
        }
    });

    function clearItemFields() {
        $('#Units').val(0); // select first item as default
        $('#txtCount').val(1);
        $('#txtUnitPrice').val(0);
        $('#lblOffer').html('-');
        $('#txtDiscount').val(0);
        $('#lblTotalDiscount').html(0);
        $('#txtTax').val(0);
        $('#txtCommission').val(0);
        $('#lblTotalCommission').html(0);
        $('#lblFinalPrice').html(0);
        $('#lblTotalPrice').html(0);


        // hide all items except the first one
        $("#Units option").each(function () {
            var thisValue = $(this).val();

            if (thisValue != 0) {
                $(this).hide();
            }
        });
    }

    $('#Units').change(function () {
        calculateAndShowUnitPrice();
        calculateItemPrices();
    });

    function calculateAndShowUnitPrice() {
        var selectedProduct = drpProduct.find(":selected");
        var productUnitRate = Number(selectedProduct.attr('data-unit-rate'));
        var selectedUnitId = drpUnit.find(":selected").val();
        var productMainUnitId = selectedProduct.attr('data-main-unit-id');
        var unitPrice = Number(removeComma($('#txtUnitPrice').val()));

        if (selectedUnitId == 0) {
            $('#txtUnitPrice').val(0);
        }
        else if (productUnitRate > 0) {
            if (productMainUnitId == selectedUnitId) // main unit is selected
            {
                $('#txtUnitPrice').val(addComma(unitPrice / productUnitRate));
            }
            else // subunit is selected
            {
                $('#txtUnitPrice').val(addComma(unitPrice * productUnitRate));
            }
        }
    }


    $('#Products').change(function () {
        if (productExists(factorItems, drpProduct.find(":selected").val())) {
            toastr.error("کالای " + drpProduct.find(":selected").text() + "، قبلا به لیست اضافه شده است!");
            return false;
        }

        var priceTypeId = $("#priceType").val();
        var customerId = $('#Customers').val();

        clearItemFields();

        var selectedProductVal = $(this).val();
        if (selectedProductVal != 0) {
            if (isNaN(customerId) || customerId.toString().trim() == '') {
                toastr.error('ابتدا مشتری را انتخاب کنید.');
                return false;
            }

            $.ajax({
                url: "/Factor/GetSellProductInfo/",
                type: "Get",
                dataType: "Json",
                data: {
                    productId: selectedProductVal,
                    customerId: customerId,
                    priceTypeId: priceTypeId
                },
                success: function (res) {
                    drpProduct.find(":selected").attr('data-available-count', res.AvailableCount);
                    drpProduct.find(":selected").attr('data-main-unit-name', res.MainUnitName);
                    drpProduct.find(":selected").attr('data-sub-unit-name', res.SubUnitName);
                    var availableCountWithCommas = addComma(res.AvailableCount);
                    var countMessage = "موجودی در دسترس کالای " +
                        drpProduct.find(":selected").text() + ": " +
                        availableCountWithCommas + " " +
                        res.MainUnitName;

                    if (res.SubUnitName != null && res.SubUnitName != '') {
                        countMessage += "؛ معادل " +
                            addComma(res.AvailableCount / res.UnitRate) + " " +
                            res.SubUnitName;
                    }

                    toastr.info(countMessage);

                    $("#Units option").each(function () {
                        var thisValue = $(this).val();

                        if (thisValue == res.MainUnitId || thisValue == res.SubUnitId) {
                            $(this).show();
                        }
                    });

                    $('#Units').val(res.MainUnitId); // select mainunit as default item

                    txtUnitPrice.val(addComma(res.UnitPrice));
                    if (chkAssignCommission.is(':checked')) {
                        txtCommission.val(addComma(res.Commission));
                    } else {
                        txtCommission.val(0);
                    }

                    if (chkAssignOffer.is(':checked')) {
                        var discount = res.Discount;
                        if (discount != null) {
                            var offerId = discount.OfferId;
                            var aTag = '<a href="/Offer/Offers/">جشنواره ' + offerId + '</a>';
                            lblOffer.html(aTag);

                            var offerMessage;
                            if (discount.OfferTypeId == 1) // ریالی
                            {
                                drpProduct.find(":selected").attr('data-discount-amount', discount.DiscountAmount);
                                offerMessage =
                                    'کالای «' +
                                    drpProduct.find(":selected").text() +
                                    '» مشمول جشنواره تخفیفات «' +
                                    discount.OfferName +
                                    '» است. میزان تخفیف: ' +
                                    addComma(discount.DiscountAmount) +
                                    ' ریال برای هر ' +
                                    res.MainUnitName +
                                    '.';
                                toastr.info(offerMessage);
                            }
                            else if (discount.OfferTypeId == 2) // درصدی
                            {
                                drpProduct.find(":selected").attr('data-discount-percent', discount.DiscountPercent);
                                offerMessage =
                                    'کالای «' +
                                    drpProduct.find(":selected").text() +
                                    '» مشمول جشنواره تخفیفات «' +
                                    discount.OfferName +
                                    '» است. میزان تخفیف: ' +
                                    addComma(discount.DiscountPercent) +
                                    ' درصد' +
                                    '.';
                                toastr.info(offerMessage);
                            }
                            else if (discount.OfferTypeId == 3) // کالایی
                            {
                                drpProduct.find(":selected").attr('data-discount-gift-product-id', discount.GiftProductId);
                                drpProduct.find(":selected").attr('data-discount-gift-product-name', discount.GiftProductName);
                                drpProduct.find(":selected").attr('data-discount-min-product-count', discount.MinProductCount);
                                drpProduct.find(":selected").attr('data-discount-gift-product-count', discount.GiftProductCount);
                                drpProduct.find(":selected").attr('data-discount-gift-main-unit-id', discount.GiftMainUnitId);
                                drpProduct.find(":selected").attr('data-discount-gift-main-unit-name', discount.GiftMainUnitName);
                                drpProduct.find(":selected").attr('data-discount-gift-available-count', discount.GiftAvailableCount);
                                drpProduct.find(":selected").attr('data-discount-offer-id', discount.OfferId);
                                drpProduct.find(":selected").attr('data-discount-offer-name', discount.OfferName);

                                offerMessage =
                                    'کالای «' +
                                    drpProduct.find(":selected").text() +
                                    '» مشمول جشنواره تخفیفات «' +
                                    discount.OfferName +
                                    '» است. نوع تخفیف از نوع کالایی بوده و با خرید حداقل : ' +
                                    addComma(discount.MinProductCount) +
                                    ' ' +
                                    res.MainUnitName +
                                    ' از کالای «' +
                                    drpProduct.find(":selected").text() +
                                    '»، ' +
                                    addComma(discount.GiftProductCount) +
                                    ' ' +
                                    discount.GiftMainUnitName +
                                    ' از کالای «' +
                                    discount.GiftProductName +
                                    '»، به عنوان هدیه اختصاص خواهد یافت.';
                                toastr.info(offerMessage);
                            }
                        }
                    }

                    calculateItemPrices();

                    txtCount.focus();
                    txtCount.select();
                }
            });
        }
    });

    txtDiscountOnFactor.on("change paste keyup", function () {
        calculateAndShowFactorSums();
    });

    txtAdditions.on("change paste keyup", function () {
        calculateAndShowFactorSums();
    });

    txtDeductions.on("change paste keyup", function () {
        calculateAndShowFactorSums();
    });

    $('#btnSubmit').click(function () {
        var validator = $('#FactorForm').validate();

        var customerId = $('#Customers').val();
        if (isNaN(customerId) || customerId.toString().trim() == '') {
            toastr.error('مشتری را انتخاب کنید.');
            return false;
        }

        if (factorItems.length <= 0) {
            toastr.error('فاکتور بدون آیتم قابل ثبت نیست.');
            return false;
        }

        if ($('#FactorForm').valid()) {
            swal({
                title: "آیا از ثبت فاکتور مطمئن هستید؟",
                icon: "warning",
                buttons: true,
                dangerMode: true,
                buttons: ['انصراف', 'تأیید']
            }).then((isAccepted) => {
                if (isAccepted) {
                    $.ajax({
                        type: "POST",
                        dataType: "Json",
                        url: "/Factor/AddSellFactor",
                        data: {
                            Factor: {
                                CustomerId: customerId,
                                PaperFactorCode: $('#txtPaperFactorCode').val(),
                                TotalPrice: removeComma($('#lblSumTotalPrice').html()),
                                DiscountOnFactor: removeComma($('#txtDiscountOnFactor').val()),
                                TotalDiscount: removeComma($('#lblSumDiscount').html()),
                                Additions: removeComma($('#txtAdditions').val()),
                                Deductions: removeComma($('#txtDeductions').val()),
                                TotalTax: removeComma($('#lblSumTax').html()),
                                FinalPrice: removeComma($('#lblSumFinalPrice').html()),
                                MarketerTotalCommission: removeComma($('#lblSumCommission').html()),
                                Description: $('#txtDescription').val(),
                                UnitId: drpUnit.find(":selected").val()
                            },
                            FactorItems: factorItems,
                            AssignCommission: chkAssignCommission.is(':checked'),
                            AssignOffer: chkAssignOffer.is(':checked'),
                            StrCreateDate: $('#txtCreateDate').val()
                        },
                        success: function (result) {
                            if (result.Success) {
                                var url = '/Factor/SellFactors/';
                                window.location.href = url;
                                var addedFactorId = result.Id;

                                swal({
                                    title: "فاکتور جدید با شناسه " + addedFactorId + "، با موفقیت ثبت شد.",
                                    text: 'در حال انتقال به فهرست فاکتورها...',
                                    icon: "success",
                                    button: "تایید"
                                });

                            } else {
                                toastr.error(result.Message);
                            }
                        },
                        error: function () {
                            toastr.error('خطای سمت سرور!');
                        }
                    });
                }
            });


        } else {
            var messages = validator.errorList;
            console.log(messages);
            for (var i = 0; i < messages.length; i++) {
                toastr.error(messages[i].message);
            }
        }
    });

    $("#txtCreateDate").datepicker({
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
}

function AddBackFactor(isBuy, factorItemsParam) {
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
        };

    var factorItems = factorItemsParam;

    function addComma(num) {
        return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    function removeComma(num) {
        return num.toString().replace(/,/g, '');
    }

    function calculateItemPrices(tr, shouldCalculateTax = true) {
        var lnkProduct = tr.find('#product');
        var txtCount = tr.find('#txtCount');
        var drpUnit = tr.find('#Units');
        var txtUnitPrice = tr.find('#txtUnitPrice');
        var lblTotalPrice = tr.find('#lblTotalPrice');
        var txtDiscount = tr.find('#txtDiscount');
        var lblTotalDiscount = tr.find('#lblTotalDiscount');
        var txtTax = tr.find('#txtTax');
        var txtCommission = tr.find('#txtCommission');
        var lblTotalCommission = tr.find('#lblTotalCommission');
        var lblFinalPrice = tr.find('#lblFinalPrice');
        var vatVal = $('#txtVat').val();

        var count = removeComma(txtCount.val());
        var unitPrice = removeComma(txtUnitPrice.val());
        var discount = removeComma(txtDiscount.val());
        var tax = removeComma(txtTax.val());
        var commission = removeComma(txtCommission.val());

        if (isNaN(count) || count.trim() == '') {
            count = 0;
        }

        if (isNaN(unitPrice) || unitPrice.trim() == '') {
            unitPrice = 0;
        }

        if (isNaN(discount) || discount.trim() == '') {
            discount = 0;
        }

        if (isNaN(tax) || tax.trim() == '') {
            tax = 0;
        }

        if (isNaN(commission) || commission.trim() == '') {
            commission = 0;
        }

        var selectedUnitId = drpUnit.find(":selected").val();
        var productId = lnkProduct.attr('data-product-id');
        //var productMainUnitId = lnkProduct.attr('data-main-unit-id');
        //var productUnitRate = lnkProduct.attr('data-unit-rate');
        var calculatedCount = count;

        //if (productMainUnitId == selectedUnitId) // main unit is selected
        //{
        //    calculatedCount = count;
        //}
        //else // subunit is selected
        //{
        //    calculatedCount = count * productUnitRate;
        //}

        var totalPrice = calculatedCount * unitPrice;
        if (isNaN(totalPrice)) {
            totalPrice = 0;
        }

        var totalDiscount = calculatedCount * discount;
        if (isNaN(totalDiscount)) {
            totalDiscount = 0;
        }

        lblTotalDiscount.html(addComma(totalDiscount));

        lblTotalPrice.html(addComma(totalPrice.toFixed(0)));
        var calculatedTax;
        if (shouldCalculateTax) {
            calculatedTax = Math.round((totalPrice - totalDiscount) * (vatVal / 100.0));
        } else {
            calculatedTax = tax;
        }
        if (isNaN(calculatedTax)) {
            calculatedTax = 0;
        }
        txtTax.val(addComma(calculatedTax));

        var totalCommission = calculatedCount * Number(commission);
        if (isNaN(totalCommission)) {
            totalCommission = 0;
        }
        lblTotalCommission.html(addComma(totalCommission));

        var finalPrice = Number(totalPrice) - Number(totalDiscount) + Number(calculatedTax);

        if (isNaN(finalPrice)) {
            finalPrice = 0;
        }

        lblFinalPrice.html(addComma(finalPrice.toFixed()));

        var itemIndex = factorItems.findIndex(i => i.ProductId == productId);

        factorItems[itemIndex].Count = Number(calculatedCount);
        factorItems[itemIndex].UnitId = Number(selectedUnitId);
        factorItems[itemIndex].UnitPrice = Number(unitPrice);
        factorItems[itemIndex].TotalPrice = Number(totalPrice);
        factorItems[itemIndex].Discount = Number(discount);
        factorItems[itemIndex].TotalDiscount = Number(totalDiscount);
        factorItems[itemIndex].Tax = Number(calculatedTax);
        factorItems[itemIndex].Commission = Number(commission);
        factorItems[itemIndex].TotalCommission = Number(totalCommission);
        factorItems[itemIndex].FinalPrice = finalPrice;
    }

    function calculateAndShowFactorSums() {
        var sumTotalPrice = 0;
        var sumTotalDiscount = 0;
        var sumTax = 0;
        var sumTotalCommission = 0;
        var discountOnFactor = Number(removeComma($('#txtDiscountOnFactor').val()));
        var additions = Number(removeComma($('#txtAdditions').val()));
        var deductions = Number(removeComma($('#txtDeductions').val()));

        if (isNaN(discountOnFactor) || discountOnFactor.toString().trim() == '') {
            discountOnFactor = 0;
        }

        if (isNaN(additions) || additions.toString().trim() == '') {
            additions = 0;
        }

        if (isNaN(deductions) || deductions.toString().trim() == '') {
            deductions = 0;
        }

        for (var i = 0; i < factorItems.length; i++) {
            var theItem = factorItems[i];

            sumTotalPrice += Number(theItem.TotalPrice);
            sumTotalDiscount += Number(theItem.TotalDiscount);
            sumTax += Number(theItem.Tax);
            sumTotalCommission += Number(theItem.TotalCommission);
        }

        var sumFinalPrice = sumTotalPrice - discountOnFactor - sumTotalDiscount + sumTax - deductions + additions;
        $('#lblSumTotalPrice').html(addComma(sumTotalPrice));
        $('#lblSumDiscount').html(addComma(sumTotalDiscount));
        $('#lblSumTax').html(addComma(sumTax));
        $('#lblSumCommission').html(addComma(sumTotalCommission));
        $('#lblDiscountOnFactor').html(addComma(discountOnFactor));
        $('#lblAdditions').html(addComma(additions));
        $('#lblDeductions').html(addComma(deductions));
        $('#lblSumFinalPrice').html(addComma(sumFinalPrice));
    }

    $('#tblItems > tr').each(function () {

        var txtUnitPrice = $(this).find('#txtUnitPrice');
        var txtCommission = $(this).find('#txtCommission');
        var txtCount = $(this).find('#txtCount');
        var txtDiscount = $(this).find('#txtDiscount');
        var txtTax = $(this).find('#txtTax');
        var drpUnit = $(this).find('#Units');
        var btnRemoveItem = $(this).find('#btnRemoveItem');

        txtUnitPrice.on('change paste keyup',
            function () {
                calculateItemPrices($(this).closest('tr'));
                calculateAndShowFactorSums();
            });

        txtCommission.on('change paste keyup',
            function () {
                calculateItemPrices($(this).closest('tr'));
                calculateAndShowFactorSums();
            });

        txtCount.on('change paste keyup',
            function () {
                calculateItemPrices($(this).closest('tr'));
                calculateAndShowFactorSums();
            });

        txtDiscount.on('change paste keyup',
            function () {
                calculateItemPrices($(this).closest('tr'));
                calculateAndShowFactorSums();
            });

        txtTax.on('change paste keyup',
            function () {
                calculateItemPrices($(this).closest('tr'), false);
                calculateAndShowFactorSums();
            });

        drpUnit.change(function () {
            calculateAndShowUnitPrice($(this).closest('tr'));
            calculateItemPrices($(this).closest('tr'));
            calculateAndShowFactorSums();
        });

        function calculateAndShowUnitPrice(tr) {
            var selectedProduct = tr.find('#product');
            var productUnitRate = Number(selectedProduct.attr('data-unit-rate'));
            var selectedUnitId = tr.find('#Units').find(":selected").val();
            var productMainUnitId = selectedProduct.attr('data-main-unit-id');
            var unitPrice = Number(removeComma(tr.find('#txtUnitPrice').val()));

            if (selectedUnitId == 0) {
                tr.find('#txtUnitPrice').val(0);
            }
            else if (productUnitRate > 0) {
                if (productMainUnitId == selectedUnitId) // main unit is selected
                {
                    tr.find('#txtUnitPrice').val(addComma(unitPrice / productUnitRate));
                }
                else // subunit is selected
                {
                    tr.find('#txtUnitPrice').val(addComma(unitPrice * productUnitRate));
                }
            }
        }

        btnRemoveItem.on('click',
            function () {
                swal({
                    title: "آیا می خواهید این آیتم را از جدول حذف کنید؟",
                    icon: "warning",
                    buttons: true,
                    dangerMode: true,
                    buttons: ['انصراف', 'تأیید']
                }).then((isAccepted) => {
                    if (isAccepted) {
                        $(this).closest('tr').remove();
                        var id = $(this).attr('data-product-id');
                        var theItem = factorItems.find(i => i.ProductId == id);
                        factorItems.splice(factorItems.indexOf(theItem), 1);
                        setRowNums();
                        calculateAndShowFactorSums();
                    }
                });
            });
    });

    function setRowNums() {
        $('.tdIndex').each(function (i) {
            $(this).html(addComma(i + 1));
        });
    }

    $(document).on("keyup", ".justNumber", function () {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {

            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                .replace(/^0+/, '');
        });
    });

    $('#txtDiscountOnFactor').on("change paste keyup", function () {
        calculateAndShowFactorSums();
    });

    $('#txtAdditions').on("change paste keyup", function () {
        calculateAndShowFactorSums();
    });

    $('#txtDeductions').on("change paste keyup", function () {
        calculateAndShowFactorSums();
    });

    $("#txtCreateDate").datepicker({
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

    $('#btnSubmit').click(function () {


        var validator = $('#FactorForm').validate();

        if (factorItems.length <= 0) {
            toastr.error('فاکتور بدون آیتم قابل ثبت نیست.');
            return false;
        }

        if ($('#FactorForm').valid()) {
            var postUrl;
            if (isBuy) {
                postUrl = '/Factor/AddBuyBackFactor';
            } else {
                postUrl = '/Factor/AddSellBackFactor';
            }
            swal({
                title: "آیا از ثبت فاکتور برگشتی مطمئن هستید؟",
                icon: "warning",
                buttons: true,
                dangerMode: true,
                buttons: ['انصراف', 'تأیید']
            }).then((isAccepted) => {
                if (isAccepted) {
                    $.ajax({
                        type: "POST",
                        dataType: "Json",
                        url: postUrl,
                        data: {
                            Factor: {
                                Id: $('#txtId').val(),
                                PeriodId: $('#txtPeriodId').val(),
                                TotalPrice: removeComma($('#lblSumTotalPrice').html()),
                                DiscountOnFactor: removeComma($('#txtDiscountOnFactor').val()),
                                TotalDiscount: removeComma($('#lblSumDiscount').html()),
                                Additions: removeComma($('#txtAdditions').val()),
                                Deductions: removeComma($('#txtDeductions').val()),
                                TotalTax: removeComma($('#lblSumTax').html()),
                                FinalPrice: removeComma($('#lblSumFinalPrice').html()),
                                MarketerTotalCommission: removeComma($('#lblSumCommission').html()),
                                Description: $('#txtDescription').val()
                            },
                            FactorItems: factorItems,
                            StrCreateDate: $('#txtCreateDate').val()
                        },
                        success: function (result) {
                            if (result.Success) {
                                var successUrl;
                                if (isBuy) {
                                    successUrl = '/Factor/BuyBackFactors/';
                                } else {
                                    successUrl = '/Factor/SellBackFactors/';
                                }

                                var url = successUrl;
                                window.location.href = url;
                                var addedFactorId = result.Id;

                                swal({
                                    title: "فاکتور برگشتی جدید با شناسه " + addedFactorId + "، با موفقیت ثبت شد.",
                                    text: 'در حال انتقال به فهرست فاکتورها...',
                                    icon: "success",
                                    button: "تایید"
                                });

                            } else {
                                toastr.error(result.Message);
                            }
                        },
                        error: function () {
                            toastr.error('خطای سمت سرور!');
                        }
                    });
                }
            });
        } else {
            var messages = validator.errorList;
            console.log(messages);
            for (var i = 0; i < messages.length; i++) {
                toastr.error(messages[i].message);
            }
        }
    });
}

function factorDetailsWithoutPrices() {
    var factorId = $('#factorId').val();
    var btnInvoice = $('#btnInvoice');
    var theRedTr = $('.wrongTd').closest('tr');

    theRedTr.addClass('deleted');

    if (theRedTr.length > 0) {
        $('#countError').show();
        btnInvoice.attr("disabled", true);
    }

    btnInvoice.on('click',
        function () {
            $.get("/Factor/InvoiceModal/" + factorId,
                function (res) {
                    $('#factorModal').modal('toggle'); // close current factor modal

                    $("#invoiceModal").modal(); // open invoice modal
                    $("#invoiceModalBody").html(res);
                });
        });
}

function ShowFactorDetails(factorId, periodId) {
    $.ajax({
        url: "/Factor/ShowFactorDetails",
        type: "Get",
        data: { id: factorId, periodId: periodId },
        success: function (res) {
            $('#ShowFactorModal').modal();
            $('#ShowFactorModalBody').html(res);
        }
    });
}

function ShowFactorDetailsWithoutPrices(factorId, periodId) {

    $.ajax({
        url: "/Factor/FactorDetailsWithoutPrices",
        type: "Get",
        data: { id: factorId, periodId: periodId },
        success: function (res) {
            $('#factorModal').modal();
            $('#factorModalBody').html(res);
        }
    });
}