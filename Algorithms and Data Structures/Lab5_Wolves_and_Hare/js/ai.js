"use strict"

// This is the main loop for the AI functionality.
function mainAILoop() {
    setTimeout(mainAILoop, 100)

    if (board.gameover) {
        return
    }
    nextAIMove(false) // => AI move
}

function nextAIMove(manual=true) {
    // If this function is called automatically, it should only make the next move if the AI is enabled.

    if (board.currentTurn === "wolf" && (manual || true)) { 
        wolfAI.makeMove()
    }
}

class AI {
    // Super class for the AI, implements the algorithms for both AI subclasses
    constructor() {
        this.name = ""
    }

    makeMove() { // calls alpha-beta pruning
        var depth = 0
        if(document.getElementById("easy").checked == true) 
        {
            depth = 1
        }
        else if (document.getElementById("medium").checked == true)
        {
            depth = 3
        }
        else {
            depth = 6
        }

        var baseAlpha = -1000
        var baseBeta = 1000
        var isAi = true

        // initial call
        this.evaluateMinimax(board, depth, baseAlpha, baseBeta, isAi)
    }

    terminalScore(boardState) { // checks is victory
        const victory = boardState.checkVictory(false) 

        if (victory === "") { // no one wins
            return 0
        }

        if (victory === this.name) { // if hare wins
            return 100
        } 
        
        else { // if wolves win
            return -100
        }
    }

    // copy board state / to avoid accidental changes on game board
    cloneBoard(boardState) {
        const animals = []

        boardState.animals.forEach(animal => {
            if (animal.name === "hare") {
                animals.push(new Hare(animal.x, animal.y))
            } 

            else if (animal.name === "wolf") {
                animals.push(new Wolf(animal.x, animal.y))
            }
        })
        return new Board(animals, boardState.currentTurn)
    }

    // Algorithm
    evaluateMinimax(currentBoard, depth, alpha, beta, root=false) { // root(true) means wolves make move
        // finds all animals on board (copy)
        const animals = []
        currentBoard.animals.forEach(animal => {
            if (animal.name === currentBoard.currentTurn) {
                animals.push(animal)
            }
        })

        if (root) {
            let highestSoFar = -1000
            let bestAnimal = null
            let bestMove = null

            try { 
                // try-catch is needed to break from foreach-loop in JavaScript :P
                animals.forEach(animal => {
                    const moves = animal.possibleMoves(currentBoard)

                    moves.forEach(move => {

                        // on copy make moves
                        const newBoard = this.cloneBoard(currentBoard) 
                        newBoard.animalAt(animal.x, animal.y).moveTo(move.x, move.y, newBoard)

                        const scores = this.evaluateMinimax(newBoard, depth - 1, alpha, beta) // recursion

                        // set new alpha OR beta
                        if (scores.beta < alpha) { // if hare makes things better for wolves
                            alpha = scores.beta
                            throw BreakException // prune this branch
                        } 
                        else if (scores.alpha > beta) { // if wolves make things better for hare
                            beta = scores.alpha
                            throw BreakException // prune this branch
                        }
   
                        if (scores.lowest > highestSoFar) { // if the new score is higher than the prev.score set it as best score
                            highestSoFar = scores.lowest
                            bestAnimal = animal
                            bestMove = move
                        }
                    })
                })
            }
            catch (e) {
                if (e !== BreakException) throw e;
            }
                
            bestAnimal.moveTo(bestMove.x, bestMove.y) // AI makes the best move
            return
        } 
        
        // when the depth is 6 (max) 
        else if (depth === 0 || this.terminalScore(currentBoard) !== 0) {
            var nalpha = null
            var nbeta = null
            var lowest = this.evaluateScore(currentBoard) // counts ev.sc. of a board 
        
            if (currentBoard.currentTurn === "wolf") { 
                nalpha = lowest
            } 
            else if (currentBoard.currentTurn === "hare") {
                nbeta = lowest
            }
            return {
                lowest: lowest,
                all: [lowest],
                alpha: nalpha,
                beta: nbeta
            }
        } 
        
        else // root - false => imitation of hare moves
        { 
            let lowestScore = 1000
            const allScores = []
            animals.forEach(animal => {
                const moves = animal.possibleMoves(currentBoard)
                moves.forEach(move => {
                    const newBoard = this.cloneBoard(currentBoard)

                    newBoard.animalAt(animal.x, animal.y).moveTo(move.x, move.y, newBoard)

                    // recursion
                    const scores = this.evaluateMinimax(newBoard, depth - 1, alpha, beta)

                    if (scores.beta < alpha) { // if hare makes things better for wolves
                        alpha = scores.beta
                        throw BreakException // prune this branch
                    } 
                    else if (scores.alpha > beta) { // if wolves make things better for hare
                        beta = scores.alpha
                        throw BreakException // prune this branch
                    }
                    if (scores.lowest < lowestScore) { // if the new score is lower than the prev.score set it as the prev.score
                        lowestScore = scores.lowest
                    }
                    allScores.push(...scores.all) // adds them
                })
            })
            return {
                alpha: alpha,
                beta: beta,
                lowest: lowestScore,
                all: allScores
            }
        }
    }
}

class HareAI extends AI {
    constructor() {
        super()
        this.name = "hare"
    }
}

class WolfAI extends AI {
    constructor() {
        super()
        this.name = "wolf"
    }

    evaluateScore(boardState) { // counts the score of a board
        let score = 0
        let hare = null
        const wolves = []

        boardState.animals.forEach(animal => { 
            if (animal.name === "hare") { 
                hare = animal
            } 
            else {
                wolves.push(animal) 
            }
        })
        score += hare.y * 3 

        // Let the wolves keep roughly the same y axis
        let highestWolf = -10 
        let lowestWolf = 20

        wolves.forEach(wolf => { 
            if (wolf.y > highestWolf) {
                highestWolf = wolf.y
            } 
            else if (wolf.y < lowestWolf) {
                lowestWolf = wolf.y
            }
        })
        score -= (highestWolf - lowestWolf) * 2 

        // Make sure the hare doesn't get above the wolves
        // This means the game is lost
        if (hare.y <= lowestWolf) {
            score -= 50
        }

        // Lower the score when the wolves are on average not above the hare
        let totalWidth = 0
        wolves.forEach(wolf => {
            totalWidth += wolf.x
        })
        const averageXPosition = totalWidth / wolves.length
        score -= Math.abs(averageXPosition - hare.x)

        // Make sure there are no gaps between the wolves
        // -1 and 8 are the "walls", where gaps are also discouraged
        const xPositions = []
        wolves.forEach(wolf => {
            xPositions.push(wolf.x)
        })
        xPositions.push(-1)
        xPositions.push(8)
        xPositions.sort((a, b) => {return a-b})
        let biggestGap = 0

        for (let i = 0; i < xPositions.length -1; i++) 
        {
            const gap = xPositions[i+1] - xPositions[i]
            if (gap > biggestGap) {
                biggestGap = gap
            }
        }
        if (biggestGap > 4) { // +1 gap for a hare to win
            score -= 10
        }
        
        // Make losing a no go if possible and winning top priority
        score += this.terminalScore(boardState)
        return score
    }
}

const hareAI = new HareAI()
const wolfAI = new WolfAI()
