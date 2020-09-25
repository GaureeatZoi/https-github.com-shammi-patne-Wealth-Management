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
using ZOI.BAL.DBContext;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.DAL;
using ZOI.DAL.DatabaseUtility;

namespace ZOI.BAL.Services
{
    public class EquityBrokerService : IEquityBrokerService
    {
        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IHostingEnvironment _HostingEnvironment;

        JsonResponse resp = new JsonResponse();

        public EquityBrokerService(DatabaseContext context, IHostingEnvironment HostingEnvironment, IHttpContextAccessor httpContextAccessor)
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
        public JsonResponse AddUpdate(EquityBrokers model)
        {
            try
            {
                //  If these condition true the data was not exsits in the database
                if (!ModelExsits(model))
                {
                    //  If model.ID == 0 the data goes to the Add part.                
                    if (model.ID == 0)
                    {
                        resp.Message = Constants.Service.Data_insert_failed;
                        model.CreatedOn = DateTime.Now;
                        model.CreatedBy = GetUserID();
                        _context.Set<EquityBrokers>().Add(model);
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
                            var file = Path.Combine(_HostingEnvironment.WebRootPath, "NewFolder", model.Logo);
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
                            models.AddressLine1 = model.AddressLine1;
                            models.AddressLine2 = model.AddressLine2;
                            models.CountryID = model.CountryID;
                            models.StateID = model.StateID;
                            models.CityID = model.CityID;
                            models.PinCode = model.PinCode;
                            models.IsActive = model.IsActive;
                            models.ModifiedOn = DateTime.Now;
                            models.ModifiedBy = GetUserID();
                            _context.Set<EquityBrokers>().Update(models);
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
                        _context.Set<EquityBrokers>().Update(model);
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
        public EquityBrokers GetData(long ID)
        {
            try
            {
                if (ID != 0)
                {
                    return _context.EquityBrokers.AsNoTracking().Where(e => e.ID == ID).FirstOrDefault();
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

        public bool ModelExsits(EquityBrokers model)
        {
            bool IsExsits = true;
           
                if (_context.EquityBrokers.Where(e => e.Name == model.Name && e.NSEMembercode != model.NSEMembercode && e.MCXMembercode != model.MCXMembercode && e.BSEMembercode != model.BSEMembercode && e.ID != model.ID).FirstOrDefault() == null)
                    IsExsits = false;
            else
                IsExsits = true;
            return IsExsits;

        }


        /// <summary>
        /// Data Is Exsits
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Boolean values</returns>
        public bool IsExsits(string name, string Flag,long ID)
        {
            bool IsExsits = true;
            if (Flag=="Name")
            {
                if (_context.EquityBrokers.Where(e => e.Name == name && e.ID != ID).FirstOrDefault() == null)
                    IsExsits = false;
               
            }
            else if (Flag== "NSE")
            {
                if (_context.EquityBrokers.Where(e => e.Name == name && e.ID != ID).FirstOrDefault() == null)
                    IsExsits = false;
            }
            else if (Flag== "BSE")
            {
                if (_context.EquityBrokers.Where(e => e.Name == name && e.ID != ID).FirstOrDefault() == null)
                    IsExsits = false;
            }
            else if (Flag== "MCX")
            {
                if (_context.EquityBrokers.Where(e => e.Name == name && e.ID != ID).FirstOrDefault() == null)
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
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetEquityBrokerSummary, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new EquityBrokers
                {
                    ID = a.Field<long>("Id")
                   ,
                    Name = a.Field<string>("BrokerName")
                   ,
                    BSEMembercode = a.Field<string>("BSEMemberCode")
                   ,
                    NSEMembercode = a.Field<string>("NSEMemberCode")
                   ,
                    MCXMembercode = a.Field<string>("MCXMemberCode")
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
        public IEnumerable<SelectListItem> GetCompanyList()
        {
            var company = _context.CompanyMaster.AsNoTracking().OrderBy(e => e.NameOfCompany).Where(e => e.IsActive == true).Select(e => new SelectListItem()
            {
                Value = (e.ID).ToString(),
                Text = e.NameOfCompany
            }).ToList();
            return company;
        }

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

        //public JsonResponse UploadLogo(EquityBrokers modal)
        //{
        //    try
        //    {
        //        if (modal != null)
        //        {
        //            var updateModal = _context.equityBrokers.Where(e => e.ID == modal.ID).FirstOrDefault();

        //            updateModal.Logo = modal.Logo;
        //            updateModal.ModifiedBy = 1;
        //            updateModal.ModifiedOn = DateTime.UtcNow;
        //            _context.Set<EquityBrokers>().Update(updateModal);
        //            if (_context.SaveChanges() > 0)
        //            {
        //                resp.Status = Constants.ResponseStatus.Success;
        //                resp.Message = "Record Updated Successfully";
        //            }
        //            else
        //            {
        //                resp.Status = "F";
        //                resp.Message = "Image not upload. please try again!";
        //            }
        //        }
        //        else
        //        {
        //            resp.Status = "F";
        //            resp.Message = "something went wrong !";
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return resp;
        //}

        /// <summary>
        /// Check Image in the database
        /// </summary>
        /// <param name="modal"></param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse CheckImage(EquityBrokers model)
        {
            //Check the Image file name in the model and return the values if the condition true it retun the model logo name.
            if (_context.EquityBrokers.Where(e => e.Logo == model.Logo).FirstOrDefault() != null)
            {
                resp.Status = Constants.ResponseStatus.Success;
                resp.Data = model.Logo;
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
