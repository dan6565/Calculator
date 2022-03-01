using NUnit.Framework;
using System;
using System.IO;
using CalculatorProject;

namespace CalculatorProject.Tests
{
    public class CalculatorTests
    {
        private Calculator _calculator;
        [SetUp]
        public void Setup()
        {
            _calculator = new Calculator();
        }

        [TestCase("-1/1", -1)]
        [TestCase("-3+4*2-3*3", -4)]
        [TestCase("1+15/3+2*3", 12)]
        [TestCase("-1+(5-2)*3", 8)]
        [TestCase("10/(4+1)/(-2)", -1)]
        [TestCase("(3+4)/7+2*3/(-1+2)",7)]
        public void TryCalculate_ValidExpression_Values(string input, int expected)
        {
            _calculator.TryCalculate(input, out int actual);
            Assert.AreEqual(expected, actual);
        }
        [TestCase("2/0", "Error: Devide by zero")]
        [TestCase("1+15/0+2*3", "Error: Devide by zero")]
        [TestCase("10/(4+1)/(-2+2)", "Error: Devide by zero")]     
        public void TryCalculate_InValidExpression_ErrorDevideByZero(string input, string expected)
        {
            _calculator.TryCalculate(input, out int result);
            string actual = _calculator.Error;
            Assert.AreEqual(expected, actual);
        }
        [TestCase("4*(5-x)", "Invalid expression")]
        [TestCase("1+++5", "Invalid expression")]
        [TestCase("6+2-x", "Invalid expression")]
        public void TryCalculate_InValidExpression_InvalidExpression(string input, string expected)
        {
            _calculator.TryCalculate(input, out int result);
            string actual = _calculator.Error;
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TryCalculate_NullExpression_Exception()
        {
            string expression = null;
            Assert.Throws<ArgumentNullException>(() => _calculator.CalculateFile(expression));
        }
        [Test]
        public void CalculateFile_NullPath_Exception()
        {
            string path = null;
            Assert.Throws<ArgumentNullException>(() => _calculator.CalculateFile(path));
        }
        [Test]
        public void CalculateFile_NotExistFile_Exception()
        {
            string path = "df";
            Assert.Throws<FileNotFoundException>(() => _calculator.CalculateFile(path));
        }
    }
}