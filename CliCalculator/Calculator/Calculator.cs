using CliCalculator.Tokenizer;
using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Calculator
{
    public class Calculator
    {
        public Calculator(IToken[] tokens)
        {
            this.tokens = tokens;
        }

        private readonly IToken[] tokens;

        public int ResolveExpression()
        {
            var stack = new Stack<IToken>();

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Operand: 
                        stack.Push(token); 
                        break;
                    case TokenType.BinaryOperator:
                        var firstOperand = (OperandToken) stack.Pop();
                        var secondOperand = (OperandToken) stack.Pop();
                        switch (((BinaryOperatorToken)token).OperatorType)
                        {
                            case OperatorType.Sum:
                                stack.Push(new OperandToken(secondOperand.Number + firstOperand.Number));
                                break;
                            case OperatorType.Subtract:
                                stack.Push(new OperandToken(secondOperand.Number - firstOperand.Number));
                                break;
                            case OperatorType.Multiply:
                                stack.Push(new OperandToken(secondOperand.Number * firstOperand.Number));
                                break;
                            case OperatorType.Divide:
                                stack.Push(new OperandToken(secondOperand.Number / firstOperand.Number));
                                break;
                            case OperatorType.Power:
                                stack.Push(new OperandToken(secondOperand.Number ^ firstOperand.Number));
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
