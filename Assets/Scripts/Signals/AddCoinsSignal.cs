namespace Match3.Signals
{
    public struct AddCoinsSignal
    {
        public readonly int Value;

        public AddCoinsSignal(int value)
        {
            Value = value;
        }
    }
}