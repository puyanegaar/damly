$(document).ready(function () {
    $(document).ajaxStart(function () {
        $("#progress").css("display", "block");
    });
    $(document).ajaxComplete(function () {
        $("#progress").css("display", "none");
    });
});
$(document).ajaxError(function (e, xhr) {
    if (xhr.status == 403) {
        var response = $.parseJSON(xhr.responseText);
        window.location = response.Url;
    }
});
