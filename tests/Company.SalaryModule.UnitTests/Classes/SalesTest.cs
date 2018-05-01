using System;
using Company.SalaryModule.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Company.SalaryModule.UnitTests.Classes
{
    [TestClass]
    public class SalesTest
    {
        [TestMethod]
        public void SalesTest_CalculateActualSalary_Subordinates_Success()
        {
            //Arrange
            var sales = new Sales("Sales", 400, DateTime.Today.AddYears(-1));
            var subordinate = new Employee("Employee1", 200, DateTime.Today);
            sales.AddSubordinate(subordinate);

            var expectedSalary = 400 + 400 * Sales.SLS_ADDITION + 200 * Sales.SLS_SUBADDITION;

            //Act
            var actualSalary = sales.CalculateActualSalary();

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }

        [TestMethod]
        public void Sales_CalcualteActualSalary_Success()
        {
            //Arrange
            var sales = new Sales("Sales", 200, DateTime.Today.AddYears(-1));
            var expectedSalary = 200 + 200 * Sales.SLS_ADDITION;

            //Act
            var actualSalary = sales.CalculateActualSalary();

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }

        [TestMethod]
        public void Sales_CalcualteActualSalaryMaxValue_Success()
        {
            //Arrange
            var sales = new Sales("Sales", 200, DateTime.Today.AddYears(-37));
            var expectedSalary = 200 + 200 * Sales.SLS_MAXYEARADD;

            //Act
            var actualSalary = sales.CalculateActualSalary();

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }
    }
}
