NewAccount = {

    self: this

    , uri: 'https://localhost:44332/api/User'

    , init: function () {
        this.addControl();
        this.attachEvent();
    }

    , addControl: function () {
        self.$txtName = document.getElementById('txtName'); 
        self.$txtLogin = document.getElementById('txtLogin'); 
        self.$txtPassword = document.getElementById('txtPassword');
        self.$iconPassword = document.getElementById('iconPassword');
        self.$btnShowHidePassword = document.getElementById('btnShowHidePassword');
        self.$btnCancel = document.getElementById('btnCancel');
        self.$btnSave = document.getElementById('btnSave');
        self.$formNewUser = document.getElementById('formNewUser');
        self.$messageNewUser = document.getElementById('messageNewUser');
    }

    , attachEvent: function () {
        self.$btnShowHidePassword.addEventListener('click', function () {
            if (self.$txtPassword.getAttribute('type') == 'password') {
                self.$iconPassword.classList.remove('fa-eye-slash');
                self.$iconPassword.classList.add('fa-eye');
                self.$txtPassword.setAttribute('type', 'text');
            }
            else {
                self.$iconPassword.classList.add('fa-eye-slash');
                self.$iconPassword.classList.remove('fa-eye');
                self.$txtPassword.setAttribute('type', 'password');
            }
        });

        self.$btnCancel.addEventListener('click', function () {
            document.location.href = 'login.html';
        });

        self.$btnSave.addEventListener('click', function () {
            NewAccount.save();
        });
    }

    , save: function () {

        if (self.$txtName.value.length === 0
            || self.$txtLogin.value.length === 0
            || self.$txtPassword.value.length === 0) {
            alert('Type the required information.');
            return;
        }

        const newUser = {
            name: self.$txtName.value,
            login: self.$txtLogin.value,
            password: self.$txtPassword.value
        };

        fetch(NewAccount.uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newUser)
        })
            .then((response) => {
                if (response.ok) {
                    self.$formNewUser.style.display = 'none';
                    self.$messageNewUser.style.display = 'block';
                    console.log('New user created.');
                } else {
                    console.log(response);
                }
            })
            .catch(error => {
                console.error('Unable to add item.', error)
                alert('Error');
            });
    }
}
