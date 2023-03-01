using Match3;
//using Match3.Zenject;
using UnityEngine;
using Zenject;

public class GameMonoInstaller : MonoInstaller
{
    [SerializeField] private Element elementPrefab;

    public override void InstallBindings()
    {
        // SignalBusInstaller.Install(Container);
        // SignalsInstaller.Install(Container);
        SignalInstaller.Install(Container);
        Container.BindInterfacesAndSelfTo<BoardController>().AsSingle().NonLazy();
        Container.BindFactory<ElementConfigItem, ElementPosition, Element, Element.Factory>().FromComponentInNewPrefab(elementPrefab);
    }
}