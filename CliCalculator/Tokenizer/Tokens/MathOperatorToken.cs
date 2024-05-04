namespace CliCalculator.Tokenizer.Tokens
{
    public class  MathOperatorToken: IOperator
    {
        public static readonly char[] operatorSymbols = { '+', '-', '/', '*', '^' };

        public MathOperatorToken(char operatorChar)
        {
            Symbol = operatorChar.ToString();
            this.OperatorType = operatorChar switch
            {
                '+' => OperatorType.Sum,
                '-' => OperatorType.Subtract,
                '*' => OperatorType.Multiply,
                '/' => OperatorType.Divide,
                '^' => OperatorType.Power,
                _ => throw new ArgumentOutOfRangeException(),
            };

            Priority = GetPriority(this.OperatorType);
        }

        public int Priority { get; private set; }

        public TokenType Type => TokenType.MathOperator;

        public OperatorType OperatorType { get; private set; }

        public string Symbol { get; private set; }

        public override string ToString()
        {
            return Symbol;
        }

        private static int GetPriority(OperatorType operatorType)
        {
            return operatorType switch
            {
                OperatorType.Sum or OperatorType.Subtract => 0,
                OperatorType.Multiply or OperatorType.Divide => 1,
                OperatorType.Power => 2,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public void ConvertToUnary()
        {
            this.Symbol = "u" + this.Symbol;
            this.OperatorType = OperatorType.UnaryMinus;
            this.Priority = 3;
        }

        public IToken DoOperation(OperandToken? leftOperand, OperandToken? rightOperand)
        {
            switch (this.OperatorType)
            {
                case OperatorType.UnaryMinus:
                    return new OperandToken(rightOperand.Number * -1);
                case OperatorType.Sum:
                    return new OperandToken(leftOperand!.Number + rightOperand.Number);
                case OperatorType.Subtract:
                    return new OperandToken(leftOperand!.Number - rightOperand.Number);
                case OperatorType.Multiply:
                    return new OperandToken(leftOperand!.Number * rightOperand.Number);
                case OperatorType.Divide:
                    return new OperandToken(leftOperand!.Number / rightOperand.Number);
                case OperatorType.Power:
                    return new OperandToken(Math.Pow(leftOperand!.Number, rightOperand.Number));
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    public enum OperatorType
    {
        UnaryMinus,
        Sum,
        Subtract,
        Multiply,
        Divide,
        Power
    }
}
