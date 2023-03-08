using Match3;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameConfigInstaller", menuName = "Installers/GameConfigInstaller")]
public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller> // что-то на подобии SO 
{ // сюда SO передавать ток через вот этот путь Installers/GameConfigInstaller (потом туда передавать обычные SO)
    [SerializeField] private ElementsConfig elementsConfig;
    [SerializeField] private BoardConfig boardConfig;
    public override void InstallBindings() // тут процесс передачи зависимостей в контейнер(для Сцены(уровня и т.д))
    {
        // Container.BindInstance - для одного конфига (Container.BindInstanceS - дофига и трошки)
        Container.BindInstances(        
            elementsConfig,  // это SO (Юнити) кидем его в контейнер (Игровой Установочник Конфигов)
            boardConfig);  
        // тут передаем ссылки на объекты, которые хотим положить в контейнер (можно через запетую скок хочш прям)
    }
}