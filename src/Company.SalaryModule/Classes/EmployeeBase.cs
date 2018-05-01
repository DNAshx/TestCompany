using System;

namespace Company.SalaryModule.Classes
{
    public class EmployeeBase
    {
        protected decimal _baseSalary;

        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public DateTime StartWorkingDate { get; private set; }

        public virtual decimal CalculateActualSalary(DateTime? salaryDate = null)
        {
            return _baseSalary;
        }

        /// <summary>
        /// Initialize employee without name, salary = 0 and startworkDate is today
        /// </summary>
        public EmployeeBase()
        {
            Id = Guid.NewGuid();
            Name = "";
            _baseSalary = 0;
            StartWorkingDate = DateTime.Today;
        }

        public EmployeeBase(string name, decimal baseSalary, DateTime startWorkingDate)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name can not be empty", nameof(name));
            }
            if (baseSalary <= 0)
            {
                throw new ArgumentException("Salary can not be 0 or less.", nameof(baseSalary));
            }

            Id = Guid.NewGuid();
            Name = name;
            _baseSalary = baseSalary;
            StartWorkingDate = startWorkingDate;            
        }

        protected decimal GetAdditionToSalaryInPercent(decimal yearAddition, decimal limitAddition, DateTime? salaryDate = null)
        {
            if (!salaryDate.HasValue)
                salaryDate = DateTime.Now;

            var percent = 0.0m;
            //we're adding 1 to each result as it's addition should contain 100% + %add
            percent = Decimal.Multiply(CalculateWorkingYears(StartWorkingDate, salaryDate), yearAddition);

            return percent <= limitAddition ? percent + 1 : limitAddition + 1;
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
