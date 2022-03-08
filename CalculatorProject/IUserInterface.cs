using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorProject
{
    public interface IUserInterface
    {
        string Input();
        void Print(string sentence);
    }
}
