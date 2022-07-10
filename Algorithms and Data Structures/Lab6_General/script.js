// interface elements
let pScoreCont = document.getElementById('p-score');
let bScoreCont = document.getElementById('b-score');
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

// is required to await animations
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

// both player AND bot
class Player {
    constructor() {
        // stores combinations and info about their completion
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

        // stores player score
        this.score = 0;
    }

    get scoreValue() { return this.score; } 

    setScore(val) { this.score = val; } // score setter

    get completedCombos() { return this.cCombo; } 

    // mark combo as completed
    markCombo(cName) {
        this.cCombo[cName] = true;
    }
}

class Dice {
    constructor(num) {
        // container containing gif and svg
        this.dice = document.getElementById('dice-' + num)
            .getElementsByClassName('inner-dice')[0];

        // checkbox linked to dice
        this.cb = document.getElementById('dice-' + num)
            .getElementsByClassName('dice-' + num + '-cb')[0];

        // dice score - number that was rolled
        this.score = 1;

        // dices' sequence number (5 dices total)
        this.number = num;
    }

    get diceNumber() { return this.number; }

    get diceScore() { return this.score; }

    setCB(value = true) { this.cb.checked = value; } // dice checkbox setter

    async rollDice() {
        if (this.cb.checked == false) return;   // roll only if checkbox is checked
        this.animation(true);                   // start animation
        await sleep(1000);                      // gives animation 1 second to play
        this.score = getRandomInt(1, 6); 
        this.animation();                       // stops animation (and sets respective score svg)
    }

    // play / stop animation
    animation(isShuffling = false) {
        // hide all svg and gif
        this.dice.getElementsByClassName('animation')[0].classList.add('hidden');
        for (var i = 1; i <= 6; i++) {
            this.dice.getElementsByClassName(i + '')[0].classList.add('hidden');
        }

        // make either gif or svg visible
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
        // create and store dices
        let d = [];
        for(var i = 1; i <= 5; i++) {
            d.push(new Dice(i));
        }
        this.dices = d;

        this.rollNumber = 0;
        this.combo = {}; // what combo is it
        this.round = 1;
        this.over = false;
    }

    get currentRound() { return this.round; }

    get isOver() { return this.over; }

    get combination() { return this.combo; }

    // helper for calcValue()
    checkStreet(counts) {
        // monitor through all scores
        // if all dices are unique and are
        // in a one of the sequences:
        // 1-2-3-4-5 || 2-3-4-5-6
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
        // cant roll if the game is over...
        if (this.over) return;
        // ..or if you have already rolled 3 times
        if (this.rollNumber >= 3) return;

        this.rollNumber++;

        // on the first roll ALL dices are rolled
        if (this.rollNumber == 1) {
            this.dices.forEach(dice => {    
                dice.setCB();
            });
        }

        // roll dices
        this.dices.forEach(dice => {
            dice.rollDice();
        });
    }

