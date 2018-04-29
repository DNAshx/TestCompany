using Company.SalaryModule.Classes;
using Company.SalaryModule.Services.Interfaces;
using Company.SalaryModule.Storages.Interfaces;
using System;
using System.Collections.Generic;

namespace Company.SalaryModule.Services
{
    public partial class EmployeeService : IEmployeeService
    {
        private IEmployeeStorage _employeeStorage;

        public EmployeeService(IEmployeeStorage employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public List<EmployeeBase> GetAllEmployees()
        {
            return _employeeStorage.GetAllEmployee();
        }

        public EmployeeBase GetEmployeeByName(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException($"{nameof(name)} attribute has no value.");

            return _employeeStorage.GetEmployeByName(name);
        }       

        public void SaveEmployee(EmployeeBase employee)
        {
            _employeeStorage.SaveEmployee(employee);
        }
    }
}
