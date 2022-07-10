var xmlHttp = new XMLHttpRequest();             // create new http request

xmlHttp.open("GET", "/sync", false);            // GET /sync
xmlHttp.send(null);                             // send request

let params = JSON.parse(xmlHttp.responseText);  // save response

// set bg from server data
document.getElementsByClassName("cookie-bg")[0].style.backgroundImage = "url(" + params.canvas.bgimages.cookie + ")";
document.getElementsByClassName("pizza-bg")[0].style.backgroundImage = "url(" + params.canvas.bgimages.pizza + ")";
document.getElementsByClassName("donut-bg")[0].style.backgroundImage = "url(" + params.canvas.bgimages.donut + ")";
document.getElementsByClassName("coke-bg")[0].style.backgroundImage = "url(" + params.canvas.bgimages.coke + ")";


let work = document.getElementsByClassName('work')[0];
let sheeesh = document.getElementById('storage-data');

// work area visibility toggler
function workArea(visible) {
    if(visible == true) {
        work.classList.remove("none");

        // *** clear local storage (upon pressing play button)
        localStorage.setItem("data", "");
        sheeesh.innerText = "";
        data = [];
    }
    else {
        work.classList.add("none");
        
        sheeesh.innerText = JSON.stringify(data);   // draw local storage content in box 2
        localStorage.setItem("data", data);         // save data to local storage
    }
} 

let start = document.getElementById('start-button');
let reload = document.getElementById('reload-button');

// set button inner texts from server data
start.innerText = params.buttons.start;
reload.innerText = params.buttons.reload;
document.getElementById('close-button').innerText = params.buttons.close;
document.getElementById('play-button').innerText = params.buttons.play;

// start button function
function pressedStart() {
    start.onclick = function(){};           // remove onclick funtion
    start.classList.add('button-inactive');
    moveSquare("right");                    // start animation
    registerInfo("Animation started.");
}

// reload button function
function reloadButton() {
    reload.classList.add('none');
    start.classList.remove('none');

    // reset square position
    startPosition(); 

    registerInfo("Animation reloaded.");
}

let square = document.getElementById('square');
let anim = document.getElementsByClassName('anim')[0];


// set square properties based on server data
square.style.backgroundColor = params.canvas.square.color;
square.style.width = params.canvas.square.size + "px";
square.style.height = params.canvas.square.size + "px";

// set anim size based on server data
let animH = params.canvas.height;
let animW = Math.floor(window.innerWidth / 100 * params.canvas.width.percentage) - 35;

let sqSize = params.canvas.square.size;


function setCoordinates(x, y) {
    square.style.top = y + "px";
    square.style.left = x + "px";
}

let x = animW / 2;
let y = animH / 2;
setCoordinates(x, y);
let shift = 1;

function startPosition() {
    x = animW / 2;
    y = animH / 2;
    setCoordinates(x, y);
    shift = 1;
}
startPosition();

function moveSquare(where) {
    var next;

    switch(where) {
        case "right":
            x += shift;
            next = "up";
            break;

        case "up": 
            y -= shift;
            next = "left";
            break;

        case "left":
            x -= shift;
            next = "down";
            break;

        case "down":
            y += shift;
            next = "right";
            break;
    }

    setCoordinates(x, y);

    if(x < animW / 2) 
    {
        if(y < animH / 2) {
            registerInfo("Cookie section.");
        }
        else {
            registerInfo("Coke section.");
        }
    }
    else {
        if(y < animH / 2) {
            registerInfo("Donut section.");
        }
        else {
            registerInfo("Pizza section.");
        }
    }

    // stop if edge reached
    if (x <= 0 || x >= animW - sqSize || y <= 0 || y >= animH - sqSize) {
        start.onclick = pressedStart;
        start.classList.add('none');
        start.classList.remove('button-inactive');

        reload.classList.remove('none');

        registerInfo("Animation stopped.");
        return;
    }
    shift += 1;

    setTimeout(function(){
        moveSquare(next);
    }, 10);
}

/* == LocalStorage == */
let data = localStorage.getItem("data");

try {
    data = JSON.parse(data);
}
catch {
    data = [];
}

/* == notifications == */
let notification = document.getElementById('notifications');

function registerInfo(info) {
    notification.innerText = info;
    info = Date.now() + " - " + info;
    data.push(info);
}
