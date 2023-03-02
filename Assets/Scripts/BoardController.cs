using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace Match3
{
    public class BoardController : IInitializable, IDisposable
    {
        private readonly ElementsConfig _config;
        private readonly Element.Factory _factory;
        private readonly BoardConfig _boardConfig;
        private readonly SignalBus _signalBus;

        private Element[,] _elements;
        private Element _firstSelected;
        private bool _isBlocked;
        private int _row;
        private int _column;

        public BoardController(ElementsConfig config, Element.Factory factory, BoardConfig boardConfig, SignalBus signalBus)
        {
            _config = config;
            _factory = factory;
            _boardConfig = boardConfig;
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
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
                return;

            var element = signal.Element;
            if (_firstSelected == null)
            {
                _firstSelected = element;
                _firstSelected.SetSelected(true);
            }
            else
            {
                if (isCanSwap(_firstSelected, element))
                {
                    _firstSelected.SetSelected(false);
                    Swap(_firstSelected, element);
                    _firstSelected = null;
                    CheckBoard();
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
        
        private List<Element> SearchLines()
        {
            var foundMatches = new List<Element>();

            for (int y = 0; y < _row; y++)
            {
                for (int x = 2; x < _column; x++)
                {
                    var firstElement = _elements[x - 2, y];
                    var secondElement = _elements[x - 1, y];
                    var thirdElement = _elements[x, y];

                    if (firstElement.ID == secondElement.ID && firstElement.ID == thirdElement.ID)
                    {
                        foundMatches.Add(firstElement);
                        foundMatches.Add(secondElement);
                        foundMatches.Add(thirdElement);
                    }
                }
            }
            
            for (int x = 0; x < _column; x++)
            {
                for (int y = 2; y < _row; y++)
                {
                    var firstElement = _elements[x, y - 2];
                    var secondElement = _elements[x, y - 1];
                    var thirdElement = _elements[x, y];

                    if (firstElement.ID == secondElement.ID && firstElement.ID == thirdElement.ID)
                    {
                        foundMatches.Add(firstElement);
                        foundMatches.Add(secondElement);
                        foundMatches.Add(thirdElement);
                    }
                }
            }

            return foundMatches;
        }

        private void DisableElements(List<Element> elementsForCollecting)
        {
            foreach (var element in elementsForCollecting)
            {
                element.Disable();
            }
        }
        
        private void NormalizeBoard()
        {
            DropDown();
            FillEmpties();
        }

        private void DropDown()
        {
            for (int y = _row - 1; y >= 0; y--)
            {
                for (int x = 0; x < _column; x++)
                {
                    if (_elements[x, y].IsActive == false)
                    {
                        for (int z = y - 1; z >= 0; z--)
                        {
                            if (_elements[x, z].IsActive == true)
                            {
                                Swap(_elements[x, y], _elements[x, z]);
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        private void FillEmpties()
        {
            for (int y = 0; y < _row; y++)
            {
                for (int x = 0; x < _column; x++)
                {
                    if (_elements[x, y].IsActive == false)
                    {
                        // var element = GetPossibleElement(x, y, _column, _row);
                        var rnd = Random.Range(0, _config.Items.Length);
                        var element = _config.Items[rnd];
                        _elements[x, y].ResetElement(element);
                    }
                }
            }
        }

        private bool isCanSwap(Element first, Element second)
        {
            var pos1 = first.GridPosition;
            var pos2 = second.GridPosition;

            var comparePos = pos1;
            comparePos.x += 1;
            if (comparePos == pos2)
            {
                return true;
            }
            
            comparePos = pos1;
            comparePos.x -= 1;
            if (comparePos == pos2)
            {
                return true;
            }
            
            comparePos = pos1;
            comparePos.y += 1;
            if (comparePos == pos2)
            {
                return true;
            }
            
            comparePos = pos1;
            comparePos.y -= 1;
            if (comparePos == pos2)
            {
                return true;
            }

            return false;
        }
        
        private void Swap(Element first, Element second)
        {
            _elements[(int)first.GridPosition.x, (int)first.GridPosition.y] = second;
            _elements[(int)second.GridPosition.x, (int)second.GridPosition.y] = first;

            var pos = second.transform.localPosition;
            var gridPos = second.GridPosition;

            second.SetLocalPosition(first.transform.localPosition, first.GridPosition);
            first.SetLocalPosition(pos, gridPos);
        }

        private void GenerateElements()
        {
            _column = _boardConfig.SizeX;
            _row = _boardConfig.SizeY;
            var offset = _boardConfig.ElementOffset;

            _elements = new Element[_column, _row];

            var startPos = new Vector2(-_column * 0.5f + offset * 0.5f, _row * 0.5f - offset * 0.5f);

            for (int y = 0; y < _row; y++)
            {
                for (int x = 0; x < _column; x++)
                {
                    var pos = startPos + new Vector2(offset * x, -offset * y);
                    var element = _factory.Create(GetPossibleElement(x, y, _column, _row), new ElementPosition(pos, new Vector2(x, y)));
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
                if (_elements[x, y].IsActive)
                {
                    tempList.Remove(_elements[x, y].ConfigItem);
                }
            }

            x = column - 1;
            y = row;
            
            if (x >= 0 && x < columnCount && y >= 0 && y < rowCount)
            {
                if (_elements[x, y].IsActive)
                {
                    tempList.Remove(_elements[x, y].ConfigItem);
                }
            }

            return tempList[Random.Range(0, tempList.Count)];
        }
    }
}