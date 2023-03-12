namespace Match3.Signals
{
    public struct OnBoardMatchSignal
    {
        public readonly int Value;

        public OnBoardMatchSignal(int value)
        {
            Value = value;
        }
    }
}