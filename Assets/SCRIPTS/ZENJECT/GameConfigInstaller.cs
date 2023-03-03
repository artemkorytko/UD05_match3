using Match3;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameConfigInstaller", menuName = "Installers/GameConfigInstaller")]
public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller>
{
    // ссылка на элементс конфиг
    [SerializeField] private ElementsConfig elementsConfig;
    [SerializeField] private BoardConfig boardConfig;

    public override void InstallBindings()
    {
        // и биндится тут ссылка выше
        Container.BindInstances( elementsConfig, boardConfig);
    }
}