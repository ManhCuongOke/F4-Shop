// lấy số lượng sản phẩm giỏ hàng
window.onload = () => {
    getCartInfo();
}

function getCartInfo() {
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/Cart/GetCartInfo', true);
    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const data = JSON.parse(xhr.responseText);
            console.log(data);
            let html = "";
            html += data.map((obj, index) => `
            <tr id="product__${obj.pK_iProductID}">
                <td data-label="Chọn">
                    <input type="checkbox" class="cart__checkout-input" name="" id="" />
                </td>
                <td data-label="Tên sản phẩm">
                    <p class="cart__product-name">${obj.sProductName}</p>
                </td>
                <td data-label="Ảnh"><img src="/img/${obj.sImageUrl}" /></td>
                <td data-label="Đơn giá">${obj.dUnitPrice} đ</td>
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
                <td data-label="Thành tiền" id="money">${obj.dMoney} đ</td>
                <td>
                    <div class='btn-tools'>
                        <a class='btn-tool btn-tool__del'
                            href='javascript:deleteProduct(@cart.PK_iProductID)' title='Xoá sản phẩm'><i
                                class='uil uil-trash'></i></a>
                    </div>
                </td>
            </tr>
            `).join('');
            document.getElementById("table__body").innerHTML = html;
        }
    }
    xhr.send(null);
}

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
                const money = JSON.parse(xhr.responseText);
                const trParent = parentElement.parentNode.parentNode;
                // console.log(money.money);
                trParent.querySelector("#money").innerText = `${money.money} đ`;
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
                const money = JSON.parse(xhr.responseText);
                const trParent = parentElement.parentNode.parentNode;
                trParent.querySelector("#money").innerText = `${money.money} đ`;
            }
        };
        xhr.send(formData);
    } else if ((parseInt(tru) == 0)) {
        if (confirm("Bạn muốn xoá sản phẩm này khỏi giỏ hàng")) {

        }
    }
}

function deleteProduct(productID) {
    if (confirm("Bạn có chắc muốn xoá sản phẩm này khỏi giỏ hàng?")) {
        var formData = new FormData();
        formData.append('productID', productID);
        var xhr = new XMLHttpRequest();
        xhr.open('post', '/Cart/DeleteProduct', true);
        xhr.onreadystatechange = () => {
            if (xhr.readyState == 4 && xhr.status == 200) {
                const msg = JSON.parse(xhr.responseText);
                document.getElementById("product__" + productID).style.display = 'none';
                // alert(`${msg.msg}`);
                toast({title: "Thông báo", msg: `${msg.msg}`, type: "success", duration: 5000});
            } 
        };
        xhr.send(formData);
    }
}

// Checkout
function checkout() {
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/Order/Checkout', true);
    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && xhr.status == 200) {
            data = JSON.parse(xhr.responseText);
            data.map(obj => {
                document.querySelector(".table__total-price").innerText = `${obj.dTotalMoney} đ`;
            });
        }
    };
    xhr.send(null);
}