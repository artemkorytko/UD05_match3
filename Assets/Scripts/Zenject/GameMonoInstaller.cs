using Match3;
using UnityEngine;
using Zenject;

public class GameMonoInstaller : MonoInstaller
{
    [SerializeField] private Element elementPrefab;

    public override void InstallBindings()
    {
        SignalInstaller.Install(Container);
       
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BoardController>().AsSingle().NonLazy();
        Container.BindFactory<ElementConfigItem, ElementPosition, Element, Element.Factory>().FromComponentInNewPrefab(elementPrefab);
    }
}