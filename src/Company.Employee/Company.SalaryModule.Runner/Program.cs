using Company.SalaryModule.Classes;
using Company.SalaryModule.Enums;
using Company.SalaryModule.Services;
using Company.SalaryModule.Storages.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.SalaryModule.Runner
{
    class Program
    {
        /// <summary>
        /// Temprorary storage of IEmployeeStorage to manipulate with temprorary data
        /// </summary>
        class EmployeesStorage : IEmployeeStorage
        {
            private List<EmployeeBase> _employees = new List<EmployeeBase>();

            public List<EmployeeBase> GetAllEmployee()
            {
                return _employees;
            }

            public EmployeeBase GetEmployeByName(string name)
            {
                return _employees.FirstOrDefault(e => e.Name.Equals(name));
            }

            public void SaveEmployee(EmployeeBase employee)
            {
                _employees.Add(employee);
            }
        }

        private static EmployeesStorage _storage;

        static void Main(string[] args)
        {
            //Initializing
            _storage = new EmployeesStorage();

            //get and add employees to the storage
            var flag = false;
            while (!flag)
            {
                var empl = GetEmployee();
                AddEmployee(empl);

                //option to add as much employees as you want
                var result = string.Empty;
                while (result.ToUpper().Equals("Y") || result.ToUpper().Equals("N"))
                {
                    Console.Write("New employee? (Y/N):");
                    result = Console.ReadLine();
                    flag = result.ToUpper().Equals("N");
                }
            }
            
            //var service = new SalaryService(_storage);

            //print results
           // DrawResults(service);

            Console.ReadLine();
        }

        public static EmployeeBase GetEmployee()
        {
            var enterData = string.Empty;
            var employee = new EmployeeBase();

            //name
            Console.Write("Enter employee name:");
            employee.Name = Console.ReadLine();

            //start working date
            employee.StartWorkingDate = GetData<DateTime>("start date", "dd/MM/YYYY");

            //salary base
            employee.BaseSalary = GetData<decimal>("base salary", "1234.49");

            //type
            employee.Type = GetEmployeeType();

            if (employee.Type == EmployeeTypeEnum.Manager || employee.Type == EmployeeTypeEnum.Sales)
            {
                var mngr = employee.Type == EmployeeTypeEnum.Manager ? (ManagerBase)MapToManager(employee) 
                                                                     : (ManagerBase)MapToSales(employee);                

                var flag = false;
                while (!flag)
                {
                    Console.Write("Do you wanna add subordinate?(Y/N):");
                    var result = Console.ReadLine();
                    flag = result.ToUpper().Equals("N");
                    if (!flag)
                        mngr.SubordinatesList.Add(GetEmployee());
                }
                mngr.SubordinatesList.ForEach(s => _storage.SaveEmployee(s));

                return mngr;
            }
            else if (employee.Type == EmployeeTypeEnum.Employee)
            {
                return MapToEmployee(employee);
            }

            return employee;
        }

        private static void AddEmployee(EmployeeBase employee)
        {
            if (employee == null)
                return;

            _storage.SaveEmployee(employee);
        }

        private static EmployeeTypeEnum GetEmployeeType()
        {
            var flag = false;

            while (!flag)
            {
                Console.Write($"Select employee type (M)anager, (E)mployee, (S)ales:");

                var emplType = (char)Console.ReadLine().FirstOrDefault();
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

        private static void DrawResults(SalaryService service)
        {
            //output results
            DrawTable.PrintRow(new string[] { "Name", "Salary", "Start Date", "Type" });
            DrawTable.PrintRow(new string[] { "", "", "", "" });
            foreach (var empl in _storage.GetAllEmployee())
            {
                DrawTable.PrintRow(new string[] { empl.Name,
                    string.Format("{0:0.00}", service.GetActualSalary(empl)),
                    empl.StartWorkingDate.ToString("dd/MM/yyyy"),
                    empl.Type.ToString() });
            }
            var sum = string.Format("{0:0.00}", service.GetSalaryOfAllCompany());
            Console.WriteLine($"Total: {sum}");
        }

        #region Mapping

        private static Manager MapToManager(EmployeeBase employee)
        {
            return new Manager()
            {
                Name = employee.Name,
                BaseSalary = employee.BaseSalary,
                StartWorkingDate = employee.StartWorkingDate,                
                SubordinatesList = new List<EmployeeBase>(),
                Type = EmployeeTypeEnum.Manager
            };
        }

        private static Sales MapToSales(EmployeeBase employee)
        {
            return new Sales()
            {
                Name = employee.Name,
                BaseSalary = employee.BaseSalary,
                StartWorkingDate = employee.StartWorkingDate,
                SubordinatesList = new List<EmployeeBase>(),
                Type = EmployeeTypeEnum.Sales
            };
        }

        private static Employee MapToEmployee(EmployeeBase employee)
        {
            return new Employee()
            {
                Name = employee.Name,
                BaseSalary = employee.BaseSalary,
                StartWorkingDate = employee.StartWorkingDate,
                Type = EmployeeTypeEnum.Employee
            };
        }

        #endregion
    }
}
