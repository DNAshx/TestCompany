using System;

namespace Company.SalaryModule.Classes
{
    public class Sales : ManagerBase
    {
        //addition for each working year
        public const decimal SLS_ADDITION = 0.01m;
        //addition of subordinates salary
        public const decimal SLS_SUBADDITION = 0.003m;
        //max percent of salary which can be added
        public const decimal SLS_MAXYEARADD = 0.35m;

        public Sales(string name, decimal baseSalary, DateTime startWorkingDate) 
            : base(name, baseSalary, startWorkingDate)
        {
        }

        public override decimal CalculateActualSalary(DateTime? salaryDate = null)
        {

            if (!salaryDate.HasValue)
                salaryDate = DateTime.Now;

            //calculate all aditions for working years
            var result = _baseSalary * GetAdditionToSalaryInPercent(SLS_ADDITION, SLS_MAXYEARADD, salaryDate);

            //add percent of salary of all subordinates
            result += GetSubordinatesSalary(salaryDate.Value) * SLS_SUBADDITION;

            return result;

        }
    }
}
