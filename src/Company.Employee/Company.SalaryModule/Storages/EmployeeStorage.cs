using Company.SalaryModule.Classes;
using Company.SalaryModule.Storages.Interfaces;
using System;

namespace Company.SalaryModule.Storages
{
    public class EmployeeStorage : IEmployeeStorage
    {
        private ICompanyStorage _companyStorage;

        public EmployeeStorage(ICompanyStorage companyStorage)
        {
            _companyStorage = companyStorage;

            Initilize();            
        }

        private void Initilize()
        {
            InitializeCompany();
            InitializeEmployees();
        }

        private void InitializeCompany()
        {
            _companyStorage.AddCompany(new CompanyObject("Company 1"));
            _companyStorage.AddCompany(new CompanyObject("Company 2"));            
        }

        private void InitializeEmployees()
        {
            var companyList = _companyStorage.GetAllCompanies();

            var empl = new EmployeeBase("1", 1, DateTime.Today);
            empl = new Employee("Employee 1 Comp1", 300, new DateTime(2013, 07, 22));
            companyList[0].AddEmployeeWithSubordinates(empl);

            empl = new Manager("Manager 1 Comp1", 500, new DateTime(2015, 03, 01));
            var subEmpl = new Employee("Employee 2 COmp1", 250, new DateTime(2017, 11, 20));
            ((ManagerBase)empl).AddSubordinate(subEmpl);
            companyList[0].AddEmployeeWithSubordinates(empl);

            empl = new Sales("Sales 1 Comp1", 400, new DateTime(2015, 03, 01));
            subEmpl = new Employee("Employee 3 Comp1", 350, new DateTime(2011, 10, 12));
            ((ManagerBase)empl).AddSubordinate(subEmpl);
            companyList[0].AddEmployeeWithSubordinates(empl);

            empl = new Employee("Employee 1 Comp 2", 300, new DateTime(2013, 07, 22));
            companyList[1].AddEmployeeWithSubordinates(empl);

            empl = new Manager("Manager 1 Comp2", 500, new DateTime(2015, 03, 01));
            companyList[1].AddEmployeeWithSubordinates(empl);            
        }

        public void SaveEmployee(EmployeeBase employee, CompanyObject company)
        {
            company.AddEmployeeWithSubordinates(employee);
        }
    }
}
