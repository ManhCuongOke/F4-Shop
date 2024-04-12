let index = 0;
var rolar = true;
const sliderNumber = document.querySelectorAll(".banner-left-content__top-link");
const balls = document.querySelector(".banner-left-content__bottom-pagination");

for (let i = 0; i < sliderNumber.length; i++) {
    const div = document.createElement("div");
    div.id = i;
    balls.appendChild(div);
}
document.getElementById("0").classList.add("banner-circle-fill")

var pos = document.querySelectorAll(".banner-left-content__bottom-pagination div")

for (let i = 0; i < pos.length; i++) {
    pos[i].addEventListener('click', () => {
        index = pos[i].id;
        document.querySelector(".banner-left-content__top").style.right = index * 100 + "%";
        document.querySelector(".banner-circle-fill").classList.remove("banner-circle-fill");
        document.getElementById(index).classList.add("banner-circle-fill");
    });
}

function sliderAuto() {
    index = index + 1;
    if (index > sliderNumber.length - 1) {
        index = 0;
    }
    document.querySelector(".banner-left-content__top").style.right = index * 100 + "%";
    document.querySelector(".banner-circle-fill").classList.remove("banner-circle-fill");
    document.getElementById(index).classList.add("banner-circle-fill");
}

setInterval(sliderAuto, 3000)

// Next/Prev Banner
const btnNextBanner = document.querySelector(".banner-left-content__top-btn-icon-next");
const btnPrevBanner = document.querySelector(".banner-left-content__top-btn-icon-prev");

btnNextBanner.addEventListener('click', () => {
    index = index + 1;
    if (index > sliderNumber.length - 1) {
        index = 0;
    }
    document.querySelector(".banner-left-content__top").style.right = index * 100 + "%";
    document.querySelector(".banner-circle-fill").classList.remove("banner-circle-fill");
    document.getElementById(index).classList.add("banner-circle-fill");
});

btnPrevBanner.addEventListener('click', () => {
    index = index - 1;
    if (index <= 0) {
        index = sliderNumber.length - 1;
        document.querySelector(".banner-left-content__top").style.right = index * 100 + "%";
        document.querySelector(".banner-circle-fill").classList.remove("banner-circle-fill");
        document.getElementById(index).classList.add("banner-circle-fill");
    }
});

// Slider Category
const btnRightTwo = document.querySelector(".fa-arrow-right");
const btnLeftTwo = document.querySelector(".fa-arrow-left");
const categoryNumberTwo = document.querySelectorAll(".category-content-list");

btnRightTwo.addEventListener('click', () => {
    index = index + 1;
    if (index > categoryNumberTwo.length - 1) {
        index = 0;
    }
    document.querySelector(".category-content").style.right = index * 100 + "%";
});

btnLeftTwo.addEventListener('click', () => {
    index = index + 1;
    if (index > categoryNumberTwo.length - 1) {
        index = 0;
    }
    document.querySelector(".category-content").style.right = index * 100 + "%";
});

// Lấy thông tin
window.onload = () => {
    getData();
};

function getData() {
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/Home/GetData', true);
    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const data = JSON.parse(xhr.responseText);
            console.log(data);
            let htmlProduct = "";
            let htmlCategory = "";
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

            htmlProduct += data.products.map(obj => `
            <div class="col l-2 c-6 m-4">
                <a class="home-product-item" href="/product/detail/${obj.pK_iProductID}">
                    <div class="home-product-item__img"
                        style="background-image: url(/img/${obj.sImageUrl});"></div>
                    <h4 class="home-product-item__name">${obj.sProductName}</h4>
                    <div class="home-product-item__price">
                        <span class="home-product-item__price-old">1.200 000đ</span>
                        <span class="home-product-item__price-current">${obj.dPrice} đ</span>
                    </div>
                    <div class="home-product-item__action">
                        <span class="home-product-item__like home-product-item__like--liked">
                            <i class="home-product-item__like-icon-empty far fa-heart"></i>
                            <i class="home-product-item__like-icon-fill fas fa-heart"></i>
                        </span>
                        <div class="home-product-item__rating">
                            <i class="home-product-item__star--gold fas fa-star"></i>
                            <i class="home-product-item__star--gold fas fa-star"></i>
                            <i class="home-product-item__star--gold fas fa-star"></i>
                            <i class="home-product-item__star--gold fas fa-star"></i>
                            <i class="fas fa-star"></i>
                        </div>
                        <span class="home-product-item__sold"> 88 Đã bán</span>
                    </div>
                    <div class="home-product-item__origin">
                        <span class="home-product-item__brand">Who</span>
                        <span class="home-product-item__origin-name">Nhật Bản</span>
                    </div>
                    <div class="home-product-item__favourite">
                        <i class="fas fa-check"></i>
                        <span>Yêu thích</span>
                    </div>
                    <div class="home-product-item__sale-off">
                        <span class="home-product-item__sale-off-percent">53%</span>
                        <span class="home-product-item__sale-off-label">GIẢM</span>
                    </div>
                </a>
            </div>
            `).join('');

            htmlCategory += data.categories.map(obj => `
            <li class="category-item-home">
                <a href="/product/index/${obj.pK_iCategoryID}" class="category-item-link">
                    <div class="category-item__img" style="background-image: url(/img/category/${obj.sCategoryImage});">
                    </div>
                    <div class="category-item__name">${obj.sCategoryName}</div>
                </a>
            </li>
            `).join('');

            document.querySelector(".header__cart-list-item").innerHTML = htmlCartDetail;
            document.querySelector(".category-list").innerHTML = htmlCategory;
            document.querySelector(".product__container").innerHTML = htmlProduct;
        }
    }
    xhr.send(null);
}
