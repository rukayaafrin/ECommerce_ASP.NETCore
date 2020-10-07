window.onload = function () {
    let err_msg = document.getElementById("err_msg");

    let form = document.getElementById("form");
    form.onsubmit = function () {
        let username = document.getElementById("username").value.trim();
        let password = document.getElementById("password").value.trim();
        if (username.length == 0 || password.length == 0) {
            err_msg.innerHTML = "Please fill up all fields.";
            return false;
        }
        return true;
    }
    let elems = document.getElementsByClassName("form-control");
    for (let i = 0; i < elems.length; i++) {
        elems[i].onfocus = function () {
            err_msg.innerHTML = "";
        }
    }
}