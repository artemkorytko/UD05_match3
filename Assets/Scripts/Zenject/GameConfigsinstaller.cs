using Match3;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameConfigsinstaller", menuName = "Installers/GameConfigsinstaller")]
public class GameConfigsinstaller : ScriptableObjectInstaller<GameConfigsinstaller>
{
    [SerializeField] private ElementsConfig elementsConfig;
    [SerializeField] private BoardConfig boardConfig;
    public override void InstallBindings()
    {
        Container.BindInstances(
            elementsConfig,
            boardConfig);
    }
}