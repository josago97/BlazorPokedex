export function isCollapsed() {
    var element = document.getElementsByClassName("page")[0];

    return getComputedStyle(element).flexDirection == "column";
}

export function setFavicon(imgUrl) {
    let element = document.getElementById("favicon");

    if (element && imgUrl) element.href = imgUrl;
}