    keep() {
        // cant keep if you did not roll
        if (this.rollNumber < 1) return;

        // if you rolled a combo
        // set combo as completed and count score
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
        // count repeatings of dice scores
        // each item in counts[] represents
        // the amount of a certain number
        // in the dice combo
        // indexes of counts[] represent
        // the actual numbers
        // numbers:   1  2  3  4  5  6
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

        // example: if board has *five* dices
        // with the same number
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

        // if no combination was rolled, count numbers
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
    // mark a player whose turn it is to play
    if (afterLot) { // only after lot has been played
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

    // mark completed combos
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

    // if the game is over mark winner
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

    // disable the buttons for the duration of the animation
    rollButton.onclick = () => {};
    keepButton.onclick = () => {};

    // wait for animation to finish
    await sleep(1000);

    state.calcValue();

    // make buttons active again
    rollButton.onclick = rollAction;
    keepButton.onclick = keepAction;
}
function keepAction() {
    state.keep();
    updateStats();

    // disable buttons if it is bot's turn & call a bot
    if (!playerTurn) {
        rollButton.onclick = () => {};
        keepButton.onclick = () => {};
        botCore();
    }
    else {
        rollButton.onclick = rollAction;
        keepButton.onclick = keepAction;
    }
}
function lotAction() {
    // loop through dices
    state.dices.forEach(async dice => {
        // uncheck all dices but the middle one
        if (dice.diceNumber != 3) {
            dice.setCB(false);
        }
        else {
            // roll middle dice
            dice.setCB(true);
            dice.rollDice();
            await sleep(1000); // wait for anim to finish

            // if dice scored <=3 - player goes first
            if (dice.diceScore <= 3) {
                playerTurn = true;
            }
            // if dice scored >=4 - bot goes first
            else {
                playerTurn = false;
                rollButton.onclick = () => { };
                keepButton.onclick = () => { };
            }

            // replace lot btn with controls
            lotButton.classList.add('hidden');
            rollButton.classList.remove('hidden');
            keepButton.classList.remove('hidden');

            afterLot = true;

            updateStats();

            // call bot if he moves first
            if (!playerTurn) {
                await sleep(1000); // wait 1 second so that you can see that bot won the lot
                botCore();
            }
        }
    }); 
}
rollButton.onclick = rollAction;
keepButton.onclick = keepAction;
lotButton.onclick = lotAction;


async function botCore() {
    while (true) {
        await rollAction(); // roll
        await sleep(1000); // wait for anim

        var decision = makeDecision(); // make decision

        // execute decision and get info if bot should keep or roll again
        var toContinue = decision();

        // stop loop if bot keeps
        if (toContinue == false) break;

        // keep if it was bot's 3rd roll
        if (state.rollNumber >= 3) {
            keepAction();
            break;
        }
    }
}

// returns function which can execute keepAction() and returns whether bot should roll again
function makeDecision() {
    // if bot rolled combination that is not numeric (general, four, fh, street)
    if (state.combination.name != "" || state.combination != undefined) {
        if (isNaN(state.combination.name)) {
            return () => {
                keepAction(); // bot will keep
                return false; // and not roll again
            };
        }
    }

    // same as the one in state.calcValue() || see row #192
    let counts = [0, 0, 0, 0, 0, 0];
    for (var i = 0; i < 5; i++) {
        switch (state.dices[i].score) {
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

    // try to roll four
    if (bot.completedCombos['four'] == false) {
        // five of the same number:
        // roll random dice
        // 1-1-1-R-1
        if (counts.includes(5)) {
            // roll random dice
            var randomDice = getRandomInt(0, 4);
            for (var i = 0; i <= 4; i++) {
                if (i == randomDice) {
                    state.dices[i].setCB(true);
                }
                else {
                    state.dices[i].setCB(false);
                }
            }
            
            return () => {
                return true;
            }
        }

        // three of the same number:
        // roll two other dices
        // 1-1-1-R-R
        if (counts.includes(3) && bot.completedCombos['fh'] == true) {
            var numsToRoll = [];

            // find two other dices
            for (var i = 0; i <= 5; i++) {
                if (counts[i] == 1) {
                    numsToRoll.push(i + 1);
                }
                else if (counts[i] == 2 && counts.indexOf(2, 2) != -1) {
                    numsToRoll.push(i + 1);
                    break;
                }
            }

            // roll them
            for (var i = 0; i <= 4; i++) {
                if (numsToRoll.includes(state.dices[i].diceScore)) {
                    state.dices[i].setCB(true);
                }
                else {
                    state.dices[i].setCB(false);
                }
            }

            return () => {
                return true;
            }
        }

        // two of the same dices:
        // roll three other dices
        // 1-1-R-R-R
        if (counts.includes(2) && bot.completedCombos['fh'] == true) {
            var numsToRoll = [];

            // find two other dices
            var winnerNum = 0;
            for (var i = 0; i <= 5; i++) {
                if (counts[i] == 2) {
                    winnerNum = i + 1;
                }
            }

            // roll them
            for (var i = 0; i <= 4; i++) {
                if (state.dices[i].diceScore != winnerNum) {
                    state.dices[i].setCB(true);
                }
                else {
                    state.dices[i].setCB(false);
                }
            }

            return () => {
                return true;
            }
        }
    }

    // try to roll fh
    if (bot.completedCombos['fh'] == false) {
        // 3 or 2 of the same number:
        // roll other two
        // 3-R-3-3-R
        // or roll other three
        // 6-R-R-6-R
        // or one, if there are
        // another pair of two similar dices
        // 2-2-R-5-5
        if (counts.includes(3) || counts.includes(2)) {
            var numsToRoll = [];
            
            // find two other
            for (var i = 0; i <= 5; i++) {
                if (counts[i] == 1) {
                    numsToRoll.push(i + 1);
                }
            }

            // roll them
            for (var i = 0; i <= 4; i++) {
                if (numsToRoll.includes(state.dices[i].diceScore)) {
                    state.dices[i].setCB(true);
                }
                else {
                    state.dices[i].setCB(false);
                }
            }

            return () => {
                return true;
            }
        }
    }

    // try to roll street
    if (bot.completedCombos['street'] == false) {
        // if there are only 2 duplicates:
        // roll one of the duplicates
        // 1-2-R-4-5
        if (counts.includes(2) && !counts.includes(3)) {
            var numsToRoll = [];

            // find duplicate
            for (var i = 0; i <= 5; i++) {
                if (counts[i] == 2) {
                    numsToRoll.push(i + 1);
                    break;
                }
            }

            // roll it
            var tmp = false;
            for (var i = 0; i <= 4; i++) {
                if (numsToRoll.includes(state.dices[i].diceScore) && !tmp) {
                    state.dices[i].setCB(true);
                    tmp = true;
                }
                else {
                    state.dices[i].setCB(false);
                }
            }

            return () => {
                return true;
            }
        }

        // if there are no duplicates:
        // there MUST be a combination of 1 and 6
        // which is not suitable for the combo
        // so we roll 6
        // 1-2-3-4-R
        if (!counts.includes(2) && !counts.includes(3) && !counts.includes(4)) {
            var numsToRoll = [];
            
            // find dice with 6
            if (counts[0] == 1 && counts[5] == 1) {
                numsToRoll.push(6);
            }

            // roll it
            var tmp = false;
            for (var i = 0; i <= 4; i++) {
                if (numsToRoll.includes(state.dices[i].diceScore) && !tmp) {
                    state.dices[i].setCB(true);
                    tmp = true;
                }
                else {
                    state.dices[i].setCB(false);
                }
            }

            return () => {
                return true;
            }
        }
    }

    // try to roll general (lowest priority)
    if (bot.completedCombos['general'] == false) {
        // 2/3/4 duplicates:
        // roll the other dices
        // 2-2-R-2-2 / 2-2-R-R-2 / 2-2-R-R-R
        if (counts.includes(4) || counts.includes(3) || counts.includes(2)) {
            var numsToRoll = [];
            
            // find others
            for (var i = 0; i <= 5; i++) {
                if (counts[i] == 1) {
                    numsToRoll.push(i + 1);
                }
            }

            // roll them
            for (var i = 0; i <= 4; i++) {
                if (numsToRoll.includes(state.dices[i].diceScore)) {
                    state.dices[i].setCB(true);
                }
                else {
                    state.dices[i].setCB(false);
                }
            }

            return () => {
                return true;
            }
        }
    }

    return () => {
        return true; // bot will roll again
    }
}