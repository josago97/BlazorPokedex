export function isCollapsed() {
    var element = document.getElementsByClassName("page")[0];

    return getComputedStyle(element).flexDirection == "column";
}
