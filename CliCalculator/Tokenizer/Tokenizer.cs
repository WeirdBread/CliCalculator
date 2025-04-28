using CliCalculator.Tokenizer.TokenFactories;
using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Tokenizer
{
    public class Tokenizer : ITokenizer
    {
        private readonly string inputExpression = string.Empty;

        private readonly ITokenFactory tokenFactory;

        public Tokenizer(string inputExpression, ITokenFactory tokenFactory)
        {
            this.inputExpression = inputExpression;
            this.tokenFactory = tokenFactory;
        }

        public TokenCollection GenerateTokens()
        {
            var result = new TokenCollection();
            var splitedExpression = this.SplitByTokens();

            for (var i = 0; i < splitedExpression.Count; i++)
            {
                var token = this.tokenFactory.CreateToken(splitedExpression[i]);
                if (token is null) 
                    continue;

                if (token is MathOperatorToken operatorToken && (!result.Any() || result[^1] is MathOperatorToken or OpenParenthesisToken))
                {
                    operatorToken.ConvertToUnary();
                }

                result.Add(token);
            }
            return result;
        }

        private List<string> SplitByTokens()
        {
            var tokens = new List<string>();
            var buffer = new Buffer();

            for (int i = 0; i < inputExpression.Length; i++)
            {
                ResolveChar(inputExpression[i], tokens, ref buffer);
            }

            if (buffer.BufferString is not null)
            {
                tokens.Add(buffer.BufferString);
            }

            return tokens;
        }

        private static void ResolveChar(char ch, IList<string> tokens, ref Buffer buffer)
        {
            if (char.IsWhiteSpace(ch))
            {
                buffer.PopIntoList(tokens);
                return;
            }

            var charIsDigit = char.IsDigit(ch);
            var charIsLetter = char.IsLetter(ch);
            var charIsPoint = ch is '.';

            if (charIsDigit || charIsLetter || charIsPoint)
            {
                if (buffer.BufferString is null)
                {
                    buffer.BufferString = ch.ToString();
                    buffer.IsNumber = charIsDigit;
                }
                else if ((buffer.IsNumber && (charIsDigit || charIsPoint)) || (!buffer.IsNumber && charIsLetter))
                {
                    buffer.BufferString += ch.ToString();
                }
                return;
            }

            buffer.PopIntoList(tokens);
            tokens.Add(ch.ToString());
        }

        private struct Buffer
        {
            public string? BufferString { get; set; }

            public bool IsNumber { get; set; }

            public bool PopIntoList(IList<string> list)
            {
                if (this.BufferString is null) 
                    return false;

                list.Add(BufferString);
                this.BufferString = null;
                this.IsNumber = false;
                return true;
            }
        }
    }
}
