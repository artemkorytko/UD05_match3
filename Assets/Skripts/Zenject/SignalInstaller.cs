using Skripts.Signals;
using Zenject;

namespace Skripts.Zenject
{
    public class SignalInstaller : Installer<SignalInstaller>

    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<OnElementClickSignal>();
        }
    }
}