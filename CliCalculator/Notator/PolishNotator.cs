using CliCalculator.Tokenizer;
using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Notator
{
    public static class PolishNotator
    {
        public static TokenCollection PolandizeTokens(TokenCollection tokens)
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
                    case TokenType.Operator:
                        var operatorToken = (OperatorToken)token;
                        while (stack.TryPeek(out var stackToken))
                        {
                            if (stackToken is OperatorToken stackOperator && stackOperator.Priority >= operatorToken.Priority)
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

            while(stack.Count > 0)
            {
                result.Add(stack.Pop());
            }

            return result;
        }
    }
}
