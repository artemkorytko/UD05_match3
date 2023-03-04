using System;
using System.Collections.Generic;
using Match3.Signals;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

// СОЗДАЕТ ДОСКУ СОГЛАСНО РАЗМЕРАМ
// ШАРПОВСКИЙ КЛАСС - new class/Interface
// отвечает за создание брюликов
// его надо забиндить в GameMonoInstaller

namespace Match3
{
    // наследуется от зенджектовского awake
    // IDisposable - для отписки сигналбаса
    public class BoardController : IInitializable, IDisposable
    {
        private readonly ElementsConfig _config;
        private readonly Element.Factory _factory;
        private readonly BoardConfig _boardConfig;
        private readonly SignalBus _signalBus;

        private Element[,] _elementsArray;
        private Element _firstSelected; // ссылка на предыдущий кликнутый для свапов
        private bool _isBlocked; //для фиксирования пока идет проверка CheckBoard

        // конструктор, который на вход принимает элементs конфиг и фабрику
        // требует вон борд конфиг и сразу ему переменную ниже, и сверху ее надо тоже поле писать (автоматически на серое)
        public BoardController(ElementsConfig config, Element.Factory factory, BoardConfig boardConfig, SignalBus signalBus)
        {
            _config = config; // АК копирует сверху слово, = такое же, alt enter создать
            _factory = factory;
            _boardConfig = boardConfig;
            _signalBus = signalBus; 
        }

        public void Initialize()
        {
            Debug.Log(" дошло до inint Board Controller");
            // для проверки что вообще создает, было ранее - кого и в какой позиции - в нулях например
            // _factory.Create(_config.GetItemByKey("Pink"), new ElementPosition(Vector2.zero, Vector2.zero));
            
            GenerateElements();
            // подписаться < на какое событие > 
            // ждем что ктото зафайрит сущность типа OnElementClickSignal 
            // это происходит внутри Element
            _signalBus.Subscribe<OnElementClickSignal>(OnElementClick);
        }

        // это типо он-дестрой но зенджектовский
        public void Dispose()
        {
            // отписаться
            _signalBus.Unsubscribe<OnElementClickSignal>(OnElementClick);
        }

        // ждет сигнал на вход - когда будет клик по сигналу
        // сигнал принес тот элемент, с который был кликнут и работаем с ним
        private void OnElementClick(OnElementClickSignal signal)
        {
            if(_isBlocked) // если кликнули пока заблокировано (идет проверка) - шлем все нафиг
                return;
            
            var element = signal.Element; // элемент получили из сигнала
            
            // если кликнули на тот же самый - снимаем выделение
            
            // если ранее не кликали - мы и есть _firstSelected
            if (_firstSelected == null)
            {
                _firstSelected = element;
                
                // говорим выделиться
                _firstSelected.SetSelected(true);
            }
            else // если кликнули уже был _firstSelected
            {
                // рядом ли он, можем ли свайпнуться
                if (IsCanSwap(_firstSelected, element)) // который есть, в тот которому клик
                {
                    _firstSelected.SetSelected(false); //снять выделение
                    Swap(_firstSelected, element); // откуда было выделено, в то куда кликнули
                    _firstSelected = null; // сбросить выделение
                    //-------------- ВРЕМЕННО ЗАКОММИЧЕНО ------------------------------------
                    //CheckBoard();
                }
                else //не по соседнему, не можем свайпнуться
                {   
                    // если равен то кликнули сами по себе второй раз
                    if (_firstSelected == element)
                    {
                        _firstSelected.SetSelected(false); //снять с себя выделение
                        _firstSelected = null; // сказать что никто не выделен - ссылка на себя
                    }
                    else
                    {
                        _firstSelected.SetSelected(false); // снять выделение с себя
                        _firstSelected = element; //обновить себя мы теперь первый выделенный
                        _firstSelected.SetSelected(true); // ыыыыы
                    }
                }
            }
        }

        //---------------- проверка что изменилось ---------------------------
        // пока идет проверка - доска должна быть заблокирована нет ли матча, может быть анимация
        // игнорируем клики
        private void CheckBoard()
        {
            _isBlocked = true; // заблокировано все

            bool isNeedRecheck = false;
            
            // завели массив элементов которые можем собрать за время проверки доски
            var elementsForCollecting = new List<Element>(); // список типа Element

            do // повторяем проверку пока не закончатся матчи
               // хоть раз сделает и повторит если флаг изменится на тру
            {
                isNeedRecheck = false; // сбросить флаг
                elementsForCollecting.Clear(); // чистим список
                elementsForCollecting = SearchLines(); // проверяем по рядам

                if (elementsForCollecting.Count > 0) // если нашлись элементы котороые надо собрать, то мы их выключаем
                {
                    DisableElements(elementsForCollecting); // выключаем и передаем массив которые надо
                    // ПООТОМ ДОБАВИМ signal for counter
                    NormalizeBoard(); // создаем новые элементы
                    isNeedRecheck = true;
                }
            } while (isNeedRecheck);

            _isBlocked = false; // когда логика доработала снимаем проверку
        }

        
        //----------- СОЗДАТЬ НОВЫЕ ЭЛЕМЕНТЫ --------------------------------
        private void NormalizeBoard()
        {
            
        }

