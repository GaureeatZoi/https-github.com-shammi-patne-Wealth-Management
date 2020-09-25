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
using ZOI.BAL;
using ZOI.BAL.DBContext;
using ZOI.BAL.Models;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.DAL.DatabaseUtility.Services
{
    public class SchemeService : ISchemeService
    {
        private readonly DatabaseContext _context;
        
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        JsonResponse resp = new JsonResponse();

        public SchemeService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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

        #region Fill DropDowns
        /// <summary>
        /// Fill Rta
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetRTAList()
        {
            return _context.RTA.AsNoTracking().OrderBy(e => e.Name).Where(e => e.IsActive == true).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.Name
            }).ToList();

        }

        /// <summary>
        /// Fill Amc 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetAmcList()
        {
            return _context.AMC.AsNoTracking().OrderBy(e => e.Name).Where(e => e.IsActive == true).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.Name
            }).ToList();

        }


        /// <summary>
        /// Fill SchemeType
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetSchemeTypeList()
        {
            List<SelectListItem> scheme = new List<SelectListItem>();
            DataSet dataSet = new ADODataFunction().ExecuteDataset(Constants.Procedures.FillSchemesDropdown, null);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable schemeDataTable = dataSet.Tables[0];
                if (schemeDataTable != null && schemeDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < schemeDataTable.Rows.Count; i++)
                    {
                        scheme.Add(new SelectListItem()
                        {
                            Text = string.IsNullOrEmpty(schemeDataTable.Rows[i]["Name"].ToString()) ? "" : Convert.ToString(schemeDataTable.Rows[i]["Name"]),
                            Value = string.IsNullOrEmpty(schemeDataTable.Rows[i]["ID"].ToString()) ? "" : Convert.ToString(schemeDataTable.Rows[i]["ID"]),
                        });
                    }
                }
                else
                {
                    scheme.DefaultIfEmpty();
                }
            }
            return scheme;
        }

        /// <summary>
        /// Fill Plan dropdown
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetPlanList()
        {
            List<SelectListItem> plan = new List<SelectListItem>();
            DataSet dataSet = new ADODataFunction().ExecuteDataset(Constants.Procedures.FillSchemesDropdown, null);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable planDataTable = dataSet.Tables[1];
                if (planDataTable != null && planDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < planDataTable.Rows.Count; i++)
                    {
                        plan.Add(new SelectListItem()
                        {
                            Text = string.IsNullOrEmpty(planDataTable.Rows[i]["Name"].ToString()) ? "" : Convert.ToString(planDataTable.Rows[i]["Name"]),
                            Value = string.IsNullOrEmpty(planDataTable.Rows[i]["ID"].ToString()) ? "" : Convert.ToString(planDataTable.Rows[i]["ID"]),
                        });
                    }
                }
                else
                {
                    plan.DefaultIfEmpty();
                }
            }
            return plan;
        }

        /// <summary>
        /// Fill OptionType 
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetOPtionTypeList()
        {
            List<SelectListItem> option = new List<SelectListItem>();
            DataSet dataSet = new ADODataFunction().ExecuteDataset(Constants.Procedures.FillSchemesDropdown, null);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable optionDataTable = dataSet.Tables[2];
                if (optionDataTable != null && optionDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < optionDataTable.Rows.Count; i++)
                    {
                        option.Add(new SelectListItem()
                        {
                            Text = string.IsNullOrEmpty(optionDataTable.Rows[i]["Name"].ToString()) ? "" : Convert.ToString(optionDataTable.Rows[i]["Name"]),
                            Value = string.IsNullOrEmpty(optionDataTable.Rows[i]["ID"].ToString()) ? "" : Convert.ToString(optionDataTable.Rows[i]["ID"]),
                        });
                    }
                }
                else
                {
                    option.DefaultIfEmpty();
                }
            }
            return option;
        }

        public List<SelectListItem> GetSeriesList()
        {
            List<SelectListItem> series = new List<SelectListItem>();
            DataSet dataSet = new ADODataFunction().ExecuteDataset(Constants.Procedures.FillSchemesDropdown, null);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable seriesDataTable = dataSet.Tables[3];
                if (seriesDataTable != null && seriesDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < seriesDataTable.Rows.Count; i++)
                    {
                        series.Add(new SelectListItem()
                        {
                            Text = string.IsNullOrEmpty(seriesDataTable.Rows[i]["Name"].ToString()) ? "" : Convert.ToString(seriesDataTable.Rows[i]["Name"]),
                            Value = string.IsNullOrEmpty(seriesDataTable.Rows[i]["ID"].ToString()) ? "" : Convert.ToString(seriesDataTable.Rows[i]["ID"]),
                        });
                    }
                }
                else
                {
                    series.DefaultIfEmpty();
                }
            }
            return series;
        }

        public List<SelectListItem> GetAssetClassList()
        {
            List<SelectListItem> assetClass = new List<SelectListItem>();
            DataSet dataSet = new ADODataFunction().ExecuteDataset(Constants.Procedures.FillSchemesDropdown, null);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable assetClassDataTable = dataSet.Tables[4];
                if (assetClassDataTable != null && assetClassDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < assetClassDataTable.Rows.Count; i++)
                    {
                        assetClass.Add(new SelectListItem()
                        {
                            Text = string.IsNullOrEmpty(assetClassDataTable.Rows[i]["Name"].ToString()) ? "" : Convert.ToString(assetClassDataTable.Rows[i]["Name"]),
                            Value = string.IsNullOrEmpty(assetClassDataTable.Rows[i]["ID"].ToString()) ? "" : Convert.ToString(assetClassDataTable.Rows[i]["ID"]),
                        });
                    }
                }
                else
                {
                    assetClass.DefaultIfEmpty();
                }
            }
            return assetClass;
        }

        //Get Minimum Holding Period
        public List<SelectListItem> GetMinimumHoldingPeriodDaysList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            DataSet dataSet = new ADODataFunction().ExecuteDataset(Constants.Procedures.FillSchemesDropdown, null);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable dataTable = dataSet.Tables[5];
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        list.Add(new SelectListItem()
                        {
                            Text = string.IsNullOrEmpty(dataTable.Rows[i]["Name"].ToString()) ? "" : Convert.ToString(dataTable.Rows[i]["Name"]),
                            Value = string.IsNullOrEmpty(dataTable.Rows[i]["ID"].ToString()) ? "" : Convert.ToString(dataTable.Rows[i]["ID"]),
                        });
                    }
                }
                else
                {
                    list.DefaultIfEmpty();
                }
            }
            return list;
        }
        
        //Get Minimum Holding Period Months
        public List<SelectListItem> GetMinimumHoldingPeriodMonthsList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            DataSet dataSet = new ADODataFunction().ExecuteDataset(Constants.Procedures.FillSchemesDropdown, null);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable dataTable = dataSet.Tables[6];
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        list.Add(new SelectListItem()
                        {
                            Text = string.IsNullOrEmpty(dataTable.Rows[i]["Name"].ToString()) ? "" : Convert.ToString(dataTable.Rows[i]["Name"]),
                            Value = string.IsNullOrEmpty(dataTable.Rows[i]["ID"].ToString()) ? "" : Convert.ToString(dataTable.Rows[i]["ID"]),
                        });
                    }
                }
                else
                {
                    list.DefaultIfEmpty();
                }
            }
            return list;
        }
        
        //Get Minimum Holding Period Years
        public List<SelectListItem> GetMinimumHoldingPeriodYearsList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            DataSet dataSet = new ADODataFunction().ExecuteDataset(Constants.Procedures.FillSchemesDropdown, null);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable dataTable = dataSet.Tables[7];
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        list.Add(new SelectListItem()
                        {
                            Text = string.IsNullOrEmpty(dataTable.Rows[i]["Name"].ToString()) ? "" : Convert.ToString(dataTable.Rows[i]["Name"]),
                            Value = string.IsNullOrEmpty(dataTable.Rows[i]["ID"].ToString()) ? "" : Convert.ToString(dataTable.Rows[i]["ID"]),
                        });
                    }
                }
                else
                {
                    list.DefaultIfEmpty();
                }
            }
            return list;
        }
        
        // GetCountryList
        public IEnumerable<SelectListItem> GetCountryList()
        {
            return _context.Country.AsNoTracking().OrderBy(e => e.CountryName).Where(e => e.IsActive == true).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.CountryName
            }).ToList();
        }

        #endregion

        #region Add,Update and Deactivate the data
        /// <summary>
        /// Add Update Scheme 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResponse AddUpdate(SchemeViewModel model)
        {
            try
            {
                SchemeRestrictedNationality res = new SchemeRestrictedNationality();
                SchemeRestrictedInvestorType invester = new SchemeRestrictedInvestorType();
                //  If model.ID == 0 the data goes to the Add part.   
                if (!IsExsits(model.schemeMaster.SchemeName, model.schemeMaster.ID))
                {
                    int insertSchme, insertTransaction,  insertReg, updateScheme = 0, updateTran = 0, updateReg = 0, updateNationality = 0, updateInvestorType = 0; 
                    if (model.schemeMaster.ID == 0)
                    {
                        //Insert SchemeMaster Table
                        model.schemeMaster.CreatedOn = DateTime.Now;
                        model.schemeMaster.CreatedBy = GetUserID();
                        _context.Set<Schema>().Add(model.schemeMaster);
                        insertSchme = _context.SaveChanges();

                        //Insert SchemeTrasactionDetails table
                        model.SchemeTransactionDetails.SchemeID = model.schemeMaster.ID;
                        model.SchemeTransactionDetails.CreatedOn = DateTime.Now;
                        model.SchemeTransactionDetails.CreatedBy = GetUserID();
                        model.SchemeTransactionDetails.IsActive = true;
                        _context.Set<SchemeTransaction>().Add(model.SchemeTransactionDetails);
                        insertTransaction = _context.SaveChanges();
                        if (insertTransaction != 0)
                        {
                            if (model.SchemeTransactionDetails.IsRecurring == true)
                            {
                                model.Frequency.IsActive = true;
                                model.Frequency.CreatedOn = DateTime.Now;
                                model.Frequency.CreatedBy = GetUserID();
                                model.Frequency.SchemeID = model.schemeMaster.ID;
                                _context.Set<Frequency>().Add(model.Frequency);
                                int l = _context.SaveChanges();
                            }
                        }

                        //Insert Scheme RegistrationDetails table
                        model.schemeRegistrationDetails.CreatedOn = DateTime.Now;
                        model.schemeRegistrationDetails.CreatedBy = GetUserID();
                        model.schemeRegistrationDetails.IsActive = true;
                        model.schemeRegistrationDetails.SchemeID = model.schemeMaster.ID;
                        _context.Set<SchemeRegistration>().Add(model.schemeRegistrationDetails);
                        insertReg = _context.SaveChanges();


                        //Insert RestrictedNationality
                        if (model.SchemeTransactionDetails.RestrictedNationality != null)
                        {
                            foreach (var item in model.SchemeTransactionDetails.RestrictedNationality)
                            {
                                res = null;
                                res= new SchemeRestrictedNationality();
                                res.IsActive = true;
                                res.CreatedOn = DateTime.Now;
                                res.CreatedBy = GetUserID();
                                res.SchemeID = model.schemeMaster.ID;
                                res.CountryId = Convert.ToInt32(item);
                                _context.Set<SchemeRestrictedNationality>().Add(res);
                                _context.SaveChangesAsync();
                            }
                        }

                        //Insert RestrictedInvestorType
                        if (model.SchemeTransactionDetails.InvestorType != null)
                        {
                            foreach (var item in model.SchemeTransactionDetails.InvestorType)
                            {
                                invester = null;
                                invester = new SchemeRestrictedInvestorType();
                                invester.IsActive = true;
                                invester.CreatedOn = DateTime.Now;
                                invester.CreatedBy = GetUserID();
                                invester.SchemeID = model.schemeMaster.ID;
                                invester.TypeId = Convert.ToInt32(item);
                                _context.Set<SchemeRestrictedInvestorType>().Add(invester);
                                 _context.SaveChangesAsync();
                            }
                        }

                        if (insertSchme != 0 && insertTransaction != 0 && insertReg != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Data_insert_success;
                        }
                        else
                        {
                            resp.Status = Constants.ResponseStatus.Failed;
                            resp.Message = Constants.Service.Data_insert_failed;
                        }
                    }
                    //  Else data goes to the Update part.
                    else
                    {
                        resp.Message = Constants.Service.Data_Update_failed;
                        var schemeDetails = GetData(model.schemeMaster.ID);
                        var transactionDetails = GetTransactionData(model.schemeMaster.ID);
                        var registrationDetils = GetRegistrationData(model.schemeMaster.ID);
                        var restrictedNational = GetRestrictedNationalData(model.schemeMaster.ID);
                        var restrictedInvesterType = GetRestrictedInvestorData(model.schemeMaster.ID);
                        //Update SchemeMaster Table
                        if (schemeDetails != null)
                        {
                            schemeDetails.SchemeCode = model.schemeMaster.SchemeCode;
                            schemeDetails.SchemeName = model.schemeMaster.SchemeName;
                            schemeDetails.SchemeTypeID = model.schemeMaster.SchemeTypeID;
                            schemeDetails.RTAID = model.schemeMaster.RTAID;
                            schemeDetails.AMCID = model.schemeMaster.AMCID;
                            schemeDetails.RTACode = model.schemeMaster.RTACode;
                            schemeDetails.PlanID = model.schemeMaster.PlanID;
                            schemeDetails.RTAProdCode = model.schemeMaster.RTAProdCode;
                            schemeDetails.IsActive = model.schemeMaster.IsActive;
                            schemeDetails.ModifiedOn = DateTime.Now;
                            schemeDetails.ModifiedBy = GetUserID();
                            _context.Set<Schema>().Update(schemeDetails);
                            updateScheme = _context.SaveChanges();

                        }
                        //Update SchemeTransaction table
                        if (transactionDetails != null)
                        {
                            transactionDetails.SIP = model.SchemeTransactionDetails.SIP;
                            transactionDetails.STP = model.SchemeTransactionDetails.STP;
                            transactionDetails.SWP = model.SchemeTransactionDetails.SWP;
                            transactionDetails.Demat = model.SchemeTransactionDetails.Demat;
                            transactionDetails.IncludedUnitsOfExDate = model.SchemeTransactionDetails.IncludedUnitsOfExDate;
                            transactionDetails.ModifiedBy = GetUserID();
                            transactionDetails.ModifiedOn = DateTime.Now;
                            _context.Set<SchemeTransaction>().Update(transactionDetails);
                            updateTran = _context.SaveChanges();
                            //check recurring is true, if its true  frequency is add
                            if (transactionDetails.IsRecurring == true)
                            {
                                var frequency = GetFrequncyData(model.schemeMaster.ID);
                                if (frequency != null)
                                {
                                    frequency.Daily = model.Frequency.Daily;
                                    frequency.Weekly = model.Frequency.Weekly;
                                    frequency.Monthly = model.Frequency.Monthly;
                                    frequency.Quartely = model.Frequency.Quartely;
                                    frequency.HalfYearly = model.Frequency.HalfYearly;
                                    frequency.Yearly = model.Frequency.Yearly;
                                    frequency.ModifiedBy = GetUserID();
                                    frequency.ModifiedOn = DateTime.Now;
                                    _context.Set<Frequency>().Update(frequency);
                                    int J = _context.SaveChanges();
                                    if (J != 0)
                                    {
                                        resp.Status = Constants.ResponseStatus.Success;
                                        resp.Message = Constants.Service.Data_Update_success;
                                    }
                                }
                                else
                                {
                                    model.Frequency.IsActive = true;
                                    model.Frequency.CreatedOn = DateTime.Now;
                                    model.Frequency.SchemeID = model.schemeMaster.ID;
                                    _context.Set<Frequency>().Add(model.Frequency);
                                    int J = _context.SaveChanges();
                                }
                            }
                        }
                        //Insert Scheme Registration table
                        if (registrationDetils != null)
                        {
                            registrationDetils.AMFICode = model.schemeRegistrationDetails.AMFICode;
                            registrationDetils.BseCode = model.schemeRegistrationDetails.BseCode;
                            registrationDetils.CloseDate = model.schemeRegistrationDetails.CloseDate;
                            registrationDetils.ISIN = model.schemeRegistrationDetails.ISIN;
                            registrationDetils.RefISIN = model.schemeRegistrationDetails.RefISIN;
                            registrationDetils.IssueOpenDate = model.schemeRegistrationDetails.IssueOpenDate;
                            registrationDetils.ReOpenDate = model.schemeRegistrationDetails.ReOpenDate;
                            registrationDetils.NSESymbol = model.schemeRegistrationDetails.NSESymbol;
                            registrationDetils.SchemeOption = model.schemeRegistrationDetails.SchemeOption;
                            registrationDetils.SeriesId = model.schemeRegistrationDetails.SeriesId;
                            registrationDetils.ModifiedBy = GetUserID();
                            registrationDetils.ModifiedOn = DateTime.Now;
                            _context.Set<SchemeRegistration>().Update(registrationDetils);
                            updateReg = _context.SaveChanges();
                            
                        }

                        //Update Restricted National table
                        var national = _context.SchemeRestrictedNationalities.Where(e => e.SchemeID == model.schemeMaster.ID).ToList();
                        if (national != null)
                        {
                            foreach (var item in national)
                            {
                                item.IsActive = false;
                                _context.SchemeRestrictedNationalities.Update(item);
                                _context.SaveChanges();
                            }
                        }
                        if (model.SchemeTransactionDetails.RestrictedNationality != null)
                        {
                            foreach (var item in model.SchemeTransactionDetails.RestrictedNationality)
                            {
                                res = null;
                                res = new SchemeRestrictedNationality();
                                res.IsActive = true;
                                res.CreatedOn = DateTime.Now;
                                res.CreatedBy = GetUserID();
                                res.SchemeID = model.schemeMaster.ID;
                                res.CountryId =Convert.ToInt32(item);
                                _context.Set<SchemeRestrictedNationality>().Add(res);
                                updateNationality = _context.SaveChanges();
                                model.schemeRestrictedNationality.ID = 0;

                            }
                        }
                        //Update Investor table
                        var investorType= _context.SchemeRestrictedInvestorTypes.Where(e => e.SchemeID == model.schemeMaster.ID).ToList();
                        if (investorType != null)
                        {
                            foreach (var item in investorType)
                            {
                                item.IsActive = false;
                                _context.SchemeRestrictedInvestorTypes.Update(item);
                                _context.SaveChanges();
                            }
                        }
                        if (model.SchemeTransactionDetails.InvestorType != null)
                        {
                            foreach (var item in model.SchemeTransactionDetails.InvestorType)
                            {
                                invester = null;
                                invester = new SchemeRestrictedInvestorType();
                                invester.IsActive = true;
                                invester.CreatedOn = DateTime.Now;
                                invester.CreatedBy = GetUserID();
                                invester.SchemeID = model.schemeMaster.ID;
                                invester.TypeId = Convert.ToInt32(item);
                                _context.Set<SchemeRestrictedInvestorType>().Add(invester);
                                updateInvestorType = _context.SaveChanges();
                                model.schemeRestrictedInvestorType.ID = 0;
                            }
                        }
                        //Check all table is updated
                        if (updateScheme != 0 && updateTran != 0 && updateReg != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Data_Update_success;
                        }
                        else
                        {
                            resp.Status = Constants.ResponseStatus.Failed;
                            resp.Message = Constants.Service.Data_Update_failed;
                        }

                    }
                }
                else
                {
                    resp.Status = Constants.ResponseStatus.Failed;
                    resp.Message = "Scheme Name is already exists..";
                }
            }
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
                throw ex;
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
                        _context.Set<Schema>().Update(model);
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

        //Get Data For SchemeMaster
        public Schema GetData(long ID)
        {
            if (ID != 0)
            {
                try
                {
                    return _context.SchemeMaster.AsNoTracking().Where(e => e.ID == ID).FirstOrDefault();
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

        //Get SchemeTransactionData
        public SchemeTransaction GetTransactionData(long SchemeID)
        {
            if (SchemeID != 0)
            {
                try
                {
                    return _context.SchemeTransactionDetails.AsNoTracking().Where(e => e.SchemeID == SchemeID).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        //Get SchemeRegistration Data
        public SchemeRegistration GetRegistrationData(long SchemeID)
        {
            if (SchemeID != 0)
            {
                try
                {
                    return _context.SchemeRegistrationDetails.AsNoTracking().Where(e => e.SchemeID == SchemeID).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public Frequency GetFrequncyData(long ID)
        {
            if (ID != 0)
            {
                try
                {
                    return _context.Frequency.AsNoTracking().Where(e => e.SchemeID == ID).FirstOrDefault();
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
        /// GetRestrictedNationalData based on schemeID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public SchemeRestrictedNationality GetRestrictedNationalData(long ID)
        {
            if (ID != 0)
            {
                try
                {
                    return _context.SchemeRestrictedNationalities.Where(e => e.SchemeID == ID).FirstOrDefault();
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

        // GetRestrictedInvestorData based on schemeID
        public SchemeRestrictedInvestorType GetRestrictedInvestorData(long ID)
        {
            if (ID != 0)
            {
                try
                {
                    return _context.SchemeRestrictedInvestorTypes.AsNoTracking().Where(e => e.SchemeID == ID).FirstOrDefault();
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
        public SchemeRestrictedNationality GetRestrictedNationality(long ID)
        {
            if (ID != 0)
            {
                try
                {
                    return _context.SchemeRestrictedNationalities.AsNoTracking().Where(e => e.SchemeID == ID).FirstOrDefault();
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
 
        public List<string> GetSchemesRestrictedNationality(long ID)
        {
            List<string> national = new List<string>();
            SqlParameter[] ObjParams = new SqlParameter[] {
                     new SqlParameter("@SchemeID", ID)
                    };
            DataSet dataSet = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetNationalityAndInvesterType, ObjParams);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable dataTable = dataSet.Tables[0];
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        // national.a(dataTable.Rows[i]["CountryID"].ToString());
                        national.Add(
                           Convert.ToString(dataTable.Rows[i]["CountryID"]));
                    }
                }
            }
            return national;
        }
        public List<string> GetSchemesRestrictedInvestorType(long ID)
        {
            //string[] investorType = null;
            List<string> investorType = new List<string>();
            SqlParameter[] ObjParams = new SqlParameter[] {
                     new SqlParameter("@SchemeID", ID)
                    };
            DataSet dataSet = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetNationalityAndInvesterType, ObjParams);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable dataTable = dataSet.Tables[1];
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        //investorType.Append(dataTable.Rows[i]["TypeID"].ToString());
                        investorType.Add(Convert.ToString(dataTable.Rows[i]["TypeID"]));
                    }
                }
            }
            return investorType;
        }

        public bool IsExsits(string name, long ID)
        {
            bool IsExsits = true;

            if (_context.SchemeMaster.Where(e => e.SchemeName == name && e.ID != ID).FirstOrDefault() == null)
            {
                IsExsits = false;
            }

            return IsExsits;

        }
        #endregion

        #region SchemeDetails for Listing

        //Get Scheme Data For Listing
        public IEnumerable<SchemeViewModel> Summary()
        {
            DataSet data = new DataSet();
            IEnumerable<SchemeViewModel> schemeMasters = null;
            try
            {
                data = new ADODataFunction().ExecuteDataset("dbo.Admin_GetSchemeMasterData", null, CommandType.StoredProcedure);
                DataTable dataTable = data.Tables[0];
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        schemeMasters = dataTable.AsEnumerable().Select(row => new SchemeViewModel
                        {
                            schemeMaster = new Schema
                            {
                                ID = row.Field<long>("ID"),
                                SchemeCode = row.Field<string>("SchemeCode"),
                                RtaName = row.Field<string>("RTAName"),
                                SchemeName = row.Field<string>("SchemeName"),
                                //SchemeTypeName = row.Field<string>("SchemeType"),
                                AmcName = row.Field<string>("AMCName"),
                                PlanName = row.Field<string>("PlanName"),
                                RTACode = row.Field<string>("RTACode"),
                                ShortName = row.Field<string>("ShortName"),
                                OldSchemeName = row.Field<string>("OldSchemeName"),
                                OptionTypeName = row.Field<string>("OptionType"),
                                IsActiveText = row.Field<string>("IsActive"),
                                LastUpdatedDate = row.Field<string>("LastUpdatedDate")
                            },
                            SchemeTransactionDetails = new SchemeTransaction
                            {
                                IsRecurring = row.Field<bool>("IsRecurring"),
                                SIP = row.Field<bool>("SIP"),
                                STP = row.Field<bool>("STP"),
                                SWP = row.Field<bool>("SWP"),
                                Demat = row.Field<bool>("Demat"),
                                IncludedUnitsOfExDate = row.Field<bool>("IncludedUnitsExDate"),
                            },
                            schemeRegistrationDetails = new SchemeRegistration
                            {
                                AMFICode = row.Field<string>("AMFICode"),
                                ISIN = row.Field<string>("ISIN"),
                                NSESymbol = row.Field<string>("NSESymbol"),
                                BseCode = row.Field<string>("BSECode"),
                                RefISIN = row.Field<string>("OldISIN")
                            }
                        });

                    }
                }
                return schemeMasters;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

    }
}
