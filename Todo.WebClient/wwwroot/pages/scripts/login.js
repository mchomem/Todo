﻿Login = {

    self: this

    , init: function () {
        this.addConstrol();
        this.attachEvent();
    }

    , addConstrol: function () {
        self.$txtUser = document.getElementById('txtUser');
        self.$txtPassword = document.getElementById('txtPassword');
        self.$btnLogin = document.getElementById('btnLogin');
        self.$signInLabel = document.getElementById('signInLabel');
        self.$loader = document.getElementById('loader');
    }

    , attachEvent: function () {
        self.$btnLogin.addEventListener('click', function () {
            Login.getAuthentication();
        });

        self.$txtUser.addEventListener('keyup', function (event) {
            if (event.keyCode === 13) {
                Login.getAuthentication();
            }
        });

        self.$txtPassword.addEventListener('keyup', function (event) {
            if (event.keyCode === 13) {
                Login.getAuthentication();
            }
        });
    }

    , getAuthentication: function () {

        if (self.$txtUser.value.length === 0 || self.$txtPassword.value.length === 0) {
            alert('Login and password required.');
            return
        }

        let url = `https://localhost:44332/api/user?login=${self.$txtUser.value}&password=${self.$txtPassword.value}`;

        self.$signInLabel.style.display = 'none';
        self.$loader.style.display = 'block';

        fetch(url, {
            method: 'get'            
            , headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        })
            .then(response => response.json())
            .then(data => {
                sessionStorage.setItem('user', JSON.stringify(data));
                document.location.href = 'home.html';
            })
            .catch(error => console.error('Error', error));
    }

}
