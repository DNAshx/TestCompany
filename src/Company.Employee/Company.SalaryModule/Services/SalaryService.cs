using Company.SalaryModule.Classes;
using Company.SalaryModule.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Company.SalaryModule.Services
{
    public class SalaryService : ICompanySalaryService
    {
        public decimal GetSalaryOfAllCompany(CompanyObject company)
        {
            var emplList = new List<EmployeeBase>(company.Employees.Values);

            return emplList.Sum(e => e.CalculateActualSalary());
        }
    }
}
