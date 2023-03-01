using Match3;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ProjectConfigsInstaller", menuName = "Installers/ProjectConfigsInstaller")]
public class ProjectConfigsInstaller : ScriptableObjectInstaller<ProjectConfigsInstaller>
{
    [SerializeField] private ProjectSettings _projectSettings;
    
    public override void InstallBindings()
    {
        Container.BindInstance(_projectSettings);
    }
}