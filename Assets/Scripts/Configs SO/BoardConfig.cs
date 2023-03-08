using UnityEngine;

namespace Match3
{
    [CreateAssetMenu(fileName = "BoardConfig", menuName = "Configs/BoardConfig", order = 0)]
    public class BoardConfig : ScriptableObject
    {
        [SerializeField] private int sizeX = 5; // колл по X 
        [SerializeField] private int sizeY = 7;// колл по Y
        [SerializeField] private float elementOffset = 1f;// отступ между элементами 

        public int SizeX => sizeX;

        public int SizeY => sizeY;

        public float ElementOffset => elementOffset;
    }
}