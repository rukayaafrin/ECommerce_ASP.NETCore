window.onload = function ()
{
    let elemList = document.getElementsByClassName("addtocart");
    for (i = 0; i < elemList.length; i++) {
        elemList[i].onclick = AddToCart;
    }

    let searchform = document.getElementById("searchform");
    let searchfield = document.getElementById("searchfield");

    searchform.onsubmit = function ()
    {
        if ((searchfield.value.trim()).length !== 0)
            return true;
        else
            return false;
    }

    searchfield.onchange = function()
    {
        if ((searchfield.value.trim()).length === 0)
            window.location = "/";       
    }

    searchfield.onkeydown = function (event)
    {
        if (event.key === "Backspace" || event.key === "Delete")
        {
            if ((searchfield.value.trim()).length === 1) 
                window.location = "/";
        }
    }
    
}




function AddToCart(event) {
    let elem = event.currentTarget;

    sendAddToCart(true, elem.getAttribute("product_id"))

}

function sendAddToCart(added, productId) {

    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Home/UpdateCart");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                console.log("Status: " + data.status);
                let cartnumber = document.getElementById("cart_counter");
                cartnumber.innerHTML = data.cartNumber
            }
        }
    };

    xhr.send(JSON.stringify({ Added: added, ProductId: productId }));
}

