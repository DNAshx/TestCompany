using Company.SalaryModule.Classes;
using Company.SalaryModule.Constants;
using Company.SalaryModule.Services;
using Company.SalaryModule.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using XAssert = Xunit.Assert;

namespace Company.SalaryModule.UnitTests.Services
{
    [TestClass]
    public class SalaryServiceTest
    {
        private IEmployeeService _employeeService;
        private SalaryService _service;

        public SalaryServiceTest()
        {
            _employeeService = Substitute.For<IEmployeeService>();
            _service = new SalaryService(_employeeService);
        }

        [TestMethod]
        public void GetActualSalaryOfAllCompany_Success()
        {
            //Arrange
            var workersList = new List<EmployeeBase>()
            {
                new Employee()
                {
                    Name = "Employee 1",
                    BaseSalary = 200,
                    StartWorkingDate = DateTime.Today.AddYears(-2)
                },
                new Manager()
                {
                    Name = "Manager 1",
                    BaseSalary = 500,
                    StartWorkingDate = DateTime.Today.AddYears(-1)
                },
                new Sales()
                {
                    Name = "Sales 1",
                    BaseSalary = 350,
                    StartWorkingDate = DateTime.Today.AddYears(-3)
                }
            };
            //salary = sum(base + base * (0.0*(kpi) * workYears))
            _employeeService.GetAllEmployees().ReturnsForAnyArgs(workersList);
            var expectedSalary = workersList[0].BaseSalary * (1 + 2 * AdditionsConstants.EMPL_ADDITION) +
                workersList[1].BaseSalary * (1 + AdditionsConstants.MNG_ADDITION) +
                workersList[2].BaseSalary * (1 + 3 * AdditionsConstants.SLS_ADDITION);

            //Act
            var actualSalary = _service.GetSalaryOfAllCompany();

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }

        [TestMethod]
        public void GetActualSalary_Employee_Success()
        {
            //Arrange
            var employee = new Employee()
            {
                Name = "Employee 1",
                BaseSalary = 200,
                StartWorkingDate = DateTime.Today.AddYears(-1)
            };
            //salary = base + base * (0.03(kpi) * workYears)
            var expectedSalary = employee.BaseSalary +
                Decimal.Multiply(employee.BaseSalary, AdditionsConstants.EMPL_ADDITION);

            //Act
            var actualSalary = _service.GetActualSalaryOfAnyType(employee);

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }

        [TestMethod]
        public void GetActualSalary_EmployeeByDate_Success()
        {
            //Arrange
            var employee = new Employee()
            {
                Name = "Employee 1",
                BaseSalary = 200,
                StartWorkingDate = DateTime.Today.AddYears(-3)
            };
            //salary = base + base * (0.03(kpi) * workYears)
            var expectedSalary = employee.BaseSalary +
                Decimal.Multiply(employee.BaseSalary, AdditionsConstants.EMPL_ADDITION);

            //Act
            var actualSalary = _service.GetActualSalaryOfAnyType(employee, DateTime.Today.AddYears(-2));

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }

        [TestMethod]
        public void GetActualSalary_Employee_Error()
        {
            //Arrange

            //Act
            Action act = () => _service.GetActualSalaryOfAnyType((EmployeeBase)null);

            //Assert
            XAssert.Throws<ArgumentNullException>(act);
        }

        [TestMethod]
        public void GetActualSalary_Manager_Success()
        {
            //Arrange
            var manager = new Manager()
            {
                Name = "Manager 1",
                BaseSalary = 500,
                StartWorkingDate = DateTime.Today.AddYears(-3),
                SubordinatesList = new List<EmployeeBase>()
                {
                    new Employee()
                    {
                        Name = "Employee 1",
                        BaseSalary = 200
                    }
                }
            };
            //salary = base + base * (0.05(kpi) * workYears) + sumSuborders*1.005 (0.5%)
            var expectedSalary = manager.BaseSalary +
                Decimal.Multiply(manager.BaseSalary, 3 * AdditionsConstants.MNG_ADDITION) +
                Decimal.Multiply(manager.SubordinatesList.First().BaseSalary, AdditionsConstants.MNG_SUBADDITION);

            //Act
            var actualSalary = _service.GetActualSalaryOfAnyType(manager);

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }

        [TestMethod]
        public void GetActualSalary_Manager_Error()
        {
            //Arrange

            //Act
            Action act = () => _service.GetActualSalaryOfAnyType((Manager)null);

            //Assert
            XAssert.Throws<ArgumentNullException>(act);
        }

        [TestMethod]
        public void GetActualSalary_Sales_Success()
        {
            //Arrange
            var sales = new Sales()
            {
                Name = "Sales 1",
                BaseSalary = 350,
                StartWorkingDate = DateTime.Today.AddYears(-2),
                SubordinatesList = new List<EmployeeBase>()
                {
                    new Employee()
                    {
                        Name = "Employee 1",
                        BaseSalary = 200
                    }
                }
            };
            //salary = base + base * (0.01(kpi) * workYears) + sumSuborders*1.003 (0.3%)
            var expectedSalary = sales.BaseSalary +
                Decimal.Multiply(sales.BaseSalary, 2 * AdditionsConstants.SLS_ADDITION) +
                Decimal.Multiply(sales.SubordinatesList.First().BaseSalary, AdditionsConstants.SLS_SUBADDITION);

            //Act
            var actualSalary = _service.GetActualSalaryOfAnyType(sales);

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }

        [TestMethod]
        public void GetActualSalary_Sales_Error()
        {
            //Arrange

            //Act
            Action act = () => _service.GetActualSalaryOfAnyType((Sales)null);

            //Assert
            XAssert.Throws<ArgumentNullException>(act);
        }
    }
}
