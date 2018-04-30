﻿using Company.SalaryModule.Classes;
using System.Collections.Generic;

namespace Company.SalaryModule.Storages.Interfaces
{
    public interface IEmployeeStorage
    {       
        void SaveEmployee(EmployeeBase employee, CompanyObject company);
    }
}