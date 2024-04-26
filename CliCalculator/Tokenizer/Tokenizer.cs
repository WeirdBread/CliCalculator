using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Tokenizer
{
    public class Tokenizer
    {
        private readonly List<string> tokens;

        public Tokenizer(string expression)
        {
            var tokens = new List<string>();

            for (int i = 0; i < expression.Length; i++)
            {
                if (char.IsWhiteSpace(expression[i]))
                {
                    continue;
                }

                if (tokens.Any() && char.IsDigit(expression[i]) && char.IsDigit(expression[i - 1]))
                {
                    tokens[^1] += expression[i];
                    continue;
                }
                tokens.Add(expression[i].ToString());
            }

            this.tokens = tokens;
        }

        public IEnumerable<IToken> GetResult()
        {
            foreach (var token in this.tokens)
            {
                switch (token)
                {
                    case var t when int.TryParse(t, out var tInt) :
                        yield return new OperandToken(tInt);
                        break;
                    case var t when t.Length is 1 && BinaryOperatorToken.operatorSymbols.Contains(t[0]) :
                        yield return new BinaryOperatorToken(t[0]);
                        break;
                    case "(":
                        yield return new OpenParenthesisToken();
                        break;
                    case ")":
                        yield return new CloseParenthesisToken();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
