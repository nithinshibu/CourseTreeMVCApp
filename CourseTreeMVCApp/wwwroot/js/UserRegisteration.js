$(function () {


    //this is the code we implement to set the CategoryId hidden text field to zero when the user cancels the registration process after clicking one of the course cards 
    $("#UserRegistrationModal").on('hidden.bs.modal', function (e) {
        $("#UserRegistrationModal input[name='CategoryId']").val('0');
    });

    //We haven't yet implemented the jquery code to display the user registration dialog to the user immediately after the user clicks a course card , before the user registration dialog is displayed
    //to the user , we want the code to insert the category id value stored in the relevant anchor tags data-categoryId custom attribute into the hidden text field on the user registration dialog 

    $('.RegisterLink').on('click', function () {
        $("#UserRegistrationModal input[name='CategoryId']").val($(this).attr('data-categoryId'));
        $("#UserRegistrationModal").modal("show");
    });

    onAcceptUserAgreementClick();

    $("#UserRegistrationModal input[name = 'AcceptUserAgreement']").on('click', onAcceptUserAgreementClick);


    function onAcceptUserAgreementClick() {
        if ($(this).is(":checked")) {
            $("#UserRegistrationModal button[name = 'register']").prop("disabled", false);
        } else {
            $("#UserRegistrationModal button[name = 'register']").prop("disabled", true);
        }
    }

    $("#UserRegistrationModal input[name = 'Email']").on('blur', function () {
        var email = $("#UserRegistrationModal input[id = 'Email']").val();
        var url = "UserAuth/UserNameExists?userName=" + encodeURIComponent(email);
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                console.log("data=" + data);
                if (data == true) {
                    //This alert_placeholder_register is the id of the div in _UserRegistrationPartial
                    PresentClosableBootstrapAlert("#alert_placeholder_register", "warning", "Invalid Email", "This email address has already been registered");

                } else {
                    CloseAlert("#alert_placeholder_register");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                let errorText = "Status: " + xhr.status + " - " + xhr.statusText;
                PresentClosableBootstrapAlert("#alert_placeholder_register", "danger", "Error!", errorText);
                //console.error(thrownError + '\r\n' + xhr.statusText + '\r\n' + xhr.responseText);
            }
        })

    });

    var registerUserButton = $("#UserRegistrationModal button[name = 'register']").on('click', onUserRegisterClick);

    function onUserRegisterClick() {
        var url = "UserAuth/RegisterUser";

        var antiForgeryToken = $("#UserRegistrationModal input[name='__RequestVerificationToken']").val();
        var email = $("#UserRegistrationModal input[name='Email']").val();
        var password = $("#UserRegistrationModal input[name='Password']").val();
        var confirmPassword = $("#UserRegistrationModal input[name='ConfirmPassword']").val();
        var firstName = $("#UserRegistrationModal input[name='FirstName']").val();
        var lastName = $("#UserRegistrationModal input[name='LastName']").val();
        var address1 = $("#UserRegistrationModal input[name='Address1']").val();
        var address2 = $("#UserRegistrationModal input[name='Address2']").val();
        var postCode = $("#UserRegistrationModal input[name='PostCode']").val();
        var phoneNumber = $("#UserRegistrationModal input[name='PhoneNumber']").val();
        // This categoryId was supposed to come here after the commit (id=29d626c9522b6c5a18519a7b6e8ecbfc12aefc40)
        var categoryId = $("#UserRegistrationModal input[name='CategoryId']").val();

        var user = {
            __RequestVerificationToken: antiForgeryToken,
            Email: email,
            Password: password,
            ConfirmPassword: confirmPassword,
            FirstName: firstName,
            LastName: lastName,
            Address1: address1,
            Address2: address2,
            PostCode: postCode,
            PhoneNumber: phoneNumber,
            AcceptUserAgreement: true,
            // This categoryId was supposed to come here after the commit (id=29d626c9522b6c5a18519a7b6e8ecbfc12aefc40)
            CategoryId: categoryId
        };

        $.ajax({
            type: "POST",
            url: url,
            data: user,
            success: function (data) {

                var parsed = $.parseHTML(data);

                var hasErrors = $(parsed).find("input[name='RegistrationInValid']").val() == 'true';

                if (hasErrors) {

                    $("#UserRegistrationModal").html(data);
                    var registerUserButton = $("#UserRegistrationModal button[name = 'register']").on('click', onUserRegisterClick);
                    $("#UserRegistrationModal input[name = 'AcceptUserAgreement']").on('click', onAcceptUserAgreementClick);

                    $("#UserRegistrationForm").removeData("validator");
                    $("#UserRegistrationForm").removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse("#UserRegistrationForm");
                }
                else {
                    location.href = '/Home/Index';
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                let errorText = "Status: " + xhr.status + " - " + xhr.statusText;

                PresentClosableBootstrapAlert("#alert_placeholder_register", "danger", "Error!", errorText);

                //console.error(thrownError + '\r\n' + xhr.statusText + '\r\n' + xhr.responseText);
            }

        });
    }


});