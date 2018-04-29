using Company.SalaryModule.Classes;
using System;

namespace Company.SalaryModule.Services.Interfaces
{
    public interface ICompanySalaryService
    {
        decimal GetSalaryOfAllCompany();

        decimal GetActualSalaryOfAnyType(EmployeeBase employee, DateTime? salaryDate = null);
    }
}
