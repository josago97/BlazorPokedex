export function isCollapsed() {
    var element = document.getElementsByClassName('page')[0];

    return getComputedStyle(element).flexDirection == 'column';
}

export function setFavicon(imgUrl) {
    let element = document.querySelector('link[rel="icon"]');

    if (element && imgUrl) element.href = imgUrl;
}

export function changeFavicon(iconUrl) {
    let existingFavicon = null; 
    let faviconElement = document.head ? document.head.querySelector('link[rel="icon"]') : null;

    if (faviconElement) {
        existingFavicon = faviconElement.href;
    }
    else if (iconUrl && iconUrl.length > 0) {
        faviconElement = document.createElement('link');
        faviconElement.rel = 'icon';
        favicon.type = 'image/x-icon';
        document.head.appendChild(faviconElement);
    }

    faviconElement.href = iconUrl;

    return existingFavicon;
}