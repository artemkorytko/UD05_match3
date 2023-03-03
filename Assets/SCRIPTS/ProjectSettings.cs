using UnityEngine;

// файл ScriptableObject! add > scriptable
// дефолтные настройки проекта
namespace Match3
{
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "Configs/ProjectSettings", order = 0)]
    public class ProjectSettings : ScriptableObject
    {
        // с каким фпс
        [SerializeField] private int targetFps;
        // разрешены ли мультитачи
        [SerializeField] private bool isMultitouch;

        // инкапcуляция - read only, только одна галочка
        // правой кнопкой на желтой лампочке выделив переменную выше
        public int TargetFps => targetFps;

        public bool IsMultitouch => isMultitouch;
    }
}