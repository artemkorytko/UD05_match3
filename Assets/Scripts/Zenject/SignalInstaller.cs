using Match3.Signals;
using Zenject;

public class SignalInstaller : Installer<SignalInstaller>
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<OnElementClickSignal>();
        Container.DeclareSignal<OnBoardMatchSignal>();
        Container.DeclareSignal<AddCoinsSignal>();
        Container.DeclareSignal<RestartButtonSignal>();
    }
}