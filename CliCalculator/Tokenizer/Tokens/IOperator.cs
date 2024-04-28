
namespace CliCalculator.Tokenizer.Tokens
{
    public interface IOperator : IToken
    {
        public int Priority { get; }
    }
}
