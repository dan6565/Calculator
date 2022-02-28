using CalculatorProject;
var calculator = new Calculator();
while (true)
{
    string expression = Console.ReadLine();
    calculator.Parse(expression);
    int result = calculator.Result;
    Console.WriteLine(result);
}

Console.ReadLine();