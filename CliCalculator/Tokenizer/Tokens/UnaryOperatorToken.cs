namespace CliCalculator.Tokenizer.Tokens
{
    public class UnaryOperatorToken : IOperator
    {
        public UnaryOperatorToken()
        {
            Symbol = "u-";
        }

        public TokenType Type => TokenType.UnaryOperator;

        public OperatorType OperatorType { get; private set; }

        public string Symbol { get; private set; }

        public int Priority => 0;

        public override string ToString() => Symbol;
    }
}
