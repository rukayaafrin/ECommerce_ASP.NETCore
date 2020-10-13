function retrieveDate(event)
{
    let atvkeyelem = event.target;
    let atvkey = atvkeyelem.value.trim();

    if (atvkey.length == 36)
    {
        let xhr = new XMLHttpRequest();

        xhr.open("POST", "/MyPurchases/GetDate");
        xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

        xhr.onreadystatechange = function () {
            if (this.readyState == XMLHttpRequest.DONE) {
                if (this.status == 200) {
                    let data = JSON.parse(this.responseText);
                    let field = "purchasedate-" + data.pdtId;
                    let purchasedate = document.getElementById(field);
                    purchasedate.innerHTML = data.pdate;
                    atvkeyelem.value = atvkey;
                }
            }
        };

        xhr.send(JSON.stringify({ AtvKey: atvkey }));

    }
    
}

