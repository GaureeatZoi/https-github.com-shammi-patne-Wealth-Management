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
    public class SchemeNAVService : ISchemeNAVService
    {
        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        JsonResponse resp = new JsonResponse();


        public JsonResponse Summary()
        {   //Used for Employee Listing 
            //Stored Procedure Used for listing  Admin_GetEmployee
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetSchemeNAV, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new SchemeNAV
                {
                    ID = a.Field<long>("ID"),
                    RTAName = a.Field<string>("RTAName"),
                    AMCName = a.Field<string>("AMCName"),
                    SchemeName = a.Field<string>("SchemeName"),
                    NAV = a.Field<decimal>("NAV"),
                    //NAVDate = a.Field<DateTime>(" NAVDate"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate"),
                   // IsActiveText = a.Field<string>("Status")
                }).ToList();
            }
            catch (Exception Ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }
    }
}
