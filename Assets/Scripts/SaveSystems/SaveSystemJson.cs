﻿using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace.SaveSystems
{
    public class SaveSystemJson : ISaveSystem
    {
        private const string SAVE_KEY = "SaveSystemJson";
        private GameData _gameData;
        public GameData GameData => _gameData;

        public UniTask Initialize()
        {
            if(PlayerPrefs.HasKey(SAVE_KEY))
                LoadData();
            else
                _gameData = new GameData();
            
            return UniTask.CompletedTask;
        }

        public void SaveData()
        {
            string jsonData = JsonUtility.ToJson(_gameData);
            PlayerPrefs.SetString(SAVE_KEY, jsonData);
        }

        public UniTask<bool> LoadData()
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            _gameData = JsonUtility.FromJson<GameData>(jsonData);

            return new UniTask<bool>(true);
        }
    }
}