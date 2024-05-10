var menuToggle = document.querySelector('.mobile-menu-toggle');
var filterMenuToggle = document.querySelector('.filter-menu-toggle');
var menuBtn = document.querySelector('.menu-btn');
var menuList = document.querySelector('.mobile-menu #menu__wrapper');
var menuListA = document.querySelector('.mobile-menu li a');
var menuWrapper = document.querySelectorAll('#menu__wrapper li .menu-btn');
var menuLink = document.querySelectorAll('#menu__wrapper-link');
var overlayMenu = document.querySelector('.mobile__menu .overlay__menu')

const colorLinks = document.querySelectorAll('.product__color .color');
const sizeLinks = document.querySelectorAll('.product__size .size');
const addToCartButton = document.querySelector('.addToCart');

document.addEventListener("DOMContentLoaded", function() {
    window.addEventListener("scroll", function() {
        var scrollY = window.scrollY;
        var headerBottom = document.querySelector(".header-bottom");

        if (scrollY > 100) {
            headerBottom.classList.add("fixed-bar");
        } else {
            headerBottom.classList.remove("fixed-bar");
        }
    });
});


colorLinks.forEach(link => {
    link.addEventListener('click', function(event) {
        if (this.classList.contains('active')) {
            this.classList.remove('active');
        } else {
            colorLinks.forEach(otherLink => {
                if (otherLink !== this) {
                    otherLink.classList.remove('active');
                }
            });
            this.classList.add('active');
        }
        
        updateAddToCartStatus();
        
        event.preventDefault();
    });
});

sizeLinks.forEach(link => {
    link.addEventListener('click', function(event) {
        if (this.classList.contains('active')) {
            this.classList.remove('active');
        } else {
            sizeLinks.forEach(otherLink => {
                if (otherLink !== this) {
                    otherLink.classList.remove('active');
                }
            });
            this.classList.add('active');
        }
        
        updateAddToCartStatus();
        
        event.preventDefault();
    });
});

function updateAddToCartStatus() {
    const colorIsActive = Array.from(colorLinks).some(link => link.classList.contains('active'));
    const sizeIsActive = Array.from(sizeLinks).some(link => link.classList.contains('active'));

    if (colorIsActive && sizeIsActive) {
        addToCartButton.classList.remove('disabled');
    } else {
        addToCartButton.classList.add('disabled');
    }
}

overlayMenu.addEventListener('click', function() {
    document.body.classList.remove('mmenu-active')
})

function toggleMenu(e) {
    document.body.classList.toggle('mmenu-active');

    e.preventDefault();
}

function btnToggle(e) {
    menuList.classList.toggle('d-block');

    e.preventDefault();
}

function menuWrapperToggle(e) {
    menuLink.classList.toggle('d-block');

    e.preventDefault();
}

menuToggle.addEventListener('click', toggleMenu);

menuBtn.addEventListener('click', btnToggle);

menuWrapper.forEach(function(menuBtn) {
    menuBtn.addEventListener('click', function(e) {
        var menu = menuBtn.parentElement.nextElementSibling;
        
        if (menu && getComputedStyle(menu).display === 'none') {
            menu.style.display = 'block';
        } else {
            menu.style.display = 'none';
        }
        
        e.preventDefault();
    });
});

menuLink.forEach(function(link) {
    var lastUlElement = link;
    if (lastUlElement) {
        lastUlElement.classList.add('new-class');
    }
});

