
let images = []
let imageIndex = 0;

const nextImage = () => {
    if (imageIndex < images.length - 1) {
        imageIndex++
    } else {
        imageIndex = 0
    }

    resetImages()
    setMainImage()
}

const previousImage = () => {
    if (imageIndex < 1) {
        imageIndex = images.length - 1
    } else {
        imageIndex--
    }

    resetImages()
    setMainImage()
}

const getCarouselImages = () => {
    const carouselImages = document.querySelectorAll('.image-carousel img')
    images = [...carouselImages]

    if (images.length < 1) return;

    resetImages()
    setMainImage()
}

const resetImages = () => {
    images.forEach(element => {
        element.style.fontSize = '1em'
        element.classList.add('contrast-img')
        element.classList.add('inactive')
    });
}

const setMainImage = () => {
    images[imageIndex].style.fontSize = '1.1em'
    images[imageIndex].classList.remove('contrast-img')
}

getCarouselImages()