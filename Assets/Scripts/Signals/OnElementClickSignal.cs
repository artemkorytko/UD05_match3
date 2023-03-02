namespace Match3.Signals
{
    public struct OnElementClickSignal
    {
        public readonly Element Element;

        public OnElementClickSignal(Element element)
        {
            Element = element;
        }
    }
}