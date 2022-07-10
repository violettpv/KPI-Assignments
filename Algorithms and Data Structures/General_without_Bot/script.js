// interface elements
let pScoreCont = document.getElementById('p-score');
let bScoreCont = document.getElementById('b-score');
// let tGeneral = document.getElementById('t-general');
// let tFour = document.getElementById('t-four');
// let tFh = document.getElementById('t-th');
// let tStreet = document.getElementById('t-street');
// let tSix = document.getElementById('t-6');
// let tFive = document.getElementById('t-5');
// let tFours = document.getElementById('t-4');
// let tThree = document.getElementById('t-3');
// let tTwo = document.getElementById('t-2');
// let tOne = document.getElementById('t-1');
// let diceOne = document.getElementById('dice-1');
// let diceOneCb = document.getElementById('dice-1-cb');
// let diceTwo = document.getElementById('dice-2');
// let diceTwoCb = document.getElementById('dice-2-cb');
// let diceThree = document.getElementById('dice-3');
// let diceThreeCb = document.getElementById('dice-3-cb');
// let diceFour = document.getElementById('dice-4');
// let diceFourCb = document.getElementById('dice-4-cb');
// let diceFive = document.getElementById('dice-5');
// let diceFiveCb = document.getElementById('dice-5-cb');
let rollButton = document.getElementById('roll-btn');
let keepButton = document.getElementById('keep-btn');
let lotButton = document.getElementById('lot-btn');
let playerBox = document.getElementById('p-score-box');
let botBox = document.getElementById('b-score-box');
let roundCont = document.getElementById('round');

function getRandomInt(min, max) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

class Player {
    constructor() {
        this.cCombo = {
            "general" : false,
            "four" : false,
            "fh" : false,
            "street" : false,
            "6" : false,
            "5" : false,
            "4" : false,
            "3" : false,
            "2" : false,
            "1" : false
        };

        this.score = 0;
    }

    get scoreValue() { return this.score; }

    setScore(val) { this.score = val; }

    get completedCombos() { return this.cCombo; }

    markCombo(cName) {
        this.cCombo[cName] = true;
    }
}

class Dice {
    constructor(num) {
        this.dice = document.getElementById('dice-' + num)
            .getElementsByClassName('inner-dice')[0];
        this.cb = document.getElementById('dice-' + num)
            .getElementsByClassName('dice-' + num + '-cb')[0];
        this.score = 1;
        this.number = num;
    }

    get diceNumber() { return this.number; }

    get diceScore() { return this.score; }

    setCB(value = true) { this.cb.checked = value; }

    async rollDice() {
        if (this.cb.checked == false) return;
        this.animation(true);
        await sleep(1000);
        this.score = getRandomInt(1, 6);
        this.animation();
    }

    animation(isShuffling = false) {
        this.dice.getElementsByClassName('animation')[0].classList.add('hidden');
        for (var i = 1; i <= 6; i++) {
            this.dice.getElementsByClassName(i + '')[0].classList.add('hidden');
        }

        if (isShuffling) {
            this.dice.getElementsByClassName('animation')[0].classList.add('shake');
            this.dice.getElementsByClassName('animation')[0].classList.remove('hidden');
        }
        else {
            this.dice.getElementsByClassName(this.score + '')[0].classList.remove('hidden');
        }
    }
}

class State {
    constructor(dices) {
        let d = [];
        for(var i = 1; i <= 5; i++) {
            d.push(new Dice(i));
        }
        this.dices = d;
        this.rollNumber = 0;
        this.combo = {};
        this.round = 1;
        this.over = false;
    }

    get scores() {
        var combo = [];
        dices.forEach(dice => {
           combo.push(dice.score); 
        });
        return combo;
    }

    get currentRound() {
        return this.round;
    }

    get isOver() { return this.over; }

    get combination() { return this.combo; }

    checkStreet(counts) {
        var count = {};
        counts.forEach(function(i) { count[i] = (count[i]||0) + 1;});
        if (count['1'] == 5) {
            if (counts[0] == 0 || counts[5] == 0) {
                return true;
            }
        }
        return false;
    }

    roll() {
        if (this.over) return;
        if (this.rollNumber >= 3) return;

        this.rollNumber++;
        if (this.rollNumber == 1) {
            this.dices.forEach(dice => {    
                dice.setCB();
            });
        }

        this.dices.forEach(dice => {
            dice.rollDice();
        });
    }

    keep() {
        if (this.rollNumber < 1) return;
        var comboName = this.combo.name;
        if (comboName != "" && comboName != undefined) {
            if (playerTurn) {
                player.completedCombos[comboName] = true;
                player.score += this.combo.value;
            }
            else {
                bot.completedCombos[comboName] = true;
                bot.score += this.combo.value;
            }
        }
        if (!playerTurn) {
            this.round += 1;
        } 
        playerTurn = !playerTurn;
        this.rollNumber = 0;
        if (this.round > 10) {
            this.over = true;
        }
    }

