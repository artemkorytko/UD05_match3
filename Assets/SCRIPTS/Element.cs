using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.Signals;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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

        // ссылки на картинки из префаба
        [SerializeField] private SpriteRenderer bgSpriteRenderer;
        [SerializeField] private SpriteRenderer iconSpriteRenderer;
        [SerializeField] private ParticleSystem _myParticles;
        [SerializeField] private Light2D _gemLight;

        // сохраняем себе ссылку которую получим из кконтейнера
        private ElementConfigItem _configItem;
        private ElementPosition _elementPosition;
        private SignalBus _signalBus;

        // распарсить информацию из element position в локальные переменные шоб с ним работать
        private Vector2 _localPosition; 
        private Vector2 _gridPosition;

        // гридовкую позицию на сцене будем читать снаружи поэтому инкапсулируем:
        public Vector2 GridPosition => _gridPosition;
        
        // инкапуслируем конфиг наружу:
        public ElementConfigItem ConfigItem => _configItem;

        // для установки активная ли брюлина, это для анимации обычная пропертя
        public bool IsActive { get; private set; }  // считать откуда угодно а задать только внутри

        private const int PART_ALPHA = 100;
        private int _colorR;
        private int _colorG;
        private int _colorB;

        //--------------- inject МОНОБЕХУ ------------------------------------------------------------
        // В Монобехе нельзя делать конструкторы но у ZenJect есть на этот случай спец метод Construct
        // на вход просит элемент ConfigItem
        [Inject] // аттрибут дабы конструктор заработал - требует инъекцию зависимости и контейнер передаст сюдой
        // вон там используется структура с позицией и пр
        // signalBus был подключен в ProjMonoinstaller
        public void Construct(ElementConfigItem configItem, ElementPosition elementPosition, SignalBus signalBus)
        {
            _configItem = configItem; // локальные переменные 
            _elementPosition = elementPosition; // ссылки на них
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            // типо две позиции, логика и видимая
            _localPosition = _elementPosition.LocalPosition; // в ElementPosition структуре инфа берется из... логики?
            _gridPosition = _elementPosition.GridPosition; // получили вторую позицию
            
            // применяем конфиги и локал позицию:
            SetConfig();
            SetLocalPosition();
            // сделать иконку активной
            Enable().Forget();
        }

        public void SetEffectsColor()
        {
            _colorR = _configItem.ColorR;
            _colorG = _configItem.ColorG;
            _colorB = _configItem.ColorB;

            _gemLight.color = new Color(_colorR, _colorG, _colorB, PART_ALPHA);
        }

        public void Update()
        {
            var mainXXX = _myParticles.main; // без этого не работает
            mainXXX.startColor = new Color(_colorR, _colorG, _colorB, PART_ALPHA);
        }

        //--------------------------------------------------------------------------------
        private void SetConfig()
        {
            // устанавливаем иконку, какую передали из конфига
           iconSpriteRenderer.sprite = _configItem.Sprite;
        }
        
        public void SetConfig(ElementConfigItem configItem)
        {
            _configItem = configItem;
            SetConfig();
        }

        private void SetLocalPosition()
        // async UniTask SetLocalPosition()
        {
            // тут анимация наверное
            transform.localPosition = _localPosition;
        }

        // два одинаковых, но второй public - ыыыы 
        // в этот приходит когда надо поменять местами из борд контроллера
        // public async UniTask SetLocalPosition(Vector2 localPosition, Vector2 gridPosition)
        public void SetLocalPosition(Vector2 localPosition, Vector2 gridPosition)
        {
            _localPosition = localPosition; // старое равно чему пришло
            _gridPosition = gridPosition; 
            // await SetLocalPosition();
            SetLocalPosition(); // метод на перемещение САМ В СЕБЕЕ или верхний ??? О_о
        }

        public async UniTask Enable()
        {
            gameObject.SetActive(true);
            IsActive = true;
            SetSelected(false);
            transform.localScale = Vector3.zero;
            await transform.DOScale(Vector3.one, 0.5f);
        }

        // метод ВКЛ/ВЫКЛ ЗАДНИК 
        public void SetSelected(bool isOn)
        {
            bgSpriteRenderer.enabled = isOn; // прикоооол классно! хочу так же у зайца
            
            if (isOn )
            {
                SetEffectsColor();
                _myParticles.Play();
                _gemLight.enabled = true;
            }
            else {_myParticles.Stop(); _gemLight.enabled = false;}
        }

        private void OnMouseUpAsButton() //!!!!!!!!!!!!!!!!! помни - если есть коллайдер
                                         //BoxCollider2D на родительском элементе висит у префаба
        
        {
            OnClick();
        }

        private void OnClick() // паттерн EventBUS 
        {
            // сигналбас, вызови событие - сигналы в отдельной папке скриптов Signals
            // "зафайрить"
            // создаем новый экземпляр, кидаем в signalBus, кто подписан тот получит когда вызовут
            _signalBus.Fire(new OnElementClickSignal(this));
            // шоб рассказать системе что есть такой сигнал надо в GameMonoInstaller
        }

        
        public async UniTask Disable() // выззывается бордой когда набрали массив собранных 
        {
            IsActive = false;
            
            // скейл к нулю
            await transform.DOScale(Vector3.zero, 0.5f);
            gameObject.SetActive(false); // мы его переиспользуем
        }
    }
}

