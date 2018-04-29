using Company.SalaryModule.Classes;
using System;

namespace Company.SalaryModule.Services.Interfaces
{
    public interface ISalesSalaryService
    {
        decimal GetSalesActualSalary(Sales sales, DateTime? salaryDate = null);
    }
}
