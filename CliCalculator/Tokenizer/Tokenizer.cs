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

                if (tokens.Any() 
                    && (char.IsDigit(expression[i]) && char.IsDigit(expression[i - 1])
                        || char.IsLetter(expression[i]) && char.IsLetter(expression[i - 1])))
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
            var result = new List<IToken>();
            for (var i = 0; i < this.tokens.Count; i++)
            {
                switch (this.tokens[i])
                {
                    case var t when int.TryParse(t, out var tInt) :
                        result.Add(new OperandToken(tInt));
                        break;
                    case var t when t.Length is 1 && t is "-" or "+" && (result.LastOrDefault() is null or not (OperandToken or CloseParenthesisToken)):
                        if (t is "-")
                        {
                            result.Add(new UnaryOperatorToken());
                        }
                        break;
                    case var t when t.Length is 1 && BinaryOperatorToken.operatorSymbols.Contains(t[0]) :
                        result.Add(new BinaryOperatorToken(t[0]));
                        break;
                    case "(":
                        result.Add(new OpenParenthesisToken());
                        break;
                    case ")":
                        result.Add(new CloseParenthesisToken());
                        break;
                    case var t when t is "d":
                        var isSingleDie = result.Count is 0 || result[^1] is not OperandToken;
                        result.Add(new DiceToken(isSingleDie));
                        break;
                    case var t when t is "kh" or "kl":
                        var previousDiceToken = result.Last(x => x is DiceToken) as DiceToken;
                        if (previousDiceToken is not null)
                        {
                            previousDiceToken.Modificator = t is "kh" ? DiceModificator.KeepHigh : DiceModificator.KeepLow;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return result;
        }
    }
}
