using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Tokenizer.TokenFactories
{
    public class ParenthesisTokenProvider : ITokenProvider
    {
        public Predicate<string> Predicate => (x) => x is "(" or ")";

        public IToken Provide(string token, params object[] args)
        {
            return token is "(" ? new OpenParenthesisToken() : new CloseParenthesisToken();
        }
    }
}
