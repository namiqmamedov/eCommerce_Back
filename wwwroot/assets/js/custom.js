$(document).ready(function () {
    $('.addToCart').click(function (e) {
        e.preventDefault();

        let colorID = $('.product__color .color.active').data('color-id');
        let sizeID = $('.product__size .size.active').data('size-id');
        let priceText = $('#colorPrice').text();
        let oldPriceText = $('#colorOldPrice').text();

        if (priceText != "" && colorID && sizeID != undefined) {
            let match = priceText.match(/\d+/);
            let priceOriginal = match ? match[0] : "";

            let oldPriceOriginal = "";

            if (oldPriceText != "") {
                let match1 = oldPriceText.match(/\d+/);
                oldPriceOriginal = match1 ? match1[0] : "";
            }

            let url = '';

            if (oldPriceOriginal == "") {
                url = $(this).attr('href') + '?colorID=' + colorID + '&sizeID=' + sizeID + '&price=' + priceOriginal + '&discountedPrice=' + oldPriceOriginal;
            } else {
                url = $(this).attr('href') + '?colorID=' + colorID + '&sizeID=' + sizeID + '&price=' + oldPriceOriginal + '&discountedPrice=' + priceOriginal;
            }

            fetch(url).then(res => res.text())
                .then(data => {
                    $('.cart-info').html(data);
                });
        }
        else {
            let url = $(this).attr('href');

            fetch(url).then(res => res.text())
                .then(data => {
                    $('.cart-info').html(data);
                });
        }
    });
});

$(document).on('click', '.clearBtn', function (e) {
    e.preventDefault();

    fetch($(this).attr('href'))
        .then(res => res.text())
        .then(data => {
            $('#cartIndex').html(data);

            fetch('/cart/getcart').then(res => res.text())
                .then(data => {
                    $('.cart-info').html(data);
                })
        })
})
$(document).on('click', '.close-btn', function (e) {
    e.preventDefault();

    fetch($(this).attr('href'))
        .then(res => res.text())
        .then(data => {
            $('.cart-info').html(data);
            fetch('/cart/getcartindex').then(res => res.text())
                .then(data => {
                    $('#cartIndex').html(data);
                })
        })
})

$(document).on('click', '.deleteCart', function (e) {
    e.preventDefault();

    fetch($(this).attr('href'))
        .then(res => res.text())
        .then(data => {
            $('#cartIndex').html(data);
            fetch('/cart/getcart').then(res => res.text())
                .then(data => {
                    $('.cart-info').html(data);
                })
        })
})


$(document).ready(function () {
    $('.category-link').each(function () {

        var categoryId = $(this).data('category');
        var urlParams = new URLSearchParams(window.location.search);
        var selectedCategory = urlParams.get('category');

        if (selectedCategory && categoryId == selectedCategory) {
            $(this).addClass('checked');
        }
    });

    $('.price-link').each(function () {
        var href = $(this).attr('href');
        var urlParams = new URLSearchParams(window.location.search);
        var selectedMinPrice = parseInt(urlParams.get('minPrice'));
        var selectedMaxPrice = parseInt(urlParams.get('maxPrice'));

        var priceRange = href.match(/\d+(?:\.\d+)?/g);
        var minPriceInHref = parseInt(priceRange[0]);
        var maxPriceInHref = parseInt(priceRange[1]);

        if (selectedMinPrice === minPriceInHref && selectedMaxPrice === maxPriceInHref) {
            $(this).addClass('checked');
        }
    });

    $('.sizeItem').each(function () {

        var sizeId = $(this).data('size');
        var urlParams = new URLSearchParams(window.location.search);
        var selectedSize = urlParams.get('size');

        if (selectedSize && sizeId == selectedSize) {
            $(this).find('.checkbox__checkmark').addClass('checked');
        }
    });

    $('.brandItem').each(function () {

        var brandId = $(this).data('brand');
        var urlParams = new URLSearchParams(window.location.search);
        var selectedBrand = urlParams.get('brand');

        if (selectedBrand && brandId == selectedBrand) {
            $(this).find('.checkbox__checkmark').addClass('checked');
        }
    });

    $('.colorItem').each(function () {

        var colorId = $(this).data('color');
        var urlParams = new URLSearchParams(window.location.search);
        var selectedColor = urlParams.get('color');

        if (selectedColor && colorId == selectedColor) {
            $(this).find('.checkbox__checkmark').addClass('checked');
        }
    });
});

