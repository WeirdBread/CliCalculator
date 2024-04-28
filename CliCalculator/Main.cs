using CliCalculator.Calculator;
using CliCalculator.Notator;
using CliCalculator.Tokenizer;

var input = Console.ReadLine();

if (input is null)
{
    throw new ArgumentNullException();
}

var tokenizer = new Tokenizer(input);

var tokens = tokenizer.GetResult();

var diceEvaluator = new DiceEvaluator(tokens);

tokens = diceEvaluator.EvaluateDice();

Console.WriteLine(string.Join("; ", diceEvaluator.Result));

Console.WriteLine(string.Join(" ", tokens));

var polishTokens = PolishNotator.PolandizeTokens(tokens);

Console.WriteLine(string.Join(" ", polishTokens));

var calculator = new Calculator(polishTokens.ToArray());

Console.WriteLine(calculator.ResolveExpression());
