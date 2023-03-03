using System;
using Match3.Signals;
using UnityEngine;
using Zenject;

// МОНОБЕХ - этот класс будет создаваться на сцене из кода и 
// будет получать на вход инфу о том, какой он объект и где находится
// для отдельно взятого брюлика ----> инфа должна быть уникальна
// поэтому паттерн Фабрика, идея: получаем экземпляр объекта согласно данным
// этот паттерн прилагается к зенджекту

namespace Match3
{
    public class Element : MonoBehaviour
    {
        // паттерн фабрика в зенджекте, наследуется от - ну вот так надо
        // в скобках < > дата котрая нам надо 
        // фабрика является частью этого элемента
        // объявили фабрику для которой надо уникальный ElementConfigItem, кому она принадлежит - Element
        // если создать вне то не поймешь чья она
        // фабрику для создания надо забиндить в GameMonoInstaller <----!!!!
        // публичная шоб биндилась
        public class Factory : PlaceholderFactory<ElementConfigItem, ElementPosition, Element>
        {
            // тут пусто, это ок
        }

        [SerializeField] private SpriteRenderer bgSpriteRenderer;
        [SerializeField] private SpriteRenderer iconSpriteRenderer;

        // сохраняем себе ссылку которую получим из кконтейнера
        private ElementConfigItem _configItem;
        private ElementPosition _elementPosition;
        private SignalBus _signalBus;

        private Vector2 _localPosition;
        private Vector2 _gridPosition;

        public Vector2 GridPosition => _gridPosition;
        public ElementConfigItem ConfigItem => _configItem;

        public bool IsActive { get; private set; }  // считать откуда угодно а задать только внутри

        //--------------- inject МОНОБЕХУ ------------------------------------------------------------
        // В Монобехе нельзя делать конструкторы но у ZenJect есть на этот случай спец метод Construct
        // на вход просит элемент ConfigItem
        [Inject] // аттрибут дабы конструктор заработал - требует инъекцию зависимости и контейнер передаст сюдой
        // вон там используется структура с позицией и пр
        public void Construct(ElementConfigItem configItem, ElementPosition elementPosition, SignalBus signalBus)
        {
            _configItem = configItem;
            _elementPosition = elementPosition; // ссылки на них
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

        private void Enable()
        {
            gameObject.SetActive(true);
            IsActive = true;
            SetSelected(false);
        }

        public void SetSelected(bool isOn)
        {
            bgSpriteRenderer.enabled = isOn;
        }

        private void OnMouseUpAsButton() //!!!!!!!!!!!!!!!!! помни - если есть коллайдер
                                         //BoxCollider2D на родительском элементе висит у префаба
        
        {
            OnClick();
        }

        private void OnClick() // паттерн EventBUS 
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

