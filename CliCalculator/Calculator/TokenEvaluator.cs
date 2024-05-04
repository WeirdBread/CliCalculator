using CliCalculator.Notator;
using CliCalculator.Tokenizer;
using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Calculator
{
    public class TokenEvaluator : IEvaluator
    {
        private readonly TokenCollection tokens;
        private ITokenizer tokenizer;

        public TokenEvaluator(ITokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
            this.tokens = PolishNotator.PolandizeTokens(this.tokenizer.GenerateTokens());
        }

        public double Evaluate()
        {
            var stack = new Stack<IToken>();

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Operand:
                        stack.Push(token);
                        break;
                    case TokenType.Operator:
                        var operatorToken = (OperatorToken)token;
                        var rightOperand = (OperandToken)stack.Pop();
                        var leftOperand = operatorToken.OperatorType is OperatorType.UnaryMinus ? null : (OperandToken)stack.Pop();
                        switch (((OperatorToken)token).OperatorType)
                        {
                            case OperatorType.UnaryMinus:
                                stack.Push(new OperandToken(rightOperand.Number * -1));
                                break;
                            case OperatorType.Sum:
                                stack.Push(new OperandToken(leftOperand!.Number + rightOperand.Number));
                                break;
                            case OperatorType.Subtract:
                                stack.Push(new OperandToken(leftOperand!.Number - rightOperand.Number));
                                break;
                            case OperatorType.Multiply:
                                stack.Push(new OperandToken(leftOperand!.Number * rightOperand.Number));
                                break;
                            case OperatorType.Divide:
                                stack.Push(new OperandToken(leftOperand!.Number / rightOperand.Number));
                                break;
                            case OperatorType.Power:
                                stack.Push(new OperandToken(Math.Pow(leftOperand!.Number, rightOperand.Number)));
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            return (stack.Pop() as OperandToken)?.Number ?? throw new InvalidOperationException();
        }
    }
}
