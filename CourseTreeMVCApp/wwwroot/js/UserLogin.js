$(function () {
    //var loginUserButton = $("#UserLoginModal button[name='login']").click(onUserLoginClick); (deprecated code)
    var userLoginButton = $("#UserLoginModal button[name='login']").on('click', onUserLoginClick);

    function onUserLoginClick() {

        var url = "UserAuth/Login";
        //we are submiting the Anti Forgery Token along with the users login credentials, this is a hidden textbox which is automatically included
        //in our code by the dotnet MVC 
        var antiForgeryToken = $("#UserLoginModal input[name='__RequestVerificationToken']").val();

        var email = $("#UserLoginModal input[name='Email']").val();
        var password = $("#UserLoginModal input[name='Password']").val();
        var rememberMe = $("#UserLoginModal input[name='RememberMe']").prop('checked');

        var userInput = {
            __RequestVerificationToken: antiForgeryToken,
            Email: email,
            Password: password,
            RememberMe: rememberMe
        };

        $.ajax({
            type: 'POST',
            url: url,
            data: userInput,
            success: function (data) {
                //This ajax operation allows us to update only a portion of the html rendered in the client's browser asynchronously.
                //we can avoid rendering the entire html document within the client's browser.
                //We only want to render what's inside the _UserLoginPartial partial view which contains the code for the modal dialog

                
                var parsed = $.parseHTML(data);
                var hasErrors = $(parsed).find("input[name='LoginInValid']").val() == "true";

                if (hasErrors == true) {
                    //UserLoginModal comes from Home/Index which holds the partial view
                    $("#UserLoginModal").html(data);

                    //We must write code to wire up the click event for the login button again (for the case..)
                    //..when the Login dialog is rendered to the screen through the asynchronous process after a failed login attempt)

                    userLoginButton = $("#UserLoginModal button[name='login']").on('click', onUserLoginClick);

                    //this comes from the _UserLoginPartial partial view
                    var form = $("#UserLoginForm");
                    $(form).removeData("validator");
                    $(form).removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(form);

                }
                else {
                    location.href = 'Home/Index';
                }


            },
            error: function (xhr,ajaxOptions,thrownError) {
                var errorText = "Status: " + xhr.status + " - " + xhr.statusText;

                //PresentClosableBootstrapAlert("#alert_placeholder_login", "danger", "Error!", errorText);

                console.error(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }

        });

    }

})