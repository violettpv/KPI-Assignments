// find canvas
let canvas = document.getElementById("anim-canvas");
let context = canvas.getContext("2d");

// set canvas size
canvas.height = 440 - 50;
canvas.width = Math.floor(window.innerWidth / 100 * 60) - 35;

// set starting parameters
var x = canvas.width / 2;
var y = canvas.height / 2;
var sqSize = 20;
var shift = 1;

function moveSquare(where) {
    // clear canvas
    context.clearRect(0, 0, canvas.width, canvas.height);
    var next;
    
    switch (where) {
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

    // draw square on coordinates
    context.beginPath();
    context.rect(x, y, sqSize, sqSize);
    context.fillStyle = "blue";
    context.fill();

    // stop if edge reached
    if (x <= 0 || x >= canvas.width - sqSize || y <= 0 || y >= canvas.height - sqSize) {
        return;
    }
    shift += 1;

    setTimeout(function () {
        moveSquare(next);
    }, 100);
}
moveSquare("right");