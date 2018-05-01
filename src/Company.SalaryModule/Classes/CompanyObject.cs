using System;
using System.Collections.Generic;

namespace Company.SalaryModule.Classes
{
    public class CompanyObject
    {
        public string Name { get; private set; }

        public Dictionary<Guid, EmployeeBase> Employees { get; private set; }

        public CompanyObject(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Company name cann not be empty", nameof(name));
            }

            Name = name;
            Employees = new Dictionary<Guid, EmployeeBase>();
        }

        public void AddEmployeeWithSubordinates(EmployeeBase employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            Employees.Add(employee.Id, employee);
            if ((employee as ManagerBase) != null)
            {
                (employee as ManagerBase).GetSubordinates().ForEach(e => AddEmployeeWithSubordinates(e));
            }
        }
    }
}
