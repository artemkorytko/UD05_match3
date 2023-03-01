using Match3.Signals;
using Zenject;

namespace Match3.Zenject
{
    public class SignalsInstaller : Installer<SignalsInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<OnElementClickSignal>();
        }
    }
}