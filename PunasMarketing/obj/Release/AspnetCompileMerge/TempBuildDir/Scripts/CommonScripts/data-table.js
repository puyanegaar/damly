$(document).ready(function () {
	var dataTable = $('#demo-datatables-1').dataTable();
	$("#searchbox").keyup(function () {
		dataTable.fnFilter(this.value);
	});
});

$(function () {
	$("#demo-datatables-1").dataTable();
});
$(document).ready(function () {
	$("#demo-datatables-1_info").parent().addClass('col-sm-12');
});