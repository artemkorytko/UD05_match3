using System;
using DG.Tweening;
using Match3.Signal;
using TMPro;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class CoinsText : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private SignalBus _signalBus;
        private int _currentValue;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _signalBus.Subscribe<AddCoinsSignal>(UpdateCoin);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<AddCoinsSignal>(UpdateCoin);
        }

        private void UpdateCoin(AddCoinsSignal signal)
        {
            var target = signal.Value;// Value  - это публичное поле в AddCoinsSignal!!!!
            DOTween.To(() => _currentValue, x => _currentValue = x, target, 0.5f).OnUpdate(() => _text.text = _currentValue.ToString());
        }
    }
}