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
        private List<string> _operations;  
        public int Result { get; private set; }
        public Calculator()
        {
            _values = new List<int>();
            _operations = new List<string>();
          
        }
        public void WriteToFile(string path)
        {

        }
        public void ReadFromFile(string path)
        {

        }
        public int GetResult(string expression)
        {
            return 1;
        }
        public void Parse(string expression)
        {
            while (true)
            {
                if (!expression.Contains('('))
                {
                    Result = int.Parse(Calculate(expression));
                    break;
                }
                else
                {
                    int first = expression.LastIndexOf('(');
                    int last = expression.IndexOf(')');
                    string partExpressionWuthoutBraces = expression.Substring(first+1,last-first-1);
                    string partExpressionWithBraces = expression.Substring(first, last - first + 1);
                    expression = expression.Replace( partExpressionWithBraces,Calculate(partExpressionWuthoutBraces));
                }
            }
           
        }
        private string Calculate(string expression)
        {
            _operations = expression.Split(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            _values = expression.Split(new char[] { '*', '/', '+', '-' }, StringSplitOptions.RemoveEmptyEntries).Select(value => Convert.ToInt32(value)).ToList();
            CheckSpecialCases(expression);

            while (true)
            {
                
                if (_operations.Contains("*"))
                {
                    int index = _operations.IndexOf("*");
                    int firstVar = _values[index];
                    int secondVar = _values[index + 1];
                    int result = Multiply(firstVar, secondVar);
                    Replace(index, result);
                    continue;
                }
                else if (_operations.Contains("/"))
                {
                    int index = _operations.IndexOf("/");
                    int firstVar = _values[index];
                    int secondVar = _values[index + 1];
                    int result = Devide(firstVar,secondVar);
                    Replace(index, result);
                    continue;
                }
                else if (_operations.Contains("-"))
                {
                    int index = _operations.IndexOf("-");
                    int firstVar = _values[index];
                    int secondVar = _values[index + 1];
                    int result = Subtract(firstVar, secondVar);
                    Replace(index, result);
                    continue;
                }
                else if (_operations.Contains("+"))
                {
                    int index = _operations.IndexOf("+");
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
        private void CheckSpecialCases(string expression)
        {
            if (_operations.Contains("+-"))
            {
                int index = _operations.IndexOf("+-");
                _operations[index] = "+";
                _values[index + 1] = -_values[index + 1];
            }
            if (_operations.Contains("--"))
            {
                int index = _operations.IndexOf("--");
                _operations[index] = "+";
            }
            if (_operations.Contains("/-"))
            {
                int index = _operations.IndexOf("/-");
                _operations[index] = "/";
                _values[index + 1] = -_values[index + 1];
            }
            if (_operations.Contains("*-"))
            {
                int index = _operations.IndexOf("*-");
                _operations[index] = "*";
                _values[index + 1] = -_values[index + 1];
            }
            if (_operations.Contains("-") && expression[0] == '-')
            {
                _operations.RemoveAt(0);
                _values[0] = -_values[0];
            }
        }
        private void Replace(int index,int result)
        {
            _operations.RemoveAt(index);
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
