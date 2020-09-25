using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Claims;
using ZOI.BAL;
using ZOI.BAL.DBContext;
using ZOI.BAL.Models;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.DAL.DatabaseUtility.Services
{
    public class ClientAccountMappingService : IClientAccountMappingService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DatabaseContext _context;

        public ClientAccountMappingService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public long GetUserID()
        {
            var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            return Convert.ToInt64(identity.Claims.Where(c => c.Type == "RoleID")
                  .Select(c => c.Value).SingleOrDefault());
        }

        public JsonResponse AddUpdate(ClientAccountsMapping model)
        {
            JsonResponse resp = new JsonResponse();
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@ID", model.ID);
                param[1] = new SqlParameter("@UCC", model.UCC);
                param[2] = new SqlParameter("@IsDefault", model.IsDefault);
                param[3] = new SqlParameter("@AccountTypeID", model.AccountTypeID);
                param[4] = new SqlParameter("@IsActive", 1);
                param[5] = new SqlParameter("@RMId", model.RMId);
                param[6] = new SqlParameter("@SecondaryRMId", model.SecondaryRMId);
                param[7] = new SqlParameter("@ClientID", model.ClientID);
                param[8] = new SqlParameter("@UserID", GetUserID());
                param[9] = new SqlParameter("@EffectiveFrom", model.EffectiveFrom);
                int i = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.AddUpdateClientsAccounts, param, CommandType.StoredProcedure);
                //  If it return was not equal to 0 then it affect some rows means data inserted.
                if (i != 0)
                {
                    if (model.ID != 0)
                    {
                        resp.Message = Constants.Service.Data_Update_success;
                    }
                    else
                    {
                        resp.Message = Constants.Service.Data_insert_success;
                    }
                    resp.Status = Constants.ResponseStatus.Success;
                    
                }
                // Else Show the error message.
                else
                {
                    if (model.ID != 0)
                    {
                        resp.Message = Constants.Service.Data_Update_failed;
                    }
                    else
                    {
                        resp.Message = Constants.Service.Data_insert_failed;
                    }
                    resp.Status = Constants.ResponseStatus.Failed;
                }
            }
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        public List<ClientAccountsMapping> GetClientAccountsData(long ClientID)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ClientID", ClientID);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetClientAccountsByID, param, CommandType.StoredProcedure);
                List<ClientAccountsMapping> datas = data.Tables[0].AsEnumerable().Select(a => new ClientAccountsMapping
                {
                    ID = a.Field<long>("ID")
                    ,
                    ClientID = a.Field<long>("L3_ClientID")
                    ,
                    AccountTypeID = a.Field<int>("L4_AccountTypeID")
                    ,
                    UCC = a.Field<string>("UCC")
                }).ToList();
                return datas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResponse Summary()
        {
            JsonResponse resp = new JsonResponse();
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetClientAccountSumary, null, CommandType.StoredProcedure);
                resp.Data = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new ClientAccountsMapping
                {
                    Client = a.Field<string>("Client"),
                    PrimaryRM = a.Field<string>("PrimaryRM"),
                    Accounts = a.Field<string>("AccountTypeName") ,
                    UCC = a.Field<string>("UCC"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate"),
                    IsActiveText = a.Field<string>("IsActive"),
                    ID = a.Field<long>("ID"),
                }).ToList();
            }
            catch (Exception ex)
            {
                resp.Status = "F";
                resp.Message = "Something went wrong";
            }
            return resp;
        }

        public IEnumerable<SelectListItem> ClientList()
        {
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetClientDropDown, null, CommandType.StoredProcedure);
                IEnumerable<SelectListItem> listItems = data.Tables[0].AsEnumerable().Select(a => new SelectListItem
                {
                    Text = a.Field<string>("Text"),
                    Value = Convert.ToString(a.Field<long>("Value"))
                }).ToList();
                return listItems;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<SelectListItem> EmployeeList()
        {
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.EmployeeList, null, CommandType.StoredProcedure);
                IEnumerable<SelectListItem> listItems = data.Tables[0].AsEnumerable().Select(a => new SelectListItem
                {
                    Text = a.Field<string>("Name"),
                    Value = Convert.ToString(a.Field<long>("ID"))
                }).ToList();
                return listItems;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<SelectListItem> AccountTypeList()
        {
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetAccountTypesDropDown, null, CommandType.StoredProcedure);
                IEnumerable<SelectListItem> listItems = data.Tables[0].AsEnumerable().Select(a => new SelectListItem
                {
                    Text = a.Field<string>("Name"),
                    Value = Convert.ToString(a.Field<long>("ID"))
                }).ToList();
                return listItems;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResponse IsExists(long ClientID, int AccountTypeID, string UCC)
        {
            JsonResponse resp = new JsonResponse();
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ClientID", ClientID);
                param[1] = new SqlParameter("@AccountTypeID", AccountTypeID);
                param[2] = new SqlParameter("@UCC", UCC);                
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.ClientAccontIsExists, param, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();               
            }
            catch (Exception ex)
            {
                resp.Status = "F";
                resp.Message = "Something went wrong";
            }
            return resp;
        }
          public JsonResponse ChangeStatus(long ID, bool Status)
        {
            JsonResponse resp = new JsonResponse();
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ID", ID);
                param[1] = new SqlParameter("@Status", Status);
                param[2] = new SqlParameter("@User", 1);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.ChangeStatusClientAccount, param, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();               
            }
            catch (Exception ex)
            {
                resp.Status = "F";
                resp.Message = "Something went wrong";
            }
            return resp;
        }

        public ClientAccountsMapping GetDataByID(long ID)
        {
            
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID",ID); 
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetClientAccountByID, param, CommandType.StoredProcedure);               
                return data.Tables[0].AsEnumerable().Select(a => new ClientAccountsMapping
                {
                    ID = a.Field<long>("ID"),
                    ClientID = a.Field<long>("L3_ClientID"),
                    AccountTypeID = a.Field<long>("L4_AccountTypeID"),
                    UCC = a.Field<string>("UCC"),
                    RMId = a.Field<long>("RMId"),
                    SecondaryRMId = a.Field<long>("SecondaryRMId"),
                    IsActive = a.Field<bool>("IsActive"),
                    CreatedBy = a.Field<long>("CreatedBy"),
                    CreatedOn = a.Field<DateTime>("CreatedOn"),
                    ModifiedBy = a.Field<long>("ModifiedBy"),
                    ModifiedOn = a.Field<DateTime>("ModifiedOn"),
                    IsDefault = a.Field<bool>("IsDefault"),
                    EffectiveFrom = a.Field<string>("EffectiveFrom"),
                    EffectiveTo = a.Field<string>("EffectiveTo")
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
             
        }
    }
}
