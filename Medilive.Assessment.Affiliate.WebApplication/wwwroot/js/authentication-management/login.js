$(() => {

    let showErrorMessages = (messages) => {
        if (!messages) {
            return;
        }

        let messageText = "";
        for (let index = 0; index < messages.length; index++) {
            let currentMessage = messages[index];
            messageText += currentMessage.text + "\r\n";
        }
        alert(messageText);

        //let form = document.querySelector("form");
        //for (let index = 0; index < messages.length; index++) {
        //    let currentMessage = messages[index];
        //    let formElement = document.querySelector(`#${currentMessage.code}-feedback`);
        //    formElement.innerText = currentMessage.text;
        //    formElement.classList.remove("valid-feedback");
        //    formElement.classList.add("invalid-feedback");
        //}
        //form.classList.add("was-validated");
    }

    let makeRequest = function () {

        let form = document.querySelector("form");
        let formData = new FormData(form);
        let loginInfo = {};
        loginInfo.captcha = grecaptcha.getResponse();
        formData.forEach(function (value, key) {
            loginInfo[key] = value;
        });

        $.ajax({
            method: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: "user-login",
            data: JSON.stringify({
                data: loginInfo
            })
        })
            .done(function (response) {
                if (response.isSuccessful == true) {
                    window.location.href = "/redirect?message=You have successfully logged in. You will be redirected to the homepage within 5 seconds.&url=/";
                } else {
                    grecaptcha.reset();
                    showErrorMessages(response.messages);
                }
            });
    };

    $("button").on("click", e => {
        e.preventDefault();
        makeRequest();
    });

})