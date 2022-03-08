using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorProject
{
    public class ConsoleUI : IUserInterface
    {
        public string Input()
        {
            return Console.ReadLine();
        }

        public void Print(string sentence)
        {
            Console.WriteLine(sentence);
        }
    }
}
