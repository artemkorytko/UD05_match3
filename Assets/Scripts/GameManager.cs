using System;
using DefaultNamespace.SaveSystems;
using Match3;
using Match3.Signal;
using Zenject;

namespace DefaultNamespace
{
    public class GameManager : IInitializable, IDisposable
    {
        private const int ValueCof = 10;
        private int _currentCoins;
        
        private GameData _gameData;
        
        private readonly SignalBus _signalBus;
        private readonly BoardController _boardController;
        private readonly UIManager _uiManager;
        private readonly ISaveSystem _saveSystem;
        
        public GameManager(SignalBus signalBus, BoardController boardController, UIManager uiManager, ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _uiManager = uiManager;
            _signalBus = signalBus;
            _boardController = boardController;
        }

        public async void Initialize()
        {
            await _saveSystem.Initialize();
            
            _gameData = _saveSystem.GameData;
            _currentCoins = _gameData.Reward;
            
            _uiManager.SetPanel(Panel.Menu);
            _signalBus.Subscribe<OnBoardMatchSignal>(OnBoardMatch);
            _signalBus.Subscribe<StartGameButtonSignal>(OnStartGame);
        }

        public void Dispose()
        {
            _gameData.Reward = _currentCoins;
            _signalBus.Unsubscribe<OnBoardMatchSignal>(OnBoardMatch);
            _signalBus.Unsubscribe<StartGameButtonSignal>(OnStartGame);
            _saveSystem.SaveData();
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