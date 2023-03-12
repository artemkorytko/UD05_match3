using System;
using DG.Tweening;
using Match3.Signals;
using TMPro;
using UnityEngine;
using Zenject;

// УНИВЕРСАЛЬНЫЙ ФАЙЛ ДЛЯ ТАКОГО СИГНАЛА В ПРОЕКТАХ

namespace Match3
{
    public class CoinsText : MonoBehaviour
    {
        // ссылки на сигналбас и текстовое поле
        private TextMeshProUGUI _text;
        private SignalBus _signalBus;
        private int _currentValue; // для анимации циферок

        // на вход получает сигналбас
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _signalBus.Subscribe<AddCoinsSignal>(UpdateText);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<AddCoinsSignal>(UpdateText);
        }

        private void UpdateText(AddCoinsSignal signal)
        {
            var target = signal.Value;
            
            // для анимации циферок в кол-ве очков:
            // с геттером ()=> в какое значение считываем, и сеттером  => в какое записываем
            // в OnUpdate обновляет....
            // To - анимриует виртуальное свойсмтво от начального до конечного
            // OnUpdate - задает обратный (что в скобках) вызов при каждом обновлении анимации
            DOTween.To(() => _currentValue,    x => _currentValue = x,    target,     0.5f).
                OnUpdate(() => _text.text = _currentValue.ToString());
        }


        public void MyUpdateText(int val)
        {
            _text.text = val.ToString();
        }


    }
}