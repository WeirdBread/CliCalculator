using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Tokenizer.TokenFactories
{
    public interface ITokenFactory
    {
        IToken? CreateToken(string token, params object[] tokenParams);
    }
}
