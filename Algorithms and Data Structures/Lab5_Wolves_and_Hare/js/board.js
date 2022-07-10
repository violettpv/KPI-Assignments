"use strict"

let board

class Board {
    constructor(animals, firstTurn="hare") { // hare is the first one to make a move
        this.animals = animals
        this.currentTurn = firstTurn
        this.focussedAnimal = null
        this.gameover = false
    }

    // Checks if a given move is valid for the animal object provided.
    isValidMove(animal, x, y) {
        // All failing scenarios are checked, before returning true.
        // 1st failure is if the new position is the same as the old position

        if (animal.x === x && animal.y === y) {
            return false
        }

        // Second failure is when a wolf tries to move back up to a higher row
        if (animal.name === "wolf" && y <= animal.y) {
            return false
        }

        // Third faulure is if the move is not a diagonal tile on the x axis
        if (animal.x + 1 !== x && animal.x - 1 !== x) {
            return false
        }

        // Fourth faulure is if the move is not a diagonal tile on the y axis
        if (animal.y + 1 !== y && animal.y - 1 !== y) {
            return false
        }

        // Fifth failure is if the new location is outside of the playing field
        if (x < 0 || x > 7 || y < 0 || y > 7) {
            return false
        }

        // Final check is to make sure there is no other animal already there
        let allGood = true
        this.animals.forEach(a => {
            if (x === a.x && y === a.y) {
                allGood = false
            }
        })
        return allGood
    }

    // Finds an animal for a location provided
    animalAt(x, y) { 
        let foundAnimal = null
        this.animals.forEach(animal => {
            if (animal.x === x && animal.y === y) {
                foundAnimal = animal
            }
        })
        return foundAnimal
    }

    // Makes a list of the possible moves for a type of animal
    possibleMoves(animalName) {
        let validMoves = []
        this.animals.forEach(animal => {
            if (animal.name === animalName) {
                validMoves = validMoves.concat(animal.possibleMoves())
            }
        })
        return validMoves
    }

    checkVictory(register=true) {
        // After each move this function is called to check if the game is over:
        // - 1) it checks if the hare is closed in, which means the wolves win;
        // - 2) it checks if the wolves are stuck, meaning the hare wins;
        // - 3) it checks if the hare has reached the top row of the board, which results in a win for the hare;
        // The board is reset to the startup state and a new game can begin.

        if (this.possibleMoves("hare").length === 0) {
            if (register) {
                notify("info", `Wolves win!`)
                this.addVictory("wolf")
            }
            return "wolf"
        } 
        else if (this.possibleMoves("wolf").length === 0) {
            if (register) {
                notify("info", `Hare wins!`)
                this.addVictory("hare")
            }
            return "hare"
        } 
        else {
            this.animals.forEach(animal => {
                if (animal.name === "hare" && animal.y === 0) {
                    if (register) {
                        notify("info", `Hare wins!`)
                        this.addVictory("hare")
                    }
                    return "hare"
                }
            })
        }
        return ""
    }

    // Update the win counter for the winning animal
    addVictory(animal) {
        this.gameover = true
        const ourCounter = document.getElementById(`win-${animal}`)
        const ourWins = Number(ourCounter.innerHTML) + 1
        ourCounter.innerHTML = ourWins
        setTimeout(generateNewBoard, 1000)
    }
}

// the base class of animals
class Animal {
    constructor(x, y) {
        this.x = x
        this.y = y
        this.name = ""
    }

    moveTo(x, y, place=board) {
        // Moves the animal to the specified location if this is a valid move, or if the user has enabled invalid moves

        if (this.name !== place.currentTurn || place.gameover) {
            return
        }
        
        // valid => change [x;y] => change player => remove selection of current animal =>
        if (place.isValidMove(this, x, y)) {
            this.x = x
            this.y = y
            place.currentTurn = place.currentTurn === "hare" ? "wolf" : "hare"
            place.focussedAnimal = null
            
            if (place === board) {
                updateBoard()
                board.checkVictory()
            }
        } 
        else {
            notify("warn", "Invalid move")
        }
    }
}

class Wolf extends Animal {
    // There are always 4 wolves on the board
    constructor(x, y) {
        super(x, y)
        this.name = "wolf"
    }

    // Returns all possible moves for this wolf 
    possibleMoves(place=board) {
        const allMoves = [ // list of 2 objects [x;y] - all allowed moves
            {
                x: this.x + 1,
                y: this.y + 1
            },
            {
                x: this.x - 1,
                y: this.y + 1
            }
        ]
        const validMoves = []
        allMoves.forEach(move => { // choose all legal/valid moves
            if (place.isValidMove(this, move.x, move.y)) {
                validMoves.push(move)
            }
        })
        return validMoves
    }
}
class Hare extends Animal {
    // There will always be one hare on the board
    constructor(x, y) {
        super(x, y)
        this.name = "hare"
    }

    // Returns all possible moves for the hare
    possibleMoves(place=board) {
        
        const allMoves = [
            {
                x: this.x + 1,
                y: this.y + 1
            },
            {
                x: this.x - 1,
                y: this.y + 1
            },
            {
                x: this.x + 1,
                y: this.y - 1
            },
            {
                x: this.x - 1,
                y: this.y - 1
            }
        ]
        const validMoves = []
        allMoves.forEach(move => {
            if (place.isValidMove(this, move.x, move.y)) {
                validMoves.push(move)
            }
        })
        return validMoves
    }
}

// This function resets the board to the default state
function generateNewBoard() {
    board = new Board([
        new Hare(0, 7),
        new Wolf(1, 0),
        new Wolf(3, 0),
        new Wolf(5, 0),
        new Wolf(7, 0)
    ])
    updateBoard()
}

function updateBoard() {
    // This function generates the board as html from the board object
    // It also adds the listeners(clicks) to handle user interaction

    const boardElement = document.getElementById("board")
    boardElement.innerHTML = `<div id="animals"></div>`

    boardElement.className = ""

    let squareColor = "white"

    for (let i = 0; i < 64; i++) 
    {
        const square = document.createElement("span")
        square.className = `${squareColor}-square`

        if (squareColor === "black") 
        {
            square.onclick = () => {
                if (board.focussedAnimal !== null) {
                    board.focussedAnimal.moveTo(i % 8, Math.floor(i / 8))
                }
            }
        }

        boardElement.appendChild(square)
        squareColor = squareColor === "black" ? "white" : "black"

        if ((i + 1 )% 8 === 0) {
            boardElement.appendChild(document.createElement("br"))
            squareColor = squareColor === "black" ? "white" : "black"
        }
    }
    
    const animalsHtml = document.getElementById("animals")
    board.animals.forEach(animal => {
        const animalImage = document.createElement("img")
        animalImage.src = `img/${nationalName(animal.name)}.svg`
        animalImage.onclick = () => {
            if (animal === board.focussedAnimal) {
                board.focussedAnimal = null
            } 
            
            else if (animal.name !== board.currentTurn) {
                const prettyName = nationalName(board.currentTurn, true)
                notify("warn", `${prettyName} should make the next move`)
            } 
            
            else {
                board.focussedAnimal = animal
            }
            updateBoard()
        }

        const animalElement = document.createElement("span")
        if (animal === board.focussedAnimal) {
            animalElement.className = "focussed animal"
        } 
        
        else {
            animalElement.className = "animal"
        }
        animalElement.style = `top: ${animal.y * 4}em;left: ${animal.x * 4}em;`
        animalElement.appendChild(animalImage)
        animalsHtml.appendChild(animalElement)
    })
}
