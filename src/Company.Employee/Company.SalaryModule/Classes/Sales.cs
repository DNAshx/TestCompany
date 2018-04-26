using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.SalaryModule.Enums;

namespace Company.SalaryModule.Classes
{
    public class Sales : ManagerBase
    {
        public override EmployeeTypeEnum Type { get => EmployeeTypeEnum.Sales; }
    }
}
