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

            var buffer = new Buffer();

            for (int i = 0; i < expression.Length; i++)
            {
                if (char.IsWhiteSpace(expression[i]))
                {
                    continue;
                }

                var charIsDigit = char.IsDigit(expression[i]);
                var charIsLetter = char.IsLetter(expression[i]);
                var charIsPoint = expression[i] is '.';

                if ((charIsDigit || charIsLetter || charIsPoint) && expression[i] != 'd')
                {
                    if (buffer.BufferString is null)
                    {
                        buffer.BufferString += expression[i];
                        buffer.IsNumber = charIsDigit;
                        continue;
                    } 
                    else if ((buffer.IsNumber && (charIsDigit || charIsPoint)) || (!buffer.IsNumber && charIsLetter))
                    {
                        buffer.BufferString += expression[i];
                        continue;
                    }
                }

                if (buffer.BufferString is not null)
                {
                    tokens.Add(buffer.BufferString);
                    if (charIsDigit || charIsLetter)
                    {
                        buffer.BufferString = expression[i].ToString();
                        buffer.IsNumber = charIsDigit;
                    }
                    else
                    {
                        tokens.Add(expression[i].ToString());
                        buffer.BufferString = null;
                    }
                    continue;
                }

                tokens.Add(expression[i].ToString());
            }

            if (buffer.BufferString is not null)
            {
                tokens.Add(buffer.BufferString);
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
                    case var t when double.TryParse(t, NumberStyles.Any, CultureInfo.InvariantCulture, out var tDouble)
                                && (result.Count < 2 || result[^2] is not DiceToken):
                        result.Add(new OperandToken(tDouble));
                        break;
                    case var t when t is "-" && (result.LastOrDefault() is null or not (OperandToken or CloseParenthesisToken)):
                        result.Add(new UnaryOperatorToken());
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
                        throw new ArgumentOutOfRangeException(this.tokens[i], $"Unexpected token: '{this.tokens[i]}', pos: {i + 1}:{i + 1 + this.tokens[i].Length}");
                }
            }
            return result;
        }

        private struct Buffer
        {
            public string? BufferString { get; set; }
            public bool IsNumber { get; set; }
        }
    }
}
