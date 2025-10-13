document.getElementById("loginForm").addEventListener("submit", async function (event) {
    event.preventDefault();

    const identifier = document.getElementById("identifier").value;
    const password = document.getElementById("password").value;
    const loginResult = document.getElementById("result");

    try {
        const response = await fetch('/api/User/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ identifier, password })
        });

        const data = await response.json();

        loginResult.innerText = data.message || JSON.stringify(data);
        loginResult.style.color = data.success ? 'green' : 'red';

        if (data.token) {
            localStorage.setItem('authToken', data.token);
        }

        if (data.success) {
            window.location.href = "/";
        }

    } catch (err) {
        console.error(err);
        loginResult.innerText = "Bir hata oluştu!";
        loginResult.style.color = 'red';
    }
});
