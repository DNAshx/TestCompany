using Company.SalaryModule.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.SalaryModule.Classes
{
    public class EmployeeBase
    {
        public string Name { get; set; }

        public DateTime StartWorkingDate { get; set; }

        public decimal BaseSalary { get; set; }

        public virtual EmployeeTypeEnum Type { get; set; }
    }
}
