using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Services
{
    public class TaxService
    {

        public decimal GetTax(decimal grossSalary)
        {
            if (grossSalary < 10000)
            {
                return 1000;
            }
            return 2000;
        }
    }
}
