using Match3.Signals;
using Zenject;

public class SignalInstaller : Installer<SignalInstaller>
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<OnElementClickSignal>();
    }
}