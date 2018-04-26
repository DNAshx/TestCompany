using Company.SalaryModule.Classes;
using Company.SalaryModule.Constants;
using Company.SalaryModule.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.SalaryModule.Services
{
    public partial class EmployeeService
    {
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
    }
}
