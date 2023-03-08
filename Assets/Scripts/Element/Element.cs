using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.Signal;
using UnityEngine;
using Zenject;

namespace Match3
{
    public class Element : MonoBehaviour 
    {
        // класс в классе это лепш можно создавать в разных классах  еще фабрики при ображении к этой фабрике нада писать Element.Factory<>
        public class Factory : PlaceholderFactory<ElementConfigItem, ElementPosition, Element>{} // вот сломай голову.. заводской функционал(Zenject) паттерна ФАБРИКА....
// що там происходит : указываем (передаем) шо нам тут нада (<ElementConfigItem, ElementPosition, Element>)
// ФАБРИКУ ТОЖ НАДА БИЛДИТЬ ДЛЯ ЕЕ СОЗДАНИЯ (в GameMonoInstaller, т.к это относиться к игре)
        [SerializeField] private SpriteRenderer bgSpriteRenderer;
        [SerializeField] private SpriteRenderer iconSpriteRenderer;

        private ElementConfigItem _configItem; // конфиг 
        private ElementPosition _elementPosition; // гэта позиция нашего элемента на барде (структура) (просто передает информацию)
        private SignalBus _signalBus;
        
        private Vector2 _localPosition; // беруться из структуры ElementPosition
        private Vector2 _gridPosition;
        
        public Vector2 GridPosition => _gridPosition;
        public ElementConfigItem ConfigItem => _configItem;
        public bool IsActive { get; private set; }

        [Inject]
        public void Construct(ElementConfigItem configItem, ElementPosition elementPosition, SignalBus signalBus)
        { //как бы гэта конструктор (в монобехе нельзя делать конструкторы, но вось [Inject] - без его никак(спутникВ))
             _configItem = configItem;
             _elementPosition = elementPosition;
            _signalBus = signalBus;
            // получаем из контейнера все нужные нам объекты
        }

        public void Initialize()  // инит в борде (цикл в цикле)
        {
            _localPosition = _elementPosition.LocalPosition;
            _gridPosition = _elementPosition.GridPosition;

            SetConfig(); // устанавливаем иконку
            SetLocalPosition().Forget(); // устанавливаем позицию
            Enable().Forget(); // вкл выкл бэка
        }

        public async UniTask Enable()
        {
            gameObject.SetActive(true);
            IsActive = true;
            SetSelected(false);
            
            transform.localScale = Vector3.zero;
            await transform.DOScale(Vector3.one, 0.5f);
        }

        private async UniTask SetLocalPosition()
        {
           await transform.DOLocalMove(_localPosition, 0.5f); // тут эвейт добавил все медленно но норм. работает.. 
            //transform.localPosition = _localPosition;
        }

        public async UniTask SetLocalPosition(Vector2 local, Vector2 grid)
        {
            _localPosition = local;
            _gridPosition = grid;
            await SetLocalPosition();
        }

        private void SetConfig()
        {
            iconSpriteRenderer.sprite = _configItem.Sprite;
        }

        public void SetConfig(ElementConfigItem configItem)
        {
            _configItem = configItem;
            SetConfig();
        }

        public void SetSelected(bool isOn)
        {
            bgSpriteRenderer.enabled = isOn;
        }

        private void OnMouseUpAsButton() // метод юнити клик по ему 
        {
            OnClick();
        }

        private void OnClick() // гэта тип "событие"   Invoke ? ну тип да.... 
        {
            _signalBus.Fire(new OnElementClickSignal(this)); // евент бас(это просто "склад" событиый) инстанс его происходит в ProjectMonoInstaller
            //Fire - эт вызвать событие (за файрить событие нужно их создавать как отдельный объект (он обычный класс C# (структура))) 
            //this передаем себя того по кому мы кликнули
        }

        public async UniTask Disable()
        {
            IsActive = false;
            await transform.DOScale(Vector3.zero, 0.5f);
            gameObject.SetActive(false);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}