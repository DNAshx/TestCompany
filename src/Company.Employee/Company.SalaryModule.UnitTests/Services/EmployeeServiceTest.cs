using System;
using System.Collections.Generic;
using Company.SalaryModule.Classes;
using Company.SalaryModule.Services;
using Company.SalaryModule.Storages.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;


namespace Company.SalaryModule.UnitTests.Services
{
    [TestClass]
    public class EmployeeServiceTest
    {
        private IEmployeeStorage _employeeStorage;
        private EmployeeService _service;

        public EmployeeServiceTest()
        {
            _employeeStorage = Substitute.For<IEmployeeStorage>();
            _service = new EmployeeService(_employeeStorage);            
        }

        [TestMethod]
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
            CollectionAssert.AreEqual(expectedList, result);
        }   
    }
}
