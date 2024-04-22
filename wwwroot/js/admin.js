function getData() {
    var xhr = new XMLHttpRequest();
    xhr.open('post', '/Admin/GetData', true);
    xhr.onreadystatechange = () => {
        if (xhr.readyState == 4 && xhr.status == 200) {
            const data = JSON.parse(xhr.responseText);
            console.table(data);
            let html = "";
            html += data.map((obj, index) => `
            <tr>
                <td>${index}</td>
                <td>${obj.sCategoryName}</td>
                <td>${obj.sCategoryDescription}</td>
            </tr>
            `).join('');
            document.getElementById('table__body').innerHTML = html;
        }
    }
    xhr.send(null);
}
getData();



