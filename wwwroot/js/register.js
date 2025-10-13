document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("registerForm");
    const loginBtn = document.getElementById("loginBtn");

    if (!form) { console.error("Form bulunamadý"); return; }
    if (!loginBtn) { console.error("Login button bulunamadý"); }

    form.addEventListener("submit", async function (event) {
        event.preventDefault();

        console.log("register submitted");

        const user = {
            username: document.getElementById("username").value,
            email: document.getElementById("email").value,
            passwordHash: document.getElementById("password").value
        };

        try {
            const response = await fetch("/api/User/Register", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(user)
            });

            const data = await response.json();

            document.getElementById("result").innerText = data.message || JSON.stringify(data);

            if (data.success) {
                console.log("registration success:", data);
                window.location.href = "/User/Login";
            } else {
                console.log("registration failed:", data);
            }

        } catch (err) {
            console.error("Error during registration:", err);
            document.getElementById("result").innerText = "Bir hata oluþtu!";
        }
    });

    loginBtn.addEventListener("click", function () {
        window.location.href = "/User/Login";
    });
});
