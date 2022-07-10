//============================== Task_1 ===============================//
let b1_container = document.getElementsByClassName('box b1')[0];
let x_container = b1_container.getElementsByClassName('container x')[0];
let x_text = x_container.getElementsByTagName('h1')[0].innerText;

let b4_container = document.getElementsByClassName('box b4')[0];
let y_container = b4_container.getElementsByClassName('container y')[0];
let y_text = y_container.getElementsByTagName('h2')[0].innerText;

let temp = x_text;
x_container.getElementsByTagName('h1')[0].innerText = y_text;
y_container.getElementsByTagName('h2')[0].innerText = temp;

//============================== Task_2 ===============================//
function Area() {
	let a = 6;
	let height = 5;
	let result = a * height;
    document.getElementById("area").value = result.toString();
}

//============================== Task_3 ===============================//
function getCookie(name) {
	let matches = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
	return matches ? decodeURIComponent(matches[1]) : undefined;
	// возвращает куки с указанным name, или undefined, если ничего не найдено
}

function setCookie(name, value, options = {}) {
	// options = { path: '/' };
  
	if (options.expires instanceof Date) 
	{
	  options.expires = options.expires.toUTCString();
	}
  
	let updatedCookie = encodeURIComponent(name) + "=" + encodeURIComponent(value);
  
	for (let optionKey in options) 
	{
	  updatedCookie += "; " + optionKey;
	  let optionValue = options[optionKey];
		if (optionValue !== true) 
		{
			updatedCookie += "=" + optionValue;
		}
	}
  
	document.cookie = updatedCookie;
}

function deleteCookie(name) {
    setCookie(name, "", {
        'max-age': 0
    });
}

function countWords() {
	let s = document.getElementById("inputString").value;
	s = s.replace(/(^\s*)|(\s*$)/gi,"");
	s = s.replace(/[ ]{2,}/gi," ");  // replace everything except spaces and alphanumeric characters 
	s = s.replace(/\n /,"\n");      // s = s.replace(/[^\w\s]/g,""); s = s.split(/\s+/);
	let result = s.split(' ').length;
	alert("The number of words: " + result); 
	setCookie("cookie1", result);
	console.log(document.cookie);
}

window.onload = afterReload();

function afterReload() {
	let result = confirm("The data in cookies: " + document.cookie + "\nClick OK to delete cookies.");
	if(result)  
	{
		deleteCookie("cookie1");
		document.getElementById("inputString").style.display = "none";
		document.getElementById("convertButton").style.display = "none";
		
		let res = confirm("Cookies deleted.\nPress OK to reload page and start again.");
		if(res)
		{
			window.location.reload();
		}
	} 
}

//============================== Task_4 ===============================//
let box2Event = document.getElementById("box_2");

function setColor(color = null) {
	if(color == null) 
	{
		color = localStorage.getItem('setColor');
	}
	box2Event.style.backgroundColor = color;
}
setColor();

function changeColor() {
	let input = document.getElementById("inputColor").value;

	box2Event.addEventListener("mouseout", function() {
		setColor(input); 
	}, false);

	localStorage.setItem('setColor', input);
	// console.log(localStorage.getItem('setColor'));
}

//============================== Task_5 ===============================//
function drawButton(class_name) {
	let container = document.getElementsByClassName(class_name)[0];
	let buttons = "<input name=\"input\" id=\"bgi " + class_name + "\" type=\"text\" value=\"\"><button type=\"button\" id=\"one " + class_name + "\" onclick=\"buttonOne(\'" + class_name + "\')\">One</button><button type=\"button\" id=\"two " + class_name + "\" onclick=\"buttonTwo(\'" + class_name + "\')\">Two</button>";
	if(!container.innerHTML.includes(buttons))
	{
		container.innerHTML += buttons;	
	}
	else
	{
		container.innerHTML = container.innerHTML.replace(buttons, "");
	}
}

function getElementById(parentContainer, elementId) {
    let elm = {};
    let elms = parentContainer.getElementsByTagName("*");
    for (var i = 0; i < elms.length; i++) 
	{
        if (elms[i].id === elementId) 
		{
            elm = elms[i];
            break;
        }
    }
    return elm;
}

function setImage(class_name, val = null) {
	let container = document.getElementsByClassName(class_name)[0];
	if(val == null)
	{
		let input = getElementById(container, "bgi " + class_name);
		val = input.value;
	}
	if(val == "")
	{
		container.style.backgroundImage = "none";
	}
	container.style.backgroundImage = "url(\'"+ val +"\')";
	return val;
}

function allStorage() {

    var archive = {}, // Notice change here
        keys = Object.keys(localStorage),
        i = keys.length;

    while ( i-- ) {
        archive[ keys[i] ] = localStorage.getItem( keys[i] );
    }

    return archive;
}

function storageImages() {
	for (const [key, value] of Object.entries(allStorage()))
	{
		if(key.includes('colour'))
		{
			setImage(key.replace(' colour', ""), value);
		}
	}
}
storageImages();

function buttonOne(class_name) {
	let url = setImage(class_name);
	localStorage.setItem(class_name + " colour", url);
}

function buttonTwo(class_name) {
	setImage(class_name, "");
	localStorage.removeItem(class_name + " colour");
}