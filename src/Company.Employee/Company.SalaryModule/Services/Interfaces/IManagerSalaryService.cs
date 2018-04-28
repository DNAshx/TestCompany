using Company.SalaryModule.Classes;
using System;

namespace Company.SalaryModule.Services.Interfaces
{
    public interface IManagerSalaryService
    {
        decimal GetManagerActualSalary(Manager manager, DateTime? salaryDate = null);
    }
}
