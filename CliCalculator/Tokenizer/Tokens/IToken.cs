using System;
namespace CliCalculator.Tokenizer.Tokens
{
    public interface IToken
    {
        public string Symbol { get; }
        public TokenType Type { get; }

        public string ToString();
    }
}
