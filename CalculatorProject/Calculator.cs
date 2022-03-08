using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorProject
{
    public class Calculator
    {
        private List<int> _values;
        private List<string> _operators;
        public string Error { get; private set; }
        public int Result { get; private set; }
        public Calculator()
        {
            
            _values = new List<int>();
            _operators = new List<string>();
          
        }
        public void CalculateFile(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException();
            }
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            string[] lines;
            try
            {
                lines = File.ReadAllLines(path);
            }
            catch (IOException)
            {
                return;
            }
            for (int i = 0; i < lines.Length; i++)
            {
                if (TryCalculate(lines[i], out int result))
                {
                    lines[i] += " = "+ result;
                }
                else
                {
                    lines[i] += " = " + Error;
                }
            }
            try
            {
                 File.WriteAllLines("Result.txt",lines);
            }
            catch (IOException)
            {
                return;
            }


        }
       
        public bool TryCalculate(string expression, out int result)
        {
            if (expression == null)
            {
                throw new ArgumentNullException();
            }
            Error = "No errors";

            if (!IsExpressionValid(expression))
            {
                Error = "Invalid expression";
                result = 0;
                return false;
            }
            
            while (true)
            {
                if (!expression.Contains('('))
                {
                    expression = CalculateWithoutBraces(expression);
                }
                else
                {
                    expression = CalculateWithBraces(expression);
                }

                if(int.TryParse(expression,out int value))
                {
                    result = value;
                    return true;
                }
                if (Error!="No errors")
                {
                    result = 0;
                    return false;
                }
            }
           
        }


        private string CalculateWithBraces(string expression)
        {
            int first = expression.LastIndexOf('(');
            int last = expression.IndexOf(')', first);
            string partExpressionWuthoutBraces = expression.Substring(first + 1, last - first - 1);
            string partExpressionWithBraces = expression.Substring(first, last - first + 1);
            string result = CalculateWithoutBraces(partExpressionWuthoutBraces);
            return expression.Replace(partExpressionWithBraces, result);

        }
        private string CalculateWithoutBraces(string expression)
        {
            SplitValuesAndOperators(expression);            

            CheckSpecialCases(expression);

            while (true)
            {
                
                if (_operators.Contains("*"))
                {
                    int index = _operators.IndexOf("*");
                    int firstVar = _values[index];
                    int secondVar = _values[index + 1];
                    int result = Multiply(firstVar, secondVar);
                    Replace(index, result);
                    continue;
                }
                else if (_operators.Contains("/"))
                {
                    int index = _operators.IndexOf("/");
                    int firstVar = _values[index];
                    int secondVar = _values[index + 1];
                    if (secondVar == 0)
                    {
                        Error = "Error: Devide by zero";
                        return "";
                    }
                    int result = Devide(firstVar,secondVar);
                    Replace(index, result);
                    continue;
                }
                else if (_operators.Contains("-"))
                {
                    int index = _operators.IndexOf("-");
                    int firstVar = _values[index];
                    int secondVar = _values[index + 1];
                    int result = Subtract(firstVar, secondVar);
                    Replace(index, result);
                    continue;
                }
                else if (_operators.Contains("+"))
                {
                    int index = _operators.IndexOf("+");
                    int firstVar = _values[index];
                    int secondVar = _values[index + 1];
                    int result = Sum(firstVar, secondVar);
                    Replace(index, result);
                    continue;
                }
                
                else
                {
                    return _values[0].ToString();
                }     
            }
        }

        private bool IsExpressionValid(string expression)
        {
            return HasOnlyValidCharacter(expression)
                && HasValidRatioBracesAndOperators(expression);
                            
        }       
        private void SplitValuesAndOperators(string expression)
        {
            _operators = expression.Split(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            _values = expression.Split(new char[] { '*', '/', '+', '-' }, StringSplitOptions.RemoveEmptyEntries).Select(value => Convert.ToInt32(value)).ToList();
        }
        private void CheckSpecialCases(string expression)
        {
            if (_operators.Contains("+-"))
            {
                int index = _operators.IndexOf("+-");
                _operators[index] = "+";
                _values[index + 1] = -_values[index + 1];
            }
            if (_operators.Contains("--"))
            {
                int index = _operators.IndexOf("--");
                _operators[index] = "+";
            }
            if (_operators.Contains("/-"))
            {
                int index = _operators.IndexOf("/-");
                _operators[index] = "/";
                _values[index + 1] = -_values[index + 1];
            }
            if (_operators.Contains("*-"))
            {
                int index = _operators.IndexOf("*-");
                _operators[index] = "*";
                _values[index + 1] = -_values[index + 1];
            }
            if (_operators.Contains("-") && expression[0] == '-')
            {
                _operators.RemoveAt(0);
                _values[0] = -_values[0];
            }
        }
        private bool HasValidRatioBracesAndOperators(string expression)
        {
            int minimalDifference = 2;
            int first;
            int second;
            while (true)
            {
                if (expression.Contains("(")||expression.Contains(")"))
                {
                    try
                    {
                        first = expression.LastIndexOf('(');
                        second = expression.IndexOf(')', first);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        return false;
                    }
                    if (second - first < minimalDifference)
                    {
                        return false;
                    }
                    string partExpressionWithoutBraces = expression.Substring(first + 1, second - first - 1);
                    if (!HasValidRatioOperators(partExpressionWithoutBraces))
                    {
                        return false;
                    }
                    string partExpression = expression.Substring(first, second - first + 1);
                    expression = expression.Replace(partExpression, "1");
                }
                else
                {
                    return HasValidRatioOperators(expression);  
                }                  
               
            }
            
        }
       
        private bool HasValidRatioOperators(string expression)
        {
           SplitValuesAndOperators(expression);
           CheckSpecialCases(expression);          
           foreach (var operation in _operators)
            {
                if (operation.Length > 1)
                {
                    return false;
                }
            }
            return true;
        }
        private bool HasOnlyValidCharacter(string expression)
        {
            bool isValid = true;
            foreach (var symbol in expression)
            {
                if (symbol > '9' || symbol < '0')
                {
                    if (!IsOperatorOrBrace(symbol))
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            return isValid;
        }
        private bool IsOperatorOrBrace(char symbol)
        {
            return symbol == '+' || symbol == '-' || symbol == '*' || symbol == '/' || symbol == ')' || symbol == '(';
        }
        private void Replace(int index,int result)
        {
            _operators.RemoveAt(index);
            _values.Insert(index, result);
            _values.RemoveAt(index + 1);
            _values.RemoveAt(index + 1);
        }
        private int Sum(int a, int b)
        {
            return a + b;
        }
        private int Subtract(int a, int b)
        {
            return a - b;
        }
        private int Multiply(int a, int b)
        {
            return a * b;
        }
        private int Devide(int a, int b)
        {
            return a / b;
        }
    }
}
