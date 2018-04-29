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
            Container.Register<ICompanySalaryService, SalaryService>(LifeCycle.Singleton);
            Container.Register<IEmployeeSalaryService, SalaryService>(LifeCycle.Singleton);
            Container.Register<IManagerSalaryService, SalaryService>(LifeCycle.Singleton);
            Container.Register<ISalesSalaryService, SalaryService>(LifeCycle.Singleton);

            Container.Register<IEmployeeService, EmployeeService>(LifeCycle.Singleton);

            Container.Register<IEmployeeStorage, EmployeeStorage>(LifeCycle.Singleton);            
        }
    }
}
