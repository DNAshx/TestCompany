using Company.SalaryModule.Classes;
using System;

namespace Company.SalaryModule.Services.Interfaces
{
    interface ISalesSalaryService
    {
        decimal GetSalesActualSalary(Sales sales, DateTime? salaryDate = null);
    }
}
