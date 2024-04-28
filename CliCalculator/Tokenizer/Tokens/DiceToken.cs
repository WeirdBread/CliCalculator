namespace CliCalculator.Tokenizer.Tokens
{
    public class DiceToken : IToken
    {
        public DiceToken()
        {
            Symbol = "d";
        }

        public TokenType Type => TokenType.Dice;

        public OperatorType OperatorType { get; private set; }

        public string Symbol { get; private set; }

        public override string ToString() => Symbol;
    }
}
