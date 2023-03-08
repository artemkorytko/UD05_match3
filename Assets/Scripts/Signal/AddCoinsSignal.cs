namespace Match3.Signal
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