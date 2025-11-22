document$.subscribe(() =>
{
    document.querySelectorAll("[data-fiddle]")
        .forEach(c =>
        {
            var tryItLink = document.createElement("button");
            tryItLink.addEventListener("click", () => {
                var element = document.createElement("a");
                element.style.display = "none";
                element.href = `https://dotnetfiddle.net/${c.getAttribute("data-fiddle")}`;
                element.target = "_blank";                
                document.body.appendChild(element);
                element.click();
                document.body.removeChild(element);
            });
            tryItLink.title = "Try it out in DotNetFiddle"
            tryItLink.className = "md-try-it-out md-icon";
            
            var pre = c.querySelector("pre");
            pre.insertBefore(tryItLink, pre.firstChild);
        });
});

function setHeaderTop(value) {
    var mine = Array.from(document.styleSheets).filter(h => h.href?.endsWith("/tryIt.css"))[0]
    Array.from(mine.cssRules).filter(r => r.selectorText === ".md-header")[0].style.top = value;
}
document$.subscribe(() => {
    const banner = document.querySelector("[data-info-banner]");//
    banner.addEventListener("click", () => {
        localStorage.setItem("docs-info-banner", "no-show");
        setHeaderTop("0px");
        banner.style.display = "none";
    })
    if (localStorage.getItem("docs-info-banner")) return;

    setHeaderTop("50px");
    banner.style.display = "block";
})