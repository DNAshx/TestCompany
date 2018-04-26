﻿using Company.SalaryModule.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.SalaryModule.Classes
{
    public class Manager : ManagerBase
    {
        public override EmployeeTypeEnum Type { get => EmployeeTypeEnum.Manager; }
    }
}
