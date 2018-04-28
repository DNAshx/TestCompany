using Company.SalaryModule.Classes;
using System;

namespace Company.SalaryModule.Services.Interfaces
{
    public interface IEmployeeSalaryService
    {
        decimal GetEmployeeActualSalary(Employee employee, DateTime? salaryDate = null);
    }
}
