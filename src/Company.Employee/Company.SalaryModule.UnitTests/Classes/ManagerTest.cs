using System;
using Company.SalaryModule.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Company.SalaryModule.UnitTests.Classes
{
    [TestClass]
    public class ManagerTest
    {
        [TestMethod]
        public void Manager_CalcualteActualSalary_Success()
        {
            //Arrange
            var manager = new Manager("Manager", 200, DateTime.Today.AddYears(-1));
            var expectedSalary = 200 + 200 * Manager.MNG_ADDITION;

            //Act
            var actualSalary = manager.CalculateActualSalary();

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }

        [TestMethod]
        public void Manager_CalcualteActualSalaryMaxValue_Success()
        {
            //Arrange
            var manager = new Manager("Manager", 200, DateTime.Today.AddYears(-10));
            var expectedSalary = 200 + 200 * Manager.MNG_MAXYEARADD;

            //Act
            var actualSalary = manager.CalculateActualSalary();

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }
    }
}
