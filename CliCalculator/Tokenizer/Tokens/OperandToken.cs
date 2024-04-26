﻿namespace CliCalculator.Tokenizer.Tokens
{
    public class OperandToken : IToken
    {
        public OperandToken(int digit)
        {
            Number = digit;
            Symbol = digit.ToString();
        }

        public TokenType Type => TokenType.Operand;

        public string Symbol { get; private set; }

        public int Number { get; private set; }

        public override string ToString()
        {
            return Symbol;
        }
    }
}
