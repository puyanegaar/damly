function addSupplier() {
	$(document).on("change",
		"#Supplier_ProvinceId",
		function () {
			$.ajax({
				url: "/Supplier/GetCities",
				data: { id: $("#Supplier_ProvinceId").find(":selected").val() },
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
}

function supplierManagement() {
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
}

function updateSupplier() {
	$(document).on("change",
		"#States",
		function () {
			$.ajax({
				url: "/Supplier/GetCities",
				data: { id: $("#States").find(":selected").val() },
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