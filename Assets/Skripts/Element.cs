using System;
using Skripts.Signals;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Skripts
{
    public class Element : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<ElementConfigItem, ElementPosition, Element>{}

        [SerializeField] private SpriteRenderer bgSpriteRenderer;
        [SerializeField] private SpriteRenderer iconSpriteRenderer;
        
        
        private ElementConfigItem _configItem;
        private ElementPosition _elementPosition;

        private Vector2 _localPosition;
        private Vector2 _gridPosition;
        private SignalBus _signalBus;

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

        public void Initialized()
        {
            _localPosition = _elementPosition.LocalPosition;
            _gridPosition = _elementPosition.GridPosition;
            SetConfig();
            SetLocalPosition();
            Enabled();

        }
        
        private void SetConfig()
        {
            iconSpriteRenderer.sprite = _configItem.Sprite;
            
        }
        
        private void SetLocalPosition()
        {
            transform.localPosition = _localPosition;
        }

        public void SetLocalPosition(Vector2 localPosition, Vector2 gridPosition)
        {
            _localPosition = localPosition;
            _gridPosition = gridPosition;
            SetLocalPosition();
        }
        
        private void Enabled()
        {
            gameObject.SetActive(true);
            IsActive = true;
            SetSelected(false);
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

        public void Disabled()
        {
            IsActive = false;
            gameObject.SetActive(false);
        }
    }
}