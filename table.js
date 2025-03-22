
document.querySelectorAll("input").forEach(input => {
    input.addEventListener("focus", function() {
        this.style.background = "#8a4f1a"; // Change background color
        this.style.border = "5px solid brown"; // Highlight border
        this.style.transform = "scale(1.1)"; // Make it bigger
        this.style.transition = "transform 0.2s ease-in-out";
    });

    input.addEventListener("blur", function() {
        this.style.background = "#d8b075"; // Reset to original
        this.style.border = "2px solid black"; // Reset border
        this.style.transform = "scale(1)"; 
    });
});


document.getElementById("btn").onclick = function() {
    let classname = document.getElementById("classname").value.trim();
    let npeople = document.getElementById("npeople").value.trim();
    let description = document.getElementById("description").value.trim();

    if (classname === "" || npeople === "" || description === "") {
        alert("Please fill in all fields!");
        return;
    }

    let tablebody = document.querySelector("table tbody");
    let newrow = document.createElement("tr");

    newrow.innerHTML = `
        <td>${classname}</td>
        <td>${npeople}</td>
        <td>${description}</td>
    `;
    
    newrow.addEventListener("dblclick", function(){
        tablebody.removeChild(newrow)
    });

    tablebody.appendChild(newrow);

    // Clear input fields after adding row
    document.getElementById("classname").value = "";
    document.getElementById("npeople").value = "";
    document.getElementById("description").value = "";
};