using Company.SalaryModule.Classes;
using Company.SalaryModule.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Company.SalaryModule.UnitTests.Services
{
    [TestClass]
    public class SalaryServiceTest
    {
        private SalaryService _service;

        public SalaryServiceTest()
        {
            _service = new SalaryService();
        }

        [TestMethod]
        public void GetActualSalaryOfAllCompany_Success()
        {
            //Arrange
            var comp = new CompanyObject("Company1");
            comp.AddEmployeeWithSubordinates(new Employee("Employee 1", 200, DateTime.Today.AddYears(-2)));
            comp.AddEmployeeWithSubordinates(new Manager("Manager 1", 500, DateTime.Today.AddYears(-1)));
            comp.AddEmployeeWithSubordinates(new Sales("Sales 1", 350, DateTime.Today.AddYears(-3)));
            
            //salary = sum(base + base * (0.0*(kpi) * workYears))
            
            var expectedSalary = 200 * (1 + 2 * Employee.EMPL_ADDITION) +
                500 * (1 + Manager.MNG_ADDITION) +
                350 * (1 + 3 * Sales.SLS_ADDITION);

            //Act
            var actualSalary = _service.GetSalaryOfAllCompany(comp);

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }
    }
}
