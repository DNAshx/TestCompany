using Company.SalaryModule.Classes;
using System.Collections.Generic;

namespace Company.SalaryModule.Storages.Interfaces
{
    public interface IEmployeeStorage
    {
        EmployeeBase GetEmployeByName(string name);

        List<EmployeeBase> GetAllEmployee();

        void SaveEmployee(EmployeeBase employee);
    }
}
