namespace CliCalculator.Tokenizer.Tokens
{
    public class OpenParenthesisToken : IToken
    {
        public string Symbol => "(";

        public TokenType Type => TokenType.OpenParenthesis;

        public override string ToString()
        {
            return Symbol;
        }
    }
}
