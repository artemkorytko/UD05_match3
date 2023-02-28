using Skripts;
using UnityEngine;
using Zenject;

public class ProjectMonoInstaler : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ProjectSetup>().AsSingle().NonLazy();
    }
}