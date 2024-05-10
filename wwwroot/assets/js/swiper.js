var Swipes = new Swiper('#swiper-container', {
    loop: true,
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
    pagination: {
        el: '.swiper-pagination',
    },
});

var Swipes = new Swiper('.service-box', {
    loop: true,
    slidesPerView: 4,
    paginationClickable: true,
    spaceBetween: 20,
    breakpoints: {
        200: {
            slidesPerView: 1,
            spaceBetween: 24,
            resistanceRatio: 0.85
        },
        768: {
            slidesPerView: 3,
            spaceBetween: 20,
        },
        992: {
            slidesPerView: 4,
            spaceBetween: 20,
            resistanceRatio: 0.85
        },
    },
});

var Swipes = new Swiper('.category-box', {
    loop: true,
    slidesPerView: 4,
    paginationClickable: true,
    spaceBetween: 20,
    breakpoints: {
        200: {
            slidesPerView: 3,
        },
        768: {
            slidesPerView: 6,
        },
        992: {
            slidesPerView: 8,
        },
    },
});

var Swipes = new Swiper('.deal-wrapper', {
    loop: false,
    slidesPerView: 5,
    paginationClickable: true,
    spaceBetween: 20,
    breakpoints: {
        200: {
            slidesPerView: 2,
            spaceBetween: 24,
            resistanceRatio: 0.85
        },
        768: {
            slidesPerView: 4,
            spaceBetween: 20,
        },
        992: {
            slidesPerView: 5,
            spaceBetween: 20,
        }
    },
});

var Swipes = new Swiper('#deal-wrapper', {
    loop: false,
    slidesPerView: 4,
    paginationClickable: true,
    spaceBetween: 20,
    breakpoints: {
        200: {
            slidesPerView: 2,
            spaceBetween: 24,
        },
        768: {
            slidesPerView: 2,
            spaceBetween: 28,
        },
        992: {
            slidesPerView: 4,
            spaceBetween: 28,
        }
    },
});

var Swipes = new Swiper('#product-wrapper', {
    loop: false,
    slidesPerView: 4,
    paginationClickable: true,
    spaceBetween: 20,
    breakpoints: {
        200: {
            slidesPerView: 2,
        },
        768: {
            slidesPerView: 2,
        },
        992: {
            slidesPerView: 4,
        },
        1200: {
            slidesPerView: 4,
        }
    },
});

var Swipes = new Swiper('.recent-wrapper', {
    loop: false,
    slidesPerView: 8,
    paginationClickable: true,
    spaceBetween: 20,
    breakpoints: {
        200: {
            slidesPerView: 2,
            spaceBetween: 24,
            resistanceRatio: 0.85
        },
        768: {
            slidesPerView: 5,
            spaceBetween: 20,
        },
        992: {
            slidesPerView: 6,
            spaceBetween: 20,
        }
    },
});

var Swipes = new Swiper('#service-box', {
    loop: false,
    slidesPerView: 8,
    paginationClickable: true,
    spaceBetween: 20,
    breakpoints: {
        200: {
            slidesPerView: 2,
            spaceBetween: 24,
            resistanceRatio: 0.85
        },
        768: {
            slidesPerView: 4,
            spaceBetween: 20,
        },
        992: {
            slidesPerView: 6,
            spaceBetween: 20,
        }
    },
});

var galleryThumbs = new Swiper('.gallery-thumbs', {
    spaceBetween: 10,
    slidesPerView: 4,
    loop: true,
    freeMode: true,
    loopedSlides: 5,
    watchSlidesVisibility: true,
    watchSlidesProgress: true,
  });
  var galleryTop = new Swiper('.gallery-top', {
    spaceBetween: 10,
    loop: true,
    loopedSlides: 5,
    navigation: {
      nextEl: '.swiper-button-next',
      prevEl: '.swiper-button-prev',
    },
    thumbs: {
      swiper: galleryThumbs,
    },
  });