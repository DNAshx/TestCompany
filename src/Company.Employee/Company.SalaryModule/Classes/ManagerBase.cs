using System;
using System.Collections.Generic;

namespace Company.SalaryModule.Classes
{
    public class ManagerBase : EmployeeBase
    {
        private List<EmployeeBase> _subordinatesList = new List<EmployeeBase>();

        public ManagerBase(string name, decimal baseSalary, DateTime startWorkingDate)
            : base(name, baseSalary, startWorkingDate)
        {
        }

        public void AddSubordinate(EmployeeBase subordinate)
        {
            _subordinatesList.Add(subordinate);
        }

        public List<EmployeeBase> GetSubordinates()
        {
            return _subordinatesList;
        }
        
        protected decimal GetSubordinatesSalary(DateTime salaryDate)
        {
            var sumSalary = 0m;
            _subordinatesList.ForEach(empl => sumSalary += empl.CalculateActualSalary(salaryDate));

            return sumSalary;
        }
    }
}
