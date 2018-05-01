using System;

namespace Company.SalaryModule.Classes
{
    public class Manager : ManagerBase
    {
        //addition for each working year
        public const decimal MNG_ADDITION = 0.05m;
        //addition of subordinates salary
        public const decimal MNG_SUBADDITION = 0.005m;
        //max percent of salary which can be added
        public const decimal MNG_MAXYEARADD = 0.4m;

        public Manager(string name, decimal baseSalary, DateTime startWorkingDate) 
            : base(name, baseSalary, startWorkingDate)
        {
        }

        public override decimal CalculateActualSalary(DateTime? salaryDate = null)
        {
            
            if (!salaryDate.HasValue)
                salaryDate = DateTime.Now;

            //calculate all aditions for working years
            var result = _baseSalary * GetAdditionToSalaryInPercent(MNG_ADDITION, MNG_MAXYEARADD, salaryDate);

            //add percent of salary of all subordinates
            result += GetSubordinatesSalary(salaryDate.Value) * MNG_SUBADDITION;

            return result;
        }
    }
}
