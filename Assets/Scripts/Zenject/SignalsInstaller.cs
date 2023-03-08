using Match3.Signal;
using Zenject;

namespace Match3.Zenject
{
    public class SignalsInstaller : Installer<SignalsInstaller>  // c# класс который занимается только слаживанием событий в "бус"
    { 
        public override void InstallBindings()
        {
            Container.DeclareSignal<OnElementClickSignal>(); // слаживаем в контейнер 
            Container.DeclareSignal<OnBoardMatchSignal>(); // слаживаем в контейнер 
            Container.DeclareSignal<AddCoinsSignal>(); // слаживаем в контейнер 
            Container.DeclareSignal<RestartButtonSignal>(); // слаживаем в контейнер 
            Container.DeclareSignal<StartGameButtonSignal>(); // слаживаем в контейнер 
        }
    }
}