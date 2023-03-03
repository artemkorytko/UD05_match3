using UnityEngine;

// СТРУКТУРА - ТОЛЬКО ПЕРЕДАЕТ ИНФУ
// хранит логическую позицию элемента + локальную позицию

// небольшой контейнер в памяти
// берет дату из логики и передаст внутрь класса Element

namespace Match3
{
    public struct ElementPosition
    {
        public readonly Vector2 LocalPosition; // визуальная позиция
        public readonly Vector2 GridPosition; // место в логической позиции
        // readonly значит, что значение сюда может быть присвоено только из конструктора
        // alt enter на них - выбрать  initialize field from constructor
        // автоматом получится во это:

        public ElementPosition(Vector2 localPosition, Vector2 gridPosition)
        {
            LocalPosition = localPosition;
            GridPosition = gridPosition;
        }
    }
}