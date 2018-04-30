using Company.SalaryModule.Classes;
using System.Collections.Generic;

namespace Company.SalaryModule.Storages.Interfaces
{
    public interface ICompanyStorage
    {
        List<CompanyObject> GetAllCompanies();

        void AddCompany(CompanyObject company);
    }
}
