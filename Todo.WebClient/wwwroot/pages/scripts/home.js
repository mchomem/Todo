﻿Home = {

    self: this

    , user: JSON.parse(sessionStorage.getItem('user'))

    , uri: 'https://localhost:44332/api/TodoItem'

    , todos: []

    , init: function () {
        this.addControl();
        this.getUserData();
        this.attachEvent();
        this.getItems();
    }

    , addControl: function () {
        self.$spanUser = document.getElementById('spanUser');
        self.$btnExit = document.getElementById('btnExit');
        self.$addName = document.getElementById('add-name');
        self.$txtDeadLine = document.getElementById('txtDeadLine');
        self.$btnAddName = document.getElementById('btnAddName');
        self.$editId = document.getElementById('edit-id');
        self.$editName = document.getElementById('edit-name');
        self.$editDeadLine = document.getElementById('edit-deadLine');
        self.$editIsComplete = document.getElementById('edit-isComplete');
        self.$btnSaveEdit = document.getElementById('btnSaveEdit');
        self.$loader = document.getElementById('loader');
        self.$listing = document.getElementById('listing');
        self.$noData = document.getElementById('noData');
        self.$counter = document.getElementById('counter');
        self.$todosListing = document.getElementById('todosListing');
    }

    , getUserData: function () {
        self.$spanUser.innerHTML = Home.user.name.toUpperCase();
    }

    , attachEvent: function () {

        self.$btnExit.addEventListener('click', function () {
            sessionStorage.setItem('user', null);
            document.location.href = 'login.html';
        });

        self.$btnAddName.addEventListener('click', function () {
            Home.addItem();
        });

        self.$btnSaveEdit.addEventListener('click', function () {
            Home.updateItem();
        });
    }

    , getItems: function () {
        fetch(Home.uri + '/?userId=' + Home.user.userID, {
            method: 'GET'
            , headers: {
                'Authorization': `Bearer ${Home.user.token}`
            }
        })
            .then(response => response.json())
            .then(data => Home._displayItems(data))
            .catch(error => {
                alert('Unable to get items.');
                console.error('Unable to get items.', error);
            });
    }

    , addItem: function () {
        if (self.$addName.value.length === 0) {
            alert('Type a value');
            return;
        }

        const todoItem = {
            isDone: false,
            name: self.$addName.value.trim(),
            deadLine: (self.$txtDeadLine.value.length === 0 ? null : self.$txtDeadLine.value),
            createdBy: Home.user
        };

        fetch(Home.uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json'
                , 'Content-Type': 'application/json'
                , 'Authorization': `Bearer ${Home.user.token}`
            },
            body: JSON.stringify(todoItem)
        })
            .then(response => {
                response.json();
            })
            .then(() => {
                Home.getItems();
                self.$addName.value = '';
                self.$txtDeadLine.value = '';
                alert('New record added.');
            })
            .catch(error => console.error('Unable to add item.', error));
    }

    , deleteItem: function (id) {
        var op = confirm('Do you want to delete this record?');

        if (!op) {
            return;
        }

        fetch(`${Home.uri}/${id}`, {
            method: 'DELETE'
            , 'Authorization': `Bearer ${Home.user.token}`
        })
            .then(() => Home.getItems())
            .catch(error => console.error('Unable to delete item.', error));
    }

    , displayEditForm: function (id) {
        const item = todos.find(item => item.todoItemID === id);
        self.$editName.value = item.name;
        self.$editDeadLine.value = moment(item.deadLine).format('yyyy[-]MM[-]DD');
        self.$editId.value = item.todoItemID;
        self.$editIsComplete.checked = item.isDone;
    }

    , updateItem: function () {
        const itemId = self.$editId.value;

        const item = {
            todoItemID: parseInt(itemId, 10),
            name: self.$editName.value.trim(),
            deadLine: self.$editDeadLine.value,
            isDone: self.$editIsComplete.checked
        };

        fetch(`${Home.uri}/${itemId}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json'
                , 'Content-Type': 'application/json'
                , 'Authorization': `Bearer ${Home.user.token}`
            },
            body: JSON.stringify(item)
        })
            .then(() => Home.getItems())
            .catch(error => console.error('Unable to update item.', error));

        var successMessage = document.getElementById('success-message');
        successMessage.classList.remove('d-none');

        setTimeout(function () {
            document.getElementById('btnClose').click();
            successMessage.classList.add('d-none');
        }, 2000);

        return false;
    }

    , _displayCount: function (itemCount) {
        const name = (itemCount === 1) ? '' : 's';
        self.$counter.innerText = `${itemCount} record${name}`;
    }

    , _displayItems: function (data) {
        const tBody = self.$todosListing;
        tBody.innerHTML = '';

        Home._displayCount(data.length);

        const button = document.createElement('button');

        data.forEach(item => {

            let isDone = document.createElement('div');
            let innerSpan = document.createElement('span');
            innerSpan.classList.add('badge', 'mt-1', item.isDone ? 'bg-success' : 'bg-danger');
            innerSpan.innerHTML = item.isDone ? 'Yes' : 'No';
            innerSpan.style.width = '40px';
            isDone.append(innerSpan);

            let iconEdit = document.createElement('i');
            iconEdit.classList.add('fas', 'fa-marker');

            let editButton = button.cloneNode(false);
            editButton.setAttribute('title', 'Edit this!');
            editButton.appendChild(iconEdit);
            editButton.setAttribute('onclick', `Home.displayEditForm(${item.todoItemID})`);
            editButton.classList.add('btn', 'btn-sm', 'btn-primary');
            editButton.setAttribute('data-bs-toggle', 'modal');
            editButton.setAttribute('data-bs-target', '#todoModalEdit');

            let iconDelete = document.createElement('i');
            iconDelete.classList.add('fas', 'fa-minus-circle');

            let deleteButton = button.cloneNode(false);
            deleteButton.setAttribute('title', 'Remove this!');
            deleteButton.appendChild(iconDelete);
            deleteButton.setAttribute('onclick', `Home.deleteItem(${item.todoItemID})`);
            deleteButton.classList.add('btn', 'btn-sm', 'btn-danger');

            let tr = tBody.insertRow();

            let td1 = tr.insertCell(0);
            td1.appendChild(isDone);

            let charLimit = 15;

            let td2 = tr.insertCell(1);
            let task = document.createTextNode(item.name.length > charLimit ? `${item.name.substring(0, charLimit)} ...` : item.name);
            td2.appendChild(task);

            let td3 = tr.insertCell(2);
            let deadLine = document.createTextNode(item.deadLine !== null ? moment(item.deadLine).format('DD[/]MM[/]yyyy') : '-');
            td3.appendChild(deadLine);

            let td4 = tr.insertCell(3);
            td4.appendChild(editButton);

            let td5 = tr.insertCell(4);
            td5.appendChild(deleteButton);

        });

        todos = data;

        self.$loader.style.display = 'none';
        self.$listing.style.display = 'block';
    }

}
