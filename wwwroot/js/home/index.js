



















function cartCounting() {
    var counterobj = document.getElementById("cart_counter");

    var count = parseInt(counterobj.innerHTML);
    //object.value: when
    count++;

    counterobj.innerHTML = count;

}