document.getElementById("entry-image").addEventListener("click", function() {
    document.getElementById("entry-screen").classList.add("hidden");

    let mainDiv = document.querySelector(".main");
    if (mainDiv) {
        mainDiv.classList.remove("hidden");
    } else {
        console.error("Main div not found!");
    }
});

let Oname = "admin"
let Opassword = "admin"

let username;
let password;

document.getElementById("submit").onclick = function(){
    username = document.getElementById("name").value;
    password = document.getElementById("password").value;
    if(username==Oname && password==Opassword){
        window.location.href = "table.html";
    }
    else{
        
    }
}

document.addEventListener("keydown",function(event){
    if(event.key.toLowerCase() === "h" && document.activeElement.tagName !== "INPUT"){
        let div = document.querySelector(".login");
        if(div){
            div.classList.toggle("hidden");
        }
    }
})

let time = document.getElementById("current-time");
setInterval(()=>{
    let d = new Date();
    time.innerHTML = d.toLocaleTimeString();
},1000)