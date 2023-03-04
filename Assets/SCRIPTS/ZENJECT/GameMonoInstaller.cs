using Match3;
using Match3.Signals;
// using Match3.Zenject; // убрать
using UnityEngine;
using Zenject;

public class GameMonoInstaller : MonoInstaller
{
    // ссылка на элемент
    [SerializeField] private Element elementPrefab;

    public override void InstallBindings()
    {
        // инсталлер сигналов надо перед основной логикой
        // SignalBusInstaller.Install(Container);
        SignalsInstaller.Install(Container);
        // SignalInstaller.Install(Container);
        
        // биндится борд котнтроллер (создает класс BC)
        Container.BindInterfacesAndSelfTo<BoardController>().AsSingle().NonLazy();
        
        //-------- вот сюда #### БИНДИТСЯ ФАБРИКА #### из Element -------------------------------
        // сложность строки - надо принять и простить
        // уникальняа дата дублируется в скобках <>
        Container.BindFactory<ElementConfigItem, ElementPosition, Element, Element.Factory>().FromComponentInNewPrefab(elementPrefab);
        // Element - к какому классу относится
        // Element.Factory - кого используем в качестве фабрики
        // FromComponentInNewPrefab(elementPrefab) - какой префаб используем для создания фабрики

        
    }
}