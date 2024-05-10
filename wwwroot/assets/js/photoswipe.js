lightGallery(document.getElementById('animated-thumbnails-gallery'), {
    thumbnail:true
}); 

const reviewGalleries = document.querySelectorAll('#review-gallery');

reviewGalleries.forEach(gallery => {
    lightGallery(gallery, {
        thumbnail: true
    });
});
