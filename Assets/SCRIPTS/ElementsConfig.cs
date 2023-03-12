using System;
using System.Linq;
using UnityEngine;

// КОНФИГ ШОБ ХРАНИТЬ РАЗНЫЕ КАРТИНКИ ДЛЯ ЭЛЕМЕНТОВ РАЗНОГО ЦВЕТА
// есть визуал
// и есть логический объект которые тоже будут разные


namespace Match3
{
    //массив видов наших объектов
    [CreateAssetMenu(fileName = "ElementsConfig", menuName = "Configs/ElementsConfig", order = 0)]
    public class ElementsConfig : ScriptableObject
    {
        // заводим настраиваемый из редактора конфиг
        // маасив инкапсулируем alt-enter read only
        [SerializeField] private ElementConfigItem[] items;

        
        // инкапсуляция массива
        public ElementConfigItem[] Items => items;

        // АК обычно сразу создает публичный метод который возвращает элемент конфига по какому-то запросу
        // nen по запросу логического ключа
        public ElementConfigItem GetItemByKey(string key)
        {
            // возвращает LINQ перебор коллекции - дай первый у кого ключ как мы передали в скобках
            // если не найдет вернят null
            return items.FirstOrDefault(item => item.Key == key);
        }
    }

    //----------------- отдельный класс ----------------------------------
    // связка картинка + ЛОГИЧЕСКИЙ айдишник ключ 
    [Serializable] // шоб настраивать в редакторе
    public class ElementConfigItem
    {
        [SerializeField] private string key; // ключ
        [SerializeField] private Sprite sprite; // картинка

        [SerializeField] private int colorR;
        [SerializeField] private int colorG;
        [SerializeField] private int colorB;
        public string Key => key;

        public Sprite Sprite => sprite;

        public int ColorR => colorR;

        public int ColorG => colorG;

        public int ColorB => colorB;
    }
    
    
}