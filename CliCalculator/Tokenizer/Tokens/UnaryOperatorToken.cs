namespace CliCalculator.Tokenizer.Tokens
{
    public class UnaryOperatorToken : IToken
    {
        public UnaryOperatorToken()
        {
            Symbol = "-";
        }

        public TokenType Type => TokenType.UnaryOperator;

        public OperatorType OperatorType { get; private set; }

        public string Symbol { get; private set; }

        public override string ToString() => Symbol;
    }
}
