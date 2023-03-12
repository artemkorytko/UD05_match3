using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Match3.Signals;
using Zenject;


namespace Match3
{
    [Serializable]
    public class SaveToJson //: ISaveSystem
    {
        // ключ для PlayerPrefs
        private const string SAVE_KEY = "SAVE_MATCH3";

        // ссылка на GameData
        // private DataToSave => DataToSave;
        private int PointsToSave;

        // private readonly CoinsText _coinsText;
        private readonly SignalBus _signalBus;
        // private readonly BoardConfig _boardConfig;
        // private readonly GameManager _gameManager;

        // public DataToSave PubDataToSave { get;  } 
        // конструктор 
        
        
        // ----------------------------------------
        private DataToSave dataToSave;
        public DataToSave DataToSave => dataToSave;
        // ----------------------------------------

        // вот это я не понимаю что - типо инкапсуляция для ZenJect c перечислением кого упоминаем в файле???
        public SaveToJson(SignalBus signalBus) // CoinsText coinsText)// GameManager gameManager)
        {
            _signalBus = signalBus;
            // _coinsText = coinsText;
            //_gameManager = gameManager;
        }


        // вынесли ее в публичное поле заинкапуслировавши:
        // public DataToSave _PublDataToSave => PublDataToSave;
        // private static string Path = Application.persistentDataPath + "/saveData.data";

        public UniTask Initialize(int defaultValue)
        {
            //###################### типо подписалась на сигнал #################################################
            // _signalBus.Subscribe<SaveSignal>();

            // PointsToSave = value;

            // проверяем, если мы уже что-то по этому пути сохранили
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                // если нету, то тогда загружаем
                LoadData();
            }

            else // там нету ничего, то создать пустую дату с нуля
            {
                PointsToSave = defaultValue;
                // DataToSave = new DataToSave();
                // заиниченная дата
            }

            return UniTask.CompletedTask;
        }


        //-------------------------- достаем и приводим к нашему классу ------------------------------------------
        public UniTask<bool> LoadData()
        {
            // "десериализовать"
            // получаем строку и записываем в стринг
            // string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            

            // "парсить"
            // достать из джейсона. Указать в какой тип привести - тут надо распарсить в GameData
            // было - DataToSave = JsonUtility.FromJson<DataToSave>(jsonData);
            //PointsToSave = JsonUtility.FromJson<int>(jsonData);

            Debug.Log($" Загрузка из JSON...");
            
            // FIRE ???
            _signalBus.Fire(new AddCoinsSignal(PointsToSave));  // не работает :/
            
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            dataToSave = JsonUtility.FromJson<DataToSave>(jsonData);
            return new UniTask<bool>(true);
        }


        //--------------------------- сохраняем -------------------------------------------------------------------
        
        
        
        public void SaveDataJSON()
        {
            Debug.Log($" Something has been saved on quit {dataToSave.Points}");

            string jsonData = JsonUtility.ToJson(dataToSave);
            PlayerPrefs.SetString(SAVE_KEY, jsonData);
        }


        //--------------------------- для полного ресета накликанного ----------------------------------------------
        
        public void ResetSaved()
        {
            dataToSave.Points = 0;
            
            SaveDataJSON();
        }
        
        private void Dispose()
        {
            //###################### типо отписалась от сигнала #################################################
            // _signalBus.Unsubscribe<SaveSignal>();
        }
    }
}