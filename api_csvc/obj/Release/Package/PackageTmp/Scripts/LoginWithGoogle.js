function handleCredentialResponse(response) {
    let info = JSON.parse(atob(response.credential.split('.')[1]));
    Session_Login(info.email, info.name);
}

function signIn() {
    google.accounts.oauth2.initTokenClient({
        client_id: "779950978274-lcdtkgaqbs53bmip9fpkfiala5g6i735.apps.googleusercontent.com",
        scope: "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email",
        callback: (response) => {
            if (response.access_token) {
                fetch("https://www.googleapis.com/oauth2/v3/userinfo", {
                    headers: { "Authorization": `Bearer ${response.access_token}` }
                })
                    .then(res => res.json())
                    .then(userInfo => {
                        Session_Login(userInfo.email, userInfo.name);
                    });
            }
        }
    }).requestAccessToken();
}

async function Session_Login(email, fullname) {
    const res = await $.ajax({
        url: `${BASE_URL}/login-with-google`,
        type: 'POST',
        dataType: 'JSON',
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: {
            email: email,
            name: fullname,
        },
    });
    if (res.success) {
        if (res.idRole == 2) {
            window.location.href = `/trang-chu-admin`;
        } else {
            const Toast = Swal.mixin({
                toast: true,
                position: "top-end",
                showConfirmButton: false,
                timer: 3000,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.onmouseenter = Swal.stopTimer;
                    toast.onmouseleave = Swal.resumeTimer;
                }
            });
            Toast.fire({
                icon: "error",
                title: "Tài khoản bạn không thuộc phân quyền Admin, vui lòng liên hệ với người phụ trách để biết thêm chi tiết"
            });
        }
    } else {
        const Toast = Swal.mixin({
            toast: true,
            position: "top-end",
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.onmouseenter = Swal.stopTimer;
                toast.onmouseleave = Swal.resumeTimer;
            }
        });
        Toast.fire({
            icon: "error",
            title: res.message
        });
    }
}

function Logout_Session() {
    $.ajax({
        url: `${BASE_URL}/clear_session`,
        type: 'POST',
        success: function (res) {
            if (res.success) {
                localStorage.removeItem('authInfo');
                location.replace("/");
            }
        }
    });
}

function logout() {
    Logout_Session();
}
