using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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

namespace ZOI.BAL.Services
{
    public class ClientFamilyService : IClientFamilyService
    {
        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientFamilyService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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
        /// Fill the Group dropdown. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetDropdownGroups()
        {
            List<SelectListItem> groupList = new List<SelectListItem>();
            groupList = _context.ClientGroup.Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.GroupName
            }).ToList();
            return groupList;
        }

        /// <summary>
        /// Add Update Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse AddUpdate(ClientFamily model)
        {
            JsonResponse resp = new JsonResponse();
            try
            {
                //  If these condition true the data was not exsits in the database
                if (!IsExsits(model.FamilyName))
                {
                    //  If model.ID == 0 the data goes to the Add part.                
                    if (model.Id == 0)
                    {

                        model.IsActive = true;
                        model.CreatedOn = DateTime.Now;
                        model.CreatedBy = GetUserID();
                        _context.Set<ClientFamily>().Add(model);
                        int i = _context.SaveChanges();

                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Data_insert_success;
                        }
                        else
                        {
                            resp.Message = Constants.Service.Data_insert_failed;
                        }
                    }
                    //  Else data goes to the Update part.
                    else
                    {
                        resp.Message = Constants.Service.Data_Update_failed;
                        var models = GetData(model.Id);
                        if (models != null)
                        {
                            models.FamilyName = model.FamilyName;
                            models.FamilyShortName = model.FamilyShortName;
                            models.GroupID = model.GroupID;
                            models.AddressLine1 = model.AddressLine1;
                            models.AddressLine2 = model.AddressLine2;
                            models.CountryID = model.CountryID;
                            models.StateID = model.StateID;
                            models.CityID = model.CityID;
                            models.IsActive = true;
                            models.ModifiedOn = DateTime.Now;
                            models.ModifiedBy = GetUserID();
                            _context.Set<ClientFamily>().Update(models);
                            int i = _context.SaveChanges();
                            if (i != 0)
                            {
                                resp.Status = Constants.ResponseStatus.Success;
                                resp.Message = Constants.Service.Data_Update_success;
                            }
                        }
                    }
                }
                // The data was in the database so, It return the else part
                else
                {
                    resp.Message = Constants.ControllerMessage.Data_Exsists;
                }
            }
            catch (Exception)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        /// <summary>
        /// Data Is Exsits
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Boolean values</returns>
        public bool IsExsits(string name)
        {
            var model = _context.ClientFamily.Where(e => e.FamilyName == name).FirstOrDefault();
            //If the condition true the data with this name doestn't have the duplicate value in the database.
            if (model != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the model.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Return  model.</returns>
        public ClientFamily GetData(long ID)
        {
            ClientFamily clientFamily = new ClientFamily();
          
            try
            {
                if (ID != 0)
                {
                    DataSet data = new DataSet();
                    SqlParameter[] ObjParams = new SqlParameter[] {
                     new SqlParameter("@ID", ID)
                    };
                    data = new ADODataFunction().ExecuteDataset("dbo.Admin_GetClientFamilyDataFromID", ObjParams, CommandType.StoredProcedure);
                    DataTable clientGroupTable = data.Tables[0];
                    if (clientGroupTable != null && clientGroupTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < clientGroupTable.Rows.Count; i++)
                        {
                            clientFamily.Id = Convert.ToInt32(clientGroupTable.Rows[i]["ID"]);
                            clientFamily.FamilyName = clientGroupTable.Rows[i]["FamilyName"].ToString();
                            clientFamily.FamilyShortName = clientGroupTable.Rows[i]["FamilyShortName"].ToString();
                            clientFamily.GroupID = Convert.ToInt32(clientGroupTable.Rows[i]["GroupID"]);
                            clientFamily.AddressLine1 = clientGroupTable.Rows[i]["AddressLine1"].ToString();
                            clientFamily.AddressLine2 = clientGroupTable.Rows[i]["AddressLine2"].ToString();
                            clientFamily.CountryID = Convert.ToInt32(clientGroupTable.Rows[i]["CountryId"]);
                            clientFamily.StateID = Convert.ToInt32(clientGroupTable.Rows[i]["StateId"]);
                            clientFamily.CityID = Convert.ToInt32(clientGroupTable.Rows[i]["CityId"]);
                            clientFamily.Pincode = Convert.ToInt32(clientGroupTable.Rows[i]["PinCode"]);
                            clientFamily.Longitude = Convert.ToDecimal(clientGroupTable.Rows[i]["Longitude"]);
                            clientFamily.Latitude = Convert.ToDecimal(clientGroupTable.Rows[i]["Latitude"]);
                            clientFamily.IsActive = Convert.ToBoolean(clientGroupTable.Rows[i]["IsActive"]);
                            clientFamily.CreatedOn = Convert.ToDateTime(clientGroupTable.Rows[i]["CreatedOn"]);
                        }
                    }
                    return clientFamily;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Data Summary
        /// </summary>
        /// <returns>Jsonresponse</returns>
        public IEnumerable<ClientFamily> Summary()
        {
            DataSet data = new DataSet();
            IEnumerable<ClientFamily> clientfamily;
            try
            {
                data = new ADODataFunction().ExecuteDataset("dbo.Admin_GetClientFamilySummary", null, CommandType.StoredProcedure);

                clientfamily = data.Tables[0].AsEnumerable().Select(dataRow => new ClientFamily
                {
                    Id = dataRow.Field<long>("Id"),
                    FamilyName = dataRow.Field<string>("FamilyName"),
                    FamilyShortName = dataRow.Field<string>("FamilyShortName"),
                    Groups = dataRow.Field<string>("GroupName"),
                    AddressLine1 = dataRow.Field<string>("AddressLine1"),
                    AddressLine2 = dataRow.Field<string>("AddressLine2"),
                    Countries = dataRow.Field<string>("CountryName"),
                    States = dataRow.Field<string>("StateName"),
                    Cities = dataRow.Field<string>("CityName"),
                    Pincode = dataRow.Field<long>("Pincode"),
                    Longitude = dataRow.Field<decimal>("Longitute"),
                    Latitude = dataRow.Field<decimal>("Latitude"),
                    IsActive = dataRow.Field<bool>("IsActive"),
                    LastUpdatedDate = dataRow.Field<string>("CreatedOn")

                }).ToList();
                return clientfamily;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Change the Status
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse Deactivate(long ID,bool Status)
        {
            JsonResponse resp = new JsonResponse();
            try
            {
                //If the ID was not null It goes to the change status part.
                if (ID != 0)
                {
                    var model = GetData(ID);
                    if (model!=null)
                    {
                        model.IsActive = Status;
                        model.ModifiedOn = DateTime.Now;
                        model.ModifiedBy = GetUserID();
                        _context.Set<ClientFamily>().Update(model);
                        int i = _context.SaveChanges();
                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Status_changed_success;
                        }
                    }                   
                    // Else it Show the error message.
                    else
                    {
                        resp.Message = Constants.Service.Common_message;
                    }
                }
                else
                {
                    resp.Message = Constants.Service.Common_message;
                }
            }
            catch
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;

        }

    }
}
