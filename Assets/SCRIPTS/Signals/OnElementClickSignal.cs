using UnityEditor;

namespace Match3.Signals
{
    
    // сигналы это дата контейнеры - СТРУКТУРА
    public struct OnElementClickSignal
    {
       // именно тот ПкфзршсыОщиЬщву которому кликнули
        public readonly Element Element;

        // инициализация через конструктор раз рид онли
        public OnElementClickSignal(Element element)
        {
            Element = element;
        }
    }
}