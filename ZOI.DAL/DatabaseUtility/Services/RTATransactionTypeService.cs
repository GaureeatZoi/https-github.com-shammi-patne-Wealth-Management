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
using ZOI.BAL;
using ZOI.BAL.DBContext;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.DAL;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.DAL.DatabaseUtility.Services
{
    public class RTATransactionTypeService : IRTATransactionTypeService
    {
        JsonResponse resp = new JsonResponse();

        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public RTATransactionTypeService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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



        public JsonResponse AddUpdate(RTATransactionTypes model)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[7]; 
                param[0] = new SqlParameter("@ID", model.ID);
                param[1] = new SqlParameter("@RTAId", model.RTAId);
                param[2] = new SqlParameter("@RTATransactionType", model.RTATransactionType);
                param[3] = new SqlParameter("@UserID", GetUserID());
                param[4] = new SqlParameter("@IsActive", model.IsActive);
                param[5] = new SqlParameter("@Remarks", model.Remarks);
                param[6] = new SqlParameter("@TransactionTypeID ", model.TransactionTypeID);
                int id = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.AddUpdateRTATransactionType, param, CommandType.StoredProcedure);
                //  If i != 0 means it affect one data So the data was Inserted.
                if (id != 0)
                {
                    if (model.ID > 0)
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
                        _context.Set<RTATransactionTypes>().Update(model);
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

        public RTATransactionTypes GetTransactionType(long ID)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", ID);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetRTATransactionTypeByID, param, CommandType.StoredProcedure);

                RTATransactionTypes model = data.Tables[0].AsEnumerable().Select(a => new RTATransactionTypes
                {
                    ID = a.Field<long>("ID")
                   ,
                    RTATransactionType = a.Field<string>("RTATransactionType")
                   ,
                    Remarks = a.Field<string>("Remarks")
                   ,
                    RTAId = a.Field<long>("RTAId")
                  ,
                    TransactionTypeID = a.Field<int>("TransactionTypeID")
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
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.RTATransactionTypeSummary, null, CommandType.StoredProcedure);
                resp.Data = data.Tables[0].AsEnumerable().Select(a => new RTATransactionTypes
                {
                    ID = a.Field<long>("ID")
                   ,
                    RTA = a.Field<string>("RTAName")
                   ,
                    RTATransactionType = a.Field<string>("RTATransactionType")
                   ,
                    Remarks = a.Field<string>("Remarks")
                   ,
                    TransactionType = a.Field<string>("TransactionType")
                   ,
                    IsActiveText = a.Field<string>("IsActive")
                   ,
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate")
                }).ToList();
                resp.Status= Constants.ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        public JsonResponse UnmappedSummary()
        {
            try
            {
                DataSet drdata = new DataSet();
                drdata = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetTransactionTypeDropDown, null, CommandType.StoredProcedure);
                var dd = "";
                for (int i = 0; i < drdata.Tables[0].Rows.Count; i++)
                {
                    dd += "<option value = '" + drdata.Tables[0].Rows[i]["Value"] +  "' >" + drdata.Tables[0].Rows[i]["Text"] + "</option>";
                }
                dd += "</select> ";
                   
                    DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.Getunmappedtransactiontypes, null, CommandType.StoredProcedure);
                StringBuilder sb = new StringBuilder();
                var body = "<tr>";
                 
                for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                {
                    body += "<td colspan='1'><input type='hidden' class='hdn-id' name='RTATransactionTypes.ID' value=" + data.Tables[0].Rows[i]["ID"] + ">" + data.Tables[0].Rows[i]["row_num"] + "</td>";
                    body += "<td colspan='1'>" + data.Tables[0].Rows[i]["RTAName"] + "</td>";
                    body += "<td colspan='1'>" + data.Tables[0].Rows[i]["RTATransactionType"] + "</td>";
                    body += "<td colspan='1'>" + data.Tables[0].Rows[i]["Remarks"] + "</td>";
                    body += "<td colspan='2'>  <select name='RTATransactionTypes.TransactionTypeID' class='rtatran' id='transactionId' name = 'RTATransactionTypes-TransactionTypeID' ><option value = '0' > --Select-- </option > " + dd + "</td>";
                    body += "</tr>";
                }
                resp.Data = body;
                resp.Status = Constants.ResponseStatus.Success;
                
     
            }
            catch (Exception ex)
            {
                resp.Data = "";
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        public JsonResponse MapData(List<TransactionTypeMappingList> model)
        {
           try
            {
                if (model != null && model.Count > 0)
                {
                    //  Convert the List into DataTable.
                    DataTable dt = ConvertToDataTable(model);
                    //  Set the parameter to pass the procedure.
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@TableParam", dt);
                    param[1] = new SqlParameter("@UserID", GetUserID());
                  
                    int id = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.UpdateMapping, param, CommandType.StoredProcedure);
                    if (id != 0)
                    {
                        resp.Status = Constants.ResponseStatus.Success;
                        resp.Message = Constants.ControllerMessage.TransactionType_Mapping_Success;
                       
                    }
                    // Else Show the error message.
                    else
                    {
                        resp.Message = Constants.ControllerMessage.TransactionType_Mapping_Failed;
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

           



        public IEnumerable<SelectListItem> FillDropDown(string Procedure)
        {
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Procedure, null, CommandType.StoredProcedure);
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

        public bool IsExsits(long RTAID, string RTAType,long ID)
        {
            bool IsExsits = true;
            //If the condition true the data with this name doestn't have the duplicate value in the database.
            if (_context.rtaTransactionTypes.Where(e => e.RTAId == RTAID && e.RTATransactionType == RTAType &&  e.ID !=  ID).FirstOrDefault() == null)
            {
                IsExsits = false;
            }
            return IsExsits;
        }


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
    