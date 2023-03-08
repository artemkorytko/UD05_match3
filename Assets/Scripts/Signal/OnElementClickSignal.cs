namespace Match3.Signal
{
    public struct OnElementClickSignal
    {
        public readonly Element Element; // элемент который мы будем хранить здеся
// readonly значит что он будет инициализирован в конструкторе 
        public OnElementClickSignal(Element element)
        {
            Element = element;
        }
    }
}