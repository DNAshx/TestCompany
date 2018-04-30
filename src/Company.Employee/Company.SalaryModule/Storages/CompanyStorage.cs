using System;
using System.Collections.Generic;
using Company.SalaryModule.Classes;
using Company.SalaryModule.Storages.Interfaces;

namespace Company.SalaryModule.Storages
{
    public class CompanyStorage : ICompanyStorage
    {
        private List<CompanyObject> _companyList = new List<CompanyObject>();

        public List<CompanyObject> GetAllCompanies()
        {
            return _companyList;
        }

        public void AddCompany(CompanyObject company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            _companyList.Add(company);
        }
    }
}
