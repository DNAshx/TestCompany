using Company.SalaryModule.Classes;
using Company.SalaryModule.Constants;
using Company.SalaryModule.Enums;
using Company.SalaryModule.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Company.SalaryModule.Services
{
    public class SalaryService : ICompanySalaryService, IEmployeeSalaryService, IManagerSalaryService
    {
        private IEmployeeService _employeeService;

        public SalaryService(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public decimal GetSalaryOfAllCompany()
        {
            var salary = 0.0m;
            _employeeService.GetAllEmployees().ForEach(e => salary += GetActualSalary(e));

            return salary;
        }

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

            switch (employee.Type)
            {
                case EmployeeTypeEnum.Employee:
                    return GetActualSalary(employee as Employee, salaryDate);

                case EmployeeTypeEnum.Manager:
                    return GetManagerActualSalary(employee as Manager, salaryDate);

                case EmployeeTypeEnum.Sales:
                    return GetActualSalary(employee as Sales, salaryDate);
            }

            return employee.BaseSalary;
        }

        public decimal GetManagerActualSalary(Manager manager, DateTime? salaryDate = null)
        {
            if (!salaryDate.HasValue)
                salaryDate = DateTime.Now;

            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            //calculate all aditions for working years
            var result = manager.BaseSalary * GetAdditionToSalaryInPercent(manager.Type, manager.StartWorkingDate, salaryDate);

            //add percent of salary of all subordinates
            result += GetSubordinatesSalary(manager.SubordinatesList) * AdditionsConstants.MNG_SUBADDITION;

            return result;
        }

        public decimal GetSalesActualSalary(Sales sales, DateTime? salaryDate = null)
        {
            if (!salaryDate.HasValue)
                salaryDate = DateTime.Now;

            if (sales == null)
                throw new ArgumentNullException(nameof(sales));

            //calculate all aditions for working years
            var result = sales.BaseSalary * GetAdditionToSalaryInPercent(sales.Type, sales.StartWorkingDate, salaryDate);

            //add percent of salary of all subordinates
            result += GetSubordinatesSalary(sales.SubordinatesList) * AdditionsConstants.SLS_SUBADDITION;

            return result;
        }

        public decimal GetEmployeeActualSalary(Employee employee, DateTime? salaryDate = null)
        {
            if (!salaryDate.HasValue)
                salaryDate = DateTime.Now;

            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            return employee.BaseSalary * GetAdditionToSalaryInPercent(employee.Type, employee.StartWorkingDate, salaryDate);
        }

        #region private

        private decimal GetAdditionToSalaryInPercent(EmployeeTypeEnum employeeType, DateTime startDate, DateTime? salaryDate = null)
        {
            if (!salaryDate.HasValue)
                salaryDate = DateTime.Now;

            var percent = 0.0m;
            //we're adding 1 to each result as it's addition should contain 100% + %add
            switch (employeeType)
            {
                case EmployeeTypeEnum.Employee:
                    percent = Decimal.Multiply(CalculateWorkingYears(startDate, salaryDate), AdditionsConstants.EMPL_ADDITION);
                    return percent <= LimitsConstants.EMPL_MAXYEARADD ? percent + 1 : LimitsConstants.EMPL_MAXYEARADD + 1;

                case EmployeeTypeEnum.Manager:
                    percent = Decimal.Multiply(CalculateWorkingYears(startDate, salaryDate), AdditionsConstants.MNG_ADDITION);
                    return percent <= LimitsConstants.MNG_MAXYEARADD ? percent + 1 : LimitsConstants.MNG_MAXYEARADD + 1;

                case EmployeeTypeEnum.Sales:
                    percent = Decimal.Multiply(CalculateWorkingYears(startDate, salaryDate), AdditionsConstants.SLS_ADDITION);
                    return percent <= LimitsConstants.SLS_MAXYEARADD ? percent + 1 : LimitsConstants.SLS_MAXYEARADD + 1;
            }

            return 1;
        }

        private decimal GetSubordinatesSalaryAdditionByEmployeeType(ManagerBase manager)
        {
            var result = GetSubordinatesSalary(manager.SubordinatesList);

            switch (manager.Type)
            {
                case EmployeeTypeEnum.Manager:
                    return result * AdditionsConstants.MNG_SUBADDITION;

                case EmployeeTypeEnum.Sales:
                    return result * AdditionsConstants.SLS_SUBADDITION;
            }

            return 0;
        }

        private decimal GetSubordinatesSalary(List<EmployeeBase> SubordinatesList)
        {
            var sumSalary = 0m;
            SubordinatesList.ForEach(empl => sumSalary += GetActualSalary(empl));

            return sumSalary;
        }

        private int CalculateWorkingYears(DateTime startDate, DateTime? salaryDate = null)
        {
            if (!salaryDate.HasValue)
                salaryDate = DateTime.Now;

            if (startDate == DateTime.MinValue)
                return 0;

            var begining = new DateTime(1, 1, 1);
            var span = (DateTime)salaryDate - startDate;

            //return 0 if date in futures
            if (span.Milliseconds < 0)
                return 0;
            // Because we start at year 1 for the Gregorian
            // calendar, we must subtract a year here.
            int result = (begining + span).Year - 1;

            return result > 0 ? result : 0;
        }

        #endregion
    }
}
