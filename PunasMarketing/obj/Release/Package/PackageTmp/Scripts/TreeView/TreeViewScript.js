$(document).on("ready", function () {
    $(".Sarfasltreeview li > ul").css('display','none'); // Hide all 2-level ul 
    $(document).on("click", ".collapsible", function (e) {
        e.preventDefault();
        $(this).toggleClass("Sarfaslcollapse Sarfaslexpand");
        $(this).closest('li').children('ul').slideToggle();
    });
});

$(document).on("click", "#selType li span a#GroupName", function () {
    $("#GroupParent").val($(this).text().trim());
    $("#Group_GroupParentId").val($(this).attr("value"));
    var parent = $(this).attr("parentid");
  //  alert($(this).parents('li').last().find('a#GroupName').attr('value'));
    if (parent != null) {
        $("#Group_GroupParentId").attr("data", $(this).attr("parentid"));
    }
    else {
        $("#Group_GroupParentId").attr("data", "");
    }
 
});

$(document).on("click", "#selType li span a#GroupName", function () { // when li is clicked
    if ($("#MainGroup").prop("checked") == false) {

        $(this).addClass("sel")
        $(this).parents().siblings().find("a").removeClass("sel"); // swap class

    }


});

$(document).on("change", "#MainGroup", function () { // when li is clicked
    $('#DivParent').slideToggle("slow", function () {
      
    });
    $("#Group_GroupParentId").attr("value", "");
    $("#selType li span a#GroupName").parents().siblings().find("a").removeClass("sel"); // swap class
    $("#GroupParent").val("");
  
  
   
});


$(document).on("click", "#selType li span a.EditGroup", function (e) {
    $("#selType li span a#GroupName").parents().siblings().find("a").removeClass("sel"); // swap class
    $("#EGroup").modal({ backdrop: true });
    $("#EditGroupName").val($(this).attr("value"));
    $("#EditGroupParetId").val($(this).attr("parentid"))
    $("#EditGroupId").val($(this).attr("GroupId"))
    if ($(this).is("[parentid]")) {

        $("#EditGroupParent").prop("checked", false);
    }
    else {
        $("#EditGroupParent").prop("checked", true);

    }

});

$(document).on("click", "#selType li span a.DeleteGroup", function (e) {

    DeleteGroup($(this).attr("GroupId"));
});

$(document).on("click", "#selType li span a.VisbilityGroup", function (e) {

    Visibility($(this).attr("GroupId"));
});

$("#EGroup").on('hidden.bs.modal', function () {
    $("#EditGroupParent").prop("checked", false);
});
