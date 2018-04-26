using Company.SalaryModule.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.SalaryModule.Storages.Interfaces
{
    public interface IEmployeeStorage
    {
        EmployeeBase GetEmployeByName(string name);

        List<EmployeeBase> GetAllEmployee();
    }
}
