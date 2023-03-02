﻿using System;
using System.Linq;
using UnityEngine;

namespace Skripts
{
    [CreateAssetMenu(fileName = "ElementsConfig", menuName = "Configs/ElementsConfig", order = 0)]
    public class ElementsConfig : ScriptableObject
    {
        [SerializeField] private ElementConfigItem[] items;


        public ElementConfigItem[] Items => items;

        public ElementConfigItem GetItemByKey(string key)
        {
            return items.FirstOrDefault(item => item.Key == key);
        }
    }
    
    [Serializable]
    public class ElementConfigItem
    {
        [SerializeField] private string key;
        [SerializeField] private Sprite sprite;

        public string Key => key;

        public Sprite Sprite => sprite;
    }
}