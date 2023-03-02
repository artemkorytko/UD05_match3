using Skripts;
using UnityEngine;
using Zenject;

public class ProjectMonoInstaler : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.BindInterfacesAndSelfTo<ProjectSetup>().AsSingle().NonLazy();
    }
}