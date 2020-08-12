function turn_to_memberlist() {
    const username = document.getElementById('username').value;

    localStorage.setItem("USERNAME", username);
    window.location.href = "memberlist.html";

    return;
}

