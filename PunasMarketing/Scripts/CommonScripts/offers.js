function addOffer() {

	$("#startDate").datepicker({
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

	$("#expDate").datepicker({
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

	$('#AddProductToOfferButton').click(function () {
		$('#OfferItemForm input[type = text]').val("");
		$('#OfferItems_GiftProductId').prop('selectedIndex', 0);
		$('#OfferType').prop('selectedIndex', 0);
		$('#OfferItems_ProductId').prop('selectedIndex', 0);
		$('#offerModal').modal();
    });

	$(function () {
		$("#DiscountAmount").hide();
		$("#DiscountPercent").hide();
		$(".prooffer").hide();
		$('.cusval').hide();
    });

    $("#OfferType").change(function () {
        var offerType = $("#OfferType").val();

        switch (offerType) {
            case "1":
                $("#DiscountAmount").show();
                $("#DiscountPercent").hide();
                $(".prooffer").hide();
                $('#OfferItemForm input[type = text]').val("");
                $('#OfferItems_GiftProductId').prop('selectedIndex', 0);
                $('.cusval').hide();
                break;

            case "2":
                $("#DiscountAmount").hide();
                $(".prooffer").hide();
                $("#DiscountPercent").show();
                $('#OfferItemForm input[type = text]').val("");
                $('#OfferItems_GiftProductId').prop('selectedIndex', 0);
                $('.cusval').hide();
                break;

            case "3":
                $("#DiscountAmount").hide();
                $("#DiscountPercent").hide();
                $(".prooffer").show();
                $('#OfferItemForm input[type = text]').val("");
                $('.cusval').hide();
                break;

            default:
                $("#DiscountAmount").hide();
                $("#DiscountPercent").hide();
                $(".prooffer").hide();
                $('#OfferItemForm input[type = text]').val("");
                $('#OfferItems_GiftProductId').prop('selectedIndex', 0);
                $('.cusval').hide();
        }

    });

    $('#AddOfferItemButton').click(function () {
        var offerType = $("#OfferType").val();
        switch (offerType) {
            case "0":
                $('#OfferTypeErrorMessage').show();
                return false;
                break;
            case "1":
                if ($('#OfferItems_DiscountAmount').val() === "") {
                    $('#DiscountAmountErrorMessage').show();
                    return false;
                }
                break;

            case "2":
                if ($('#OfferItems_DiscountPercent').val() === "") {
                    $('#DiscountPercentErrorMessage').show();
                    return false;
                }
                break;
            case "3":
                if ($('#OfferItems_MinProductCount').val() === "" || $('#OfferItems_GiftProductCount').val() === "" || $('#OfferItems_GiftProductId').prop('selectedIndex') === 0) {
                    $('#MinProductCountErrorMessage').show();
                    $('#GiftProductIdErrorMessage').show();
                    $('#GiftProductCountErrorMessage').show();
                    return false;
                }

                break;
            default:
                return false;
        }
    });
}

function updateOffer() {
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
		changeYear: true,
		yearRange: '1350:1420',
		changeMonth: true,
	});

	$("#ExpDate").datepicker({
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

    $('#AddProductToOfferButton').click(function () {
        $('#OfferItemForm input[type = text]').val("");
        $('#OfferItems_GiftProductId').prop('selectedIndex', 0);
        $('#OfferType').prop('selectedIndex', 0);
        $('#OfferItems_ProductId').prop('selectedIndex', 0);
        $('#offerModal').modal();
    });

    $(function () {
        $("#DiscountAmount").hide();
        $("#DiscountPercent").hide();
        $(".prooffer").hide();
        $('.cusval').hide();


    });

    $("#OfferType").change(function () {
        var offerType = $("#OfferType").val();

        switch (offerType) {
            case "1":
                $("#DiscountAmount").show();
                $("#DiscountPercent").hide();
                $(".prooffer").hide();
                $('#OfferItemForm input[type = text]').val("");
                $('#OfferItems_GiftProductId').prop('selectedIndex', 0);
                $('.cusval').hide();
                break;

            case "2":
                $("#DiscountAmount").hide();
                $(".prooffer").hide();
                $("#DiscountPercent").show();
                $('#OfferItemForm input[type = text]').val("");
                $('#OfferItems_GiftProductId').prop('selectedIndex', 0);
                $('.cusval').hide();


                break;

            case "3":
                $("#DiscountAmount").hide();
                $("#DiscountPercent").hide();
                $(".prooffer").show();
                $('#OfferItemForm input[type = text]').val("");
                $('.cusval').hide();
                break;
            default:
                $("#DiscountAmount").hide();
                $("#DiscountPercent").hide();
                $(".prooffer").hide();
                $('#OfferItemForm input[type = text]').val("");
                $('#OfferItems_GiftProductId').prop('selectedIndex', 0);
                $('.cusval').hide();
        }

    });

    $('#AddOfferItemButton').click(function () {
        var offerType = $("#OfferType").val();
        switch (offerType) {
            case "0":
                $('#OfferTypeErrorMessage').show();
                return false;
                break;
            case "1":
                if ($('#OfferItems_DiscountAmount').val() === "") {
                    $('#DiscountAmountErrorMessage').show();
                    return false;
                }
                break;

            case "2":
                if ($('#OfferItems_DiscountPercent').val() === "") {
                    $('#DiscountPercentErrorMessage').show();
                    return false;
                }
                break;
            case "3":
                if ($('#OfferItems_MinProductCount').val() === "" ||
                    $('#OfferItems_GiftProductCount').val() === "" ||
                    $('#OfferItems_GiftProductId').prop('selectedIndex') === 0) {
                    $('#MinProductCountErrorMessage').show();
                    $('#GiftProductIdErrorMessage').show();
                    $('#GiftProductCountErrorMessage').show();
                    return false;
                }

                break;
            default:
                return false;
        }
    });

    $(document).on("click", "#offerItemsListInUpdateMode tr td a#Delete", function () {
	    var Row = $(this);
	    var id = $(this).attr('value');


	    $.post("/Offer/DeleteOfferItemInUpdateMode/" + id,
		    function (result) {
			    if (result.Success) {
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
    });
}

function CloseOfferModal() {
	$("#offerModal").modal('hide');
	$("#DiscountAmount").hide();
	$("#DiscountPercent").hide();
	$(".prooffer").hide();
}

function DeleteOfferItem(id) {
	$.get("/Offer/DeleteOfferItem/" + id,
		function (res) {
			$("#offeritemlist").html(res);
		});
}