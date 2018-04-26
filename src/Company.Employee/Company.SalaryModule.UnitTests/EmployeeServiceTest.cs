using System;
using System.Collections.Generic;
using System.Linq;
using Company.SalaryModule.Classes;
using Company.SalaryModule.Constants;
using Company.SalaryModule.Services;
using Company.SalaryModule.Storages.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using XAssert = Xunit.Assert;

namespace Company.SalaryModule.UnitTests
{
    [TestClass]
    public class EmployeeServiceTest
    {
        private IEmployeeStorage _employeeStorage;
        private EmployeeService _service;

        public EmployeeServiceTest()
        {
            _employeeStorage = NSubstitute.Substitute.For<IEmployeeStorage>();
            _service = new EmployeeService(_employeeStorage);            
        }

        [Fact]
        public void GetAllEmployees_Success()
        {
            //Arrange
            var result = new List<EmployeeBase>();
            var expectedList = new List<EmployeeBase>()
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
            _employeeStorage.GetAllEmployee().ReturnsForAnyArgs(expectedList);

            //Act
            result = _service.GetAllEmployees();

            //Assert
            Assert.AreEqual(expectedList.Count, result.Count);
        }

        [Fact]
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
            _employeeStorage.GetAllEmployee().ReturnsForAnyArgs(workersList);
            var expectedSalary = workersList[0].BaseSalary * (1 + 2 * AdditionsConstants.EMPL_ADDITION) +
                workersList[1].BaseSalary * (1 + AdditionsConstants.MNG_ADDITION) +
                workersList[2].BaseSalary * (1 + 3*AdditionsConstants.SLS_ADDITION);

            //Act
            var actualSalary = _service.GetSalaryOfAllCompany();

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }

        [Fact]
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
            var actualSalary = _service.GetActualSalary(employee);

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }

        [Fact]
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
            var actualSalary = _service.GetActualSalary(employee, DateTime.Today.AddYears(-2));

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }

        [Fact]
        public void GetActualSalary_Employee_Error()
        {
            //Arrange
            
            //Act
            Action act = () => _service.GetActualSalary((EmployeeBase)null);

            //Assert
            XAssert.Throws<ArgumentNullException>(act);
        }

        [Fact]
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
            var actualSalary = _service.GetActualSalary(manager);

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }

        [Fact]
        public void GetActualSalary_Manager_Error()
        {
            //Arrange

            //Act
            Action act = () => _service.GetActualSalary((Manager)null);

            //Assert
            XAssert.Throws<ArgumentNullException>(act);
        }

        [Fact]
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
            var actualSalary = _service.GetActualSalary(sales);

            //Assert
            Assert.AreEqual(expectedSalary, actualSalary);
        }

        [Fact]
        public void GetActualSalary_Sales_Error()
        {
            //Arrange

            //Act
            Action act = () => _service.GetActualSalary((Sales)null);

            //Assert
            XAssert.Throws<ArgumentNullException>(act);
        }
    }
}
