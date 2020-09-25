using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.Models;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IEmployeeService
    {
        JsonResponse Summary();
        public IEnumerable<Employee> ListAll();
        Employee GetData(int ID);
        public bool IsExsits(string name, int ID);
        public JsonResponse Deactivate(int ID, bool Status);
       public JsonResponse AddUpdate(Employee model);
        public IEnumerable<SelectListItem> GetDepartmentList();
        public IEnumerable<SelectListItem> GetSubDepartmentList(int? DeptId);
        public IEnumerable<SelectListItem> GetRoleList();
        public IEnumerable<SelectListItem> GetReportingToList();
        public IEnumerable<SelectListItem> GetDesignationList();
        public IEnumerable<SelectListItem> GetGenderList();


    }
}
