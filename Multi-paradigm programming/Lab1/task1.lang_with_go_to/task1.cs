using System;
using System.IO;

namespace Lab_1_Multi_paradigm_programming
{
    class Task1
    {
        static void Main(string[] args)
        {
            string text = File.ReadAllText("task1.lang_with_go_to/text.txt"); 
            string[] words = new string[100];
            string word = "";
            char letter = ' ';
            var wordListLastIndex = 0; // индекс последнего слова в списке
            var index = 0; // итератор
            var jIndex = 0; // второй итератор
            var wStart = 0; // начальный индекс слова
            var wEnd = 0; // конечный --//--
            string[] wordKeys = new string[100]; // слова
            int[] wordValues = new int[100]; // кол-во слов
            var checkDictionaryIndex = 0; // итератор словаря
            var checkDictionaryResult = -1; // индекс слова в словаре, если оно (слово) там есть
            var checkDictionaryLastIndex = 0; // индекс последнего слова в словаре
            var tmp = "";
            var tmp2 = 0;

            loop1: // перебираем текст, ищем слова
                if(text[index] == ' ' || text[index] == '\r' || index + 1 == text.Length) {
                    wEnd = index - 1; // определяем конец слова
                    
                    // если это последняя строка в тексте
                    if (index + 1 == text.Length) {
                        wEnd++;
                    }

                    // считываем слово
                    addWordLoop:
                        if(wStart != wEnd + 1) { // перебираем буквы
                            // если большая буква -> сделать маленькой
                            if(text[wStart] >= 'A' && text[wStart] < 'a') {
                                letter = (char)(text[wStart] + 32);
                            }
                            // игнорировать знаки припенания
                            else if (text[wStart] == '.' || text[wStart] == '!' || text[wStart] == '?' || text[wStart] == ',' || text[wStart] == '-') {
                                wStart++;
                                goto addWordLoop;
                            }
                            else {
                                letter = text[wStart];
                            }
                            // добавить букву в слово
                            word += letter;
                            wStart++; // увеличить итератор (что бы взять след. букву)
                            goto addWordLoop;
                        }

                    // игнорировать "стоп-слова"
                    if(word != "for" && word != "the" && word != "as" && word != "in" && word != "a" && word != "on" && word != "") {
                        words[wordListLastIndex] = word; // добавить слово в словарь
                        wordListLastIndex++; // увел. индекс последнего слова
                    }
                    word = "";
                    wStart += 1; // след слово будет идти после пробела -> пропустить пробел
                }

                if(text.Length - 1 > index) { // повторить цикл, если еще не перебрали все символы
                    index++;

                    if(text[index] == '\n') { // если конец строки, пропустить \r\n
                        index++;
                        wStart++;
                    }
                    goto loop1;
                }

            index = 0;

            // считаем слова
            countWords:
                checkDictionaryResult = -1;

                checkDictionary: // перебираем словарь в поисках слова
                    // если нашли слово, прервать
                    if(wordKeys[checkDictionaryIndex] == words[index]) {
                        checkDictionaryResult = checkDictionaryIndex;
                        goto checkDictionaryEnd;
                    }
                    // если нет, прододжать пока не закончится словарь
                    if(checkDictionaryIndex != checkDictionaryLastIndex) {
                        checkDictionaryIndex++;
                        goto checkDictionary;
                    }
                checkDictionaryIndex = 0;
                checkDictionaryEnd:

                // если нашли слово в словаре
                if(checkDictionaryResult != -1) {
                    wordValues[checkDictionaryResult]++; // увел. его кол-во
                    checkDictionaryResult = -1;
                }    
                // если не нашли, добавить в словарь
                else {
                    wordKeys[checkDictionaryLastIndex] = words[index];
                    wordValues[checkDictionaryLastIndex] = 1;
                    checkDictionaryLastIndex++; // увел. индекс последнего слова в словаре
            } 
                if(index != wordListLastIndex - 1) { // повторить, пока не закончатся слова
                    index++;
                    goto countWords;
                }

            // бабл сортировка по кол-ву слов
            index = 0;
            startouter:
                if(index >= checkDictionaryLastIndex - 1) {
                    goto endouter;
                }
                jIndex = 0;
            startinner:
                if (jIndex >= checkDictionaryLastIndex - 1) {
                    goto endinner;
                }
                if (wordValues[jIndex] > wordValues[jIndex + 1]) {
                    goto noswap;
                }
                tmp = wordKeys[jIndex];
                tmp2 = wordValues[jIndex];
                wordKeys[jIndex] = wordKeys[jIndex + 1];
                wordValues[jIndex] = wordValues[jIndex + 1];
                wordKeys[jIndex + 1] = tmp;
                wordValues[jIndex + 1] = tmp2;
            noswap:
                jIndex++;
                goto startinner;
            endinner:
                index++;
                goto startouter;
            endouter:

            index = 0;

            write:
                Console.WriteLine(wordKeys[index] + " - " + wordValues[index]);
                if (index != checkDictionaryLastIndex - 1) {
                    index++;
                    goto write;
                }
        }
    }
}
