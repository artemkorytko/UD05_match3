using System;
using Match3.Signals;
using UnityEngine;
using Zenject;

namespace Match3
{
    public class GameManager : IInitializable, IDisposable
    {
        // начисляется очков:
        private const int ValueCof = 10;
        private readonly SignalBus _signalBus;
        private readonly SaveToJson _saveToJson;
        private readonly BoardController _boardController;

        public int _coinsValue = 0;
        // ------------------------dataToSave----------------
        public DataToSave dataToSave;

        // конструктор
        public GameManager(SignalBus signalBus, BoardController boardController, SaveToJson saveToJson)
        {
            _saveToJson = saveToJson;
            _signalBus = signalBus;
            _boardController = boardController;
        }

        public void Initialize()
        {
            dataToSave = new DataToSave();
            
            // подписка  
            _signalBus.Subscribe<OnBoardMatchSignal>(OnBoardMatch);
            _signalBus.Subscribe<RestartButtonSignal>(DoRestart);
            _signalBus.Subscribe<SaveSignal>(SaveDataGM);

            _saveToJson.Initialize(_coinsValue);
        }

        private void SaveDataGM()
        {
            _saveToJson.SaveDataJSON();
        }

        private void DoRestart()
        {
            Debug.Log("Restaaaaaart - from GM!!!");
            _boardController.DoRestart();
            ResetScore();
        }

        
        public void Dispose()
        {
            _signalBus.Unsubscribe<OnBoardMatchSignal>(OnBoardMatch);
            _signalBus.Unsubscribe<RestartButtonSignal>(DoRestart);
            
            //################# предположим, что это срабатывает на выключении #######################################
            
            dataToSave.Points = _coinsValue;
            _signalBus.Fire(new SaveSignal(dataToSave));
            _signalBus.Unsubscribe<SaveSignal>(SaveDataGM);
        }

        
        
        
        private void OnBoardMatch(OnBoardMatchSignal signal)
        {
            // считанное из сигнала * вверху задали
            _coinsValue += signal.Value * ValueCof;
            _signalBus.Fire(new AddCoinsSignal(_coinsValue));
        }

        public void ResetScore()
        {
            Debug.Log(" Reset score ");
            _coinsValue = 0;
            // обновляет отображение очков
            _signalBus.Fire(new AddCoinsSignal(_coinsValue));
        }

        
    }
}