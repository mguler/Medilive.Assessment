$(() => {


    let showErrorMessages = (messages) => {

        if (!messages) {
            return;
        }

        messages.forEach(message => {
            let textElement = document.querySelector(`#${message.code}-feedback`);
            textElement.innerText = message.text;
            textElement.classList.add("invalid-feedback");
            document.querySelector(`#${message.code}`).classList.add("is-invalid");
        });

    }

    let removeErrorMessages = () => {
        document.querySelectorAll(".feedback").forEach(element => element.classList.remove("invalid-feedback"));
        document.querySelectorAll(".is-invalid").forEach(element => element.classList.remove("is-invalid"));
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

        let queryString = window.location.search;
        let urlParams = new URLSearchParams(queryString);
        let referralCode = urlParams.get('referralCode') 
        userInfo["referralCode"] = referralCode;

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
