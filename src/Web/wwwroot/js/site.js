const API_URL = "http://localhost:5268/api/values";

let currentPage = 1;
const pageSize = 3;
let fullData = [];

const itemTemplateElement = document.getElementById('itemTemplate');
const itemsContainerElement = document.getElementById('itemsContainer');
const formElement = document.getElementById('itemForm');
const dataTableElement = document.getElementById("dataTable");
const paginationElement = document.getElementById("pagination");

let count = 1;

function addItem() {
    const clone = itemTemplateElement.content.cloneNode(true);

    const codeInput = clone.querySelector('[placeholder="Code"]');
    const valueInput = clone.querySelector('[placeholder="Value"]');

    codeInput.value = count;
    valueInput.value = 'value' + count;

    ++count;

    itemsContainerElement.appendChild(clone);
}

function removeItem(button) {
    button.closest('.item').remove();
}

function moveUp(button) {
    const item = button.closest('.item');
    const previous = item.previousElementSibling;
    if (previous) {
        itemsContainerElement.insertBefore(item, previous);
    }
}

function moveDown(button) {
    const item = button.closest('.item');
    const next = item.nextElementSibling;
    if (next) {
        itemsContainerElement.insertBefore(next, item);
    }
}

formElement.addEventListener('submit', function (event) {
    event.preventDefault();

    const items = Array.from(itemsContainerElement.querySelectorAll('.item'));
    const data = items.map(item => {
        const code = parseInt(item.querySelector('[placeholder="Code"]').value);
        const value = item.querySelector('[placeholder="Value"]').value;

        return {[code]: value};
    });

    console.log(JSON.stringify(data));

    fetch(API_URL, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (!response.ok)
                throw new Error("Ошибка при отправке");

            alert(`Успешно отправлено`);
        })
        .catch(error => {
            alert(`Ошибка: ${error}`);
        });
});

async function getValues() {
    const res = await fetch(API_URL);
    fullData = await res.json();
    currentPage = 1;
    renderTable();
    renderPagination();
}

function renderTable() {
    const start = (currentPage - 1) * pageSize;
    const pageData = fullData.slice(start, start + pageSize);

    dataTableElement.innerHTML = "";
    for (const item of pageData) {
        const row = `<tr><td>${item.orderId}</td><td>${item.code}</td><td>${item.value}</td></tr>`;
        dataTableElement.insertAdjacentHTML("beforeend", row);
    }
}

function renderPagination() {
    const pageCount = Math.ceil(fullData.length / pageSize);
    paginationElement.innerHTML = "";

    if (pageCount <= 1) return;

    for (let i = 1; i <= pageCount; i++) {
        const li = document.createElement("li");
        li.className = "page-item" + (i === currentPage ? " active" : "");
        li.innerHTML = `<a class="page-link" href="#">${i}</a>`;
        li.addEventListener("click", (event) => {
            event.preventDefault();
            currentPage = i;
            renderTable();
            renderPagination();
        });
        paginationElement.appendChild(li);
    }
}
