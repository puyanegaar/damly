function addWarehouse() {
	$(document).on("change",
		"#JobTitles",
		function () {
			var defaultHtml = '<select class="md-form-control" id="Personnels" name="Personnels"><option value="">ابتدا عنوان شغلی را انتخاب کنید</option></select><label class="md-control-label" for="">کاربر</label><span class="has-error md-help-block valError"><span class="field-validation-valid" data-valmsg-for="Warehouse.KeeperId" data-valmsg-replace="true"></span></span>';
			var selectedIndex = $("#JobTitles").prop('selectedIndex');

			if (selectedIndex != 0) {
				$.ajax({
					url: "/Warehouse/GetPersonnels",
					data: { id: $("#JobTitles").find(":selected").val() },
					type: "Post",
					dataType: "Json",
					success: function (result) {
						$("#DrpPersonnels").html(result.Html);
						if (result.Success) {

						}
					},
					error: function () {
						$("#DrpPersonnels").html(defaultHtml);
					}
				});
			} else {
				$("#DrpPersonnels").html(defaultHtml);
			}
		});
}

function updateWarehouse() {
	$(document).on("change",
		"#JobTitles",
		function () {
			var selectedIndex = $("#JobTitles").prop('selectedIndex');

			if (selectedIndex != 0) {
				$.ajax({
					url: "/Warehouse/GetPersonnels",
					data: { id: $("#JobTitles").find(":selected").val() },
					type: "Post",
					dataType: "Json",
					success: function (result) {
						$("#DrpPersonnels").html(result.Html);
						if (result.Success) {

						}
					},
					error: function () {
						$("#DrpPersonnels").html('<select class="md-form-control" id="Personnels" name="Personnels"><option value="">کاربر</option></select>');
					}
				});
			} else {
				$("#DrpPersonnels").html('<select class="md-form-control" id="Personnels" name="Personnels"><option value="">کاربر</option></select>');
			}
		});
}

function createsuccess(data) {
	window.location.href = 'Suppliers';
}