        //------------ выключить массив собранных ---------------------------
        private void DisableElements(List<Element> elementsForCollecting)
        {
            // пробежаться и удалить элеиенты с борды
            foreach (var element in elementsForCollecting)
            {
                element.Disable(); // выключись, внутри файла Element
            }
        }

        //-------------- проверяем 
        private List<Element> SearchLines()
        {
            return null;
        }
        

        //-------- заменяет один элемент на второй ----------------------------------------
        private void Swap(Element first, Element second)
        {
            // меням в логике
            _elementsArray[(int) first.GridPosition.x, (int) first.GridPosition.y] = second;
            _elementsArray[(int) second.GridPosition.x, (int) second.GridPosition.y] = first;

            // надо менять визуально - надо закешировать позиции второму:
            var position = second.transform.localPosition;
            var gridPosition = second.GridPosition;

            // это методы у элемента, передаем туда (АНИМАЦИЯ ТАМ)
            second.SetLocalPosition(first.transform.localPosition, first.GridPosition);
            first.SetLocalPosition(position, gridPosition);
        }

        //--------- рядом ли? ----------------------------------------------------------------
        private bool IsCanSwap(Element first, Element second)
        {
            // работаем с гридовской позицией, проверяем находится ли рядом
            var pos1 = first.GridPosition; // позиция первого выделенного
            var pos2 = second.GridPosition; // кликнутый

            var comparePosition = pos1; // кого сравниваем
            comparePosition.x += 1; // шаг вправо
            if (comparePosition == pos2) // значит это соседний возвращаем тру
            {
                return true;
            }

            comparePosition = pos1;
            comparePosition.x -= 1; // слева
            if (comparePosition == pos2)
            {
                return true;
            }

            comparePosition = pos1;
            comparePosition.y += 1; // сверху
            if (comparePosition == pos2)
            {
                return true;
            }

            comparePosition = pos1;
            comparePosition.y -= 1; // снизу
            if (comparePosition == pos2)
            {
                return true;
            }

            return false; // во всех других случаях возвращаем фолс - значит он не соседний
        }

        
        private void GenerateElements()
        {
            // надо пройти по всему полю и создавать по рядам элементы
            
            // считывает данные из борд конфига
            var column = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;
            var offset = _boardConfig.ElementOffset;

            // храниться будут два значения - двоичный массив
            // для инициализации массива передаем внутриь [] количество столбов и рядов
            _elementsArray = new Element[column, row];

            // ноль в центре - надо сместиться начало расстановки на половину ширины на отрицательное значение
            //                              Х                                 Y
            var startPosition = new Vector2(-column * 0.5f + offset * 0.5f, row * 0.5f - offset * 0.5f);
            for (int y = 0; y < row; y++) // пока меньше количества рядочков
            {
                for (int x = 0; x < column; x++) //колоночек
                {
                    // куда ставим 
                    var position = startPosition + new Vector2(offset * x, -offset * y);
                    
                    // для фабрики: 
                    // var element - ссылка для добавления в массив
                    // из функции фильтровки GetPossible 
                    var element = _factory.Create(GetPossibleElement(x, y, column, row), new ElementPosition(position, new Vector2(x, y)));
                    element.Initialize(); //каждому инит
                    _elementsArray[x, y] = element; //добавить его в массив
                }
            }
        }
        
        

        //-------------------- проверка шоб цвета не совпадали ----------------------------------------
        // ээээээ он возвращает ElementConfigItem из ElementConfig - один из пяти
        
        // на вход позиции, и сколько всего рядов и столбиков
        private ElementConfigItem GetPossibleElement(int column, int row, int columnCount, int rowCount)
        {
            // временный массив лист для вычислений, передаем туда массив всех конфигов
            // будем удалять оттуда не подоходящие
            var tempList = new List<ElementConfigItem>(_config.Items);

            // проверка сверху
            int x = column;
            int y = row - 1; //если ноль то пропустим ибо сверху ничего, 1 - берем верхний ряд
            
            if (x >= 0 && x < columnCount && y >= 0 && y < rowCount) 
            {
                // я не понимаю что тут в массиве.................да и вообще
                
                // знать инициализирован он или нет, если нет - то берем
                // потом поменяем на инициализированный пока активный
                if (_elementsArray[x, y].IsActive)
                {
                    tempList.Remove(_elementsArray[x, y].ConfigItem); // убираем из листа
                }
            }

            // проверка слева
            x = column - 1; 
            y = row;

            if (x >= 0 && x < columnCount && y >= 0 && y < rowCount)
            {
                if (_elementsArray[x, y].IsActive)
                {
                    tempList.Remove(_elementsArray[x, y].ConfigItem);
                }
            }

            return tempList[Random.Range(0, tempList.Count)]; // возвращаем рандомный из того что осталось
        }
    }
}