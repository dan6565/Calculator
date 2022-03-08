using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorProject
{
    public class ProgramCalculator
    {
        private IUserInterface _userInterface;
        private Calculator _calculator;
        public ProgramCalculator(IUserInterface userInterface)
        {
            _userInterface = userInterface;
            _calculator = new Calculator();
        }
        public void Start()
        {
            _userInterface.Print(
                "This program calculates easy mathematical expressions!\n" +
                "It has two modes of operation\n" +
                "1 - work with user interface\n" +
                "2 - calculate file");
            while (true)
            {
                _userInterface.Print("Select the mode and enter \"1\" or \"2\":");
                string mode = InputMode();
                if (mode == "1")
                {
                    CalculateFromUI();
                }
                else
                {
                    CalculateFile();
                }
                _userInterface.Print("Enter \"end\", if you want to exit");
                string answer = _userInterface.Input();
                if (answer == "end")
                {
                    break;
                }
            }
           
        }
        public void CalculateFromUI()
        {
            _userInterface.Print("You have chosen the user interface mode!\n" +
                    "To exit, enter \"end\".");
            while (true)
            {
                _userInterface.Print("Enter expression:");
                string expression = _userInterface.Input();
                if (expression == "end")
                {
                    break;
                }
                if (_calculator.TryCalculate(expression, out int result))
                {
                    _userInterface.Print(result.ToString());
                }
                else
                {
                    _userInterface.Print(_calculator.Error);
                }

            }
        }
        public void CalculateFile()
        {
            _userInterface.Print("You have chosen the calculate file mode!\n" +
                    "To exit, enter \"end\".");
            string path = InputPath();
            _calculator.CalculateFile(path);
            _userInterface.Print("File is calculated and saved in Result.txt, " +
                "it is located in the folder with program!");            
        }
        private string InputPath()
        {
            _userInterface.Print("Enter path to file:");
            string path = _userInterface.Input();
            while (true)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    if (File.Exists(path))
                    {
                        return path;
                    }
                    else
                    {
                        _userInterface.Print("File not found!");
                    }
                }
                else
                {
                    _userInterface.Print("Path can't be empty");
                }
                _userInterface.Print("Enter path again");
                path = _userInterface.Input();
            }
        }
        private string InputMode()
        {
            string input = _userInterface.Input();
            while (true)
            {
                if (input == "1" || input == "2")
                {
                    return input;
                }
                else
                {
                    _userInterface.Print("The mode is entered incorrectly\n" +
                        "Enter enter \"1\" or \"2\"");
                    input = _userInterface.Input();
                }
            }
        }
    }
}
