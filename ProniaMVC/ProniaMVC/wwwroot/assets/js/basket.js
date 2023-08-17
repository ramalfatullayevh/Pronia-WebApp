let addBasketBtns = document.querySelectorAll(".add-to-basket");
addBasketBtns.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();

    let url = btn.getAttribute("href");

    fetch(url).then(res => {
        if (res.status == 200) {
            
        }
        else {
            alert("Error")
        }
    })
}))
