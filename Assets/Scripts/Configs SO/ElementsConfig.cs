using System;
using System.Linq;
using UnityEngine;

namespace Match3
{
    [CreateAssetMenu(fileName = "ElementsConfig", menuName = "Configs/ElementsConfig", order = 0)]
    public class ElementsConfig : ScriptableObject
    {
        [SerializeField] private ElementConfigItem[] items; // здеся заполняем класс что ниже(в инспекторе)

        public ElementConfigItem[] Items => items;

        public ElementConfigItem GetItemByKey(string key) // эт просто проверка, что будет объект создан можно его рип сделать. (артем пропагандирует нада делать в so)
        {
            return items.FirstOrDefault(item => item.Key == key);
        }
    }

    [Serializable]  // отобразить в инспекторе 
    public class ElementConfigItem // проста класс для хранения ключа и вьюшки
    {
        [SerializeField] private string key;
        [SerializeField] private Sprite sprite;

        public string Key => key;

        public Sprite Sprite => sprite;
    }
    // после создания КОНфига нужно его передать в GameConfigInstaller

}