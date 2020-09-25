using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.DAL.DatabaseUtility.Services
{
    public class ClientFolioService : IClientFolioService
    {
        JsonResponse resp = new JsonResponse();
        public JsonResponse AddUpdate(ClientFolioViewModel model)
        {
            try
            {                
                SqlParameter[] param = new SqlParameter[12];
                param[0] = new SqlParameter("@ID", model.clientFolio.Id);
                param[1] = new SqlParameter("@FolioNo", model.clientFolio.FolioNo);
                param[2] = new SqlParameter("@FolioDate", model.clientFolio.FolioDate);
                param[3] = new SqlParameter("@InvestorName", model.clientFolio.InvestorName);
                param[4] = new SqlParameter("@AMCID", model.clientFolio.AMCID);
                param[5] = new SqlParameter("@SchemeID", model.clientFolio.SchemeID);
                param[6] = new SqlParameter("@PAN", model.clientFolio.PAN);
                param[7] = new SqlParameter("@ClientID", model.clientFolio.ClientID);
                param[8] = new SqlParameter("@AccountTypeID", model.clientFolio.AccountTypeID);
                param[9] = new SqlParameter("@UserID", 1);
                param[10] = new SqlParameter("@IsActive", model.clientFolio.IsActive);
                param[11] = new SqlParameter("@AccountID", model.clientFolio.AccountID);
                int id = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.AddUpdateClientFolios, param, CommandType.StoredProcedure);
                //  If i != 0 means it affect one data So the data was Inserted.
                if (id != 0)
                {
                    if (model.clientFolio.Id>0)
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
            throw new NotImplementedException();
        }

        public IEnumerable<SelectListItem> FillDropDown(string procedure, long? parameter)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Parameter", parameter);
                data = new ADODataFunction().ExecuteDataset(procedure, param, CommandType.StoredProcedure);
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

         public IEnumerable<SelectListItem> FillAccountDropDown(string procedure, long? param1, long? param2)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@ClientID", param1);
                param[1] = new SqlParameter("@AccountTypeID", param2);
                data = new ADODataFunction().ExecuteDataset(procedure, param, CommandType.StoredProcedure);
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


        public ClientFolio GetClientFolio(long ID)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@ID", ID);
                param[1] = new SqlParameter("@UserID", 1);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetClientFoliosByID, param, CommandType.StoredProcedure);

                ClientFolio model = data.Tables[0].AsEnumerable().Select(a => new ClientFolio
                {
                    Id = a.Field<long>("Id")
                   ,
                    FolioNo = a.Field<string>("FolioNo")
                   ,
                    FolioDate = a.Field<string>("FolioDate")
                   ,
                    InvestorName = a.Field<string>("InvestorName")
                   ,
                    RTAID = a.Field<long>("RTAID")
                   ,
                    AMCID = a.Field<long>("AMCID")
                   ,
                    SchemeID = a.Field<long>("SchemeID")
                   ,
                    PAN = a.Field<string>("PAN")
                   ,
                    ClientID = a.Field<long>("ClientID")
                   ,
                    RTATableID = a.Field<long>("RTATableID")
                   ,
                    IsActive = a.Field<bool>("IsActive")

                }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ClientMain GetInvestorDetail(long ID)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", ID);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetInvestorDetail, param, CommandType.StoredProcedure);
                ClientMain model = data.Tables[0].AsEnumerable().Select(a => new ClientMain
                {
                    ClientName = a.Field<string>("Name")
                    ,
                    PAN = a.Field<string>("PAN")
                }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResponse IsExsits(string FolioNo, long SchemaID)
        {
            try
            {
                //If the condition true the data with this name doestn't have the duplicate value in the database.
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@FolioNo", FolioNo);
                param[1] = new SqlParameter("@SchemaID", SchemaID);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.IsClientFolioExsits, param, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("Status"),
                    Message = a.Field<string>("Message")
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {

                resp.Message = Constants.Service.Common_message;
            }           
            return resp;
        }

        public JsonResponse Summary()
        {
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetClientFolioSummary, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new ClientFolio
                {
                    Id = a.Field<long>("Id")
                     ,
                    FolioNo = a.Field<string>("FolioNo")
                     ,
                    FolioDate = a.Field<string>("FolioDate")
                     ,
                    InvestorName = a.Field<string>("InvestorName")
                     ,
                    IsActiveText = a.Field<string>("IsActive")
                     ,
                    AMCName = a.Field<string>("AMCName")
                     ,
                    PAN = a.Field<string>("PAN")
                     ,
                    // SchemeName = a.Field<string>("IsActive")
                    //,
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
