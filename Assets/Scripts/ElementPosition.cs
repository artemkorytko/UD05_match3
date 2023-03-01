using UnityEngine;

namespace Match3
{
    public struct ElementPosition
    {
        public readonly Vector2 LocalPosition;
        public readonly Vector2 GridPosition;

        public ElementPosition(Vector2 localPosition, Vector2 gridPosition)
        {
            LocalPosition = localPosition;
            GridPosition = gridPosition;
        }
    }
}