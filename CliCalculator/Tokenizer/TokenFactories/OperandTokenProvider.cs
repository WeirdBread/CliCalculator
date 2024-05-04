using CliCalculator.Tokenizer.Tokens;
using System.Globalization;

namespace CliCalculator.Tokenizer.TokenFactories
{
    internal class OperandTokenProvider : ITokenProvider
    {
        private double value;
        public Predicate<string> Predicate => (x) => double.TryParse(x, NumberStyles.Any, CultureInfo.InvariantCulture, out value);

        public IToken Provide(string token, params object[] args)
        {
            return new OperandToken(value);
        }
    }
}
