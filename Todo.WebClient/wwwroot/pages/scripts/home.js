const Home = {
    self: this
    , userCache: JSON.parse(sessionStorage.getItem('user'))
    , uriTodoItem: 'https://localhost:44332/api/TodoItem'
    , uriUser: 'https://localhost:44332/api/user'
    , imagePicture: undefined
    , todos: []

    , init: function () {
        this.addControls();
        this.getUserData();
        this.attachEvent();
        this.getItems();
    }

    , addControls: function () {
        // Controls for header app.
        self.$spanUser = document.getElementById('spanUser');
        self.$btnEditUserAccount = document.getElementById('btnEditUserAccount');
        self.$toogleTheme = document.getElementById('toogleTheme');
        self.$iconToogleTheme = document.getElementById('iconToogleTheme');
        self.$btnExit = document.getElementById('btnExit');

        // Constrols for form insert todo item.
        self.$addName = document.getElementById('add-name');
        self.$txtDeadLine = document.getElementById('txtDeadLine');
        self.$btnAddName = document.getElementById('btnAddName');

        // Controls for modal user edit.
        self.$imgUserPicture = document.getElementById('imgUserPicture');
        self.$userPicture = '';
        self.$btnLoadPicture = document.getElementById('btnLoadPicture');
        self.$btnDeletePicture = document.getElementById('btnDeletePicture');
        self.$txtUserName = document.getElementById('txtUserName');
        self.$buttonDeleteMyAccount = document.getElementById('buttonDeleteMyAccount');
        self.$btnCloseEditUser = document.getElementById('btnCloseEditUser');
        self.$btnSaveEditUser = document.getElementById('btnSaveEditUser');

        // Controls for modal change password.
        self.$txtCurrentPassword = document.getElementById('txtCurrentPassword');
        self.$iconCurrentPassword = document.getElementById('iconCurrentPassword');
        self.$btnShowHideCurrentPassword = document.getElementById('btnShowHideCurrentPassword');
        self.$txtNewPassword = document.getElementById('txtNewPassword');
        self.$iconNewPassword = document.getElementById('iconNewPassword');
        self.$btnShowHideNewPassword = document.getElementById('btnShowHideNewPassword');
        self.$btnCloseChangePassword = document.getElementById('btnCloseChangePassword');
        self.$btnSaveChangePassword = document.getElementById('btnSaveChangePassword');

        // Controls for modal form edit todo item.
        self.$editId = document.getElementById('edit-id');
        self.$editTaskName = document.getElementById('edit-taskName');
        self.$editDeadLine = document.getElementById('edit-deadLine');
        self.$editIsComplete = document.getElementById('edit-isComplete');
        self.$btnClose = document.getElementById('btnClose');
        self.$btnSaveEdit = document.getElementById('btnSaveEdit');

        // Another controls.
        self.$loader = document.getElementById('loader');
        self.$listing = document.getElementById('listing');
        self.$noData = document.getElementById('noData');
        self.$counter = document.getElementById('counter');
        self.$todosListing = document.getElementById('todosListing');
    }

    , attachEvent: function () {

        self.$toogleTheme.addEventListener('click', function () {
            if (self.$iconToogleTheme.classList.contains('fa-moon')) {
                self.$iconToogleTheme.classList.remove('fa-moon');
                self.$iconToogleTheme.classList.add('fa-sun');
                Home.applyTheme('dark');
            } else {
                self.$iconToogleTheme.classList.remove('fa-sun');
                self.$iconToogleTheme.classList.add('fa-moon');
                Home.applyTheme('light');
            }
        });

        self.$btnExit.addEventListener('click', function () {
            sessionStorage.setItem('user', null);
            document.location.href = 'login.html';
        });

        self.$btnEditUserAccount.addEventListener('click', function () {
            Home.loadUser();
        });

        self.$imgUserPicture.addEventListener('click', function () {
            self.$btnLoadPicture.click();
        });

        self.$btnLoadPicture.addEventListener('change', function () {
            Home.upLoadPicture();
        });

        self.$btnDeletePicture.addEventListener('click', async function () {
            let confirm = await Swal.fire({
                title: 'Question',
                text: 'Do you want to delete your picture?',
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'Yes',
                cancelButtonText: 'No'
            });

            if (!confirm.isConfirmed) {
                return;
            }

            Home.deletePicture();
        });

        self.$btnShowHideCurrentPassword.addEventListener('click', function () {
            if (self.$txtCurrentPassword.getAttribute('type') == 'password') {
                self.$iconCurrentPassword.classList.remove('fa-eye-slash');
                self.$iconCurrentPassword.classList.add('fa-eye');
                self.$txtCurrentPassword.setAttribute('type', 'text');
            } else {
                self.$iconCurrentPassword.classList.add('fa-eye-slash');
                self.$iconCurrentPassword.classList.remove('fa-eye');
                self.$txtCurrentPassword.setAttribute('type', 'password');
            }
        });

        self.$btnShowHideNewPassword.addEventListener('click', function () {
            if (self.$txtNewPassword.getAttribute('type') == 'password') {
                self.$iconNewPassword.classList.remove('fa-eye-slash');
                self.$iconNewPassword.classList.add('fa-eye');
                self.$txtNewPassword.setAttribute('type', 'text');
            } else {
                self.$iconNewPassword.classList.add('fa-eye-slash');
                self.$iconNewPassword.classList.remove('fa-eye');
                self.$txtNewPassword.setAttribute('type', 'password');
            }
        });

        self.$buttonDeleteMyAccount.addEventListener('click', async function () {
            await Home.deleteUserAccount();
        });

        self.$btnCloseEditUser.addEventListener('click', function () {
            Home.clearUserEditForm();
        });

        self.$btnSaveEditUser.addEventListener('click', function () {
            Home.saveUser();
        });

        self.$btnCloseChangePassword.addEventListener('click', function () {
            Home.clearUserChangePasswordForm();
        });

        self.$btnSaveChangePassword.addEventListener('click', function () {
            Home.changePassword();
        });

        self.$btnAddName.addEventListener('click', function () {
            Home.addItem();
        });

        self.$btnSaveEdit.addEventListener('click', function () {
            Home.updateItem();
        });
    }

    , getUserData: function () {
        self.$spanUser.innerHTML =
            (Home.userCache.name.length <= 10
                ? Home.userCache.name
                : `${Home.userCache.name.substring(0, 10)} ...`).toUpperCase();
    }

    , getItems: function () {
        fetch(`${Home.uriTodoItem}/?userId=${Home.userCache.userID}`, {
            method: 'GET'
            , headers: {
                'Authorization': `Bearer ${Home.userCache.token}`
            }
        })
            .then(response => response.json())
            .then(data => Home._displayItems(data))
            .catch(error => {
                Swal.fire({
                    title: 'Error!',
                    text: 'Unable to get items.',
                    icon: 'error',
                    confirmButtonText: 'Ok'
                });
                console.error('Unable to get items.', error);

                self.$loader.style.display = 'none';
            });
    }

    , addItem: function () {
        if (self.$addName.value.length === 0) {
            Swal.fire({
                title: 'Warning',
                text: 'Type a value',
                icon: 'warning',
                confirmButtonText: 'Ok'
            });
            return;
        }

        const todoItem = {
            isDone: false,
            name: self.$addName.value.trim(),
            deadLine: (self.$txtDeadLine.value.length === 0 ? null : self.$txtDeadLine.value),
            createdByID: Home.userCache.userID,
        };

        fetch(Home.uriTodoItem, {
            method: 'POST'
            , headers: {
                'Accept': 'application/json'
                , 'Content-Type': 'application/json'
                , 'Authorization': `Bearer ${Home.userCache.token}`
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
                Swal.fire({
                    title: 'Information',
                    text: 'New record added.',
                    icon: 'info',
                    confirmButtonText: 'Ok'
                });
            })
            .catch(error => console.error('Unable to add item.', error));
    }

    , completeTask: async function (id) {
        let confirm = await Swal.fire({
            title: 'Question',
            text: 'Do you want to mark this task as complete?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'
        });

        if (!confirm.isConfirmed) {
            return;
        }

        fetch(`${Home.uriTodoItem}/complete/${id}`, {
            method: 'PUT'
            , headers: {
                'Accept': 'application/json'
                , 'Content-Type': 'application/json'
                , 'Authorization': `Bearer ${Home.userCache.token}`
            }
        })
            .then(response => {
                response.json();
            })
            .then(() => {
                Home.getItems();
            })
            .catch(error => console.error('Unable to process item.', error));
    }

    , deleteItem: async function (id) {

        let confirm = await Swal.fire({
            title: 'Question',
            text: 'Do you want to delete this record?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'
        });

        if (!confirm.isConfirmed) {
            return;
        }

        fetch(`${Home.uriTodoItem}/${id}`, {
            method: 'DELETE'
            , headers: {
                'Authorization': `Bearer ${Home.userCache.token}`
            }
        })
        .then(() => {
            Home.getItems();
            Swal.fire({
                title: 'Information',
                text: 'Todo item deleted.',
                icon: 'info',
                confirmButtonText: 'Ok'
            });
        })
        .catch(error => {
            console.error('Unable to delete item.', error)
            Swal.fire({
                title: 'Error',
                text: error,
                icon: 'error',
                confirmButtonText: 'Ok'
            });
        });
    }

    , displayEditForm: function (id) {
        const item = todos.find(item => item.todoItemID === id);
        self.$editTaskName.value = item.name;
        self.$editDeadLine.value = moment(item.deadLine).format('yyyy[-]MM[-]DD');
        self.$editId.value = item.todoItemID;
        self.$editIsComplete.checked = item.isDone;
    }

    , updateItem: function () {
        const itemId = self.$editId.value;

        const item = {
            todoItemID: parseInt(itemId, 10),
            name: self.$editTaskName.value.trim(),
            deadLine: self.$editDeadLine.value,
            isDone: self.$editIsComplete.checked
        };

        fetch(`${Home.uriTodoItem}/${itemId}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json'
                , 'Content-Type': 'application/json'
                , 'Authorization': `Bearer ${Home.userCache.token}`
            },
            body: JSON.stringify(item)
        })
            .then(() => Home.getItems())
            .catch(error => console.error('Unable to update item.', error));

        var successMessage = document.getElementById('success-message');
        successMessage.classList.remove('d-none');

        setTimeout(function () {
            self.$btnClose.click();
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

        if (data.length === 0) {
            Home._displayNoData(tBody);
            return;
        }

        Home._displayCount(data.length);

        const button = document.createElement('button');

        data.forEach(item => {
            let isDone = document.createElement('div');
            let innerSpan = document.createElement('span');
            innerSpan.classList.add('badge', 'mt-1', item.isDone ? 'bg-success' : 'bg-danger');
            innerSpan.innerHTML = item.isDone ? 'Yes' : 'No';
            innerSpan.style.width = '40px';
            isDone.append(innerSpan);

            let iconCompleteTask = document.createElement('i');
            iconCompleteTask.classList.add('fas', 'fa-check');

            let completeTaskButton = button.cloneNode(false);
            completeTaskButton.setAttribute('title', 'Complete Task');
            completeTaskButton.appendChild(iconCompleteTask);
            completeTaskButton.setAttribute('onclick', `Home.completeTask(${item.todoItemID})`);
            completeTaskButton.classList.add('btn', 'btn-sm', 'btn-success', 'me-2');

            let iconEdit = document.createElement('i');
            iconEdit.classList.add('fas', 'fa-edit');

            let editButton = button.cloneNode(false);
            editButton.setAttribute('title', 'Edit this!');
            editButton.appendChild(iconEdit);
            editButton.setAttribute('onclick', `Home.displayEditForm(${item.todoItemID})`);
            editButton.classList.add('btn', 'btn-sm', 'btn-primary', 'me-2');
            editButton.setAttribute('data-bs-toggle', 'modal');
            editButton.setAttribute('data-bs-target', '#todoModalEdit');

            if (item.isDone) {
                completeTaskButton.setAttribute('disabled', 'disabled');
                editButton.setAttribute('disabled', 'disabled');
            }

            let iconDelete = document.createElement('i');
            iconDelete.classList.add('fas', 'fa-minus-circle');

            let deleteButton = button.cloneNode(false);
            deleteButton.setAttribute('title', 'Remove this!');
            deleteButton.appendChild(iconDelete);
            deleteButton.setAttribute('onclick', `Home.deleteItem(${item.todoItemID})`);
            deleteButton.classList.add('btn', 'btn-sm', 'btn-danger');

            let tr = tBody.insertRow();

            // Check if is late.
            if (Home.checkIsLate(item.deadLine) && !item.isDone) {
                tr.style.backgroundColor = '#ffb3b3';
                let spanIsLate = document.createElement('span');
                spanIsLate.classList.add('badge', 'm-1', 'bg-danger');
                spanIsLate.innerHTML = 'Is late';
                isDone.append(spanIsLate);
            }

            // Column Is Complete?
            let td1 = tr.insertCell(0);
            td1.classList.add('text-center');
            td1.appendChild(isDone);

            let charLimit = 15;

            // Column Task
            let td2 = tr.insertCell(1);
            let task = document.createTextNode(item.name.length > charLimit ? `${item.name.substring(0, charLimit)} ...` : item.name);
            td2.appendChild(task);

            // Column Dead line
            let td3 = tr.insertCell(2);
            td3.classList.add('text-center');
            let deadLine = document.createTextNode(item.deadLine !== null ? moment(item.deadLine).format('DD[/]MM[/]yyyy') : '-');
            td3.appendChild(deadLine);

            // Column Actions
            let td4 = tr.insertCell(3);
            td4.classList.add('text-center');
            td4.appendChild(completeTaskButton);
            td4.appendChild(editButton);
            td4.appendChild(deleteButton);
        });

        todos = data;

        self.$loader.style.display = 'none';
        self.$listing.style.display = 'block';
    }

    , _displayNoData: function (tBody) {
        let tr = tBody.insertRow();
        let td1 = tr.insertCell(0);
        td1.setAttribute('colspan', '4');
        td1.appendChild(self.$noData);

        self.$noData.setAttribute('style', 'display:block');

        self.$loader.style.display = 'none';
        self.$listing.style.display = 'block';
    }

    , checkIsLate: function (date) {
        return Date.parse(date) < Date.now();
    }

    , loadUser: function () {
        self.$txtUserName.value = Home.userCache.name;

        if (Home.userCache.picture != undefined && Home.userCache.picture.length != 0) {
            self.$imgUserPicture.setAttribute('src', `data:image/png;base64,${Home.userCache.picture}`);
            self.$userPicture = Home.userCache.picture;
        }
    }

    , deleteUserAccount: async function () {
        let confirm = await Swal.fire({
            title: 'Question',
            html: 'Do you want to delete your account?<br>All task logs will be lost.<br>This action is irreversible.',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'
        });

        if (!confirm.isConfirmed) {
            return;
        }

        fetch(`${Home.uriUser}/${Home.userCache.userID}`, {
            method: 'DELETE'
            , headers: {
                'Authorization': `Bearer ${Home.userCache.token}`
                , 'Content-type': 'application/json; charset=utf-8'
            }
        }).then(() => {
            document.location.href = 'account-deleted.html';
        });
    }

    , saveUser: function () {
        let user = {
            userId: Home.userCache.userID
            , name: self.$txtUserName.value
            , isActive: Home.userCache.isActive
            , picture: self.$userPicture
        };

        fetch(`${Home.uriUser}/${Home.userCache.userID}`, {
            method: 'PUT'
            , headers: {
                'Authorization': `Bearer ${Home.userCache.token}`
                , 'Content-type': 'application/json; charset=utf-8'
            }
            , body: JSON.stringify(user)
        })
            .then(() => {
                Home.userCache.picture = user.picture;
                Home.userCache.name = user.name;
                sessionStorage.setItem('user', JSON.stringify(user));
                Home.getUserData();

                Swal.fire({
                    title: 'Informartion',
                    text: 'User updated.',
                    icon: 'info',
                    confirmButtonText: 'Ok'
                });
            });

        self.$btnCloseEditUser.click();
    }

    , changePassword: function () {
        let currentPassword = self.$txtCurrentPassword.value;
        let newPassword = self.$txtNewPassword.value;

        if (self.$txtCurrentPassword.value.length === 0
            || self.$txtNewPassword.value.length === 0) {
            Swal.fire({
                title: 'Warning',
                text: 'Fill the required filds.',
                icon: 'warning',
                confirmButtonText: 'Ok'
            });
            return;
        }

        fetch(`${Home.uriUser}/change-password/?userId=${Home.userCache.userID}&currentPassword=${currentPassword}&newPassword=${newPassword}`, {
            method: 'PUT'
            , headers: {
                'Authorization': `Bearer ${Home.userCache.token}`
            }
        })
            .then(() => {
                Home.clearUserChangePasswordForm();
                Swal.fire({
                    title: 'Information',
                    text: 'Password changed.',
                    icon: 'info',
                    confirmButtonText: 'Ok'
                });
                self.btnCloseChangePassword.click();
            })
            .catch(error => {
                Swal.fire({
                    title: 'Error',
                    text: error,
                    icon: 'error',
                    confirmButtonText: 'Ok'
                });

                console.error(`${error}`)
            });
    }

    , upLoadPicture: function () {
        let file = self.$btnLoadPicture.files[0];

        if (file === undefined)
            return;

        imagePicture = self.$btnLoadPicture.files[0];
        let reader = new FileReader();

        reader.onloadend = function () {
            self.$imgUserPicture.src = reader.result;
            var surrogate = reader.result.substring(0, reader.result.lastIndexOf(',') + 1);
            self.$userPicture = reader.result.replace(surrogate, '');
        }

        if (imagePicture) {
            reader.readAsDataURL(imagePicture);
        } else {
            self.$imgUserPicture.src = "#";
        }
    }

    , deletePicture: function () {
        fetch(`${Home.uriUser}/delete-user-picture/${Home.userCache.userID}`, {
            method: 'DELETE'
            , headers: {
                'Authorization': `Bearer ${Home.userCache.token}`
            }
        })
            .then(() => {
                let user = Home.userCache;
                Home.userCache.picture = null;
                sessionStorage.setItem('user', JSON.stringify(user));
                self.$imgUserPicture.setAttribute('src', '../assets/images/user-default-picture.png');
                Swal.fire({
                    title: 'Information',
                    text: 'User picture delete!',
                    icon: 'info',
                    confirmButtonText: 'Ok'
                });
            })
            .catch(error => {
                console.error(`${error}`)
                Swal.fire({
                    title: 'Error',
                    text: error,
                    icon: 'error',
                    confirmButtonText: 'Ok'
                });
            });
    }

    , clearUserEditForm: function () {
        self.$imgUserPicture.setAttribute('src', '../assets/images/user-default-picture.png');
    }

    , clearUserChangePasswordForm: function () {
        self.$txtCurrentPassword.value = '';
        self.$txtNewPassword.value = '';
    }

    , applyTheme: function (theme) {
        document.body.setAttribute('class', '');
        document.body.classList.add(theme);

        document.querySelectorAll('#userModalEdit > div.modal-dialog > div.modal-content > div.modal-header').forEach(function (el, index, arr) {
            el.classList.remove('dark', 'light');
        });

        document.querySelectorAll('div.modal-dialog > div.modal-content > div.modal-header').forEach(function (el, index, arr) {
            el.classList.add(theme);
        });

        document.querySelectorAll('div.modal-dialog > div.modal-content > div.modal-body').forEach(function (el, index, arr) {
            el.classList.remove('dark', 'light');
        });

        document.querySelectorAll('div.modal-dialog > div.modal-content > div.modal-body').forEach(function (el, index, arr) {
            el.classList.add(theme);
        });

        document.querySelectorAll('div.modal-dialog > div.modal-content > div.modal-footer').forEach(function (el, index, arr) {
            el.classList.remove('dark', 'light');
        });

        document.querySelectorAll('div.modal-dialog > div.modal-content > div.modal-footer').forEach(function (el, index, arr) {
            el.classList.add(theme);
        });

        document.querySelectorAll('div.card>div.card-header').forEach(function (el, index, arr) {
            el.classList.remove('dark', 'light');
        });

        document.querySelectorAll('div.card>div.card-header').forEach(function (el, index, arr) {
            el.classList.add(theme);
        });

        document.querySelectorAll('div.card>div.card-body').forEach(function (el, index, arr) {
            el.classList.remove('dark', 'light');
        });

        document.querySelectorAll('div.card>div.card-body').forEach(function (el, index, arr) {
            el.classList.add(theme);
        });

        document.querySelectorAll('div.card>div.card-footer').forEach(function (el, index, arr) {
            el.classList.remove('dark', 'light');
        });

        document.querySelectorAll('div.card>div.card-footer').forEach(function (el, index, arr) {
            el.classList.add(theme);
        });

        document.querySelectorAll('table').forEach(function (el, index, arr) {
            el.classList.remove('dark', 'light');
        });

        document.querySelectorAll('table').forEach(function (el, index, arr) {
            el.classList.add(theme);
        });
    }
}
