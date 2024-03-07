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