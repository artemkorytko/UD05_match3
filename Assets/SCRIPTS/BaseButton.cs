using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
// благодаря этому коду больше НИКОГДА не пишем код для кнопок 
// кнопки просто создаем как RestartButtonSignal - RestartButton

namespace Match3
{
    // абстрактный класс
    // <T> тип дженерик                  where такое ибо все сигналы - структуры
    //                                   защита чтобы абы что не передать
    public abstract class BaseButton<T> : MonoBehaviour where T : struct
    {
        // ссылка на кнопку
        private Button _button;
        

        //===================================================================
        // вот это можно тырить из др файлов, или вывести в базу наследования
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        //==================== до сих ========================================

        
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            
            // подписываемся:
            _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            // вызывать
            _signalBus.Fire<T>();
        }
    }
}