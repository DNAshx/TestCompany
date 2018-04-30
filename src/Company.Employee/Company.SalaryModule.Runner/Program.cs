using Company.SalaryModule.Classes;
using Company.SalaryModule.Services.Interfaces;
using Company.SalaryModule.Storages.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Container = SimpleInjector.Container;

namespace Company.SalaryModule.ConsoleRunner
{
    class Program
    {
        private static Container _container;
        private static CompanyObject _currentCompany;

        static void Main(string[] args)
        {
            //Initializing
            var bootStrapper = new EmployeeBootstrapper();
            _container = bootStrapper.Container;
            var employee = _container.GetInstance<IEmployeeStorage>();
            var companyStorage = _container.GetInstance<ICompanyStorage>();

            //get and add employees to the storage
            var flag = false;
            while (!flag)
            {
                var result = "";
                

                //option to add as much employees as you want
                while (!result.ToUpper().Equals("Y") && !result.ToUpper().Equals("N"))
                {
                    Console.Write("New employee? (Y/N):");
                    result = Console.ReadLine();
                    flag = result.ToUpper().Equals("N");
                }

                if (flag)
                    break;

                while (!result.ToUpper().Equals("1") && !result.ToUpper().Equals("2"))
                {
                    Console.Write("There are 2 companies. Select one (1,2):");
                    result = Console.ReadLine();
                    if (result.Equals("1"))
                        _currentCompany = companyStorage.GetAllCompanies()[0];
                    if (result.Equals("2"))
                        _currentCompany = companyStorage.GetAllCompanies()[1];
                }
                Console.WriteLine($"You selected '{_currentCompany.Name}'.");

                var empl = GetEmployee();
                AddEmployee(empl);
            }
            
            //print results
            DrawResults();

            Console.ReadLine();
        }

        public static EmployeeBase GetEmployee()
        {
            //name
            Console.Write("Enter employee name:");
            var name = Console.ReadLine();

            //start working date
            var startWorkingDate = GetData<DateTime>("start date", "MM/dd/YYYY");

            //salary base
            var baseSalary = GetData<decimal>("base salary", "1234.49");

            //type
            var employeeType = GetEmployeeType();

            var employee = new EmployeeBase();

            switch (employeeType)
            {
                case EmployeeTypeEnum.Employee:
                    employee = new Employee(name, baseSalary, startWorkingDate);
                    break;
                case EmployeeTypeEnum.Manager:
                    employee = new Manager(name, baseSalary, startWorkingDate);
                    break;
                case EmployeeTypeEnum.Sales:
                    employee = new Sales(name, baseSalary, startWorkingDate);
                    break;
            }

            if (employeeType == EmployeeTypeEnum.Manager || employeeType == EmployeeTypeEnum.Sales)
            { 
                var flag = false;
                while (!flag)
                {
                    Console.Write("Do you wanna add subordinate?(Y/N):");
                    var result = Console.ReadLine();
                    flag = result.ToUpper().Equals("N");
                    if (!flag)
                    {
                        ((ManagerBase)employee).AddSubordinate(GetEmployee());
                    }
                }                
            }

            return employee;
        }

        private static void AddEmployee(EmployeeBase employee)
        {
            _currentCompany.AddEmployeeWithSubordinates(employee);
        }

        private static EmployeeTypeEnum GetEmployeeType()
        {
            var flag = false;

            while (!flag)
            {
                Console.Write($"Select employee type (M)anager, (E)mployee, (S)ales:");

                var emplType = Console.ReadLine().FirstOrDefault();
                switch (Char.ToUpper(emplType))
                {
                    case 'M':
                        return EmployeeTypeEnum.Manager;

                    case 'E':
                        return EmployeeTypeEnum.Employee;

                    case 'S':
                        return EmployeeTypeEnum.Sales;
                }

                Console.Write($"Incorrect type (M)anager, (E)mployee, (S)ales (Enter - continue, Esq to exit)");

                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                    return EmployeeTypeEnum.Empty;
            }

            return EmployeeTypeEnum.Empty;
        }

        private static T GetData<T>(string dataName, string formatString)
        {
            var flag = false;
            T result = default(T);

            while (!flag)
            {
                Console.Write($"Enter {dataName} ({formatString}):");
                var enterData = Console.ReadLine();

                result = Convert<T>(enterData);
                flag = Comparer<T>.Default.Compare(result, default(T)) > 0;

                if (!flag)
                {
                    Console.WriteLine($"Incorrect {dataName} format ({formatString}) (Enter - continue, Esq to exit)");
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                        return default(T);
                }
            }

            return result;
        }

        private static T Convert<T>(string input)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));            

            if (converter != null)
            {
                //Cast ConvertFromString(string text) : object to (T)
                try
                {                    
                    return (T)converter.ConvertFromString(input);
                }
                catch(FormatException ex)
                {
                    Console.WriteLine("Error format: " + ex.Message);
                    return default(T);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex.Message);
                    return default(T);
                }
            }

            return default(T);
        }

        private static void DrawResults()
        {
            //output results
            DrawTable.PrintRow(new string[] { "Name", "Salary", "Start Date", "Company" });
            DrawTable.PrintRow(new string[] { "", "", "", "" });

            var companyStorage = _container.GetInstance<ICompanyStorage>();
            var compSalaryService = _container.GetInstance<ICompanySalaryService>();

            foreach (var comp in companyStorage.GetAllCompanies())
            {
                foreach (var empl in comp.Employees)
                {
                    DrawTable.PrintRow(new string[] { empl.Value.Name,
                    string.Format("{0:0.00}", empl.Value.CalculateActualSalary()),
                    empl.Value.StartWorkingDate.ToString("dd/MM/yyyy"),
                    comp.Name });
                }

                var sum = string.Format("{0:0.00}", compSalaryService.GetSalaryOfAllCompany(comp));
                Console.WriteLine($"Total: {sum}");
            }            
        }
    }
}
