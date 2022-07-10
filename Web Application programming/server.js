const express = require('express');
const bodyParser = require('body-parser'); // node packages
const app = express(); // экземпляр  
const port = 3000;  

app.use(express.static("lab4")); // статический доступ к файлам 
app.use(bodyParser.urlencoded({ extended: true })); // настройка библиотеки для оперирования объектами, что пришли в запросе

const fs = require('fs'); // подкл. файловой сист.

// метод асинх. записи 
async function saveData(json, path) {
    await fs.writeFile(__dirname + path, JSON.stringify(json), 'utf8', () => {});
}

// метод считывания
function readData(path) {
    return fs.readFileSync(__dirname + path, 'utf8', () => {});
}

// принимаем данные
app.post('/saveData', async (req, res) => { // req=request, res=response
    await saveData(req.body, '/data/data.json');
    res.redirect('back');
});

// получаем данные, которые были сохранены ранее
app.get('/getData', async (req, res) => {
    var data = readData('/data/data.json');
    res.send(data);
});

app.get('/sync', async (req, res) => {
    var data = readData('/data/data4.json');
    res.send(data);
});

app.get('/', async (req, res) => {
    res.redirect("index4.html");
});

app.listen(port, () => { }); // запуск