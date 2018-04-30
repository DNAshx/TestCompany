using Company.SalaryModule.IoC;
using Company.SalaryModule.Services;
using Company.SalaryModule.Services.Interfaces;
using Company.SalaryModule.Storages;
using Company.SalaryModule.Storages.Interfaces;

namespace Company.SalaryModule
{
    public class EmployeeBootstrapper
    {
        public readonly IContainer Container;

        public EmployeeBootstrapper()
        {
            Container = new SimpleIocContainer();

            Initialize();
        }

        private void Initialize()
        {
            Container.Register<ICompanySalaryService, SalaryService>();
            Container.Register<IEmployeeSalaryService, SalaryService>();
            Container.Register<IManagerSalaryService, SalaryService>();
            Container.Register<ISalesSalaryService, SalaryService>();

            Container.Register<IEmployeeService, EmployeeService>();

            Container.Register<IEmployeeStorage, EmployeeStorage>();
        }
    }
}
