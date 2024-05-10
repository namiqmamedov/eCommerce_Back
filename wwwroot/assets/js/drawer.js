function toggle() {
    var nav = document.querySelector("#nav"),
        drawerMenu = document.querySelector(".drawer-menu"),
        btn = document.querySelector("#btn"),
        overlayMenu = document.getElementById("overlay-menu");

    nav.classList.toggle("open");
    drawerMenu.classList.toggle("menu-open");
    btn.classList.toggle("is-active");

    if (overlayMenu.style.display === "none") {
        overlayMenu.style.display = "block";
    } else {
        overlayMenu.style.display = "none";
    }
}

var overlayMenu = document.getElementById("overlay-menu");

overlayMenu.addEventListener('click', function() {
    toggle(); 
});
