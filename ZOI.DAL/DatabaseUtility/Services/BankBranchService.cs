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
    public class BankBranchService : IBankBranchService
    {
        private readonly DatabaseContext _context;

        JsonResponse resp = new JsonResponse();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public BankBranchService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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
        /// Add Update Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse AddUpdate(BankBranch model)
        {
            try
            {
                //  If these condition true the data was not exsits in the database
                if (!IsExsits(model.Name, model.Id, model.BankID))
                {
                    //  If model.ID == 0 the data goes to the Add part.                
                    if (model.Id == 0)
                    {
                        model.CreatedOn = DateTime.Now;
                        model.CreatedBy = GetUserID();
                        _context.Set<BankBranch>().Add(model);
                        int i = _context.SaveChanges();
                        //  If i != 0 means it affect one data So the data was Inserted.
                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Data_insert_success;
                        }
                        //esle It gives some error report of data insertion.
                    }
                    //  Else data goes to the Update part.
                    else
                    {
                        resp.Message = Constants.Service.Data_Update_failed;
                        var models = GetData(model.Id);
                        if (models != null)
                        {
                            models.AddressLine1 = model.AddressLine1;
                            models.AddressLine2 = model.AddressLine2;
                            models.CountryID = model.CountryID;
                            models.StateID = model.StateID;
                            models.CityID = model.CityID;
                            models.PinCode = model.PinCode;
                            models.IsActive = model.IsActive;
                            models.ModifiedOn = DateTime.Now;
                            models.ModifiedBy = GetUserID();
                            _context.Set<BankBranch>().Update(models);
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
        /// Change the Status
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse Deactivate(long ID, bool Status)
        {
            try
            {
                //If the ID was not null It goes to the change status part.
                if (ID != 0)
                {
                    resp.Message = Constants.Service.Common_message;
                    var model = GetData(ID);
                    if (model != null)
                    {
                        model.IsActive = Status;
                        model.ModifiedOn = DateTime.Now;
                        model.ModifiedBy = GetUserID();
                        _context.Set<BankBranch>().Update(model);
                        int i = _context.SaveChanges();
                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Status_changed_success;
                        }
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


        /// <summary>
        /// Get the model.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Return the model.</returns>
        public BankBranch GetData(long ID)
        {
            if (ID != 0)
            {
                try
                {
                    return _context.BankBranch.AsNoTracking().Where(e => e.Id == ID).FirstOrDefault();
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Data Is Exsits
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Boolean values</returns>
        public bool IsExsits(string name, long ID, long Bank)
        {
            bool IsExsits = true;

            if (_context.BankBranch.Where(e => e.Name == name && e.Id != ID && e.BankID == Bank).FirstOrDefault() == null)
            {
                IsExsits = false;
            }

            return IsExsits;

        }

        /// <summary>
        /// Data Is Exsits
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Boolean values</returns>
        public bool IsIFSCExsits(string code)
        {
            bool IsExsits = true;

            if (_context.BankBranch.Where(e => e.IFSCCode == code).FirstOrDefault() == null)
            {
                IsExsits = false;
            }

            return IsExsits;

        }

        /// <summary>
        /// Data Is Exsits
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Boolean values</returns>
        public bool IsMICRExsits(string code)
        {
            bool IsExsits = true;

            if (_context.BankBranch.Where(e => e.MICRCode == code).FirstOrDefault() == null)
            {
                IsExsits = false;
            }

            return IsExsits;

        }


        /// <summary>
        /// Data Summary
        /// </summary>
        /// <returns>Jsonresponse</returns>
        public JsonResponse Summary()
        {
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetBankBranchSummary, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new BankBranch
                {
                    Id = a.Field<long>("Id")
                   ,
                    BankName = a.Field<string>("BankName")
                   ,
                    Name = a.Field<string>("BranchName")
                   ,
                    AddressLine1 = a.Field<string>("AddressLine1")
                   ,
                    IFSCCode = a.Field<string>("IFSCCode")
                   ,
                    CityName = a.Field<string>("CityName")
                   ,
                    StateName = a.Field<string>("StateName")
                   ,
                    PinCode = a.Field<long>("PinCode")
                   ,
                    CountryName = a.Field<string>("CountryName")
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

        //For Dropdown


        public IEnumerable<SelectListItem> GetAddressesDropDowns(string Procedure, long? Parameter, string Flag)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Parameters", Parameter);
                param[1] = new SqlParameter("@Flag", Flag);
                data = new ADODataFunction().ExecuteDataset(Procedure, param, CommandType.StoredProcedure);
                return data.Tables[0].AsEnumerable().Select(a => new SelectListItem
                {
                    Text = a.Field<string>("Name"),
                    Value = Convert.ToString(a.Field<long>("ID"))
                }).ToList();
                
            }
            catch (Exception ex)
            {
                return null;
            }
             
        }


        /// <summary>
        /// Return the Bank List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetBankList()
        {
            return _context.Bank.AsNoTracking().OrderBy(e => e.Name).Where(e => e.IsActive == true).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.Name
            }).ToList();

        }

        /// <summary>
        /// Return the Country List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetCountryList()
        {
            return _context.Country.AsNoTracking().OrderBy(e => e.CountryName).Where(e => e.IsActive == true).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.CountryName
            }).ToList();
        }

        /// <summary>
        /// Return the State List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetStateList(long? CountryId)
        {
            return _context.State.AsNoTracking().OrderBy(e => e.StateName).Where(e => e.CountryId == CountryId && e.IsActive).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.StateName
            }).ToList();
        }

        /// <summary>
        /// Return the City List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetCityList(long? StateID)
        {
            if (StateID == null)
            {
                var data = _context.City.AsNoTracking().OrderBy(e => e.CityName).Where(e => e.IsActive).Select(e => new SelectListItem()
                {
                    Value = (e.Id).ToString(),
                    Text = e.CityName
                }).ToList();
                return data;

            }


            var d = _context.City.AsNoTracking().OrderBy(e => e.CityName).Where(e => e.IsActive && e.StateId == StateID).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.CityName
            }).ToList();
            return d;           
        }

    }
}
