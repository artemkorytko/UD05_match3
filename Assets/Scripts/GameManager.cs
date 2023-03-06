using System;
using Match3.Signals;
using Zenject;

namespace Match3
{
    public class GameManager : IInitializable, IDisposable
    {
        private const int ValueCof = 10;
        private readonly SignalBus _signalBus;

        private int _coinsValue;

        public GameManager(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<OnBoardMatchSignal>(OnBoardMatch);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<OnBoardMatchSignal>(OnBoardMatch);
        }

        private void OnBoardMatch(OnBoardMatchSignal signal)
        {
            _coinsValue += signal.Value * ValueCof;
            _signalBus.Fire(new AddCoinsSignal(_coinsValue));
        }
    }
}