$(document).ready(function () {
    var urlParams = new URLSearchParams(window.location.search);
    var selectedOption = urlParams.get('orderby');
    if (!selectedOption) {
        selectedOption = 'default'; 
    }

    $('select[name="orderby"]').val(selectedOption);

    $('select[name="orderby"]').change(function () {
        var selectedOption = $(this).children("option:selected").val();
        var currentUrl = window.location.href.split('?')[0];
        var newUrl = currentUrl + '?orderby=' + selectedOption;

        fetch(newUrl)
            .then(res => res.text())
            .then(data => {
                window.location.href = newUrl;
            })
            .catch(error => {
                console.error('Fetch error:', error);
            });
    });

    var selectedShowOption = urlParams.get('show');
    if (!selectedShowOption) {
        selectedShowOption = '12';
    }

    $('select[name="item-count"]').val(selectedShowOption);
    $('select[name="item-count"]').change(function () {
        var selectedShowOption = $(this).children("option:selected").val();
        var currentUrl = window.location.href.split('?')[0];
        var newUrl = currentUrl + '?show=' + selectedShowOption;

        fetch(newUrl)
            .then(res => res.text())
            .then(data => {
                window.location.href = newUrl;
            })
            .catch(error => {
                console.error('Fetch error:', error);
            });
    });


    var selectedCategory = urlParams.get('categoryID');

    if (!selectedCategory || !$('select[name="categoryID"] option[value="' + selectedCategory + '"]').length) {
        $('select[name="categoryID"]').val('All');
    } else {
        $('select[name="categoryID"]').val(selectedCategory);
    }
});

$(document).ready(function () {
    $('.page-link').click(function (e) {
        e.preventDefault();

        var page = $(this).attr('data-page');
        var currentPageUrl = window.location.href;
        var newUrl;

        if (currentPageUrl.includes('page=')) {
            var regex = /(\?|&)page=\d+/g;
            newUrl = currentPageUrl.replace(regex, '$1page=' + page);
        } else {
            var separator = currentPageUrl.includes('?') ? '&' : '?';
            newUrl = currentPageUrl + separator + 'page=' + page;
        }

        window.location.href = newUrl;
    });
});

$(document).on('click', '.deleteWishlist', function (e) {
    e.preventDefault();

    fetch($(this).attr('href'))
        .then(res => res.text())
        .then(data => {
            $('#wishlistIndex').html(data);
        })
})

$(document).on('click', '.minusCount', function (e) {
    e.preventDefault();

    let inputCount = $(this).closest('.input-group').find('.quantity').val();

    var colorID = $(this).attr("data-color-id")

    var sizeID = $(this).attr("data-size-id")

    if (inputCount >= 2) {
        inputCount--;
        $(this).next().val(inputCount);
        let url = $(this).attr('href') + '/?count=' + inputCount + '&colorID=' + colorID + '&sizeID=' + sizeID;

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('#cartIndex').html(data);

                fetch('/cart/getcart').then(res => res.text())
                    .then(data => {
                        $('.cart-info').html(data);
                    });
            });
    }
})

$(document).on('click', '.plusCount', function (e) {
    e.preventDefault();

    let inputCount = $(this).closest('.input-group').find('.quantity').val();

    var colorID = $(this).attr("data-color-id")

    var sizeID = $(this).attr("data-size-id")

    if (inputCount > 0) {
        inputCount++;
    }
    else {
        inputCount = 1;
    }

    $(this).prev().val(inputCount); inputCount

    let url = $(this).attr('href') + '/?count=' + inputCount + '&colorID=' + colorID + '&sizeID=' + sizeID;


    fetch(url)
        .then(res => res.text())
        .then(data => {
            $('#cartIndex').html(data);

            fetch('/cart/getcart').then(res => res.text())
                .then(data => {
                    $('.cart-info').html(data);
                });
        });

})

$(document).on('mouseover click', ".ratingStar", function (event) {
    event.stopPropagation();
    var starValue = $(this).attr("data-value");
    $("#ratingValue").val(starValue);

    $(".ratingStar").addClass("far").removeClass("fas");
    $(this).addClass("fas").removeClass("far");
    $(this).prevAll(".ratingStar").addClass("fas").removeClass("far");
});

$(document).on('click', ".color", function () {
    var colorPrice = $(this).attr("data-price");
    var discountedPrice = $(this).attr("data-old-price");

    if (!discountedPrice || discountedPrice.trim() === "") {
        $("#colorPrice").text("$" + colorPrice);
        $("#colorOldPrice").text("");
    } else {
        $("#colorPrice").text("$" + discountedPrice);
        $("#colorOldPrice").text("$" + colorPrice);
    }
});

$(document).ready(function () {
    var colorPrice = $(".color").first().data("price");
    var oldPrice = $(".color").first().data("old-price");

    if (colorPrice && oldPrice) {
        $("#colorPrice").text("$" + oldPrice);
        $("#colorOldPrice").text("$" + colorPrice);
    }
});

