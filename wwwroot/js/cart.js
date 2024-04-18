// lấy số lượng sản phẩm giỏ hàng, khi khai báo window.onload ở site.js thì ở file này ta không khai báo nữa
function getCartInfo() {
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/Cart/GetCartInfo', true);
    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const data = JSON.parse(xhr.responseText);
            console.log(data);
            let html = "";
            html += data.cartDetails.map((obj, index) => `
            <tr id="product__${obj.pK_iProductID}">
                <td data-label="Chọn">
                    <input type="checkbox" class="cart__checkout-input" name="" id="" />
                </td>
                <td data-label="Tên sản phẩm">
                    <p class="cart__product-name">${obj.sProductName}</p>
                </td>
                <td data-label="Ảnh"><img src="/img/${obj.sImageUrl}" /></td>
                <td data-label="Đơn giá">${money(obj.dUnitPrice)} đ</td>
                <td data-label="Số lượng">
                    <div class="cart__count-btns">
                        <button type="button" class="cart__btn-add"
                            onclick="tru(event, ${obj.pK_iProductID}, ${obj.dUnitPrice})">-</button>
                        <input name="qnt" type="text" id="qnt" value="${obj.iQuantity}"
                            class="cart__count-input" />
                        <button type="button" class="cart__btn-sub"
                            onclick="cong(event, ${obj.pK_iProductID}, ${obj.dUnitPrice})">+</button>
                    </div>
                </td>
                <td data-label="Thành tiền" id="money">${money(obj.dMoney)} đ</td>
                <td>
                    <div class='btn-tools'>
                        <a class='btn-tool btn-tool__del'
                            href='javascript:deleteProduct(${obj.pK_iProductID})' title='Xoá sản phẩm'><i
                                class='uil uil-trash'></i></a>
                    </div>
                </td>
            </tr>
            `).join('');
            document.querySelector(".table__total-count").innerText = `Có ${data.cartCount} sản phẩm trong giỏ hàng của bạn`;
            document.getElementById("table__body").innerHTML = html;
        }
    }
    xhr.send(null);
}
getCartInfo();

// Tăng số lượng sản phẩm trong giỏ hàng
function cong(event, productID, unitPrice) {
    // console.log(productID);
    const parentElement = event.target.parentNode;
    // console.log(parentElement);
    var cong = parentElement.querySelector("#qnt").value;
    var input = parentElement.querySelector("#qnt");
    if (parseInt(cong) < 100) { // Nếu sau này ta convert biến này sang int, double thì không dùn constance cho biến qnt
        input.value = parseInt(cong) + 1;
        console.log(input.value);
        var formData = new FormData(); // Gửi dữ liệu dạng formData
        formData.append('quantity', input.value);
        formData.append('productID', productID);
        formData.append('unitPrice', unitPrice)
        var xhr = new XMLHttpRequest();
        xhr.open('post', '/Cart/Quantity', true);
        xhr.onreadystatechange = () => {
            if (xhr.readyState == 4 && xhr.status == 200) {
                const data = JSON.parse(xhr.responseText);
                const trParent = parentElement.parentNode.parentNode;
                // console.log(money.money);
                trParent.querySelector("#money").innerText = `${money(data.money)} đ`;
            }
        }
        xhr.send(formData);
    }
}

// Giảm số lượng sản phẩm trong giỏ hàng
function tru(event, productID, unitPrice) {
    const parentElement = event.target.parentNode;
    var tru = parentElement.querySelector("#qnt").value;
    var input = parentElement.querySelector("#qnt");
    if (parseInt(tru) > 1) {
        input.value = parseInt(tru) - 1;
        var formData = new FormData();
        formData.append('quantity', input.value);
        formData.append('productID', productID);
        formData.append('unitPrice', unitPrice);
        var xhr = new XMLHttpRequest();
        xhr.open('post', '/Cart/Quantity', true);
        xhr.onreadystatechange = () => {
            if (xhr.readyState == 4 && xhr.status == 200) {
                const data = JSON.parse(xhr.responseText);
                const trParent = parentElement.parentNode.parentNode;
                trParent.querySelector("#money").innerText = `${money(data.money)} đ`;
            }
        };
        xhr.send(formData);
    } else {
        deleteProduct(productID);
    }
}

