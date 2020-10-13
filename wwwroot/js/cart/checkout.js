function checkLogin()
{
    let elemWelcomeUser = document.getElementById("welcomeuser");

    if (elemWelcomeUser == null) {
        window.location.href = "/Login/Index";
        return false;
    }

    else
    {
        return true;
    }
}