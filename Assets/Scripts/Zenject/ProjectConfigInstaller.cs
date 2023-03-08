using Match3;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ProjectConfigInstaller", menuName = "Installers/ProjectConfigInstaller")]
public class ProjectConfigInstaller : ScriptableObjectInstaller<ProjectConfigInstaller>
{
    [SerializeField] private ProjectSettingsConfig projectSettingsConfig; // ссылка на SO
    public override void InstallBindings()
    {
        Container.BindInstance(projectSettingsConfig); // отправляем его в контейнер 
    }
}