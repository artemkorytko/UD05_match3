using System;
using Match3;
using Match3.Signal;
using Zenject;

namespace DefaultNamespace
{
    public class GameManager : IInitializable, IDisposable
    {
        private const int ValueCof = 10;
        private int _currentCoins;
        
        private readonly SignalBus _signalBus;
        private readonly BoardController _boardController;
        private readonly UIManager _uiManager;

        public GameManager(SignalBus signalBus, BoardController boardController, UIManager uiManager)
        {
            _uiManager = uiManager;
            _signalBus = signalBus;
            _boardController = boardController;
        }

        public void Initialize()
        {
            _uiManager.SetPanel(Panel.Menu);
            _signalBus.Subscribe<OnBoardMatchSignal>(OnBoardMatch);
            _signalBus.Subscribe<StartGameButtonSignal>(OnStartGame);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<OnBoardMatchSignal>(OnBoardMatch);
            _signalBus.Unsubscribe<StartGameButtonSignal>(OnStartGame);
        }

        private void OnStartGame()
        {
            _uiManager.SetPanel(Panel.Game);
            _boardController.StartGame();
        }

        private void OnBoardMatch(OnBoardMatchSignal signal)
        {
            _currentCoins += signal.Value * ValueCof; // Value  - это публичное поле в OnBoardMatchSignal!!!!
            _signalBus.Fire(new AddCoinsSignal(_currentCoins));
        }
    }
}