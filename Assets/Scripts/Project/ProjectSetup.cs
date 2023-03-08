using System;
using UnityEngine;
using Zenject;

namespace Match3 
{
// - гэта обычный класс c# 
    public class ProjectSetup : IInitializable, IDisposable, IFixedTickable, ITickable, ILateTickable 
    // чтоб эти дяди(интерфейсы) работали надА их забилдить в ProjectMonoInstaller -> Container.BindInterfacesAndSelfTo<ProjectSetup>90.что-то там
    {
        private readonly ProjectSettingsConfig _settingsConfig; 

        public ProjectSetup(ProjectSettingsConfig settingsConfig) //(важно!!!) zenject ползает по констуктарам и передает ссылки(важно!!!), по этому все создоваемые ссылки в конструктор
        {
            _settingsConfig = settingsConfig;
        }

        public void Initialize() /*IInitializable(интерфейс Zenject) весело делает Awake ци Start?      - все вызывает DI(диспенсер инжект)   */ 
        {
            Application.targetFrameRate = _settingsConfig.TargetFps; // устанавливаем fps
            Input.multiTouchEnabled = _settingsConfig.IsMultitouch; // проверяем наличие "тача"
        }

        public void Dispose() /*IDisposable весело делает Дестрой  */
        {
            //цикава
        }

        public void FixedTick()/*IFixedTickable весело делает Фиксет апдейт  */
        {
            //цикава
        }

        public void Tick()/*ITickable весело делает Апдейт  тута можно считывать Input(нажатия)*/
        {
            //цикава
            //Debug.Log("Ай эм ProjectSetup энд зЫс май Апдейт ыыыыы");
        }

        public void LateTick()/*ILateTickable весело делает Лейт апдейт */
        {
            //цикава
        }
    }
}