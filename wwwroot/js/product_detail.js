// Tăng số lượng sản phẩm trong chi tiết sản phẩm
function increaseProduct(event) {
    const parentElement = event.target.parentNode;
    var increase = parentElement.querySelector("#qnt").value;
    if (parseInt(increase) < 100) {
        parentElement.querySelector("#qnt").value = parseInt(increase) + 1;
    }
}

// Giảm số lượng sản phẩm trong chi tiết sản phẩm
function reduceProduct(event) {
    const parentElement = event.target.parentNode;
    var reduce = parentElement.querySelector("#qnt").value;
    if (parseInt(reduce) > 0) {
        parentElement.querySelector("#qnt").value = parseInt(reduce) - 1;
    }
}

//AddToCart
function addToCart(productID, price) {
    var quantity = document.getElementById("qnt").value;
    if (parseInt(quantity) == 0) {
        toast({title: "Thông báo", msg: "Bạn chưa nhập số lượng sản phẩm!", type: "success", duration: 5000});
        // alert('Bạn chưa nhập số lượng sản phẩm!');
    } else {
        var formData = new FormData();
        formData.append('productID', productID);
        formData.append('unitPrice', price);
        formData.append('quantity', quantity);
        var xhr = new XMLHttpRequest();
        xhr.open('post', '/Cart/AddToCart', true);
        xhr.onreadystatechange = () => {
            if (xhr.readyState == 4 && xhr.status == 200) {
                const data = JSON.parse(xhr.responseText);
                console.log(data);

                let htmlCartDetail = "";

                htmlCartDetail += data.model.cartDetails.map(obj => `
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

                document.querySelector(".header__cart-notice").innerText = data.model.cartCount;
                document.querySelector(".header__cart-list-item").innerHTML = htmlCartDetail;
                toast({title: "Thông báo", msg: `${data.msg}`, type: "success", duration: 5000});
            }
        }
        xhr.send(formData);
    }
}