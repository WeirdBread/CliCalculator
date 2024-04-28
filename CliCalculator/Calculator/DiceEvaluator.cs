using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Calculator
{
    public class DiceEvaluator
    {
        public DiceEvaluator(IEnumerable<IToken> tokens)
        {
            this.Tokens = tokens.ToList();
            this.Result = new List<DiceEvaluationResult>();
        }

        public IList<IToken> Tokens { get; private set; }

        public IList<DiceEvaluationResult> Result { get; private set; }

        public IEnumerable<IToken> EvaluateDice()
        {
            for (int i = 0; i < this.Tokens.Count; i++)
            {
                if (this.Tokens[i] is DiceToken diceToken)
                {
                    var leftOperandNumber = ((OperandToken)this.Tokens[i - 1]).Number;
                    var rightOperandNumber = ((OperandToken)this.Tokens[i + 1]).Number;

                    var result = GetResult(leftOperandNumber, rightOperandNumber, diceToken.Modificator);
                    this.Result.Add(result);

                    this.Tokens[i] = new OperandToken(result.Sum);
                    this.Tokens.Remove(this.Tokens[i - 1]);
                    this.Tokens.Remove(this.Tokens[i]);

                }
            }

            return this.Tokens;
        }

        private static DiceEvaluationResult GetResult(int diceToRoll, int edges, DiceModificator modificator)
        {
            var rnd = new Random();
            var count = 0;
            var diceEvaluationResult = new DiceEvaluationResult();
            switch (modificator)
            {
                case DiceModificator.None:
                    while (count < diceToRoll)
                    {
                        count++;
                        var rollResult = rnd.Next(1, edges + 1);
                        diceEvaluationResult.DiceRolled.Add(rollResult);
                        diceEvaluationResult.Sum += rollResult;
                    }
                    diceEvaluationResult.Expression = $"{diceToRoll}d{edges}";
                    break;
                case DiceModificator.KeepHigh:
                    var buffer = 0;
                    while (count < diceToRoll)
                    {
                        count++;
                        var rollResult = rnd.Next(1, edges + 1);
                        diceEvaluationResult.DiceRolled.Add(rollResult);
                        if (rollResult > buffer)
                        {
                            buffer = rollResult;
                            diceEvaluationResult.Sum = rollResult;
                        }
                    }
                    diceEvaluationResult.Expression = $"{diceToRoll}d{edges}kh";
                    break;
                case DiceModificator.KeepLow:
                    buffer = 0;
                    while (count < diceToRoll)
                    {
                        count++;
                        var rollResult = rnd.Next(1, edges + 1);
                        diceEvaluationResult.DiceRolled.Add(rollResult);
                        if (buffer == 0 || rollResult < buffer)
                        {
                            buffer = rollResult;
                            diceEvaluationResult.Sum = rollResult;
                        }
                    }
                    diceEvaluationResult.Expression = $"{diceToRoll}d{edges}kl";
                    break;
                default:
                    break;
            }
            return diceEvaluationResult;
        }


        public sealed class DiceEvaluationResult
        {
            public DiceEvaluationResult()
            {
                this.DiceRolled = new List<int>();
            }

            public DiceEvaluationResult(string expression, int sum, params int[] diceRolled)
            {
                this.Expression = expression;
                this.Sum = sum;
                this.DiceRolled = diceRolled.ToList();
            }

            public string? Expression { get; set; }

            public int Sum { get; set; }

            public IList<int> DiceRolled { get; set; }

            public override string ToString()
            {
                return $"{this.Expression} = {this.Sum} [{string.Join(", ", DiceRolled)}]";
            }
        }
    }
}
