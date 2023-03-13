using System;
using Match3.Signals;
using Zenject;

namespace Match3
{
    public class GameManager : IInitializable, IDisposable
    {
        private const int ValueCof = 10;
        private readonly SignalBus _signalBus;
        private readonly ISaveSystem _saveSystem;

        private int _coinsValue = -1;
        private GameData _gameData;

        private int Score
        {
            get => _coinsValue;
            set
            {
                if (_coinsValue == value)
                    return;
                _coinsValue = value;
                _signalBus.Fire(new AddCoinsSignal(_coinsValue));
            }
        }

        public GameManager(SignalBus signalBus, ISaveSystem saveSystem)
        {
            _signalBus = signalBus;
            _saveSystem = saveSystem;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<OnBoardMatchSignal>(OnBoardMatch);
            _signalBus.Subscribe<StartGameSignal>(OnGameStart);
        }

        private void OnGameStart()
        {
            _gameData = _saveSystem.LoadData();
        }

        public void Dispose()
        {
            _saveSystem.SaveData(_gameData);
            _signalBus.Unsubscribe<StartGameSignal>(OnGameStart);
            _signalBus.Unsubscribe<OnBoardMatchSignal>(OnBoardMatch);
        }

        private void OnBoardMatch(OnBoardMatchSignal signal)
        {
            Score += signal.Value * ValueCof;
        }
    }
}