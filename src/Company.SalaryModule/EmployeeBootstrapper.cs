using Company.SalaryModule.Services;
using Company.SalaryModule.Services.Interfaces;
using Company.SalaryModule.Storages;
using Company.SalaryModule.Storages.Interfaces;
using SimpleInjector;

namespace Company.SalaryModule
{
    public class EmployeeBootstrapper
    {
        public readonly Container Container;

        public EmployeeBootstrapper()
        {
            Container = new Container();

            Initialize();
        }

        private void Initialize()
        {
            Container.Register<ICompanySalaryService, SalaryService>(Lifestyle.Singleton);
            
            Container.Register<IEmployeeStorage, EmployeeStorage>(Lifestyle.Singleton);
            Container.Register<ICompanyStorage, CompanyStorage>(Lifestyle.Singleton);
        }
    }
}
