using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Match3.Signals;
using UnityEngine;
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
            _signalBus.Subscribe<RestartButtonSignal>(DoRestart);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<OnElementClickSignal>(OnElementClick);
            _signalBus.Unsubscribe<RestartButtonSignal>(DoRestart);
        }

        private void DoRestart()
        {
            Debug.Log("Restart");
        }

        private async void OnElementClick(OnElementClickSignal signal)
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
                if (IsCanSwap(_firstSelected, element))
                {
                    _firstSelected.SetSelected(false);
                    Swap(_firstSelected, element);
                    _firstSelected = null;
                    await CheckBoard();
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

        private async UniTask CheckBoard()
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
                    await DisableElements(elementsForCollecting);
                    _signalBus.Fire(new OnBoardMatchSignal(elementsForCollecting.Count));
                    await NormalizeBoard();
                    isNeedRecheck = true;
                }
            } while (isNeedRecheck);

            _isBlocked = false;
        }

        private List<Element> SearchLines()
        {
            var elementsForCollecting = new List<Element>();

            var column = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;

            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < column; x++)
                {
                    if (_elements[x, y].IsActive && !elementsForCollecting.Contains(_elements[x, y]))
                    {
                        List<Element> checkResult;
                        bool needAddFirst = false;
                        checkResult = CheckHorizontal(x, y);
                        if (checkResult != null && checkResult.Count >= 2)
                        {
                            needAddFirst = true;
                            elementsForCollecting.AddRange(checkResult);
                        }

                        checkResult = CheckVertical(x, y);
                        if (checkResult != null && checkResult.Count >= 2)
                        {
                            needAddFirst = true;
                            elementsForCollecting.AddRange(checkResult);
                        }

                        if (needAddFirst)
                        {
                            elementsForCollecting.Add(_elements[x, y]);
                        }
                    }
                }
            }

            return elementsForCollecting;
        }

        private List<Element> CheckHorizontal(int x, int y)
        {
            var column = _boardConfig.SizeX;
            int nextColumn = x + 1;

            if (nextColumn >= column)
            {
                return null;
            }

            var elementsInLine = new List<Element>(column);
            var element = _elements[x, y];
            while (_elements[nextColumn, y].IsActive && element.ConfigItem.Key == _elements[nextColumn, y].ConfigItem.Key)
            {
                elementsInLine.Add(_elements[nextColumn, y]);
                if (nextColumn + 1 < column)
                {
                    nextColumn++;
                }
                else
                {
                    break;
                }
            }

            return elementsInLine;
        }

        private List<Element> CheckVertical(int x, int y)
        {
            var row = _boardConfig.SizeY;
            int nextRow = y + 1;

            if (nextRow >= row)
            {
                return null;
            }

            var elementsInLine = new List<Element>(row);
            var element = _elements[x, y];
            while (_elements[x, nextRow].IsActive && element.ConfigItem.Key == _elements[x, nextRow].ConfigItem.Key)
            {
                elementsInLine.Add(_elements[x, nextRow]);
                if (nextRow + 1 < row)
                {
                    nextRow++;
                }
                else
                {
                    break;
                }
            }

            return elementsInLine;
        }

        private async UniTask NormalizeBoard()
        {
            var column = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;

            for (int x = column - 1; x >= 0; x--)
            {
                var freeElements = new List<Element>(row);
                for (int y = row - 1; y >= 0; y--)
                {
                    if (y >= 0 && !_elements[x, y].IsActive)
                    {
                        freeElements.Add(_elements[x, y]);
                        y--;
                    }

                    if (y >= 0 && freeElements.Count > 0)
                    {
                        Swap(_elements[x, y], freeElements[0]);
                        freeElements.Add(freeElements[0]);
                        freeElements.RemoveAt(0);
                    }
                }
            }


            List<UniTask> tasks = new List<UniTask>();
            for (int y = row - 1; y >= 0; y--)
            {
                for (int x = column - 1; x >= 0; x--)
                {
                    if (!_elements[x, y].IsActive)
                    {
                        tasks.Add(GenerateRandomElements(_elements[x, y], column, row));
                    }
                }
            }

            await UniTask.WhenAll(tasks);
        }

        private async UniTask GenerateRandomElements(Element element, int column, int row)
        {
            var gridPosition = element.GridPosition;
            var elements = GetPossibleElement((int) gridPosition.x, (int) gridPosition.y, column, row);
            element.SetConfig(elements);
            await element.Enable();
        }

        private async UniTask DisableElements(List<Element> elementsForCollecting)
        {
            List<UniTask> tasks = new List<UniTask>();
            foreach (var element in elementsForCollecting)
            {
                tasks.Add(element.Disable());
            }

            await UniTask.WhenAll(tasks);
        }

        private void Swap(Element first, Element second)
        {
            _elements[(int) first.GridPosition.x, (int) first.GridPosition.y] = second;
            _elements[(int) second.GridPosition.x, (int) second.GridPosition.y] = first;

            var position = second.transform.localPosition;
            var gridPosition = second.GridPosition;

            second.SetLocalPosition(first.transform.localPosition, first.GridPosition);
            first.SetLocalPosition(position, gridPosition);
        }

        private bool IsCanSwap(Element first, Element second)
        {
            var pos1 = first.GridPosition;
            var pos2 = second.GridPosition;

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
                    var element = _factory.Create(GetPossibleElement(x, y, column, row), new ElementPosition(position, new Vector2(x, y)));
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