using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.Signals;
using UnityEngine;
using Zenject;

namespace Match3
{
    public class Element : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<ElementConfigItem, ElementPosition, Element>
        {
        }

        [SerializeField] private SpriteRenderer bgSpriteRenderer;
        [SerializeField] private SpriteRenderer iconSpriteRenderer;

        private ElementConfigItem _configItem;
        private ElementPosition _elementPosition;
        private SignalBus _signalBus;

        private Vector2 _localPosition;
        private Vector2 _gridPosition;

        public Vector2 GridPosition => _gridPosition;
        public ElementConfigItem ConfigItem => _configItem;

        public bool IsActive { get; private set; }

        [Inject]
        public void Construct(ElementConfigItem configItem, ElementPosition elementPosition, SignalBus signalBus)
        {
            _configItem = configItem;
            _elementPosition = elementPosition;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _localPosition = _elementPosition.LocalPosition;
            _gridPosition = _elementPosition.GridPosition;
            SetConfig();
            SetLocalPosition();
            Enable().Forget();
        }

        private void SetConfig()
        {
            iconSpriteRenderer.sprite = _configItem.Sprite;
        }

        private void SetLocalPosition()
        {
            transform.localPosition = _localPosition;
        }

        public void SetConfig(ElementConfigItem configItem)
        {
            _configItem = configItem;
            SetConfig();
        }

        public void SetLocalPosition(Vector2 localPosition, Vector2 gridPosition)
        {
            _localPosition = localPosition;
            _gridPosition = gridPosition;
             SetLocalPosition();
        }

        public async UniTask Enable()
        {
            gameObject.SetActive(true);
            IsActive = true;
            SetSelected(false);
            transform.localScale = Vector3.zero;
            await transform.DOScale(Vector3.one, 0.5f);
        }

        public void SetSelected(bool isOn)
        {
            bgSpriteRenderer.enabled = isOn;
        }

        private void OnMouseUpAsButton()
        {
            OnClick();
        }

        private void OnClick()
        {
            _signalBus.Fire(new OnElementClickSignal(this));
        }

        public async UniTask Disable()
        {
            IsActive = false;
            await transform.DOScale(Vector3.zero, 0.5f);
            gameObject.SetActive(false);
        }
    }
}