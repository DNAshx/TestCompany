using Company.SalaryModule.Classes;
using System.Collections.Generic;

namespace Company.SalaryModule.Services.Interfaces
{
    public interface IEmployeeService
    {
        List<EmployeeBase> GetAllEmployees();

        EmployeeBase GetEmployeeByName(string name);
    }
}
