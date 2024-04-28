using CliCalculator.Tokenizer;
using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Notator
{
    public static class PolishNotator
    {
        public static IEnumerable<IToken> PolandizeTokens(IEnumerable<IToken> tokens)
        {
            var result = new List<IToken>();
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
                    case TokenType.UnaryOperator:
                        stack.Push(token);
                        break;
                    case TokenType.Dice:
                        var diceToken = (DiceToken)token;
                        if (stack.TryPeek(out var stackToken))
                        {
                            if ((stackToken is BinaryOperatorToken binaryStackOperator && binaryStackOperator.Priority >= 1)
                                || (stackToken is UnaryOperatorToken))
                            {
                                while (stack.Count > 0 && stack.Peek() is BinaryOperatorToken or UnaryOperatorToken)
                                {
                                    result.Add(stack.Pop());
                                }
                            }
                        } 

                        stack.Push(diceToken);
                        break;
                    case TokenType.BinaryOperator:
                        var operatorToken = (BinaryOperatorToken)token;
                        if (stack.TryPeek(out stackToken))
                        {
                            if ((stackToken is BinaryOperatorToken binaryStackOperator && binaryStackOperator.Priority >= operatorToken.Priority)
                                || (stackToken is UnaryOperatorToken && operatorToken.Priority == 0))
                            {
                                while (stack.Count > 0 && stack.Peek() is BinaryOperatorToken or UnaryOperatorToken)
                                {
                                    result.Add(stack.Pop());
                                }
                            }
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
