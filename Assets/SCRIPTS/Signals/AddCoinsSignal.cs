namespace Match3.Signals
{
    //было  public class AddCoinsSignal
    public struct AddCoinsSignal
    {
        public readonly int Value;

        public AddCoinsSignal(int value)
        {
            // сколько начисляем очков инфа для каунтера, который в.... GM что ли? везде value я хз
            Value = value;
        }
    }
}