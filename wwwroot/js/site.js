// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Lây dữ liệu
window.onload = () => {
    getData();
};

function getData() {
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/Home/GetData', true);
    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const data = JSON.parse(xhr.responseText);

            let htmlCartDetail = "";

            htmlCartDetail += data.cartDetails.map(obj => `
            <li class="header__cart-item">
                <div class="header__cart-item-img">
                    <img src="/img/${obj.sImageUrl}" class="header__cart-item-img" alt="">
                </div>
                <div class="header__cart-item-info">
                    <div class="header__cart-item-head">
                        <h5 class="header__cart-item-name">${obj.sProductName}</h5>
                        <div class="header__cart-item-price-wrap">
                            <span class="header__cart-item-price">${obj.dUnitPrice} đ</span>
                            <span class="header__cart-item-multifly">X</span>
                            <span class="header__cart-item-qnt">${obj.iQuantity}</span>
                        </div>
                    </div>
                    <div class="header__cart-item-body">
                        <span class="header__cart-item-description">
                            Phân loại hàng:Bạc
                        </span>
                        <span class="header__cart-item-remove">Xoá</span>
                    </div>
                </div>
            </li>
            `).join('');

            document.querySelector(".header__cart-notice").innerText = data.cartCount;
            document.querySelector(".header__cart-list-item").innerHTML = htmlCartDetail;
        }
    }
    xhr.send(null);
}

// Tìm kiếm danh mục
function searchProducts(input) {
    document.querySelector('.header__search-history').style.display = 'block';
    var formData = new FormData();
    if (input.value != "") {
        formData.append("keyword", input.value);
    }
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/Home/Search', true);
    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const data = JSON.parse(xhr.responseText);
            let html = "";
            html +=     "<ul class='header__search-history-list'>";
            html += data.map(obj => `
                            <li class="header__search-history-item">
                                <a href="/Product/Index?categoryID=${obj.pK_iCategoryID}">${obj.sCategoryName}</a>
                            </li>`).join('');
            html +=     "</ul>";
            document.querySelector('.header__search-history').innerHTML = html;
        } 
    };
    xhr.send(formData);
}
const searchHistory = document.querySelector('.header__search-history');
window.onclick = (event) => {
    if (event.target == searchHistory) {
        searchHistory.style.display = 'none';
    }
}

// Toast
function toast({ title = "", msg = "", type = "", duration = 3000}) {
    const main = document.getElementById('toast');
    if (main) {
        const toast = document.createElement("div");
        const autoRemoveId = setTimeout(() => {
            main.removeChild(toast);
        }, duration + 1000);

        toast.onclick = (e) => {
            if (e.target.closest('.toast__close')) {
                main.removeChild(toast);
                clearTimeout(autoRemoveId);
            }
        };

        const icons = {
            success: 'uil uil-check-circle',
            error: 'uil uil-exclamation-triangle'
        };

        icon = icons[type];
        const delay = (duration / 1000).toFixed(2);

        toast.classList.add('toast', `toast--${type}`);
        toast.style.animation = `slideInLeft ease .3s, fadeOut linear 1s ${delay}s forwards`;
        toast.innerHTML = `
            <div class="toast__icon">
                <i class="${icon}"></i>
            </div>
            <div class="toast__body">
                <h3 class="toast__title">${title}</h3>
                <p class="toast__msg">${msg}</p>
            </div>
            <div class="toast__close">
                <i class="uil uil-times"></i>
            </div>
        `;
        main.appendChild(toast);
    }
}
