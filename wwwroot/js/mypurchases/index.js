window.onload = function () {
    let purchasedate = document.getElementById("purchasedate");

    purchasedate.onchange = function () {
        let dateselected = purchasedate.options[purchasedate.selectedIndex].value;
        let activkeyfield = document.getElementById("activkey");
        activkeyfield.value = "@iter[dateselected].ActivationKey"
    }

}


/*function getActivKey(dateselected)
{
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/MyPurchases/GetActivKey");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                let activkeyfield = document.getElementById("activkey");
                activkeyfield.value = data;
            }
        }
    };

    xhr.send(JSON.stringify({ dateselected: dateselected}));

}*/