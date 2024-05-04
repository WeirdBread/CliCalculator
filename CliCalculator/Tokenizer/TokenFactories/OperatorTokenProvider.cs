using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Tokenizer.TokenFactories
{
    internal class OperatorTokenProvider : ITokenProvider
    {
        public Predicate<string> Predicate => (x) => x.Length is 1 && OperatorToken.operatorSymbols.Contains(x[0]);

        public IToken Provide(string token, params object[] args)
        {
            return new OperatorToken(token[0]);
        }
    }
}
