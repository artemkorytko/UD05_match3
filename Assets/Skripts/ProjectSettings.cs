using UnityEngine;

namespace Skripts
{
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "Configs/ProjectSettings", order = 0)]
    public class ProjectSettings : ScriptableObject
    {
        [SerializeField] private int targetFps;
        [SerializeField] private bool isMultitouch;

        public int TargetFps => targetFps;

        public bool IsMultitouch => isMultitouch;
    }
}