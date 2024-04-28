using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Calculator
{
    public class DiceEvaluator
    {
        public DiceEvaluator(IEnumerable<IToken> tokens)
        {
            this.tokens = tokens.ToList();
        }

        private readonly IList<IToken> tokens;

        public IEnumerable<IToken> EvaluateDice()
        {
            for (int i = 0; i < this.tokens.Count; i++)
            {
                if (this.tokens[i] is DiceToken)
                {
                    var leftOperandNumber = ((OperandToken)this.tokens[i - 1]).Number;
                    var rightOperandNumber = ((OperandToken)this.tokens[i + 1]).Number;
                    var rnd = new Random();
                    var sum = 0;
                    var diceToRoll = leftOperandNumber;
                    while (diceToRoll > 0)
                    {
                        sum += rnd.Next(1, rightOperandNumber + 1);
                        diceToRoll--;
                    }
                    //Console.WriteLine($"Rolled on {leftOperandNumber}d{rightOperandNumber} = {sum}");
                    this.tokens[i] = new OperandToken(sum);
                    this.tokens.Remove(this.tokens[i - 1]);
                    this.tokens.Remove(this.tokens[i]);

                }
            }

            return this.tokens;
        }
    }
}
