using Match3;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ProjectConfigInstaller", menuName = "Installers/ProjectConfigInstaller")]
public class ProjectConfigInstaller : ScriptableObjectInstaller<ProjectConfigInstaller>
{
    // ссылка на конфиг общих настроек ProjectSettings
    [SerializeField] private ProjectSettings projectSettings;

    public override void InstallBindings()
    {
        // "забиндить проджект сеттингс внутрь контейнера"
        // возьмет кого передадим РУКАМИ у файла ProjectConfigInstaller по ссылке и забиндит
        // теперь создать руками в папке assetы > configs 
        // правой кн мыши > create > configs > ProjectSettings и перетянуть его в дырку ЭТОГО ТУТ файла
        Container.BindInstance(projectSettings);
    }
}