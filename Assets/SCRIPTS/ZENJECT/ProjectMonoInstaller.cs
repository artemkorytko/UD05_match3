using Match3;
using UnityEngine;
using Zenject;

public class ProjectMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        
        // забиндить ProjectSetup
        // условности: AsSingle = объект может быть создан в единиченом экземпляре (аки синглтон)
        // без NonLazy объект будет создан только тогда, когда на него будет запрос
        // пытаться обратиться
        // NonLazy = объект будет создан сразу
        // было до наследования: Container.BindInterfacesAndSelfTo<ProjectSetup>().AsSingle().NonLazy();
        // стало после:
        Container.BindInterfacesAndSelfTo<ProjectSetup>().AsSingle().NonLazy();
        // теперь забиндится как объект (получит ссылки) так и интерфейсы которые наследуем и реализуем
        // включатся и будут работать
        
        
        Debug.Log(" конструктор Project Setup отработал йиху!"); // работает
    }
}