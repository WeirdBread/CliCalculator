using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Tokenizer
{
    public class TokenCollection: List<IToken>
    {
        public override string ToString()
        {
            return string.Join(", ", this.Select(x => x.ToString()).ToArray());
        }
    }
}
