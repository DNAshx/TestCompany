using Company.SalaryModule.Classes;
using Company.SalaryModule.Storages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Company.SalaryModule.Storages
{
    public class EmployeeStorage : IEmployeeStorage
    {
        private List<EmployeeBase> _employeesList;

        public EmployeeStorage()
        {
            _employeesList = new List<EmployeeBase>()
            {
                new Employee()
                {
                    Name = "Employee 1",
                    BaseSalary = 300,
                    StartWorkingDate = new DateTime(2013, 07, 22)
                },
                new Manager()
                {
                    Name = "Manager 1",
                    BaseSalary = 500,
                    StartWorkingDate = new DateTime(2015, 03, 01),
                    SubordinatesList = new List<EmployeeBase>()
                    {
                        new Employee()
                        {
                            Name = "Employee 2",
                            BaseSalary = 250,
                            StartWorkingDate = new DateTime(2017, 11, 20)
                        }
                    }
                },
                new Sales()
                {
                    Name = "Sales 1",
                    BaseSalary = 400,
                    StartWorkingDate = new DateTime(2015, 03, 01),
                    SubordinatesList = new List<EmployeeBase>()
                    {
                        new Employee()
                        {
                            Name = "Employee 3",
                            BaseSalary = 350,
                            StartWorkingDate = new DateTime(2011, 10, 12)
                        }
                    }
                }
            };
        }
        public List<EmployeeBase> GetAllEmployee()
        {
            return _employeesList;
        }

        public EmployeeBase GetEmployeByName(string name)
        {
            return _employeesList.FirstOrDefault(e => e.Name.ToUpper().Equals(name.ToUpper()));
        }

        public void SaveEmployee(EmployeeBase employee)
        {
            _employeesList.Add(employee);
        }
    }
}
