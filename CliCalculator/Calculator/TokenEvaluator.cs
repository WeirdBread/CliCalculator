using CliCalculator.Tokenizer;
using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Calculator
{
    public class TokenEvaluator : IEvaluator
    {
        private readonly TokenCollection tokens;
        private readonly ITokenizer tokenizer;

        public TokenEvaluator(ITokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
            this.tokens = PolandizeTokens(this.tokenizer.GenerateTokens());
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
                    case TokenType.MathOperator:
                        var operatorToken = (MathOperatorToken)token;
                        var rightOperand = (OperandToken)stack.Pop();
                        var leftOperand = operatorToken.OperatorType is OperatorType.UnaryMinus ? null : (OperandToken)stack.Pop();
                        stack.Push(operatorToken.DoOperation(leftOperand, rightOperand));
                        break;
                    default:
                        break;
                }
            }

            return (stack.Pop() as OperandToken)?.Number ?? throw new InvalidOperationException();
        }

        private static TokenCollection PolandizeTokens(TokenCollection tokens)
        {
            var result = new TokenCollection();
            var stack = new Stack<IToken>();

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Unknown:
                        break;
                    case TokenType.Operand:
                        result.Add(token);
                        break;
                    case TokenType.MathOperator:
                        var operatorToken = (MathOperatorToken)token;
                        while (stack.TryPeek(out var stackToken))
                        {
                            if (stackToken is MathOperatorToken stackOperator && stackOperator.Priority >= operatorToken.Priority)
                            {
                                result.Add(stack.Pop());
                                continue;
                            }
                            break;
                        }
                        stack.Push(operatorToken);
                        break;
                    case TokenType.OpenParenthesis:
                        stack.Push(token);
                        break;
                    case TokenType.CloseParenthesis:
                        while (stack.Peek() is not OpenParenthesisToken)
                        {
                            result.Add(stack.Pop());
                        }
                        stack.Pop();
                        break;
                    default:
                        break;
                }
            }

            while (stack.Count > 0)
            {
                result.Add(stack.Pop());
            }

            return result;
        }
    }
}
