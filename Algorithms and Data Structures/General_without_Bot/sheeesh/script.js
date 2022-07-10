let diceOne = document.getElementById('dice-one');
let diceTwo = document.getElementById('dice-two');
let diceThree = document.getElementById('dice-three');
let diceFour = document.getElementById('dice-four');
let diceFive = document.getElementById('dice-five');

var result = [1, 2, 3, 4, 5, 6];

// diceOne.getElementsByClassName('dice-shuffle').classList.add('none');
// diceTwo.getElementsByClassName('dice-shuffle').classList.add('none');
// diceThree.getElementsByClassName('dice-shuffle').classList.add('none');
// diceFour.getElementsByClassName('dice-shuffle').classList.add('none');
// diceFive.getElementsByClassName('dice-shuffle').classList.add('none');

// diceOne.getElementById(result).classList.remove('none');
// diceTwo.getElementById(result).classList.remove('none');
// diceThree.getElementById(result).classList.remove('none');
// diceFour.getElementById(result).classList.remove('none');
// diceFive.getElementById(result).classList.remove('none');

// let diceShuffle = document.getElementsByClassName('dice-shuffle');




function Start() {
    diceOne.getElementsById('1').classList.remove('none');

}
Start(); 

function Roll(){
    
    diceShuffle.forEach(function(cube) {
        cube.classList.add("shuffle");
    });

    setTimeout(function() {
        diceShuffle.forEach(function(cube){
            cube.classList.remove("shuffle");
        });

        let cubeOneValue = Math.floor(Math.random()*6);
        let cubeTwoValue = Math.floor(Math.random()*6);
        let cubeThreeValue = Math.floor(Math.random()*6);
        let cubeFourValue = Math.floor(Math.random()*6);
        let cubeFiveValue = Math.floor(Math.random()*6);
        let cubeSixValue = Math.floor(Math.random()*6);

        console.log(cubeOneValue, cubeTwoValue, cubeThreeValue, cubeFourValue, cubeFiveValue, cubeSixValue);

        document.querySelector("#dice-1").setAttribute("src", gifs[cubeOneValue]);
        document.querySelector("#dice-2").setAttribute("src", gifs[cubeTwoValue]);
        document.querySelector("#dice-3").setAttribute("src", gifs[cubeThreeValue]);
        document.querySelector("#dice-4").setAttribute("src", gifs[cubeFourValue]);
        document.querySelector("#dice-5").setAttribute("src", gifs[cubeFiveValue]);
        document.querySelector("#dice-6").setAttribute("src", gifs[cubeSixValue]);

        document.querySelector("#total").innerHTML = "Your roll is " + ((cubeOneValue +1) + (cubeTwoValue + 1) + (cubeThreeValue + 1)+ (cubeFourValue + 1)+ (cubeFiveValue + 1)+ (cubeSixValue + 1));
    }, 1000);
}
// Roll();