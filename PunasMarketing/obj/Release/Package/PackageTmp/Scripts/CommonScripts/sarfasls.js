function sarfasls() {
    $("#btnAddSarfasl").click(function () {
        $.get("/Sarfasl/AddSarfaslModal/", function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });

    $(".btn-update-sarfasl").click(function () {
        $.get("/Sarfasl/UpdateSarfaslModal/" + $(this).attr('data-sarfasl-id'), function (res) {
            $("#modal").modal();
            $("#modalBody").html(res);
        });
    });
}

function deleteSarfasl() {
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

function addSarfasl(sarfasls) {
    $(document).ready(function () {
        var txtCode = $('#txtCode');
        var txtCoding = $('#txtCoding');
        var moeenCodeWarning = $('#moeenCodeWarning');
        var kolCodeWarning = $('#kolCodeWarning');
        var kolDiv = $('#kolDiv');
        var drpKolSarfasls = $('#drpKolSarfasls');
        var drpGroupSarfasls = $('#drpGroupSarfasls');
        var lblCodeDigitCountError = $('#lblCodeDigitCountError');
        var lblCodeExistsError = $('#lblCodeExistsError');

        lblCodeDigitCountError.hide();
        lblCodeExistsError.hide();
        moeenCodeWarning.hide();
        $('#kolDiv').find('*').css("visibility", "hidden");

        $('input[type=radio][name=sarfaslLevel]').change(function () {
            txtCode.val("");
            lblCodeDigitCountError.hide();

            if (this.value === 'kol') {
                kolCodeWarning.show();
                moeenCodeWarning.hide();
                kolDiv.find('*').css("visibility", "hidden");

                txtCode.attr("maxLength", 1);
            }
            else if (this.value === 'moeen') {
                kolCodeWarning.hide();
                moeenCodeWarning.show();
                kolDiv.find('*').css("visibility", "visible");

                txtCode.attr("maxLength", 2);
            }
        });

        $(document).on("change",
            "#drpGroupSarfasls",
            function () {
                if (drpGroupSarfasls.prop('selectedIndex') === 0) {
                    txtCode.val("");
                }
                else {
                    var newSarfaslCode = getNewSarfaslCode(sarfasls, drpGroupSarfasls.find(":selected").val());
                    txtCode.val(newSarfaslCode);
                }

                $.ajax({
                    url: "/Sarfasl/GetKolSarfasls",
                    data: { id: drpGroupSarfasls.find(":selected").val() },
                    type: "Post",
                    dataType: "Json",
                    success: function (result) {
                        drpKolSarfasls.html(result.Html);
                        setCoding();
                    },
                    error: function () {
                        alert("خطا!");
                    }
                });
            });

        $(document).on('change',
            '#drpKolSarfasls',
            function () {
                if (drpKolSarfasls.prop('selectedIndex') === 0) {
                    $('#kolErrorMessage').css("visibility", "visible");
                    txtCode.val("");
                } else {
                    $('#kolErrorMessage').css("visibility", "hidden");

                    var newSarfaslCode = getNewSarfaslCode(sarfasls, $("#drpKolSarfasls").find(":selected").val());
                    var formattedNumber = ("0" + newSarfaslCode).slice(-2);
                    txtCode.val(formattedNumber);
                }
                setCoding();
            });

        txtCode.change(function () {
            lblCodeDigitCountError.hide();
            setCoding();
        });

        function setCoding() {
            var txtCodeVal = txtCode.val();
            if ($.isNumeric(txtCodeVal)) {
                var coding;

                if ($('#rbnMoeen').is(':checked')) {
                    coding = getCoding($('#drpKolSarfasls option:selected').text());
                } else {
                    coding = getCoding($('#drpGroupSarfasls option:selected').text());
                }
                txtCoding.val(coding.trim() + txtCodeVal.trim());

            } else {
                txtCoding.val("");
            }
        }

        $('#btnSubmit').click(function () {
            if ($('#rbnMoeen').is(':checked')) {
                var txtCodeValLength = txtCode.val().length;

                if (drpKolSarfasls.prop('selectedIndex') === 0) {
                    $('#kolErrorMessage').css("visibility", "visible");
                    return false;
                }

                if (txtCode.val().trim() !== "" && $.isNumeric(txtCode.val()) && txtCodeValLength < 2) {
                    lblCodeExistsError.hide();
                    lblCodeDigitCountError.show();
                    return false;
                }
                else {
                    lblCodeDigitCountError.hide();
                }
            }

            var parentId;
            if ($('#rbnMoeen').is(':checked')) {
                parentId = $('#drpKolSarfasls option:selected').val();
            } else {
                parentId = $('#drpGroupSarfasls option:selected').val();
            }

            var code = $('#txtCode').val();

            if (codeExists(sarfasls, parentId, code)) {
                lblCodeDigitCountError.hide();
                lblCodeExistsError.show();

                return false;
            } else {
                lblCodeExistsError.hide();
            }

            $('#kolErrorMessage').css("visibility", "hidden");
            return true;
        });
    });
}

function updateSarfasl(sarfasls, isMoeen) {
    $(document).ready(function () {
        var txtCode = $('#txtCode');
        var txtCoding = $('#txtCoding');
        var moeenCodeWarning = $('#moeenCodeWarning');
        var kolCodeWarning = $('#kolCodeWarning');
        var kolDiv = $('#kolDiv');
        var drpKolSarfasls = $('#drpKolSarfasls');
        var drpGroupSarfasls = $('#drpGroupSarfasls');
        var lblCodeDigitCountError = $('#lblCodeDigitCountError');
        var lblCodeExistsError = $('#lblCodeExistsError');
        var kolErrorMessage = $('#kolErrorMessage');

        lblCodeDigitCountError.hide();
        lblCodeExistsError.hide();

        fillDrpKolSarfasls();

        if (isMoeen) {
            kolCodeWarning.hide();
            moeenCodeWarning.show();
            kolDiv.find('*').css("visibility", "visible");

            txtCode.attr("maxLength", 2);

            kolErrorMessage.css("visibility", "hidden");
        }
        else {
            moeenCodeWarning.hide();
            $('#kolDiv').find('*').css("visibility", "hidden");
        }

        $('input[type=radio][name=sarfaslLevel]').change(function () {
            txtCode.val("");
            lblCodeDigitCountError.hide();

            if (this.value === 'kol') {
                kolCodeWarning.show();
                moeenCodeWarning.hide();
                kolDiv.find('*').css("visibility", "hidden");

                txtCode.attr("maxLength", 1);
            }
            else if (this.value === 'moeen') {
                kolCodeWarning.hide();
                moeenCodeWarning.show();
                kolDiv.find('*').css("visibility", "visible");

                txtCode.attr("maxLength", 2);
            }
        });

        $(document).on("change",
            "#drpGroupSarfasls",
            function () {
                if (drpGroupSarfasls.prop('selectedIndex') === 0) {
                    txtCode.val("");
                }
                else {
                    var newSarfaslCode = getNewSarfaslCode(sarfasls, drpGroupSarfasls.find(":selected").val());
                    txtCode.val(newSarfaslCode);
                }

                fillDrpKolSarfasls();
            });

        $(document).on('change',
            '#drpKolSarfasls',
            function () {
                if (drpKolSarfasls.prop('selectedIndex') === 0) {
                    kolErrorMessage.css("visibility", "visible");
                    txtCode.val("");
                }
                else {
                    kolErrorMessage.css("visibility", "hidden");

                    var newSarfaslCode = getNewSarfaslCode(sarfasls, drpKolSarfasls.find(":selected").val());
                    var formattedNumber = ("0" + newSarfaslCode).slice(-2);
                    txtCode.val(formattedNumber);
                }

                setCoding();
            });

        txtCode.on('change keydown paste input', function () {
            lblCodeDigitCountError.hide();
            setCoding();
        });

        function setCoding() {
            var txtCodeVal = txtCode.val().trim();
            if ($.isNumeric(txtCodeVal)) {
                var coding;

                if ($('#rbnMoeen').is(':checked')) {
                    if (txtCodeVal.length === 1 && drpKolSarfasls.prop('selectedIndex') !== 0) {
                        txtCodeVal = "0" + txtCodeVal;
                    }
                    coding = getCoding($('#drpKolSarfasls option:selected').text());
                }
                else {
                    coding = getCoding($('#drpGroupSarfasls option:selected').text());
                }
                txtCoding.val(coding.trim() + txtCodeVal);

            } else {
                txtCoding.val("");
            }
        }

        $('#btnSubmit').click(function () {
            if ($('#rbnMoeen').is(':checked')) {
                var txtCodeValLength = txtCode.val().length;

                if (drpKolSarfasls.prop('selectedIndex') === 0) {
                    kolErrorMessage.css("visibility", "visible");
                    return false;
                }

                if (txtCode.val().trim() !== "" && $.isNumeric(txtCode.val()) && txtCodeValLength < 2) {
                    lblCodeExistsError.hide();
                    lblCodeDigitCountError.show();
                    return false;
                }
                else {
                    lblCodeDigitCountError.hide();
                }
            }

            var parentId;
            if ($('#rbnMoeen').is(':checked')) {
                parentId = $('#drpKolSarfasls option:selected').val();
            } else {
                parentId = $('#drpGroupSarfasls option:selected').val();
            }

            var newCode = $('#txtCode').val();
            var oldCode = $('#oldCode').val();

            if (newCode != oldCode && codeExists(sarfasls, parentId, newCode)) {
                lblCodeDigitCountError.hide();
                lblCodeExistsError.show();

                return false;
            } else {
                lblCodeExistsError.hide();
            }

            kolErrorMessage.css("visibility", "hidden");
            return true;
        });

        function fillDrpKolSarfasls() {
            $.ajax({
                url: "/Sarfasl/GetKolSarfasls",
                data: { id: drpGroupSarfasls.find(":selected").val(), sarfaslId: $('#SarfaslId').val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    drpKolSarfasls.html(result.Html);
                    setCoding();
                },
                error: function () {
                    alert("خطا در دریافت اطلاعات حساب های کل!");
                }
            });
        }
    });
}

function getNewSarfaslCode(sarfasls, parentId) {
    var sarfaslsByParentId = [];
    for (var i = 0; i < sarfasls.length; i++) {
        if (sarfasls[i].ParentId == parentId) {
            sarfaslsByParentId.push(sarfasls[i]);
        }
    }

    var lastCode = 0;
    if (sarfaslsByParentId.length > 0) {
        lastCode = Math.max.apply(Math, sarfaslsByParentId.map(function (o) { return o.Code }));
    }
    return lastCode + 1;
}

function codeExists(sarfasls, parentId, code) {
    var exists = false;
    for (var i = 0; i < sarfasls.length; i++) {
        if (sarfasls[i].ParentId == parentId && sarfasls[i].Code == code) {
            exists = true;
            break;
        }
    }
    return exists;
}

function getCoding(codingAndName) {
    var splitted = codingAndName.split('-');
    return splitted[0];
}

