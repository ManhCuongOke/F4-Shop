// Sắp xếp các sản phẩm trong trang sản phẩm theo giá tăng dần
function sortIncre(categoryID) {
    var formData = new FormData();
    formData.append("categoryID", categoryID);
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/Product/Sort', true);
    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const data = JSON.parse(xhr.responseText);
            console.log(data);
            let html = "";
            html += data.products.map(obj => `
            <div class="col l-2-4 c-6 m-4">
                <a class="home-product-item" href="/Product/Detail/${obj.pK_iProductID}">
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
            console.log(document.querySelector(".product__container").innerHTML = html);
            document.querySelector(".product__container").innerHTML = html;
        }
    }
    xhr.send(formData);
}