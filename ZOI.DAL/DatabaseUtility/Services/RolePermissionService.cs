using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ZOI.BAL.DBContext;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.DAL;
using ZOI.DAL.DatabaseUtility;

namespace ZOI.BAL
{
    public class RolePermissionService : IRolePermissionService
    {
        JsonResponse resp = new JsonResponse();

        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public RolePermissionService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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


        /// <summary>
        /// Give the Permission on Menu master Based On Role ID.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResponse AddUpdate(List<MenuPermissionList> model)
        {
            try
            {   
                //  If model not null then it goes to the add part.
                if (model != null && model.Count>0)
                {
                    //  Convert the List into DataTable.
                    DataTable dt = ConvertToDataTable(model);
                    //  Set the parameter to pass the procedure.
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@TableParam", dt);
                    param[1] = new SqlParameter("@RoleID", model[0].RoleID);
                    param[2] = new SqlParameter("@UserID", GetUserID());
                    int i = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.RoleMenuPermission, param, CommandType.StoredProcedure);
                    //  If it return was not equal to 0 then it affect some rows means data inserted.
                    if (i != 0)
                    {
                        resp.Status = Constants.ResponseStatus.Success;
                        resp.Message = Constants.ControllerMessage.Permission_Changed_Success;
                    }
                    // Else Show the error message.
                    else
                    {
                        resp.Message = Constants.ControllerMessage.Permission_Changed_Failed;
                    }
                }
                else
                {
                    resp.Message = Constants.Service.No_Changes;
                }
            }
            //  Else the model was null it shown teh error message to user.
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }


        /// <summary>
        /// Summary of the model to to show in datatable
        /// </summary>
        /// <returns>Json response</returns>
        public JsonResponse Summary(int RoleID)
        {
            try
            {
                // If the RoleID Greter than 0 it move on to the get the summary
                if (RoleID > 0)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@RoleID", RoleID);
                    // Get the return data in DataSet.
                    DataSet data = new DataSet();
                    data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetMenusToSetPermission, param, CommandType.StoredProcedure);
                    //  StringBuilder is used to render the summary part of the Menu permission.
                    StringBuilder sb = new StringBuilder();
                    var body = "<tr>";
                    for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                    {
                        body += "<td colspan='2'><input type='hidden' class='hdn-menu-id' value="+ data.Tables[0].Rows[i]["MenuID"] + ">" + data.Tables[0].Rows[i]["MenuName"] + "</td>";
                        if (Convert.ToBoolean(data.Tables[0].Rows[i]["ReadAccess"]) == true)
                        {
                            body += "<td colspan='1'><input type='checkbox' class='checkbox chk_read checkbox-center' checked=true /></td>";
                        }
                        else
                        {
                            body += "<td colspan='1'><input type='checkbox' class='checkbox chk_read checkbox-center'  /></td>";
                        }
                        if (Convert.ToBoolean(data.Tables[0].Rows[i]["WriteAccess"]) == true)
                        {
                            body += "<td colspan='1'><input type='checkbox' class='checkbox chk_write checkbox-center' checked=true /></td>";
                        }
                        else
                        {
                            body += "<td colspan='1'><input type='checkbox' class='checkbox chk_write checkbox-center'  /></td>";
                        }
                        if (Convert.ToBoolean(data.Tables[0].Rows[i]["ExportCSV"]) == true)
                        {
                            body += "<td colspan='2'><input type='checkbox' class='checkbox chk_exportcsv checkbox-center' checked=true /></td>";
                        }
                        else
                        {
                            body += "<td colspan='2'><input type='checkbox' class='checkbox chk_exportcsv checkbox-center'  /></td>";
                        }
                        if (Convert.ToBoolean(data.Tables[0].Rows[i]["ExportExcel"]) == true)
                        {
                            body += "<td colspan='2'><input type='checkbox' class='checkbox chk_exportexcel checkbox-center' checked=true /></td>";
                        }
                        else
                        {
                            body += "<td colspan='2'><input type='checkbox' class='checkbox chk_exportexcel checkbox-center'  /></td>";
                        }
                        if (Convert.ToBoolean(data.Tables[0].Rows[i]["ExportPDF"]) == true)
                        {
                            body += "<td colspan='2'><input type='checkbox' class='checkbox chk_exportpdf checkbox-center' checked=true /></td>";
                        }
                        else
                        {
                            body += "<td colspan='2'><input type='checkbox' class='checkbox chk_exportpdf checkbox-center'  /></td>";
                        }        
                        
                        if (Convert.ToBoolean(data.Tables[0].Rows[i]["SettingsAccess"]) == true)
                        {
                            body += "<td colspan='2'><input type='checkbox' class='checkbox chk_settings checkbox-center' checked=true /></td>";
                        }
                        else                        
                        {
                            body += "<td colspan='2'><input type='checkbox' class='checkbox chk_settings checkbox-center'  /></td>";
                        }
                        body += "</tr>";
                    }
                    resp.Data = body;
                    resp.Status = Constants.ResponseStatus.Success;
                }
                else
                {
                    resp.Message = Constants.Service.Common_message;
                }
            }
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        /// <summary>
        /// Get the roles data from Roles Table to fill the dropdown
        /// </summary>
        /// <returns>JsonResponse</returns>
        public IEnumerable<SelectListItem> GetRolesData()
        {
            try
            {
                return _context.Role.AsNoTracking().Where(e => e.IsActive).Select(e => new SelectListItem()
                {
                    Value = (e.RoleID).ToString(),
                    Text = e.Name
                }).ToList();
            }
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return null;
        }

        /// <summary>
        /// Common metgod to convert the List to datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

    }
}
