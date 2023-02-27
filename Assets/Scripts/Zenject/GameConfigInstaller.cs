using Match3;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameConfigInstaller", menuName = "Installers/GameConfigInstaller")]
public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller>
{
    [SerializeField] private ElementsConfig elementsConfig;
    public override void InstallBindings()
    {
        Container.BindInstance(elementsConfig);
    }
}