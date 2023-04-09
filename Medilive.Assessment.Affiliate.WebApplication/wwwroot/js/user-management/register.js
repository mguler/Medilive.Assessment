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
        /*
        let form = document.querySelector("form");
        for (let index = 0; index < messages.length; index++) {
            let currentMessage = messages[index];
            let formElement = document.querySelector(`#${currentMessage.code}-feedback`);
            formElement.innerText = currentMessage.text;
            formElement.classList.remove("valid-feedback");
            formElement.classList.add("invalid-feedback");
        }
        form.classList.add("was-validated");
        */
    }

    let removeErrorMessages = () => {
        let form = document.querySelector("form");
        let elements = document.querySelectorAll(".valid-feedback , .invalid-feedback");
        for (let index = 0; index < elements.length; index++) {
            var element = elements[index];
            element.innerText = "";
            element.classList.remove("invalid-feedback");
            element.classList.add("valid-feedback");
        }
        form.classList.remove("was-validated");
    }

    let fillData = (data) => {

        let select = document.querySelector("#gender");
        for (let index = 0; index< data.length; index++) {
            let item = data[index];
            let option = document.createElement("option");
            option.value = item.id;
            option.innerText = item.name;
            select.appendChild(option);
        }
        select.value = 0;
    }

    let loadReferenceData = () => {

        $.ajax({
            method: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: "gender-list"
        })
            .done((response) => {
                if (response.isSuccessful == true) {
                    fillData(response.data);
                } else {
                    console.log("could not load the gender list");
                }
            });
    };

    let register = () => {

        let form = document.querySelector("form");
        let formData = new FormData(form);
        let userInfo = {};
        userInfo.captcha = grecaptcha.getResponse();
        formData.forEach(function (value, key) {
            userInfo[key] = value;
        });

        $.ajax({
            method: "PUT",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: "new-user-registration",
            data: JSON.stringify({
                data: userInfo
            })
        })
            .done(function (response) {
                if (response.isSuccessful == true) {
                    window.location.href = "/redirect?message=Your%20registration%20is%20successful%20you%20will%20be%20redirected%20to%20login%20page%20in%205%20seconds&url=/user-login";
                } else {
                    grecaptcha.reset();
                    showErrorMessages(response.messages);
                }
            });
    };

    $("button").on("click", e => {
        e.preventDefault();
        removeErrorMessages();
        register();
    });

    loadReferenceData();
})