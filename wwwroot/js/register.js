
document.getElementById("registerForm").addEventListener("submit", async function (event) {
    event.preventDefault();

    const user = {
        username: document.getElementById("username").value,
        email: document.getElementById("email").value,
        passwordHash: document.getElementById("password").value
    };

    const response = await fetch("/api/User/Register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(user)
    });

    const data = await response.json();
    document.getElementById("result").innerText = data.message || JSON.stringify(data);
});
