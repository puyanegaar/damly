function otherTafsilis() {

	$("#btnAddOtherTafsili").click(function () {
		$.get("/OtherTafsili/AddOtherTafsiliModal/", function (res) {
			$("#modal").modal();
			$("#modalBody").html(res);
		});
	});

	$(".btn-update-other-tafsili").click(function () {
		$.get("/OtherTafsili/UpdateOtherTafsiliModal/" + $(this).attr('data-id'), function (res) {
			$("#modal").modal();
			$("#modalBody").html(res);
		});
	});
}