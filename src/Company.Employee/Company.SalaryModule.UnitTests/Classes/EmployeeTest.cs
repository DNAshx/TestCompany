using Company.SalaryModule.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Company.SalaryModule.UnitTests.Classes
{
    [TestClass]
    public class EmployeeTest
    {
        [TestMethod]
        public void Employee_CalculateActualSalary_Success()
        {
            //Arrange
            var employee = new Employee("Employee1", 200, DateTime.Today.AddYears(-1));
            var expectedSalary = 200 + 200 * Employee.EMPL_ADDITION;

            //Act
            var actualSalary = employee.CalculateActualSalary();

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }

        [TestMethod]
        public void Employee_CalcualteActualSalaryMaxValue_Success()
        {
            //Arrange
            var employee = new Employee("Employee", 200, DateTime.Today.AddYears(-12));
            var expectedSalary = 200 + 200 * Employee.EMPL_MAXYEARADD;

            //Act
            var actualSalary = employee.CalculateActualSalary();

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }
    }
}
