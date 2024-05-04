
namespace CliCalculator.Tokenizer.Tokens
{
    public interface IOperator : IToken
    {
        public int Priority { get; }

        public IToken DoOperation(OperandToken? leftOperand, OperandToken? rightOperand);
    }
}
