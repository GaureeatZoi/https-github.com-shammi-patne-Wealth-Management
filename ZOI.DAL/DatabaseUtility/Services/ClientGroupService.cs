using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
    public class ClientGroupService : IClientGroupService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DatabaseContext _context;
        public ClientGroupService(DatabaseContext context, IHttpContextAccessor _httpContextAccessor)
        {
            _context = context;
        }
        public long GetUserID()
        {
            var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            return Convert.ToInt64(identity.Claims.Where(c => c.Type == "RoleID")
                  .Select(c => c.Value).SingleOrDefault());
        }


        /// <summary>
        /// Get the City Drop Down.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Jsonresponse</returns>
        public IEnumerable<SelectListItem> GetDropdownCountry()
        {
            List<SelectListItem> countryList = new List<SelectListItem>();
            List<SelectListItem> cityList = new List<SelectListItem>();
            countryList = _context.Country.Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.CountryName
            }).ToList();
            return countryList;
        }

        /// <summary>
        /// Get the Drop down State
        /// </summary>
        /// <param name=""></param>
        /// <returns>Jsonresponse</returns>
        public IEnumerable<SelectListItem> GetDropdownState(long CountryID)
        {            
            List<SelectListItem> stateList = _context.State.Where(e=>e.CountryId==CountryID && e.IsActive).Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.StateName
            }).ToList();
            return stateList;
        }

        /// <summary>
        /// Get dropdown City
        /// </summary>
        /// <param name=""></param>
        /// <returns>Jsonresponse</returns>
        public IEnumerable<SelectListItem> GetDropdownCity(long stateId)
        {             
            List<SelectListItem>  cityList = _context.City.Where(e=>e.StateId==stateId && e.IsActive).Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.CityName
            }).ToList();
            return cityList;
        }

        /// <summary>
        /// Fill states based on Country ID
        /// </summary>
        /// <param name="country ID"></param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse FillStates(long countryId)
        {
            JsonResponse jsonResponse = new JsonResponse();
            var stateList = _context.State.Where(e => e.CountryId == countryId).Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.StateName
            }).ToList();
            jsonResponse.Data = stateList;
            return jsonResponse;
        }
        
        /// <summary>
        /// FIll cites based on State ID.
        /// </summary>
        /// <param name="State ID"></param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse FillCities(long stateId)
        {
            JsonResponse jsonResponse = new JsonResponse();
            var cityList = _context.City.Where(e => e.StateId == stateId).Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.CityName
            }).ToList();
            jsonResponse.Data = cityList;
            return jsonResponse;
        }

        /// <summary>
        /// Add Update Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse AddUpdate(ClientGroup model)
        {
            JsonResponse resp = new JsonResponse();
            try
            {
                if (model.Id == 0)
                {
                    if (!IsExsits(model.GroupName))
                    {
                        model.IsActive = true;
                        model.CreatedOn = DateTime.Now;
                        model.CreatedBy = GetUserID();
                        _context.Set<ClientGroup>().Add(model);
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
                    else
                    {
                        resp.Message = Constants.ControllerMessage.Data_Exsists;
                    }

                }
                else
                {
                    resp.Message = Constants.Service.Data_Update_failed;
                    var models = GetData(model.Id);
                    if (models!=null)
                    {
                        models.GroupName = model.GroupName;
                        models.AddressLine1 = model.AddressLine1;
                        models.AddressLine2 = model.AddressLine2;
                        models.CountryID = model.CountryID;
                        models.StateID = model.StateID;
                        models.CityID = model.CityID;
                        models.IsActive = true;
                        models.ModifiedOn = DateTime.Now;
                        models.ModifiedBy = GetUserID();
                        _context.Set<ClientGroup>().Update(models);
                        int i = _context.SaveChanges();
                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Data_Update_success;
                        }
                    }
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
        /// <returns>Boolean values</returns>
        public bool IsExsits(string name)
        {
            var model = _context.ClientGroup.Where(e => e.GroupName == name).FirstOrDefault();
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
        public ClientGroup GetData(long ID)
        {
            ClientGroup clientGroup = new ClientGroup();
            DataSet data = new DataSet();
            try
            {
                if (ID != 0)
                {
                    SqlParameter[] ObjParams = new SqlParameter[] {
                     new SqlParameter("@ID", ID)
                    };
                    data = new ADODataFunction().ExecuteDataset("dbo.Admin_GetClientGroupDataFromID", ObjParams, CommandType.StoredProcedure);
                    DataTable clientGroupTable = data.Tables[0];
                    if (clientGroupTable != null && clientGroupTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < clientGroupTable.Rows.Count; i++)
                        {
                            clientGroup.Id = Convert.ToInt32(clientGroupTable.Rows[i]["ID"]);
                            clientGroup.GroupName = clientGroupTable.Rows[i]["GroupName"].ToString();
                            clientGroup.AddressLine1 = clientGroupTable.Rows[i]["AddressLine1"].ToString();
                            clientGroup.AddressLine2 = clientGroupTable.Rows[i]["AddressLine2"].ToString();
                            clientGroup.CountryID = Convert.ToInt32(clientGroupTable.Rows[i]["CountryId"]);
                            clientGroup.StateID = Convert.ToInt32(clientGroupTable.Rows[i]["StateId"]);
                            clientGroup.CityID = Convert.ToInt32(clientGroupTable.Rows[i]["CityId"]);
                            clientGroup.Pincode = Convert.ToInt32(clientGroupTable.Rows[i]["PinCode"]);
                            clientGroup.Longitude = Convert.ToDecimal(clientGroupTable.Rows[i]["Longitude"]);
                            clientGroup.Latitude = Convert.ToDecimal(clientGroupTable.Rows[i]["Latitude"]);
                            clientGroup.IsActive = Convert.ToBoolean(clientGroupTable.Rows[i]["IsActive"]);
                            clientGroup.CreatedOn = Convert.ToDateTime(clientGroupTable.Rows[i]["CreatedOn"]);
                        }
                    }
                    return clientGroup;
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
        public IEnumerable<ClientGroup> Summary()
        {
            DataSet data = new DataSet();
            IEnumerable<ClientGroup> clientGroup;
            try
            {
                data = new ADODataFunction().ExecuteDataset("dbo.Admin_GetClientGroupSummary", null, CommandType.StoredProcedure);

                clientGroup = data.Tables[1].AsEnumerable().Select(dataRow => new ClientGroup
                {
                    Id = dataRow.Field<long>("Id"),
                    GroupName = dataRow.Field<string>("GroupName"),
                    AddressLine1 = dataRow.Field<string>("AddressLine1"),
                    Countries = dataRow.Field<string>("CountryName"),
                    States = dataRow.Field<string>("StateName"),
                    Cities = dataRow.Field<string>("CityName"),
                    Pincode = dataRow.Field<long>("Pincode"),
                    IsActiveText = dataRow.Field<string>("IsActive"),
                    LastUpdatedDate = dataRow.Field<string>("LastUpdatedDate")

                }).ToList();

                return clientGroup;

            }
            catch (Exception ex)
            {
                // StackTrace CallStack = new StackTrace(ex, true);
                //  ex.Data["ErrDescription"] = ex.Data["ErrDescription"] != null ? ex.Data["ErrDescription"] : string.Format("Error captured in {0} on Line No {1} of Method {2}", CallStack.GetFrame(0).GetFileName(), CallStack.GetFrame(0).GetFileLineNumber(), CallStack.GetFrame(0).GetMethod().ToString());
                throw ex;

            }

        }

        /// <summary>
        /// Change the Status
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse Deactivate(long ID,bool Status)
        {
            JsonResponse resp = new JsonResponse();
            try
            {
                if (ID != 0)
                {
                    var model = GetData(ID);
                    if (model!=null)
                    {
                        model.IsActive = Status;
                        model.ModifiedOn = DateTime.Now;
                        model.ModifiedBy = GetUserID();
                        _context.Set<ClientGroup>().Update(model);
                        int i = _context.SaveChanges();
                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Status_changed_success;
                        }
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
            }
            catch
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

    }
}
