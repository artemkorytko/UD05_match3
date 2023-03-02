using Skripts;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameConfigInstaller", menuName = "Installers/GameConfigInstaller")]
public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller>
{
    [SerializeField] private ElementsConfig elementsConfig;
    [SerializeField] private BoardConfig boardConfig;
    public override void InstallBindings()
    {
        Container.BindInstances(elementsConfig,boardConfig);
    }
}