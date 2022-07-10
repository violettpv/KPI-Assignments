"use strict"
/* global board generateNewBoard updateBoard mainAILoop nextAIMove */

function startup() {
    // This function is called when the page is first loaded
    // It generates a fresh board, and binds the listeners to the buttons
    generateNewBoard()
    setupListeners() 
    // Lastly the AI main loop is called, which will keep running from there
    mainAILoop()
}

// adds a reset of scores & board
function setupListeners() {
    document.getElementById("controls-reset-scores").onclick = () => {
        document.getElementById("win-hare").innerText = "0"
        document.getElementById("win-wolf").innerText = "0"
        generateNewBoard()
    }
}

// Shows a notification 
function notify(level, message, duration=3000) {
    const body = `<div class="notification notify-${level}">${message}</div>`
    const notifications = document.getElementById("notifications")
    notifications.innerHTML += body
    setTimeout(() => {
        notifications.removeChild(notifications.childNodes[0])
    }, duration)
}

function title(string) {
    return string[0].toUpperCase()  + string.slice(1)
}

// formatting names
function nationalName(name, pretty=false) {
    
    if (pretty) {
        let prettyName = title(name)

        if (prettyName == "wolf") {
            prettyName = "wolves"
        }
        return prettyName
    }
    return name
}

window.onload = startup
