namespace CliCalculator.Tokenizer.Tokens
{
    public class CloseParenthesisToken : IToken
    {
        public string Symbol => "(";

        public TokenType Type => TokenType.CloseParenthesis;

        public override string ToString()
        {
            return Symbol;
        }
    }
}
