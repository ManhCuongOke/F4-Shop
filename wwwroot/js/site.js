// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const editModal = document.querySelector(".edit__modal");


function openEditModal(orderID) {
    editModal.classList.add("active-modal");
    // console.log(orderID);
    // var formData = new FormData();
    // formData.append("orderID", orderID);
    // var xhr = new XMLHttpRequest();
    // xhr.open('post', '/Home/EditOrder', true);
    // xhr.onreadystatechange = () => {
    //     if (xhr.readyState == 4 && xhr.status == 200) {
    //         const data = JSON.parse(xhr.responseText);
    //         data.map(obj => 
    //             document.querySelector(".input-order-id").value = obj.pK_iOrderID
    //             );
    //     }
    // }
    // xhr.send(formData);
}

document.querySelector(".edit__modal-close").addEventListener("click", () => {
    editModal.classList.remove("active-modal");
});

window.onclick = (event) => {
    if (event.target == editModal) {
        editModal.classList.remove("active-modal");
    }
}

//AddToCart
function addToCart(PK_iProductID) {
    var xhr = new XMLHttpRequest();
    xhr.open('get', '/Cart/AddToCart/' + PK_iProductID + '', true);
    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const obj = JSON.parse(xhr.responseText);
            alert(obj.msg);
        }
    }
    xhr.send(null);
}

// load số lượng sản phẩm giỏ hàng
function getCartInfo() {
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/Cart/Index', true);
    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const data = JSON.parse(xhr.responseText);
            getCartCount(data);
        }
    }
    xhr.send(null);
}

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

function deleteProduct(productID) {
    if (confirm("Bạn có chắc muốn xoá sản phẩm này khỏi giỏ hàng?")) {
        var formData = new FormData();
        formData.append('productID', productID);
        var xhr = new XMLHttpRequest();
        xhr.open('post', '/Cart/DeleteProduct', true);
        xhr.onreadystatechange = () => {
            if (xhr.readyState == 4 && xhr.status == 200) {
                const msg = JSON.parse(xhr.responseText);
                alert(`${msg.msg}`);
                document.getElementById("product__" + productID).style.display = 'none';
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