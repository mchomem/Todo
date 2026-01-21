const Login = {
    self: this

    , init: function () {
        this.addControls();
        this.attachEvent();
    }

    , addControls: function () {
        self.$txtUser = document.getElementById('txtUser');
        self.$txtPassword = document.getElementById('txtPassword');
        self.$btnLogin = document.getElementById('btnLogin');
        self.$signInLabel = document.getElementById('signInLabel');
        self.$loader = document.getElementById('loader');
        self.$spanYear = document.getElementById('spanYear');

        self.$spanYear.innerHTML = new Date().getFullYear();
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
        if (self.$txtUser.value.length === 0
            || self.$txtPassword.value.length === 0) {
            Swal.fire({
                title: 'Warning',
                text: 'Login and password required.',
                icon: 'warning',
                confirmButtonText: 'Ok'
            });
            return
        }

        const url = `https://localhost:44332/api/auth/authentication?login=${self.$txtUser.value}&password=${self.$txtPassword.value}`;

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

                const responseApi = data.data;

                if (!data.success) {
                    self.$signInLabel.style.display = 'block';
                    self.$loader.style.display = 'none';
                    Swal.fire({
                        title: 'Warning',
                        text: responseApi,
                        icon: 'warning',
                        confirmButtonText: 'Ok'
                    });

                    return;
                }

                const apiReponse = data.data;
                sessionStorage.setItem('user', JSON.stringify(apiReponse));
                document.location.href = 'home.html';
            })
            .catch(error => {
                console.error('Error', error)

                self.$loader.style.display = 'none';
                self.$signInLabel.style.display = 'block';

                Swal.fire({
                    title: 'Error',
                    text: error,
                    icon: 'error',
                    confirmButtonText: 'Ok'
                });
            });
    }
}
