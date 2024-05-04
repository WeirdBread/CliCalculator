using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Tokenizer.TokenFactories
{
    public interface ITokenProvider
    {
        public Predicate<string> Predicate { get; }

        public IToken Provide(string token, params object[] args);
    }
}
