$(() => {

    let showData = (data) => {
        document.querySelector("#name").value = data.name;
        document.querySelector("#lastname").value = data.lastname;
        document.querySelector("#gender").value = data.gender;
        document.querySelector("#username").value = data.username;
        document.querySelector("#usertype").value = data.userType;
        document.querySelector("#phone").value = data.phone;
        document.querySelector("#email").value = data.email;
    }

    let loadUserSummary = () => {

        $.ajax({
            method: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: "get-user-summary"
        })
            .done((response) => {
                if (response.isSuccessful == true) {
                    let summary = document.querySelector("#user-summary");
                    summary.innerText = `There are ${response.data.administrators} administrator(s) and ${response.data.customers} customer(s) registered.`;
                } else {
                    console.log("could not load the data");
                }
            });
    };

    let loadData = () => {

        $.ajax({
            method: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: "get-user-info"
        })
            .done((response) => {
                if (response.isSuccessful == true) {
                    showData(response.data);
                } else {
                    console.log("could not load the data");
                }
            });
    };

    loadData();
    loadUserSummary();
})