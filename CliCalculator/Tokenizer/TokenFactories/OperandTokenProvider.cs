using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Tokenizer.TokenFactories
{
    internal class OperandTokenProvider : ITokenProvider
    { 
        public Predicate<string> Predicate => (x) => double.TryParse(x, out var _);

        public IToken Provide(string token, params object[] args)
        {
            return new OperandToken(double.Parse(token));
        }
    }
}
