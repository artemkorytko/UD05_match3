using System;
using System.Collections.Generic;
using Match3.Signal;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Match3
{
    public class BoardController : IInitializable, IDisposable
    {
        private readonly Element.Factory _factory;
        private readonly ElementsConfig _config;
        private readonly BoardConfig _boardConfig;

        private Element[,] _elements;
        private Element _firstSelected;

        private bool _isBloked;
        
        private SignalBus _signalBus;

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
            if(_isBloked)
                return;
            
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
            _isBloked = true;

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
                    // signal for counter
                    NormalizeBoard();
                    isNeedRecheck = true;
                }
            } while (isNeedRecheck);
            
            _isBloked = false;
        }

        private void NormalizeBoard()
        {
            throw new NotImplementedException();
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
            return null;
        }

        private void Swap(Element first, Element second)
        {
            _elements[(int)_firstSelected.GridPosition.x, (int)_firstSelected.GridPosition.y] = second;
            _elements[(int)_firstSelected.GridPosition.x, (int)_firstSelected.GridPosition.y] = _firstSelected;

            var position = second.transform.localPosition;
            var gridPosition = second.GridPosition;

            second.SetLocalPosition(first.transform.localPosition, first.GridPosition);
            first.SetLocalPosition(position, gridPosition);
        }

        private bool IsCanSwap(Element first, Element element)
        {
            var pos1 = first.GridPosition;
            var pos2 = element.GridPosition;

            var comparePos = pos1;
            comparePos.x += 1;
            if (comparePos == pos2)
                return true;

            comparePos = pos1;
            comparePos.x -= 1;
            if (comparePos == pos2)
                return true;
            
            comparePos = pos1;
            comparePos.y -= 1;
            if (comparePos == pos2)
                return true;
            
            comparePos = pos1;
            comparePos.y -= 1;
            if (comparePos == pos2)
                return true;

            return false;
        }

        private void GenerateElements()
        {
            var column = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;
            var offset = _boardConfig.ElementOffset;
            
            _elements = new Element[column,row];
            var  startPosition = new Vector2(-column * 0.5f + offset * 0.5f, row * 0.5f - offset * 0.5f);

            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < column; x++)
                {
                    var position = startPosition + new Vector2(offset * x, -offset * y);
                    var element = _factory.Create(GetPossibleElement(x,y,column,row),new ElementPosition(position, new Vector2(x, y))); //
                    
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
                    tempList.Remove(_elements[x, y].ConfigItem);
            }

            x = column - 1;
            y = row;
            
            if (x >= 0 && x < columnCount && y >= 0 && y < rowCount)
            {
                if (_elements[x, y].IsActive)
                    tempList.Remove(_elements[x, y].ConfigItem);
            }
            
            return tempList[Random.Range(0, tempList.Count)];
        }

    }
}