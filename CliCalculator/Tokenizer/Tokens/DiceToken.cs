namespace CliCalculator.Tokenizer.Tokens
{
    public class DiceToken : IOperator
    {
        public DiceToken(bool isSingleDie)
        {
            Symbol = "d";
            Modificator = DiceModificator.None;
            IsSingleDie = isSingleDie;
        }

        public DiceToken(DiceModificator modificator)
        {
            Symbol = "d";
            Modificator = modificator;
        }

        public bool IsSingleDie { get; private set; }

        public TokenType Type => TokenType.Dice;

        public DiceModificator Modificator { get; set; }

        public OperatorType OperatorType { get; private set; }

        public string Symbol { get; private set; }

        public int Priority => 4;

        public override string ToString() => Symbol;
    }

    public enum DiceModificator
    {
        None,
        KeepHigh,
        KeepLow
    }
}