    calcValue() {
        let counts = [0, 0, 0, 0, 0, 0];
        for (var i = 0; i < 5; i++) {
            switch (this.dices[i].score) {
                case 1:
                    counts[0] += 1;
                    break;

                case 2:
                    counts[1] += 1;
                    break;

                case 3:
                    counts[2] += 1;
                    break;

                case 4:
                    counts[3] += 1;
                    break;

                case 5:
                    counts[4] += 1;
                    break;

                case 6:
                    counts[5] += 1;
                    break;
            }
        }

        var name = "";
        var value = 0;

        if (counts.includes(5)) {
            if (playerTurn) {
                if (player.completedCombos["general"] == false) {
                    if (this.rollNumber == 1) {
                        value = 100;
                        this.over = true;
                    } else { value = 60; }
                    name = "general";
                }
            }
            else {
                if (bot.completedCombos["general"] == false) {
                    if (this.rollNumber == 1) {
                        value = 100;
                    } else { value = 60; }
                    name = "general";
                }
            }
        }
        else if (counts.includes(4)) {

            if (playerTurn) {
                if (player.completedCombos["four"] == false) {
                    if (this.rollNumber == 1) {
                        value = 45;
                    } else { value = 40; }
                    name = "four";
                }
            }
            else {
                if (bot.completedCombos["four"] == false) {
                    if (this.rollNumber == 1) {
                        value = 45;
                    } else { value = 40; }
                    name = "four";
                }
            }
        }
        else if (counts.includes(3) && counts.includes(2)) {
            if (playerTurn) {
                if (player.completedCombos["fh"] == false) {
                    if (this.rollNumber == 1) {
                        value = 35;
                    } else { value = 30; }
                    name = "fh";
                }
            }
            else {
                if (bot.completedCombos["fh"] == false) {
                    if (this.rollNumber == 1) {
                        value = 35;
                    } else { value = 30; }
                    name = "fh";
                }
            }
        }
        else if (this.checkStreet(counts)) {
            if (playerTurn) {
                if (player.completedCombos["street"] == false) {
                    if (this.rollNumber == 1) {
                        value = 25;
                    } else { value = 20; }
                    name = "street";
                }
            }
            else {
                if (bot.completedCombos["street"] == false) {
                    if (this.rollNumber == 1) {
                        value = 25;
                    } else { value = 20; }
                    name = "street";
                }
            }
        }

        if (name == "") {
            if (playerTurn) {
                for (var i = 6; i >= 1; i--) {
                    if (player.completedCombos[i + ""] == false && counts[i - 1] != 0) {
                        value = i * counts[i - 1];
                        name = i + "";
                        break;
                    }
                }
            }
            else {
                for (var i = 6; i >= 1; i--) {
                    if (bot.completedCombos[i + ""] == false && counts[i - 1] != 0) {
                        value = i * counts[i - 1];
                        name = i + "";
                        break;
                    }
                }
            }
        }

        this.combo = {
            "value": value,
            "name": name
        }
    }
}


// variables and data
let afterLot = false;
let playerTurn = true;
let player = new Player();
let bot = new Player();
let state = new State();




// update interface
function updateStats() {
    if (afterLot) {
        if (playerTurn) {
            playerBox.classList.add('active-player');
            botBox.classList.remove('active-player');
        }
        else {
            playerBox.classList.remove('active-player');
            botBox.classList.add('active-player');
        }
    }
    
    roundCont.innerText = state.currentRound;

    pScoreCont.innerText = player.scoreValue;
    bScoreCont.innerText = bot.scoreValue;

    if (!playerTurn) {
        for (var [key, value] of Object.entries(player.completedCombos)) {
            if (value == true) {
                document.getElementById('t-' + key + '-p')
                    .classList.add('completed');
            }
        }
    }
    else {
        for (var [key, value] of Object.entries(bot.completedCombos)) {
            if (value == true) {
                document.getElementById('t-' + key + '-b')
                    .classList.add('completed');
            }
        }
    }

    if (state.isOver) {
        playerBox.classList.remove('active-player');
        botBox.classList.remove('active-player');
        if (player.scoreValue > bot.scoreValue) {
            playerBox.classList.add('winner');
        }
        else {
            botBox.classList.add('winner');
        }
    }
}
updateStats();


// bind buttons
async function rollAction() {
    state.roll();

    rollButton.onclick = () => {};
    keepButton.onclick = () => {};

    await sleep(1000);

    state.calcValue();

    rollButton.onclick = rollAction;
    keepButton.onclick = keepAction;
}
function keepAction() {
    state.keep();
    updateStats();
}
function lotAction() {
    state.dices.forEach(async dice => {
        if (dice.diceNumber != 3) {
            dice.setCB(false);
        }
        else {
            dice.setCB(true);
            dice.rollDice();
            await sleep(1000);
            if (dice.diceScore <= 3) {
                playerTurn = true;
            }
            else {
                playerTurn = false;
            }
            lotButton.classList.add('hidden');
            rollButton.classList.remove('hidden');
            keepButton.classList.remove('hidden');
            afterLot = true;
            updateStats();
        }
    }); 
}
rollButton.onclick = rollAction;
keepButton.onclick = keepAction;
lotButton.onclick = lotAction;


