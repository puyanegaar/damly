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