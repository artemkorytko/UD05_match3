using UnityEngine;

namespace Match3
{
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "Configs/ProjectSettings", order = 0)]
    public class ProjectSettingsConfig : ScriptableObject // тут дефолтные настройти проекта!!! 
    {
        [SerializeField] private int targetFps;
        [SerializeField] private bool isMultitouch;

        public int TargetFps => targetFps;

        public bool IsMultitouch => isMultitouch;
    }
}