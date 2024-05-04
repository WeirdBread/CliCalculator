using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Tokenizer.TokenFactories
{
    internal class OperatorTokenProvider : ITokenProvider
    {
        public Predicate<string> Predicate => (x) => x.Length is 1 && MathOperatorToken.operatorSymbols.Contains(x[0]);

        public IToken Provide(string token, params object[] args)
        {
            return new MathOperatorToken(token[0]);
        }
    }
}
