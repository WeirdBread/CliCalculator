using CliCalculator.Tokenizer.Tokens;

namespace CliCalculator.Calculator
{
    public static class DiceEvaluator
    {
        public static DiceEvaluationResult EvaluateDice(DiceToken diceToken, OperandToken? leftOperand, OperandToken rightOperand)
        {
            return EvaluateDice((int)(leftOperand?.Number ?? 1), (int)rightOperand.Number, diceToken.Modificators);
        }

        private static DiceEvaluationResult EvaluateDice(int diceToRoll, int edges, IList<DiceModificator> modificators)
        {
            var rollResult = RollDice(diceToRoll, edges);

            var orderedMods = modificators.OrderByDescending(x => x.Priority);

            foreach (var mod in orderedMods)
            {
                switch (mod.Type)
                {
                    case DiceModificatorType.KeepHigh:
                        var diceRolledOrdered = rollResult.DiceRolled.PrimaryGroup.OrderByDescending(x => x);
                        rollResult.Sum = diceRolledOrdered.Take(mod.Param ?? 1).Sum();
                        break;
                    case DiceModificatorType.KeepLow:
                        diceRolledOrdered = rollResult.DiceRolled.PrimaryGroup.OrderBy(x => x);
                        rollResult.Sum = diceRolledOrdered.Take(mod.Param ?? 1).Sum();
                        break;
                    case DiceModificatorType.RerollKeepHigh:
                        var count = 1;
                        while (count < (mod.Param ?? 2))
                        {
                            count++;
                            var newRoll = EvaluateDice(diceToRoll, edges, modificators.Where(x => x.Priority > mod.Priority).ToList());
                            rollResult.DiceRolled.AddGroup(newRoll.DiceRolled);
                            if (newRoll.Sum > rollResult.Sum)
                            {
                                rollResult.Sum = newRoll.Sum;
                                rollResult.DiceRolled.PrimaryGroupIndex = rollResult.DiceRolled.Count-1;
                            }
                        }
                        break;
                    case DiceModificatorType.RerollKeepLow:
                        count = 1;
                        while (count < (mod.Param ?? 2))
                        {
                            count++;
                            var newRoll = EvaluateDice(diceToRoll, edges, modificators.Where(x => x.Priority > mod.Priority).ToList());
                            rollResult.DiceRolled.AddGroup(newRoll.DiceRolled);
                            if (newRoll.Sum < rollResult.Sum)
                            {
                                rollResult.Sum = newRoll.Sum;
                                rollResult.DiceRolled.PrimaryGroupIndex = rollResult.DiceRolled.Count - 1;
                            }
                        }
                        break;
                    case DiceModificatorType.MoreThan:
                        rollResult.Sum = rollResult.DiceRolled.PrimaryGroup.Count(x => x >= (mod.Param ?? 1));
                        break;
                    case DiceModificatorType.LessThan:
                        rollResult.Sum = rollResult.DiceRolled.PrimaryGroup.Count(x => x < (mod.Param ?? 1));
                        break;
                    case DiceModificatorType.Explosive:
                        var dicesToExplode = rollResult.DiceRolled.PrimaryGroup.Count(x => x == edges);
                        while (dicesToExplode > 0)
                        {
                            var newRoll = RollDice(dicesToExplode, edges);
                            dicesToExplode = newRoll.DiceRolled.PrimaryGroup.Count(x => x == edges);
                            rollResult.DiceRolled.PrimaryGroup.AddRange(newRoll.DiceRolled.PrimaryGroup);
                            rollResult.Sum += newRoll.Sum;
                        }
                        break;
                }
                rollResult.Expression += mod.Type.GetDescription() + mod.Param;
            }

            return rollResult;
        }

        private static DiceEvaluationResult RollDice(int diceToRoll, int edges)
        {
            var rnd = new Random();
            var count = 0;
            var result = new DiceEvaluationResult();
            while (count < diceToRoll)
            {
                count++;
                var rollResult = rnd.Next(1, edges + 1);
                result.DiceRolled.Add(rollResult);
                result.Sum += rollResult;
            }
            result.Expression = $"{diceToRoll}d{edges}";
            return result;
        }
    }

    public class DiceEvaluationResult
    {
        public DiceEvaluationResult()
        {
            this.DiceRolled = new DiceResultList();
        }

        public string? Expression { get; set; }

        public int Sum { get; set; }

        public DiceResultList DiceRolled { get; set; }

        public override string ToString()
        {
            string diceRolledString = string.Empty;
            foreach (var item in DiceRolled)
            {
                diceRolledString += $" [{string.Join(", ", item)}]";
            }
            return $"{this.Expression} = {this.Sum}" + diceRolledString;
        }
    }

    public class DiceResultList : List<List<int>>
    {
        public List<int> LastGroup => this.Last();

        public DiceResultList()
        {
            this.Add(new List<int>());
            this.PrimaryGroupIndex = 0;
        }

        public int PrimaryGroupIndex { get; set; }

        public List<int> PrimaryGroup => this[PrimaryGroupIndex];

        public List<int> AddGroup()
        {
            this.Add(new List<int>());
            return this.LastGroup;
        }

        public List<int> AddGroup(DiceResultList diceResult)
        {
            this.AddRange(diceResult);
            return this.LastGroup;
        }

        public void Add(int item)
        {
            this.LastGroup.Add(item);
        }

        public bool Contains(int item)
        {
            return this.LastGroup.Contains(item);
        }

        public int IndexOf(int item)
        {
            return this.LastGroup.IndexOf(item);
        }

        public void Insert(int index, int item)
        {
            this.LastGroup.Insert(index, item);
        }

        public bool Remove(int item)
        {
            return this.LastGroup.Remove(item);
        }
    }
}
