using UnityEngine;
// СКРПТАБЛ ОБДЖЕКТ - данные размеров борда
// раз он конфиг - добавляем его в GameConfigInstaller
// через Zenject этот файл выдается в BoardController

namespace Match3
{
    [CreateAssetMenu(fileName = "BoardConfig", menuName = "Configs/BoardConfig", order = 0)]
    public class BoardConfig : ScriptableObject
    {
        [SerializeField] private int sizeX = 5;
        [SerializeField] private int sizeY = 7;
        [SerializeField] private float elementOffset = 1f;

        // инкапсуляция шоб снаружи читать - read only
        public int SizeX => sizeX;

        public int SizeY => sizeY;

        public float ElementOffset => elementOffset;
    }
}