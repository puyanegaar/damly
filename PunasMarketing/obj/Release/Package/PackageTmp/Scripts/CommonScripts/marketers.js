function addMarketer() {

    $("#birthDate").datepicker({
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

    $("#hireDate").datepicker({
        beforeShow: function (input, inst) {
            var cal = inst.dpDiv;
            var left = $(this).offset().left;
            setTimeout(function () {
                cal.css({
                    'left': left
                });
            }, 10);
        }
    });

    $(document).on("change",
        "#Marketer_BirthStateId",
        function () {
            $.ajax({
                url: "/Marketer/GetBirthCities",
                data: { id: $("#Marketer_BirthStateId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpBirthCities").html(result.Html);
                    if (result.Success) {
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });

    $(document).on("change",
        "#Marketer_HomeStateId",
        function () {
            $.ajax({
                url: "/Marketer/GetHomeCities",
                data: { id: $("#Marketer_HomeStateId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpHomeCities").html(result.Html);
                    if (result.Success) {
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });

    $(document).on("change",
        "#Marketer_SectionId",
        function () {
            $.ajax({
                url: "/Marketer/GetJobTilte",
                data: { id: $("#Marketer_SectionId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpJobTitleId").html(result.Html);
                    if (result.Success) {
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });
}

function updateMarketer() {
    $("#birthDate").datepicker({
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
        changeMonth: true
    });

    $("#hireDate").datepicker({
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
        changeMonth: true
    });

    $("#fireDate").datepicker({
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
        changeMonth: true
    });

    $(document).on("change",
        "#Marketer_BirthStateId",
        function () {
            $.ajax({
                url: "/Marketer/GetBirthCities",
                data: { id: $("#Marketer_BirthStateId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpBirthCities").html(result.Html);
                    if (result.Success) {
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });

    $(document).on("change",
        "#Marketer_HomeStateId",
        function () {
            $.ajax({
                url: "/Marketer/GetHomeCities",
                data: { id: $("#Marketer_HomeStateId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpHomeCities").html(result.Html);
                    if (result.Success) {
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });

    $(document).on("change",
        "#Marketer_SectionId",
        function () {
            $.ajax({
                url: "/Marketer/GetJobTilte",
                data: { id: $("#Marketer_SectionId").find(":selected").val() },
                type: "Post",
                dataType: "Json",
                success: function (result) {
                    $("#DrpJobTitleId").html(result.Html);
                    if (result.Success) {
                    }
                },
                error: function () {
                    alert("خطا!");
                }
            });
        });
}