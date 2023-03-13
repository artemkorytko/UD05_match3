using UnityEngine;

namespace Match3
{
    public class SaveSystemJson : ISaveSystem
    {
        private const string SAVE_KEY = "GameData";

        public void SaveData(GameData data)
        {
            PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(data));
        }

        public GameData LoadData()
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                return JsonUtility.FromJson<GameData>(PlayerPrefs.GetString(SAVE_KEY));
            }
            else
            {
                return new GameData();
            }
        }
    }
}