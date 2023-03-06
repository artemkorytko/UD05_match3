using System;
using DG.Tweening;
using Match3.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace Match3
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
            _signalBus.Subscribe<AddCoinsSignal>(UpdateText);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<AddCoinsSignal>(UpdateText);
        }

        private void UpdateText(AddCoinsSignal signal)
        {
            var target = signal.Value;
            DOTween.To(() => _currentValue, x => _currentValue = x, target, 0.5f).OnUpdate(() => _text.text = _currentValue.ToString());
        }
    }
}