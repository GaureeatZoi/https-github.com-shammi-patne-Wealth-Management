using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public class TransactionTypeService : ITransactionTypeService
    {
        JsonResponse resp = new JsonResponse();

        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionTypeService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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


        public JsonResponse AddUpdate(TransactionTypeViewModel model)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5]; 
                param[0] = new SqlParameter("@ID", model.transactionTypes.ID);
                param[1] = new SqlParameter("@TransactionType", model.transactionTypes.Type);
                param[2] = new SqlParameter("@TransactionNature", model.transactionTypes.Nature);
                param[3] = new SqlParameter("@UserID", GetUserID());
                param[4] = new SqlParameter("@IsActive", model.transactionTypes.IsActive);
                int id = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.AddUpdateTransactionType, param, CommandType.StoredProcedure);
                //  If i != 0 means it affect one data So the data was Inserted.
                if (id != 0)
                {
                    if (model.transactionTypes.ID > 0)
                    {
                        resp.Message = Constants.Service.Data_Update_success;
                    }
                    else
                    {
                        resp.Message = Constants.Service.Data_insert_success;
                    }
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

        public JsonResponse ChangeStatus(long ID, bool Status)
        {
            try
            {
                //If the ID was not null It goes to the change status part.
                if (ID != 0)
                {
                    var model = GetTransactionType(ID);
                    if (model != null)
                    {
                        model.IsActive = Status;
                        model.ModifiedOn = DateTime.Now;
                        model.ModifiedBy = GetUserID();
                        _context.Set<TransactionTypes>().Update(model);
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
            catch(Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        public TransactionTypes GetTransactionType(long ID)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", ID);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetTransactionTypeByID, param, CommandType.StoredProcedure);

                TransactionTypes model = data.Tables[0].AsEnumerable().Select(a => new TransactionTypes
                {
                    ID = a.Field<long>("ID")
                   ,
                    Type = a.Field<string>("TransactionType")
                   ,
                    Nature = a.Field<string>("TransactionNature")
                  ,
                    IsActive = a.Field<bool>("IsActive")
                    ,
                    CreatedBy=a.Field<long>("CreatedBy")
                    ,
                    CreatedOn = a.Field<DateTime>("CreatedOn")

                }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResponse Summary()
        {
            try
            {
            //    GetUserID("RoleID");
                    DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.TransactionTypeSummary, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new TransactionTypes
                {
                    ID = a.Field<long>("ID")
                   ,
                    Type = a.Field<string>("TransactionType")
                   ,
                    Nature = a.Field<string>("TransactionNature")
                   ,
                    IsActiveText = a.Field<string>("IsActive")
                   ,
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate")
                }).ToList();
            }
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }
    }
}
