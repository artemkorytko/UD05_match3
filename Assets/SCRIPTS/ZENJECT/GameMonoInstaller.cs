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
        
        // ДЗ ДОПИСАТЬ СЮДОЙ ------------
        // будет биндиться класс внутрь интерфейса
        // почитать ка кбиндить классы в интерфейсы 
        //Container .Bind<SaveSystemWithJson>().To<ISaveSystem>();|
        
        Container .BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();

        //######################## хз надо это тут или нет, но Null  Reference ошибка #################################
        // без этого черный экран в игре:
        Container.BindInterfacesAndSelfTo<SaveToJson>().AsSingle().NonLazy();
        
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