function exitModal() {
    document.querySelector(".modal").classList.remove("open");
}

function deleteProductModal(productID) {
    var formData = new FormData();
    formData.append('productID', productID);
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/Cart/DeleteProduct', true);
    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const data = JSON.parse(xhr.responseText);
            console.log(data);
            document.querySelector(".modal").classList.remove("open");
            document.querySelector(".header__cart-notice").innerText = data.cartCount;
            document.querySelector(".table__total-count").innerText = `Có ${data.cartCount} sản phẩm trong giỏ hàng của bạn`;
            document.getElementById("product__" + productID).style.display = 'none';
            toast({ title: "Thông báo", msg: `${data.message}`, type: "success", duration: 5000 });
        }
    };
    xhr.send(formData);
}

function deleteProduct(productID) {
    let html = "";
    html += `
        <div class="modal">
            <div class="modal__overlay">
    
            </div>
            <div class="modal__body">
                <!--Form message -->
                <div class="auth-form">
                    <div class="auth-form__container">
                        <p class="auth-form__msg">Bạn muốn xoá mặt hàng này khỏi giỏ?</p>
                        <div class="auth-form__controls">
                            <button onclick="exitModal()" class="btn btn--primary">HUỶ</button>
                            <button class="btn" onclick="deleteProductModal(${productID})">ĐỒNG Ý</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        `;
    document.querySelector(".cart__message").innerHTML = html;
    document.querySelector(".modal").classList.add("open");
}

// Checkout
function checkout() {
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/Order/Checkout', true);
    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && xhr.status == 200) {
            data = JSON.parse(xhr.responseText);
            data.map(obj => {
                document.querySelector(".table__total-price").innerText = `${money(obj.dTotalMoney)} đ`;
            });
        }
    };
    xhr.send(null);
}

// Tách lấy chữ số
// Nguồn: http://vncoding.net/2015/10/30/tach-cac-chu-so-thuoc-hang-tram-hang-chuc-hang-don-vi/
function money(number) {
    let result = ``; // Nếu là chuỗi thì trán đặt biến là const
    // Vì Const là một hằng số, vì vậy khi khai báo biến const thì bạn phải gán giá trị cho nó luôn, 
    // sau này cũng không thể thay đổi giá trị cho biến.
    // Nguồn: https://freetuts.net/bien-va-khai-bao-bien-trong-javascript-265.html
    // Ví dụ số 9899999
    let millions = Math.floor(number / 1000000); // Chia cho 1000000 và làm tròn số ta được 9
    let hundred_thousand = Math.floor((number % 1000000) / 100000); // Chia lấy phần dư ta được 899999 và tiếp tục chia cho 100000 và làm tròn ta được 8
    let tens_of_thousands = Math.floor((number % 1000000 % 100000) / 10000); // Tương tự lấy phần dư của hàng trục nghìn rồi chia cho 10000 ta được 9
    let thousand = Math.floor((number % 1000000 % 100000 % 10000) / 1000); // Lấy phần dư hàng nghìn rồi chia cho 1000
    let hundreds = Math.floor((number % 1000000 % 100000 % 10000 % 1000) / 100); // Lấy phần dư hàng trăm chia cho 100
    let tens = Math.floor((number % 1000000 % 100000 % 10000 % 1000 % 100) / 10); // Lấy phần dư của hàng chục chia cho 10
    let unit = Math.floor(number % 1000000 % 100000 % 10000 % 1000 % 100 % 10); // Lấy phần dư hàng đơn vị
    if (millions == 0) {
        result = `${hundred_thousand}${tens_of_thousands}${thousand}.${hundreds}${tens}${unit}`;
    } else {
        result = `${millions}.${hundred_thousand}${tens_of_thousands}${thousand}.${hundreds}${tens}${unit}`;
    }
    return result;
}
console.log(money(9800999));