using System;

namespace Company.SalaryModule.Classes
{
    public class Employee : EmployeeBase
    {
        public const decimal EMPL_ADDITION = 0.03m;
        public const decimal EMPL_MAXYEARADD = 0.3m;

        public Employee(string name, decimal baseSalary, DateTime startWorkingDate) 
            : base(name, baseSalary, startWorkingDate)
        {
        }

        public override decimal CalculateActualSalary(DateTime? salaryDate = null)
        {
            if (!salaryDate.HasValue)
                salaryDate = DateTime.Now;
            
            return _baseSalary * GetAdditionToSalaryInPercent(EMPL_ADDITION, EMPL_MAXYEARADD, salaryDate);
        }
    }
}
