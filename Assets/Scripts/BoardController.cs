using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using Match3.Signals;
using Zenject;
using UnityEngine;
using IInitializable = Zenject.IInitializable;
using Random = UnityEngine.Random;

namespace Match3
{
    public class BoardController : IInitializable, IDisposable
    {
        private readonly ElementsConfig _config;
        private readonly Element.Factory _factory;
        private readonly BoardConfig _boardConfig;

        private Element[,] _elements;
        private readonly SignalBus _signalBus;

        private Element _firstSelected;
        private bool _isBlocked;

        public BoardController(ElementsConfig config, Element.Factory factory, BoardConfig boardConfig, SignalBus signalBus)
        {
            _config = config;
            _factory = factory;
            _boardConfig = boardConfig;
            _signalBus = signalBus;
        }
        
        
        public void Initialize()
        {
           // _factory.Create(_config.GetItemByKey("Lilac"), new ElementPosition(Vector2.zero, Vector2.zero));
           GenerateElements();
           _signalBus.Subscribe<OnElementClickSignal>(OnElementClick);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<OnElementClickSignal>(OnElementClick);
        }

        private void OnElementClick(OnElementClickSignal signal)
        {
            if (_isBlocked)
            {
                return;
            }
            var element = signal.Element;
            if (_firstSelected == null)
            {
                _firstSelected = element;
                _firstSelected.SetSelected(true);
            }
            else
            {
                if (IsCanSwap(_firstSelected, element))
                {
                    _firstSelected.SetSelected(false);
                    Swap(_firstSelected, element);
                    _firstSelected = null;
                    //CheckBoard();
                }
                else
                {
                    if (_firstSelected == element)
                    {
                        _firstSelected.SetSelected(false);
                        _firstSelected = null;
                    }
                    else
                    {
                        _firstSelected.SetSelected(false);
                        _firstSelected = element;
                        _firstSelected.SetSelected(true);
                    }
                }
            }
        }

        private void CheckBoard()
        {
            _isBlocked = true;

            bool isNeedRecheck = false;
            
            var elementsForCollecting = new List<Element>();

            do
            {
                isNeedRecheck = false;
                elementsForCollecting.Clear();
                elementsForCollecting = SearchLines();

                if (elementsForCollecting.Count > 0)
                {
                    DisableElements(elementsForCollecting);
                    //signal for counter
                    NormalizeBoard();
                    isNeedRecheck = true;
                }

            } while (isNeedRecheck);
            
            _isBlocked = false;
        }

        private void NormalizeBoard()
        {
            
        }
        
        private void DisableElements(List<Element> elementsForCollecting)
        {
            foreach (var element in elementsForCollecting)
            {
                element.Disable();
            }
        }

        private List<Element> SearchLines()
        {
            return null;// пока что
        }

        private void Swap(Element first, Element second)
        {
            _elements[(int)first.gridPosition.x, (int)first.gridPosition.y] = second;
            _elements[(int)second.gridPosition.x, (int)second.gridPosition.y] = second;

            var position = second.transform.localPosition;
            var gridPosition = second.gridPosition;

            second.SetLocalPosition(first.transform.localPosition, first.gridPosition);
            first.SetLocalPosition(position, gridPosition);
        }

        private bool IsCanSwap(Element first,Element  second)
        {
            var pos1 = first.gridPosition;
            var pos2 = second.gridPosition;

            var comparePosition = pos1;
            comparePosition.x += 1;
            if (comparePosition == pos2)
            {
                return true;
            }
            
            comparePosition = pos1;
            comparePosition.x -= 1;
            if (comparePosition == pos2)
            {
                return true;
            }
            
            comparePosition = pos1;
            comparePosition.y += 1;
            if (comparePosition == pos2)
            {
                return true;
            }
            
            comparePosition = pos1;
            comparePosition.y -= 1;
            if (comparePosition == pos2)
            {
                return true;
            }

            return false;
        }
        private void GenerateElements()
        {
            var column = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;
            var offset = _boardConfig.ElementOffset;

            _elements = new Element[column, row];

            var startPosition = new Vector2(-column * 0.5f + offset * 0.5f, row * 0.5f - offset * 0.5f);
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < column; x++)
                {
                    var position = startPosition + new Vector2(offset * x, -offset * y);
                    var element =   _factory.Create(GetPossibleElement(x,y,column, row), new ElementPosition(position, new Vector2(x, y)));
                    element.Initialize();
                    _elements[x, y] = element;
                }
            }
        }

        private ElementConfigItem GetPossibleElement(int column, int row, int columnCount, int rowCount)
        {
            var tempList = new List<ElementConfigItem>(_config.Items);

            int x = column;
            int y = row - 1;

            if (x >= 0 && x < columnCount && y >= 0 && y < rowCount)
            {
                if (_elements[x, y].IsActive )
                {
                    tempList.Remove(_elements[x, y].ConfigItem);
                }
            }

            x = column - 1;
            y = row;
            if (x >= 0 && x < columnCount && y >= 0 && y < rowCount)
            {
                if (_elements[x, y].IsActive )
                {
                    tempList.Remove(_elements[x, y].ConfigItem);
                }
            }
            return tempList[Random.Range(0, tempList.Count)];
        }
        
    }
}