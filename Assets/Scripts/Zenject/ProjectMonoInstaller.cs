using Match3;
using UnityEngine;
using Zenject;
// его вешаем на префаб в папке Resourse (если нет его то сё прОпало.. создавай (Edit/Zenject/Create Project Context))
public class ProjectMonoInstaller : MonoInstaller  // как понял этот скрипт отработает самый 1первый 
{
    public override void InstallBindings()
    {
        Debug.Log("ProjectMonoInstaller");
        
        SignalBusInstaller.Install(Container);  // инстал SignalBusInstaller (для работы "событий") - только после этого появляется "сущность" SignalBus
        //нужно теперь получить его в конструкторе
        
        
        Container.BindInterfacesAndSelfTo<ProjectSetup>().AsSingle().NonLazy(); // AsSingle - объект только может быть один!
                                                                                // NonLazy - тип отключить Ленивую ссылку (объект создан сразу)
        
    }
}