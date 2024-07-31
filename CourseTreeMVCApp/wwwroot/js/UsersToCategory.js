$(function () {

    var errorText = "An error has occured. An administrator has been notified.Please try again later.";

    //This comes from UsersToCategory/Index.cshtml page
    $("button[name='SaveSelectedUsers']").prop('disabled', true);

    //The first method is triggered when the dropdown list that contains all categories saved to the system is changed when a different category is selected by the admin
    $('select').on('change', function () {
        //We can populate the user checkbox list view with the data sent back from the server
        var url = "/Admin/UsersToCategory/GetUsersForCategory?categoryId=" + this.value;
        if (this.value != 0) {

            $.ajax({
                type: "GET",
                url: url,
                success: function (data) {
                    //we are adding the returned Partial view (_UsersListViewPartial) content to the appropriate element
                    //There is a <div id="UsersCheckList"> in UsersToCategory/Index view page
                    $("#UsersCheckList").html(data);
                    $("button[name='SaveSelectedUsers']").prop('disabled', false);

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    PresentClosableBootstrapAlert("#alert_placeholder", "danger", "An error occured!", errorText);
                    console.error("An error has occured:" + thrownError + "Status: " + xhr.status + "\r\n" + xhr.responseText);
                }
            });

        } else {
            $("button[name='SaveSelectedUsers']").prop('disabled', true);
            $("input[type=checkbox]").prop("checked", false);
            $("input[type=checkbox]").prop("disabled", true);
        }
    });

    //Will be triggered when the admin Saves the relevant input data
    $('#SaveSelectedUsers').on('click', function () {

        var url = "/Admin/UsersToCategory/SaveSelectedUsers";

        var categoryId = $("#CategoryId").val();

        var antiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        var usersSelected = [];

        $('input[type=checkbox]:checked').each(function{
            var userModel = {
                Id:$(this).attr("value")
            };
            usersSelected.push(userModel);
        });

        var usersSelectedForCategory = {
            __RequestVerificationToken: antiForgeryToken,
            CategoryId: categoryId,
            UsersSelected: usersSelected

        };

        $.ajax({
            type: "POST",
            url: url,
            success: function (data) {
                $("#UsersCheckList").html(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                PresentClosableBootstrapAlert("#alert_placeholder", "danger", "An error occured!", errorText);
                console.error("An error has occured:" + thrownError + "Status: " + xhr.status + "\r\n" + xhr.responseText);
            }
        });


    });

});