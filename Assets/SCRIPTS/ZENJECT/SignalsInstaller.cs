using Match3.Signals;
using Zenject;
// ОТДЕЛЬНЫЙ ИНСТАЛЛЕР ДЛЯ СИГНАЛОВ
// его надо тоже добавить в MonoInstaller


public class SignalsInstaller : Installer<SignalsInstaller>
// наследуется от стандартного инсталлера зенджекта передается тип
{
    // раз наследуемся - обязательно реализовать метод:
    public override void InstallBindings()
    {
        // рассказать системе что есть такой сигнал
        // все бинды сигналов надо вынести в отдельный инсталлер
        // сначала писали в Monoinstaller
        Container.DeclareSignal<OnElementClickSignal>();
        Container.DeclareSignal<OnBoardMatchSignal>();
        Container.DeclareSignal<AddCoinsSignal>();
        Container.DeclareSignal<RestartButtonSignal>();
        Container.DeclareSignal<SaveSignal>();
    }
}