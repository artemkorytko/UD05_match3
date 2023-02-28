using Skripts;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ProjectConfigInstaler", menuName = "Installers/ProjectConfigInstaler")]
public class ProjectConfigInstaler : ScriptableObjectInstaller<ProjectConfigInstaler>
{
    [SerializeField] private ProjectSettings projectSettings;
    public override void InstallBindings()
    {
        Container.BindInstance(projectSettings);
    }
}