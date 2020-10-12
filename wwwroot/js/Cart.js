
    var http = new XMLHttpRequest();
    http.open("POST", "/Cart/GetData", true);
    http.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    http.send();
http.onload = function () {
    var json_data = JSON.parse(this.responseText);
    var count = json_data.length;
    var productQty = 0;
    var productPrice = 0;
    var TotalPrice = 0;
    var CartTotal = 0;

        for (var i = 0; i < count; i++) {
            console.log(json_data[i]);
            productQty = parseInt(json_data[i]["quantity"]);
            productPrice = parseInt(json_data[i]["product"]["price"]);

            console.log(productQty);

            document.getElementById("number_" + (json_data[i]["product"]["id"])).innerHTML = productQty;
            //document.getElementById("price_" + (json_data[i]["product"]["id"])).innerHTML = productPrice;


            TotalPrice = productQty * productPrice;
            document.getElementById("Total_" + (json_data[i]["product"]["id"])).innerHTML = TotalPrice;
            console.log(TotalPrice);

            CartTotal += TotalPrice;
            document.getElementById("cTotal").innerHTML = CartTotal;
            console.log(CartTotal);
            
        }
       
    }
