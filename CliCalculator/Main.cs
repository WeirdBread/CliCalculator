using CliCalculator.Calculator;
using CliCalculator.Tokenizer;
using CliCalculator.Tokenizer.TokenFactories;
using CliCalculator.Tokenizer.Tokens;
using System.Globalization;
{
    var buffer = 0d;
    while (true)
    {
        var input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input) && MathOperatorToken.operatorSymbols.Contains(input[0]))
        {
            input = buffer.ToString("0.#####", CultureInfo.InvariantCulture) + input[0] + input.Substring(1);
        }

        if (string.IsNullOrWhiteSpace(input))
            return;

        var tokenizer = new Tokenizer(input, TokenFactory.Instance);

        try
        {
            var evaluator = new TokenEvaluator(tokenizer);
            var result = evaluator.Evaluate();
            Console.WriteLine("= " + result.ToString("0.#####", CultureInfo.InvariantCulture));
            buffer = result;
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Invalid expression");
            buffer = 0;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
            buffer = 0;
        }
    }
}