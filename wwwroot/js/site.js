// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const loginModal = document.querySelector(".modal");


function openLoginModal(orderID) {
    loginModal.classList.add("open");
}

document.querySelector(".auth-form__switch-btn").addEventListener("click", () => {
    loginModal.classList.remove("open");
});

window.onclick = (event) => {
    if (event.target == loginModal) {
        loginModal.classList.remove("open");
    }
}

// Tìm kiếm sản phẩm
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

// load số lượng sản phẩm giỏ hàng
function getCartInfo() {
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/Cart/GetCartInfo', true);
    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const data = JSON.parse(xhr.responseText);
            // getCartCount(data);
        }
    }
    xhr.send(null);
}

//Lấy thông tin sản phẩm trong giỏ hàng

function getCartCount(data) {
    let html = "";
    console.log(data);
    console.log(document.querySelector(".navbar__cart-notice").innerText);
    for (let i = 0; i < data.length; i++) {
        document.querySelector(".navbar__cart-notice").innerText = data[0].cartCount
    }
}

// Tăng, giảm số lượng sản phẩm
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
                trParent.querySelector("#money").innerText = money.money;
            }
        }
        xhr.send(formData);
    }
}

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
                trParent.querySelector("#money").innerText = money.money;
            }
        };
        xhr.send(formData);
    } else if ((parseInt(tru) == 0)) {
        if (confirm("Bạn muốn xoá sản phẩm này khỏi giỏ hàng")) {

        }
    }
}

// Tăng / Giảm số lượng sản phẩm trong chi tiết sản phẩm
function increaseProduct(event) {
    const parentElement = event.target.parentNode;
    var increase = parentElement.querySelector("#qnt").value;
    if (parseInt(increase) < 100) {
        parentElement.querySelector("#qnt").value = parseInt(increase) + 1;
    }
}

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
        alert('Bạn chưa nhập số lượng sản phẩm!');
    } else {
        var xhr = new XMLHttpRequest();
        xhr.open('get', '/Cart/AddToCart/' + productID + '/' + price + '/' + quantity + '', true);
        xhr.onreadystatechange = () => {
            if (xhr.readyState == 4 && xhr.status == 200) {
                const obj = JSON.parse(xhr.responseText);
                alert(obj.msg);
            }
        }
        xhr.send(null);
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
                alert(`${msg.msg}`);
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
                document.querySelector(".money").innerText = obj.dTotalMoney;
            });
        }
    };
    xhr.send(null);
}