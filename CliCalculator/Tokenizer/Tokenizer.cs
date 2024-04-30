using CliCalculator.Tokenizer.Tokens;
using System.Globalization;

namespace CliCalculator.Tokenizer
{
    public class Tokenizer
    {
        private readonly List<string> tokens;

        public Tokenizer(string expression)
        {
            var tokens = new List<string>();

            string? buffer = null;
            for (int i = 0; i < expression.Length; i++)
            {
                if (char.IsWhiteSpace(expression[i]))
                {
                    continue;
                }

                if (char.IsLetter(expression[i]) || char.IsDigit(expression[i]) || expression[i] == '.')
                {
                    buffer += expression[i];
                    continue;
                }

                if (buffer is not null)
                {
                    tokens.Add(buffer);
                    buffer = null;
                }

                tokens.Add(expression[i].ToString());
            }

            if (buffer is not null)
            {
                tokens.Add(buffer);
            }

            this.tokens = tokens;
        }

        public IEnumerable<IToken> GetResult()
        {
            var result = new List<IToken>();
            int? diceModBuffer = null;
            for (var i = 0; i < this.tokens.Count; i++)
            {
                switch (this.tokens[i])
                {
                    case var t when double.TryParse(t, NumberStyles.Any, CultureInfo.InvariantCulture, out var tDouble) :
                        result.Add(new OperandToken(tDouble));
                        break;
                    case var t when t is "-" && (result.LastOrDefault() is null or not (OperandToken or CloseParenthesisToken)):
                        if (t is "-")
                        {
                            result.Add(new UnaryOperatorToken());
                        }
                        break;
                    case var t when BinaryOperatorToken.operatorSymbols.Contains(t[0]) :
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
                    case var t when DiceToken.AvailableModifiersTags.Contains(t):
                        var previousDiceToken = result.Count < 2 ? null : result[^2] as DiceToken;
                        if (previousDiceToken is not null)
                        {
                            var newMod = previousDiceToken.AddModificator(t);
                            if (newMod is not null && newMod.IsLeftOriented)
                            {
                                newMod.Param = diceModBuffer;
                                diceModBuffer = null;
                            }
                        }
                        break;
                    case var t when int.TryParse(t, out var tInt):
                        previousDiceToken = result.Count < 2 ? null : result[^2] as DiceToken;
                        if (previousDiceToken is not null)
                        {
                            var diceMod = previousDiceToken.Modificators.LastOrDefault();
                            if (diceMod is not null && !diceMod.IsLeftOriented) 
                            {
                                diceMod.Param = tInt;
                                break;
                            }
                            diceModBuffer = tInt;
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
