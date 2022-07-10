using System;
using System.IO;

namespace Lab_1_Multi_paradigm_programming
{
    class Task2
    {
        static void Main(string[] args)
        {
            string text = File.ReadAllText("task2.lang_with_go_to/text2.txt");
            string[] lines = new string[20000];
            int linesIterator = 0; // итератор линии
            int linesStart = 0; // начальный индекс линии
            int linesEnd = 0; // конечный индекс линии
            int linesLastIndex = 0; // длина списка линий
            int wordsIterator = 0; // итератор слов
            int wordsStart = 0; // начальный индекс слова
            int wordsEnd = 0; // конечный индекс слова
            string[] tempWords = new string[1000]; // временный массив слов для дальшей 
            int tempWordsLastIndex = 0;
            int tempWordsIterator = 0;
            string[] wordKeys = new string[25000]; // ключ (слово)
            int[][] wordValues = new int[25000][]; // значение (страницы)
            int[] wordCounts = new int[25000]; // к-во повторения слова
            int wordValuesIterator = 0; // итератор словаря
            string currentLine = ""; 
            string word = "";
            char letter = ' ';
            var checkDictionaryIndex = 0; // итератор поиска в словаре
            var checkDictionaryResult = -1; // индекс слова в словаре, если оно (слово) там есть
            var checkDictionaryLastIndex = 0; // индекс последнего слова в словаре
            var currentPage = 1;
            var tmp = "";
            var tmp2 = new int[100];
            var tmp3 = 0;
            var index = 0; // итератор
            var jIndex = 0; // второй итератор
            var letterCheck = 0; // индекс буквы в слове (для сортировки по алфавиту)

            // считаем линии
            countLinesLoop:
                if(text[linesIterator] == '\r' || linesIterator == text.Length - 1) {
                    linesEnd = linesIterator - 1; // обозначение конечного индекса линии

                    // если это последняя строка в тексте
                    if(linesIterator == text.Length - 1) {
                        linesEnd++;
                    }

                    currentLine = "";
                    writeLineLoop:
                        if(text[linesStart] < 'A' || (text[linesStart] > 'Z' && text[linesStart] < 'a') || text[linesStart] > 'z') {
                            if (text[linesStart] != ' ') {
                                goto writeLineLoopEnd; // игнор стоп-символов
                            }
                        }

                        currentLine += text[linesStart]; // записываем символ в линию

                        writeLineLoopEnd: 
                            linesStart++; // увеличиваем индекс указателя на символ 
                            if(linesStart <= linesEnd) { // перебираем буквы до того как дойдем до конца линии
                                goto writeLineLoop;
                            }

                        linesStart = linesIterator + 2; // начало след. строки будет после \r\n
                        lines[linesLastIndex] = currentLine; // добавляем линию в список линии
                        linesLastIndex++; // индекс последней линии в списке линий
                }

                linesIterator++; // перескакиваем \n -> первый символ новой строки 
                
                if(linesIterator < text.Length) { // перебираем линии до конца текста
                    if (text[linesIterator] == '\n')
                    {
                        linesIterator++;
                    }
                    goto countLinesLoop;
                }

            linesIterator = 0;
            text = ""; // сохраняем память
            // перебираем слова в каждой линии
            countWordsLoop: 
                currentLine = lines[linesIterator];

                wordsIterator = 0;
                wordsStart = 0;
                tempWords = new string[1000];
                tempWordsLastIndex = 0;
                tempWordsIterator = 0;

                wordsLoop: 
                    if(currentLine.Length > 0 && (currentLine[wordsIterator] == ' ' || wordsIterator + 1 == currentLine.Length)) {
                        wordsEnd = wordsIterator - 1; // определяем конец слова
                        
                        // если это последняя строка в тексте
                        if (wordsIterator + 1 == currentLine.Length) {
                            wordsEnd++;
                        }

                        // считываем слово
                        addWordLoop:
                            if(wordsStart != wordsEnd + 1) { // перебираем буквы
                                // если большая буква -> сделать маленькой
                                if(currentLine[wordsStart] >= 'A' && currentLine[wordsStart] < 'a') {
                                    letter = (char)(currentLine[wordsStart] + 32);
                                }
                                else {
                                    letter = currentLine[wordsStart];
                                }
                                // добавить букву в слово
                                word += letter;
                                wordsStart++; // увеличить итератор (что бы взять след. букву)
                                goto addWordLoop;
                            }

                        // игнорировать "стоп-слова"
                        if(word != "for" && word != "the" && word != "as" && word != "in" && word != "a" && word != "on" && word != "") {
                            tempWords[tempWordsLastIndex] = word; // добавить слово в словарь
                            tempWordsLastIndex++; // увел. индекс последнего слова
                        }
                        word = "";
                        wordsStart += 1; // след слово будет идти после пробела -> пропустить пробел
                    }
                    wordsIterator++;
                    if(wordsIterator < currentLine.Length) {
                        goto wordsLoop;
                    }
                    if (currentLine.Length == 0) {
                        goto countWordsLoopEnd;
                    }

                    dictionaryLoop:
                        checkDictionaryResult = -1;
                        checkDictionary: // перебираем словарь в поисках слова
                            // если нашли слово, прервать
                            if(wordKeys[checkDictionaryIndex] == tempWords[tempWordsIterator]) {
                                checkDictionaryResult = checkDictionaryIndex;
                                goto checkDictionaryEnd;
                            }
                            // если нет, прододжать пока не закончится словарь
                            if(checkDictionaryIndex < checkDictionaryLastIndex) {
                                checkDictionaryIndex++;
                                goto checkDictionary;
                            }
                        
                        checkDictionaryEnd:
                        checkDictionaryIndex = 0;

                        // посчитать номер страницы относительно номера линии (45 линий на странице)
                        currentPage = 1;
                        int lineIndex = linesIterator;
                        lineIndexLoop:
                            if(lineIndex - 45 > 0) {
                                currentPage++;
                            }

                            lineIndex -= 45;
                            if(lineIndex > 0) {
                                goto lineIndexLoop;
                            }

                        // если нашли слово в словаре
                        if(checkDictionaryResult != -1) {
                            // игнорировать слова, которые уже встретились 100 или больше раз
                            if (wordCounts[checkDictionaryResult] < 100) {
                                // найти индекс последней страницы
                                findLoop:
                                    // если страница уже указанна, проигнорировать
                                    if (wordValues[checkDictionaryResult][wordValuesIterator] == currentPage) {
                                        goto findLoopEnd;
                                    }
                                    // если нет, добавить
                                    else if (wordValues[checkDictionaryResult][wordValuesIterator] == 0) {
                                        wordValues[checkDictionaryResult][wordValuesIterator] = currentPage;
                                        goto findLoopEnd;
                                    }

                                    wordValuesIterator++;
                                    if (wordValuesIterator < 100) {
                                        goto findLoop;
                                    }
                                findLoopEnd:
                                wordValuesIterator = 0;
                                wordCounts[checkDictionaryResult]++; // увел. счетчик слова
                            }
                        }   
                        // если не нашли, добавить в словарь
                        else {
                            wordKeys[checkDictionaryLastIndex] = tempWords[tempWordsIterator];
                            wordValues[checkDictionaryLastIndex] = new int[100];
                            wordValues[checkDictionaryLastIndex][0] = currentPage;
                            wordCounts[checkDictionaryLastIndex]++;
                            checkDictionaryLastIndex++;
                        }

                        if(tempWordsIterator < tempWordsLastIndex - 1) { // повторить, пока не закончатся слова
                            tempWordsIterator++;
                            goto dictionaryLoop;
                        }
                
                countWordsLoopEnd:
                    linesIterator++;
                    if(linesIterator < linesLastIndex) {
                        goto countWordsLoop;
                    }

            // бабл сортировка
            startouter:
                if(index >= checkDictionaryLastIndex - 1) {
                    goto endouter;
                }
                jIndex = 0;
            startinner:
                if (jIndex >= checkDictionaryLastIndex - 1) {
                    goto endinner;
                }

                letterCheck = 0;

            // соритировка по алфавиту
            alphabetSort:
                // если текущая буква совпадает, перейти к следующей
                if (wordKeys[jIndex][letterCheck] == wordKeys[jIndex + 1][letterCheck]) {
                    if (letterCheck < wordKeys[jIndex].Length - 1 && letterCheck < wordKeys[jIndex + 1].Length - 1) {
                        letterCheck++;
                        goto alphabetSort;
                    }
                }
                // сверить буквы
                else if (wordKeys[jIndex][letterCheck] < wordKeys[jIndex + 1][letterCheck]) {
                    goto noswap;
                }

                tmp = wordKeys[jIndex];
                tmp2 = wordValues[jIndex];
                tmp3 = wordCounts[jIndex];
                wordKeys[jIndex] = wordKeys[jIndex + 1];
                wordValues[jIndex] = wordValues[jIndex + 1];
                wordCounts[jIndex] = wordCounts[jIndex + 1];
                wordKeys[jIndex + 1] = tmp;
                wordValues[jIndex + 1] = tmp2;
                wordCounts[jIndex] = tmp3;
            noswap:
                jIndex++;
                goto startinner;
            endinner:
                index++;
                goto startouter;
            endouter:

            index = 0;
            jIndex = 0;

            write:
                if (wordCounts[index] < 100) {
                    Console.Write(wordKeys[index] + " - ");
                    jIndex = 0;
                    var result = 0;
                    // найти кол-во страниц
                    findLoop2:
                        if (wordValues[index][jIndex] == 0) {
                            result = jIndex - 1;
                            goto findLoop2End;
                        }

                        jIndex++;
                        if (jIndex < 100) {
                            goto findLoop2;
                        }
                    findLoop2End:
                    jIndex = 0;

                    // вывести страницы по очереди
                    writePages:
                        Console.Write(wordValues[index][jIndex]);

                    if (jIndex < result) {
                        jIndex++;
                        Console.Write(", "); // добавить запятую, если еще есть станицы
                        goto writePages;
                    }
                    Console.Write('\n');
                }
                
                if (index != checkDictionaryLastIndex - 1) {
                    index++;
                    goto write;
                }


            

        }
    }
}
