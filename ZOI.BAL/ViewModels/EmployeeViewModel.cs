using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {
        public EmployeeViewModel()
        {
            this.employee = new Employee();
        }
        public Employee employee { get; set; }

        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> SubDepartmentList { get; set; }
        public IEnumerable<SelectListItem> DesignationList { get; set; }
        public IEnumerable<SelectListItem> ReportingTo { get; set; }
        public IEnumerable<SelectListItem> Role { get; set; }
        public IEnumerable<SelectListItem> Gender { get; set; }

    }
}
