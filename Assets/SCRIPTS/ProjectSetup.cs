using UnityEngine;
using Zenject;

// КЛАСС ОТВЕЧАЕТ ЗА НАСТРОЙКИ ПРОЕКТА
namespace Match3
{
    // C# класс монобеха с конструктором, куда автоматически передлалась ссылка
    // и автоматиески отработает метод Initialize - не вызывали его нигде!
    
    // тут наследуется от интерфейса дабы у DI (Dependence Injection) 
    // были такие же жизненные циклы как у всего в юнити
    public class ProjectSetup : IInitializable // ПКМ - Implement Interface (рус - осуществ, выполнить)
        // создаст внизу
        // Еще есть интерфейсы IDispoasble, ITickable (Update для инпута), IFixedTickable для физики...
        // НННО! шоб этот интерфейс заработал - его тоже надо забиндить в ProjectMonoInstaller:///
        
    {
        // чтобы появилась эта строчка - ПКМ "Introduce project settings"
        // выделив слово "settings" в конструкторе ниже
        private readonly ProjectSettings _settings;
        
        //----------- конструктор -------------------
        // чтобы получить ссылку на проджект сеттинги
        // надо создать конструктор, который будет запрашивать эту ссылку
        // а зенджект будет ходить по конструкторам и смотреть что они просят и раздавать
        // ждёт на вход ProjeсеSettings
        public ProjectSetup(ProjectSettings settings)
        // при работе кода в переменной settings будет ссылка на наш scriptable object
        // ссылка была передана автоматически в процессе биндинга класса в файле MonoInstaller
        // передалось в контейнер, тот проверил - есть ли у тебя конструктор?
        // "я прошу Project settingb" - Нна тебе их есть у нас!
        {
            _settings = settings;
        }

        
        // сеттинги применяются ктогда это класс будет создан и инициализирован
        // создать его надо в ProjectMonoInstaller
        public void Initialize()
        {
            // автоматиески отработает метод Initialize - не вызывали его нигде! 
            // его вызыввет Dependence Injection
            // в нем применяются сеттинги
            Application.targetFrameRate = _settings.TargetFps;
            Input.multiTouchEnabled = _settings.IsMultitouch;
            // ---- итог - произойдет установка нашего проекта

            Debug.Log(" Вызыван на старте метод Initialize в файле ProjectSetup "); // работает
        }
    }
}