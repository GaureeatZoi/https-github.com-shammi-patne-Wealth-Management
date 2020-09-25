using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.DBContext;
using ZOI.BAL.Models;
using ZOI.BAL.Utilites;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.DAL.DatabaseUtility.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IHostingEnvironment _HostingEnvironment;

        JsonResponse resp = new JsonResponse();
        public CompanyService(DatabaseContext context, IHostingEnvironment HostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _HostingEnvironment = HostingEnvironment;
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
        public JsonResponse AddUpdate(CompanyMaster model)
        {
            try
            {
                //  If these condition true the data was not exsits in the database
                if (!IsExsits(model.NameOfCompany, model.ID))
                {
                    //  If model.ID == 0 the data goes to the Add part.                
                    if (model.ID == 0)
                    {
                        resp.Message = Constants.Service.Data_insert_failed;
                        model.CreatedOn = DateTime.Now;
                        model.CreatedBy = GetUserID();
                        model.IconFileData = Encoding.UTF8.GetBytes(model.Icon);
                        _context.Set<CompanyMaster>().Add(model);
                        int i = _context.SaveChanges();
                        //  If i != 0 means it affect one data So the data was Inserted.
                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Data_insert_success;
                        }
                        //esle It gives some error in data insertion.
                        else
                        {
                            var file = Path.Combine(_HostingEnvironment.WebRootPath, "NewFolder", model.Icon);
                            if (File.Exists(file))
                            {
                                File.Delete(file);
                            }
                        }
                    }

                    //  Else data goes to the Update part.
                    else
                    {
                        resp.Message = Constants.Service.Data_Update_failed;
                        var models = GetData(model.ID);
                        // If the models not null it get the database data belongs to the ID 
                        if (models != null)
                        {
                            models.NameOfCompany = model.NameOfCompany;
                            models.IconName = model.IconName;
                            models.AddressLine1 = model.AddressLine1;
                            models.AddressLine2 = model.AddressLine2;
                            models.CountryID = model.CountryID;
                            models.StateID = model.StateID;
                            models.CityID = model.CityID;
                            models.PinCode = model.PinCode;
                            models.IsActive = model.IsActive;
                            models.ModifiedOn = DateTime.Now;
                            models.ModifiedBy = GetUserID();
                            _context.Set<CompanyMaster>().Update(models);
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
            catch(Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        //Get company code
        public long GetCompanyCode()
        {
            long ID;
            DataSet data = new DataSet();
            data = new ADODataFunction().ExecuteDataset("dbo.Admin_GetCompanyCode", null, CommandType.StoredProcedure);
            if (data.Tables[0].Rows.Count<1)
            {
                ID = 0;
            }
            else
            {
                ID = Convert.ToInt32(data.Tables[0].Rows[0]["ID"]);
            }
            return ID+1;
        }

        //Check duplicate for Company Name
        public bool IsExsits(string name, long ID)
        {
            bool IsExsits = true;

            if (_context.CompanyMaster.Where(e => e.NameOfCompany == name && e.ID != ID).FirstOrDefault() == null)
            {
                IsExsits = false;
            }

            return IsExsits;
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
                        _context.Set<CompanyMaster>().Update(model);
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
        /// <returns>Return model.</returns>
        public CompanyMaster GetData(long ID)
        {
            try
            {
                if (ID != 0)
                {
                    return _context.CompanyMaster.AsNoTracking().Where(e => e.ID == ID).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
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
                data = new ADODataFunction().ExecuteDataset("dbo.Admin_GetCompanyDetails", null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new CompanyMaster
                {
                    ID = a.Field<long>("ID")
                   ,
                    NameOfCompany = a.Field<string>("NameOfCompany")
                   ,
                    AddressLine1 = a.Field<string>("AddressLine1")
                                       ,
                    AddressLine2 = a.Field<string>("AddressLine2")
                                       ,
                    AddressLine3 = a.Field<string>("AddressLine3")
                   ,
                    ContactPersonName = a.Field<string>("ContactPersonName")
                   ,
                    ContactPersonEmail = a.Field<string>("ContactPersonEmail")
                   ,
                    ContactPersonMobile = a.Field<string>("ContactPersonMobile")
                   ,
                    PinCode = a.Field<long>("PinCode")
                   ,
                    IconFileData = a.Field<byte[]>("Icon")
                   ,
                    Icon= Encoding.UTF8.GetString(a.Field<byte[]>("Icon"))
                    ,
                    IsActiveText = a.Field<string>("IsActive")
                   ,
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate")
                   
                });
             
                
            }
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        //For Dropdown

        /// <summary>
        /// Return the Country List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetCountryList()
        {
            var d = _context.Country.AsNoTracking().OrderBy(e => e.CountryName).Where(e => e.IsActive == true).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.CountryName
            }).ToList();
            return d;
        }

        /// <summary>
        /// Return the State List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetStateList(long? CountryId)
        {

            if (CountryId == null)
            {
                return _context.State.AsNoTracking().OrderBy(e => e.StateName).Where(e => e.IsActive).Select(e => new SelectListItem()
                {
                    Value = (e.Id).ToString(),
                    Text = e.StateName
                }).ToList();
            }
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
                return _context.City.AsNoTracking().OrderBy(e => e.CityName).Where(e => e.IsActive).Select(e => new SelectListItem()
                {
                    Value = (e.Id).ToString(),
                    Text = e.CityName
                }).ToList();
            }
            return _context.City.AsNoTracking().OrderBy(e => e.CityName).Where(e => e.IsActive && e.StateId == StateID).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.CityName
            }).ToList();
        }

        public JsonResponse CheckImage(CompanyMaster model)
        {
            //Check the Image file name in the model and return the values if the condition true it retun the model logo name.
            if (_context.CompanyMaster.Where(e => e.IconFileData == Encoding.UTF8.GetBytes(model.Icon)).FirstOrDefault() != null)
            {
                resp.Status = Constants.ResponseStatus.Success;
                resp.Data = model.Icon;
            }
            // Else the error function was trigger and send the message.
            else
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;

        }



    }
}
