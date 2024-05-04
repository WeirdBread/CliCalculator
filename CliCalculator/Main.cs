using CliCalculator.Calculator;
using CliCalculator.Tokenizer;
using CliCalculator.Tokenizer.TokenFactories;
using System.Globalization;
{
    var buffer = 0d;
    while (true)
    {
        char? firstKey = Console.ReadKey().KeyChar;

        var input = firstKey == '+' ? buffer.ToString("0.#####", CultureInfo.InvariantCulture) + "+" : firstKey.ToString();

        input += Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            return;

        var tokenizer = new Tokenizer(input, TokenFactory.Instance);

        var tokens = tokenizer.GenerateTokens();
        Console.WriteLine(tokens);

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