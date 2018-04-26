using Company.SalaryModule.Classes;
using Company.SalaryModule.Constants;
using Company.SalaryModule.Enums;
using Company.SalaryModule.Storages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.SalaryModule.Services
{
    public partial class EmployeeService
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

        public decimal GetSalaryOfAllCompany()
        {
            var salary = 0.0m;
            GetAllEmployees().ForEach(e => salary += GetActualSalary(e));

            return salary;
        }

        public EmployeeBase GetEmployeeByName(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException($"{nameof(name)} attribute has no value.");

            return _employeeStorage.GetEmployeByName(name);
        }

        #region GetActualSalary

        /// <summary>
        /// Get Actual sallary for now or specific date
        /// </summary>
        /// <param name="employee">current employee we looking salary for</param>
        /// <param name="salaryDate">optional: get salary for specific time</param>
        /// <returns></returns>
        public decimal GetActualSalary(EmployeeBase employee, DateTime? salaryDate = null)
        {
            if (!salaryDate.HasValue)
                salaryDate = DateTime.Now;

            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            switch(employee.Type)
            {
                case EmployeeTypeEnum.Employee:
                    return GetActualSalary(employee as Employee, salaryDate);

                case EmployeeTypeEnum.Manager:
                    return GetActualSalary(employee as Manager, salaryDate);

                case EmployeeTypeEnum.Sales:
                    return GetActualSalary(employee as Sales, salaryDate);
            }

            return employee.BaseSalary;
        }

        public decimal GetActualSalary(ManagerBase manager, DateTime? salaryDate = null)
        {
            if (!salaryDate.HasValue)
                salaryDate = DateTime.Now;

            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            //calculate all aditions for working years
            var result = manager.BaseSalary * GetAdditionToSalaryInPercent(manager.Type, manager.StartWorkingDate, salaryDate);

            //add percent of salary of all subordinates
            result += GetSubordinatesSalary(manager.SubordinatesList) * 
                (manager.Type == EmployeeTypeEnum.Manager ? AdditionsConstants.MNG_SUBADDITION 
                                                          : AdditionsConstants.SLS_SUBADDITION);

            return result;
        }

        public decimal GetActualSalary(Employee employee, DateTime? salaryDate = null)
        {
            if (!salaryDate.HasValue)
                salaryDate = DateTime.Now;

            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            return employee.BaseSalary * GetAdditionToSalaryInPercent(employee.Type, employee.StartWorkingDate, salaryDate);
        }

        #endregion GetActualSalary
    }
}
