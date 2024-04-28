﻿namespace CliCalculator.Tokenizer.Tokens
{
    public class BinaryOperatorToken : IOperator
    {
        public static readonly char[] operatorSymbols = { '+', '-', '/', '*', '^' };

        public BinaryOperatorToken(char operatorChar)
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

        public TokenType Type => TokenType.BinaryOperator;

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
    }

    public enum OperatorType
    {
        Sum,
        Subtract,
        Multiply,
        Divide,
        Power
    }
}
