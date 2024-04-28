namespace CliCalculator.Tokenizer.Tokens
{
    public class DiceToken : IToken
    {
        public DiceToken()
        {
            Symbol = "d";
            Modificator = DiceModificator.None;
        }

        public DiceToken(DiceModificator modificator)
        {
            Symbol = "d";
            Modificator = modificator;
        }

        public TokenType Type => TokenType.Dice;

        public DiceModificator Modificator { get; set; }

        public OperatorType OperatorType { get; private set; }

        public string Symbol { get; private set; }

        public override string ToString() => Symbol;
    }

    public enum DiceModificator
    {
        None,
        KeepHigh,
        KeepLow
    }
}
