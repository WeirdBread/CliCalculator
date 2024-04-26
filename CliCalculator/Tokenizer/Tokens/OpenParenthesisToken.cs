namespace CliCalculator.Tokenizer.Tokens
{
    public class OpenParenthesisToken : IToken
    {
        //public OpenParenthesisToken(bool isNegative)
        //{
        //    this.IsNegative = isNegative;
        //}

        public string Symbol => "(";

        public TokenType Type => TokenType.OpenParenthesis;

        //public bool IsNegative;

        public override string ToString()
        {
            return Symbol;
        }
    }
}
