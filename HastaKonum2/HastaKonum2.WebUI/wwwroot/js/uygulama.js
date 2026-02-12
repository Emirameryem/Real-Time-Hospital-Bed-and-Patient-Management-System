document.addEventListener("DOMContentLoaded", () => {
    const role = document.body.dataset.role;

    document.getElementById("mainApp").classList.remove("hidden");

    // Rol bazlı içerik göster
    const nav = document.getElementById("navigation");
    const dashboardContent = document.getElementById("dashboardContent");

    if (role === "Admin") {
        nav.innerHTML = `
            <a href="#" onclick="showPage('dashboard')">Dashboard</a>
            <a href="#" onclick="showPage('bedList')">Yatak Listesi</a>
        `;
        dashboardContent.innerHTML = `<p>Admin paneline hoş geldiniz!</p>`;
    }

    else if (role === "Hemsire") {
        nav.innerHTML = `
            <a href="#" onclick="showPage('dashboard')">Dashboard</a>
            <a href="#" onclick="showPage('bedList')">Yataklar</a>
            <a href="#" onclick="showPage('patientAssignment')">Hasta Atama</a>
        `;
        dashboardContent.innerHTML = `<p>Hemşire paneline hoş geldiniz!</p>`;
    }

    else if (role === "Temizlik") {
        nav.innerHTML = `
            <a href="#" onclick="showPage('dashboard')">Dashboard</a>
            <a href="#" onclick="showPage('bedList')">Temizlikteki Yataklar</a>
        `;
        dashboardContent.innerHTML = `<p>Temizlik paneline hoş geldiniz!</p>`;
    }
});

function showPage(id) {
    document.querySelectorAll(".page-content").forEach(el => el.classList.add("hidden"));
    document.getElementById(id).classList.remove("hidden");
}

function logout() {
    window.location.href = "/Auth/Login";
}
