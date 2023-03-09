using DefaultNamespace;
using DefaultNamespace.SaveSystems;
using Match3;
using Match3.Zenject;
using UnityEngine;
using Zenject;

public class GameMonoInstaller : MonoInstaller
{
    [SerializeField] private Element elementPrefab;
    [SerializeField] private UIManager uiManager;
    public override void InstallBindings()
    {
        Debug.Log("GameMonoInstaller");
        
        SignalsInstaller.Install(Container); //инстал контейнера  
        // SignalBusInstaller.Install(Container) -  происходит инстал в ProjectMonoInstaller
       
        BindSaveSystemAndGameData();
        BindGameManager();
        BindUI(); 
        BindBoard();
    }

    private void BindSaveSystemAndGameData()
    {
        Container.Bind<ISaveSystem>().To<SaveSystemJson>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameData>().AsSingle().NonLazy();
    }

    private void BindBoard()
    {
        Container.BindInterfacesAndSelfTo<BoardController>().AsSingle().NonLazy();
        Container.BindFactory<ElementConfigItem, ElementPosition, Element, Element.Factory>().FromComponentInNewPrefab(elementPrefab); 
        // билд фабрики Element.Factory
        //постой фабрику вот с такими данными (BindFactory<ElementConfigItem, ElementPosition, Element,)
        // эта фабрика находиться вот тут Element.Factory
        // FromComponentInNewPrefab(elementPrefab)   - это какой прифаб используется. 
    }

    private void BindGameManager()
    {
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
        // AsSingle - объект только может быть один!  NonLazy - тип отключить Ленивую ссылку (объект создан сразу)
    }

    private void BindUI()
    {
        uiManager = Container.InstantiatePrefabForComponent<UIManager>(uiManager.gameObject);                                                                      
        Container.Bind<UIManager>().FromInstance(uiManager).AsSingle().NonLazy();
    }
}