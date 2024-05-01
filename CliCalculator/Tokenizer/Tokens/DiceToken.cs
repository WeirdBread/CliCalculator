using System.ComponentModel;

namespace CliCalculator.Tokenizer.Tokens
{
    public class DiceToken : IOperator
    {
        public static readonly string[] AvailableModifiersTags = { "rkh", "rkl", "kh", "kl", "!", "<", ">" };

        public DiceToken(bool isSingleDie)
        {
            Symbol = "d";
            Modificators = new List<DiceModificator>();
            IsSingleDie = isSingleDie;
        }

        public bool IsSingleDie { get; private set; }

        public TokenType Type => TokenType.Dice;

        public IList<DiceModificator> Modificators { get; set; }

        public OperatorType OperatorType { get; private set; }

        public string Symbol { get; private set; }

        public int Priority => 4;

        public override string ToString() => Symbol;

        public DiceModificator? AddModificator(string? modificator, int? param = null)
        {
            var newModificator = modificator switch
            {
                "kh" => new DiceModificator(DiceModificatorType.KeepHigh, param),
                "kl" => new DiceModificator(DiceModificatorType.KeepLow, param),
                "rkh" => new DiceModificator(DiceModificatorType.RerollKeepHigh, param, true),
                "rkl" => new DiceModificator(DiceModificatorType.RerollKeepLow, param, true),
                "!" => new DiceModificator(DiceModificatorType.Explosive),
                "<" => new DiceModificator(DiceModificatorType.LessThan, param),
                ">" => new DiceModificator(DiceModificatorType.MoreThan, param),
                _ => null
            };

            if (newModificator is not null)
            {
                if (this.Modificators.Any(x => x.Priority == newModificator.Priority))
                {
                    return null;
                }

                this.Modificators.Add(newModificator);
            }

            return newModificator;
        }
    }

    public enum DiceModificatorType
    {
        None,

        [Description("kh")]
        KeepHigh,

        [Description("kl")]
        KeepLow,

        [Description("rkh")]
        RerollKeepHigh,

        [Description("rkl")]
        RerollKeepLow,

        [Description("!")]
        Explosive,

        [Description("<")]
        LessThan,

        [Description(">")]
        MoreThan
    }

    public class DiceModificator 
    {
        public DiceModificator(DiceModificatorType type, int? param = null, bool isLeftOriented = false)
        {
            Type = type;
            Param = param;
            Priority = GetPriotiry(type);
            IsLeftOriented = isLeftOriented;
        }

        public DiceModificatorType Type { get; private set; }
        public int? Param { get; set; }
        public int Priority { get; private set; }
        
        // Признак, что параметры применяются с левой стороны, иначе с правой.
        public bool IsLeftOriented { get; private set; }

        private static int GetPriotiry(DiceModificatorType type)
        {
            return type switch
            {
                DiceModificatorType.MoreThan or DiceModificatorType.LessThan => 0,
                DiceModificatorType.KeepLow or DiceModificatorType.KeepHigh => 1,
                DiceModificatorType.RerollKeepHigh or DiceModificatorType.RerollKeepLow => 2,
                DiceModificatorType.Explosive => 3,
                _ => -1
            };
        }
    }
}
