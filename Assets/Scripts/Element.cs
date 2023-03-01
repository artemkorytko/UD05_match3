using Match3.Signals;
using UnityEngine;
using Zenject;

namespace Match3
{
    public class Element : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<ElementConfigItem, ElementPosition, Element>{}

        [SerializeField] private SpriteRenderer bgSpriteRender;
        [SerializeField] private SpriteRenderer iconSpriteRender;
        
        private ElementConfigItem _configItem;
        private ElementPosition _elementPosition;
        

        private SignalBus _signalBus;
        private Vector2 _localPosition;
        private Vector2 _gridPosition;

        public Vector2 gridPosition => _gridPosition;

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
            Enable();
        }

        private void SetConfig()
        {
            iconSpriteRender.sprite = _configItem.Sprite;
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
        
        private void Enable()
        {
            gameObject.SetActive(true);
            IsActive = true;
            SetSelected(false);
        }

        public void SetSelected(bool isOn)
        {
            bgSpriteRender.enabled = isOn;
        }

        private void OnMouseUpAsButton()
        {
            OnClick();
        }

        private void OnClick()
        {
            _signalBus.Fire(new OnElementClickSignal(this));
        }

        public void Disable()
        {
            IsActive = false;
            gameObject.SetActive(false);
        }
    }
    
    
}