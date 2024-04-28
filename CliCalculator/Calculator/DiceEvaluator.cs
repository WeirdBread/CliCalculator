using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Calculator
{
    public static class DiceEvaluator
    {
        public static DiceEvaluationResult EvaluateDice(DiceToken diceToken, OperandToken? leftOperand, OperandToken rightOperand)
        {
            return GetResult(leftOperand?.Number ?? 1, rightOperand.Number, diceToken.Modificator);
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


    }

    public class DiceEvaluationResult
    {
        public DiceEvaluationResult()
        {
            this.DiceRolled = new List<int>();
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
