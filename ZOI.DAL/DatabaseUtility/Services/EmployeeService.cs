using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;
using ZOI.BAL.Models;
using ZOI.DAL.DatabaseUtility.Interface;
using ZOI.BAL.Utilites;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZOI.BAL.DBContext;
using ZOI.BAL;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ZOI.DAL.DatabaseUtility.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        JsonResponse resp = new JsonResponse();

        public EmployeeService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public long GetUserID()
        {
            var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            return Convert.ToInt64(identity.Claims.Where(c => c.Type == "RoleID")
                  .Select(c => c.Value).SingleOrDefault());
        }

        public Employee GetData(int ID)
        {  //Used for finding Employee by Id

            if (ID != 0)
            {
                try
                {
                    return _context.Employee.AsNoTracking().Where(e => (e.ID) == ID).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public JsonResponse Summary()
        {   //Used for Employee Listing 
            //Stored Procedure Used for listing  Admin_GetEmployee
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetEmpList, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new Employee
                {
                    ID = a.Field<int>("ID"),
                    EmployeeCode = a.Field<string>("EmployeeCode"),
                    EmployeeName = a.Field<string>("EmployeeName"),
                    GenderName = a.Field<string>("Gender"),
                    Email = a.Field<string>("Email"),
                    MobileNo = a.Field<string>("MobileNo"),
                    Department = a.Field<string>("Department"),
                    SubDepartment = a.Field<string>("SubDepartment"),
                    ReportingName = a.Field<string>("ReportingTo"),
                    DOB = a.Field<DateTime>("DOB"),
                    PAN = a.Field<string>("PAN"),
                    CertificationNo = a.Field<string>("CertificationNo"),
                    Designation = a.Field<string>("Designation"),
                    Role = a.Field<string>("Role"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate"),
                    IsActiveText = a.Field<string>("Status")
                }).ToList();
            }
            catch
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        public IEnumerable<Employee> ListAll()
        {
            //Used for Employee Listing 
            //Stored Procedure Used for listing  Admin_GetEmployee
            DataSet data = new DataSet();
            IEnumerable<Employee> EmployeeList;
            try
            {                
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetEmpList, null, CommandType.StoredProcedure);
                EmployeeList = data.Tables[1].AsEnumerable().Select(a => new Employee
                {
                    ID = a.Field<int>("ID"),
                    EmployeeCode = a.Field<string>("EmployeeCode"),
                    EmployeeName = a.Field<string>("EmployeeName"),
                    GenderName = a.Field<string>("Gender"),
                    Email = a.Field<string>("Email"),
                    MobileNo = a.Field<string>("MobileNo"),
                    Department = a.Field<string>("Department"),
                    SubDepartment = a.Field<string>("SubDepartment"),
                    ReportingName = a.Field<string>("ReportingTo"),
                    DOB = a.Field<DateTime>("DOB"),
                    PAN = a.Field<string>("PAN"),
                    CertificationNo = a.Field<string>("CertificationNo"),
                    Designation = a.Field<string>("Designation"),
                    Role = a.Field<string>("Role"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate"),
                    IsActiveText = a.Field<string>("Status")
                }).ToList();
                return EmployeeList;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public JsonResponse AddUpdate(Employee model)
        {   //Used for inserting and updating Employee
            //Stored Procedure Used for insert Admin_InsertEmployee
            //Stored Procedure Used for update Admin_UpdateEmployee
            // Createdby and Modifiedby parameters are hardcoded as of now 
            model.CreatedBy = 1;
            model.ModifiedBy = 1;
            if (model.ID == 0)
            {
                if (!IsExsits(model.EmployeeCode, model.ID))
                {
                    try
                    {
                         SqlParameter[] ObjParams = new SqlParameter[] {
                         new SqlParameter("@EmployeeCode",model.EmployeeCode),
                         new SqlParameter("@FName", model.FName),
                         new SqlParameter("@MName", model.MName),
                         new SqlParameter("@LName", model.LName),
                         new SqlParameter("@Gender", model.Gender),
                         new SqlParameter("@Email", model.Email),
                         new SqlParameter("@MobileNo", model.MobileNo),
                         new SqlParameter("@DepartmentId",model.DepartmentID),
                         new SqlParameter("@SubDepartmentId", model.SubDepartmentID),
                         new SqlParameter("@ReportingTo", model.ReportingTo),
                         new SqlParameter("@DOB", model.DOB),
                         new SqlParameter("@PAN", model.PAN),
                         new SqlParameter("@CertificationNo", model.CertificationNo),                       
                         new SqlParameter("@DesignationId", model.DesignationId),
                         new SqlParameter("@RoleID", model.RoleId),
                         new SqlParameter("@CreatedBy",GetUserID())
                    };

                        new ADODataFunction().ExecuteNonQuery(Constants.Procedures.InsertEmployee, ObjParams, CommandType.StoredProcedure);
                        ObjParams = null;
                        resp.Status = "S";
                        resp.Message = "Data inserted successfully";

                    }
                    catch (Exception ex)
                    {
                        resp.Status = "F";
                        resp.Message = "Data not insert";
                        throw ex;
                    }
                }
                else
                {
                    resp.Status = "F";
                    resp.Message = "This data already exists";
                }
            }
            else
            {
                resp.Message = "Data updated failed";
                try
                {
                    SqlParameter[] ObjParams = new SqlParameter[] {
                        new SqlParameter("@Id ",model.ID),
                        new SqlParameter("@EmployeeCode",model.EmployeeCode),
                         new SqlParameter("@FName", model.FName),
                         new SqlParameter("@MName", model.MName),
                         new SqlParameter("@LName", model.LName),
                         new SqlParameter("@Gender", model.Gender),
                         new SqlParameter("@Email", model.Email),
                         new SqlParameter("@MobileNo", model.MobileNo),
                         new SqlParameter("@DepartmentId",model.DepartmentID),
                         new SqlParameter("@SubDepartmentId", model.SubDepartmentID),
                         new SqlParameter("@ReportingTo", model.ReportingTo),
                         new SqlParameter("@DOB", model.DOB),
                         new SqlParameter("@PAN", model.PAN),
                         new SqlParameter("@CertificationNo", model.CertificationNo),
                         new SqlParameter("@DesignationId", model.DesignationId),
                         new SqlParameter("@RoleID", model.RoleId),
                         new SqlParameter("@ModifiedBy",GetUserID())
                    };

                    new ADODataFunction().ExecuteNonQuery(Constants.Procedures.UpdateEmployee, ObjParams, CommandType.StoredProcedure);
                    ObjParams = null;
                    resp.Status = "S";
                    resp.Message = "Data updated Successfully";
                }
                catch (Exception ex)
                {
                    resp.Status = "F";
                    resp.Message = "Data not Updated";
                    throw ex;
                }
            }
            return resp;
        }




        public bool IsExsits(string name, int ID)
        {  // For Unique Validation of Employee Name
            bool IsExsits = true;
            if (_context.Employee.Where(e => e.EmployeeCode == name && e.ID != ID).FirstOrDefault() == null)
            {
                IsExsits = false;
            }
            return IsExsits;
        }

        public JsonResponse Deactivate(int ID, bool Status)
        {
            if (ID != 0)
            {
                resp.Message = "Data not deleted";
                try
                {
                    var model = GetData(ID);
                    model.IsActive = Status;
                    model.ModifiedOn = DateTime.Now;
                    model.ModifiedBy = GetUserID();
                    _context.Set<Employee>().Update(model);
                    int i = _context.SaveChanges();
                    if (i != 0)
                    {
                        resp.Status = "S";
                        resp.Message = "Data deleted successfully";
                    }
                }
                catch (Exception ex)
                {
                    resp.Status = "F";
                    resp.Message = "Data not deleted";
                    throw ex;
                }
            }
            else
            {
                resp.Message = "ID was not found";
            }
            return resp;
        }

        public IEnumerable<SelectListItem> GetRoleList()
        { //Role  Dropdown
            try
            {
                return _context.Role.AsNoTracking().OrderBy(e => e.Name).Where(e => e.IsActive == true).Select(e => new SelectListItem()
                {
                    Value = (e.RoleID).ToString(),
                    Text = e.Name
                }).ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
           
        }

        public IEnumerable<SelectListItem> GetReportingToList()
        {// Reporting to Dropdown
            try
            {
                return _context.Employee.AsNoTracking().OrderBy(e => e.EmployeeCode).Where(e => e.IsActive == true).Select(e => new SelectListItem()
                {
                    Value = (e.ID).ToString(),
                    Text = e.FName.Trim() + " " + e.MName.Trim() + " " + e.LName.Trim()
                }).ToList();

            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public IEnumerable<SelectListItem> GetDepartmentList()
        { //Department Dropdown' 
            try
            {
                return _context.Department.AsNoTracking().OrderBy(e => e.DepartmentName).Where(e => e.IsActive == true).Select(e => new SelectListItem()
                {
                    Value = (e.ID).ToString(),
                    Text = e.DepartmentName
                }).ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
           

        }

        public IEnumerable<SelectListItem> GetSubDepartmentList(int? DeptId)
        {// Sub Department dropdown
            try
            {
                return _context.SubDepartment.AsNoTracking().OrderBy(e => e.SubDepartmentName).Where(e => e.DepartmentId == DeptId && e.IsActive == true).Select(e => new SelectListItem()
                {
                    Value = (e.ID).ToString(),
                    Text = e.SubDepartmentName
                }).ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
           
        }

        public IEnumerable<SelectListItem> GetDesignationList()
        {//Designation dropdown

            try
            {
                return _context.Designation.AsNoTracking().OrderBy(e => e.DesignationName).Where(e => e.IsActive == true).Select(e => new SelectListItem()
                {
                    Value = (e.ID).ToString(),
                    Text = e.DesignationName
                }).ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
           
        }

        public IEnumerable<SelectListItem> GetGenderList()
        {//Geneder dropdown
            try
            {
                return _context.Enum.AsNoTracking().OrderBy(e => e.EnumCode).Where(e => e.EnumType == "Gender" && e.IsActive == true).Select(e => new SelectListItem()
                {
                    Value = (e.EnumValue).ToString(),
                    Text = e.EnumCode
                }).ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
           
        }


    }
}
