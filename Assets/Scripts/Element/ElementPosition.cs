using UnityEngine;

namespace Match3
{
    public struct ElementPosition
    {
        public readonly Vector2 LocalPosition; // локальная позиция
        public readonly Vector2 GridPosition; // логическая позиция

        public ElementPosition(Vector2 localPosition, Vector2 gridPosition)
        {
            LocalPosition = localPosition;
            GridPosition = gridPosition;
        }
    }
}