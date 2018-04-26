using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.SalaryModule.Classes
{
    public class ManagerBase : EmployeeBase
    {
        private List<EmployeeBase> _subordinatesList;
        public List<EmployeeBase> SubordinatesList
        {
            get
            {
                return _subordinatesList == null ? (_subordinatesList = new List<EmployeeBase>()) : _subordinatesList;
            }
            set
            {
                _subordinatesList = value;
            }
        }
    }
}
