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
    public class ClientMainService : IClientMainService
    {
        private readonly DatabaseContext _context;

        JsonResponse resp = new JsonResponse();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientMainService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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


        public JsonResponse Summary()
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@UserID", GetUserID());
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetClientMain, param, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new ClientMain
                {
                    ID = a.Field<long>("ID")
                   ,
                    ClientName = a.Field<string>("ClientName")
                   ,
                    FamilyName = a.Field<string>("FamilyName")
                   ,
                    PAN = a.Field<string>("PAN")
                   ,
                    DOB = a.Field<string>("DOB")
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
                        model.CreatedOn = DateTime.Now;
                        //model.CreatedBy = Convert.ToInt32(GetUserID("RoleID"));
                        model.CreatedBy = GetUserID();
                        _context.Set<ClientMain>().Update(model);
                        int i = _context.SaveChanges();
                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Status_changed_success;
                        }
                    }
                }
                // Else it Show the error message.
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

        public ClientMain GetData(long ID)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", ID);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetClientMainByID, param, CommandType.StoredProcedure);
                ClientMain datas = data.Tables[0].AsEnumerable().Select(a => new ClientMain
                {
                    ID = a.Field<long>("ID")
                    ,
                    FamilyID = !string.IsNullOrEmpty(Convert.ToString(a.Field<long>("FamilyID"))) ? a.Field<long>("FamilyID") : 0
                    ,
                    DOB = !string.IsNullOrEmpty(a.Field<string>("DOB")) ? a.Field<string>("DOB") : ""
                    ,
                    FirstName = !string.IsNullOrEmpty(a.Field<string>("FirstName")) ? a.Field<string>("FirstName") : ""
                    ,
                    Gender = !string.IsNullOrEmpty(Convert.ToString(a.Field<int>("Gender"))) ? a.Field<int>("Gender") : 0
                    ,
                    IntroducerID = !string.IsNullOrEmpty(Convert.ToString(a.Field<long>("IntroducerID"))) ? a.Field<long>("IntroducerID") : 0
                    ,
                    MiddleName = !string.IsNullOrEmpty(a.Field<string>("MiddleName")) ? a.Field<string>("MiddleName") : ""
                    ,
                    LastName = !string.IsNullOrEmpty(a.Field<string>("LastName")) ? a.Field<string>("LastName") : ""
                    ,
                    OccupationID = !string.IsNullOrEmpty(Convert.ToString(a.Field<int>("OccupationID"))) ? a.Field<int>("OccupationID") : 0
                    ,
                    PAN = !string.IsNullOrEmpty(a.Field<string>("PAN")) ? a.Field<string>("PAN") : ""
                    ,
                    Title = !string.IsNullOrEmpty(Convert.ToString(a.Field<int>("Title"))) ? a.Field<int>("Title") : 0
                    ,
                    MobileNumber = !string.IsNullOrEmpty(Convert.ToString(a.Field<long>("MobileNo"))) ? a.Field<long>("MobileNo") : 0
                    ,
                    EmailId = !string.IsNullOrEmpty(a.Field<string>("EmailId")) ? a.Field<string>("EmailId") : ""
                    ,
                    ShortName = !string.IsNullOrEmpty(a.Field<string>("ShortName")) ? a.Field<string>("ShortName") : ""
                    ,
                    IsActive = a.Field<bool>("IsActive")
                }).FirstOrDefault();
                return datas;
                //return _context.clientMains.Where(e=>e.ID ==ID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ClientAddresses GetAddressData(long ID)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", ID);
                data = new ADODataFunction().ExecuteDataset("Admin_GetClientAddressesByID", param, CommandType.StoredProcedure);
                ClientAddresses addresses = data.Tables[0].AsEnumerable().Select(a => new ClientAddresses
                {
                    Current_AddressLine1 = a.Field<string>("CurrentAddressLine1")
                    ,
                    Current_AddressLine2 = a.Field<string>("CurrentAddressLine2")
                    ,
                    Current_CountryID = a.Field<long>("CurrentCountryID")
                    ,
                    Current_StateID = a.Field<long>("CurrentStateID")
                    ,
                    Current_CityID = a.Field<long>("CurrentCityID")
                    ,
                    Current_PinCode = a.Field<long>("CurrentPinCode")
                    ,
                    Current_ID = a.Field<long>("CurrentID")
                    ,
                    Coressponding_AddressLine1 = a.Field<string>("CorrespondingAddressLine1")
                    ,
                    Coressponding_AddressLine2 = a.Field<string>("CorrespondingAddressLine2")
                    ,
                    Coressponding_CountryID = a.Field<long>("CorrespondingCountryID")
                    ,
                    Coressponding_StateID = a.Field<long>("CorrespondingStateID")
                    ,
                    Coressponding_CityID = a.Field<long>("CorrespondingCityID")
                    ,
                    Coressponding_PinCode = a.Field<long>("CorrespondingPinCode")
                    ,
                    Coressponding_ID = a.Field<long>("CorrespondingID")
                    ,
                    ClientID = a.Field<long>("ClientID")
                }).FirstOrDefault();
                return addresses;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ClientMapping GetMappingDetailData(long ID)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", ID);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetMappingDetailsByID, param, CommandType.StoredProcedure);
                ClientMapping datas = data.Tables[0].AsEnumerable().Select(a => new ClientMapping
                {
                    ID = a.Field<long>("ID")
                    ,
                    ClientID = a.Field<long>("ClientID")
                    ,
                    SBUID = a.Field<long>("SBUID")
                    ,
                    RMId = a.Field<long>("RMId")
                    ,
                    SecondaryRMId = a.Field<long>("SecondaryRMId")
                    ,
                    BranchId = a.Field<long>("BranchId")
                    ,
                    KYCFormNo = a.Field<string>("KYCFormNo")
                    ,
                    ModelId = a.Field<int>("ModelId")
                    ,
                    KYCValid = a.Field<bool>("KYCValid")
                    ,
                    EffectiveFrom = a.Field<string>("EffectiveFrom")
                    ,
                    EffectiveTo = a.Field<string>("EffectiveTo")
                    ,
                    HeadOfFamily = a.Field<bool>("HeadOfFamily")

                }).FirstOrDefault();
                return datas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ClientPersonal GetPersonalDetailData(long ID)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", ID);
                data = new ADODataFunction().ExecuteDataset("Admin_GetPersonalDetailsByID", param, CommandType.StoredProcedure);
                ClientPersonal datas = data.Tables[0].AsEnumerable().Select(a => new ClientPersonal
                {
                    ID = a.Field<long>("ID")
                    ,
                    ClientID = a.Field<long>("ClientID")
                    ,
                    MotherFirstName = a.Field<string>("MotherFirstName")
                    ,
                    MotherLastName = a.Field<string>("MotherLastName")
                    ,
                    SpouseOrFatherFirstName = a.Field<string>("SpouseOrFatherFirstName")
                    ,
                    SpouseOrFatherLastName = a.Field<string>("SpouseOrFatherLastName")
                    ,
                    MaidenFirstName = a.Field<string>("MaidenFirstName")
                    ,
                    MaidenLastName = a.Field<string>("MaidenLastName")
                    ,
                    MaritalStatusID = a.Field<int>("MaritalStatusID")
                    ,
                    AnnualIncomeID = a.Field<int>("AnualIncomeID")
                    ,
                    CitizenshipID = a.Field<int>("CitizenshipID")
                    ,
                    CommodityTradeClassificationID = a.Field<int>("CommodityTradeClassificationID")
                    ,
                    EducationID = a.Field<int>("EducationID")
                    ,
                    ResidentialStatusID = a.Field<int>("ResidentialStatusID")
                    ,
                    TradingExperienceID = a.Field<int>("TradingExperienceID")
                    ,
                    IsPoliticalExperienceID = a.Field<bool>("IsPoliticalExperienceID")


                }).FirstOrDefault();
                return datas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ClientBankDetails GetBankDetailsData(long ID)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", ID);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetBankDetailsByID, param, CommandType.StoredProcedure);
                ClientBankDetails datas = data.Tables[0].AsEnumerable().Select(a => new ClientBankDetails
                {
                    ID = a.Field<long>("ID")
                    ,
                    ClientID = a.Field<long>("ClientID")
                    ,
                    AccountType = a.Field<int>("AccountType")
                    ,
                    IFSCCode = a.Field<string>("IFSCCode")
                    ,
                    BranchMICRCode = a.Field<int>("BranchMICRCode")
                    ,
                    BankAccountNumber = a.Field<string>("BankAccountNumber")
                }).FirstOrDefault();
                return datas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ClientEquityBrokerDetails GetEquityBrokersData(long ID)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", ID);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetEquityBrokerDetailsByID, param, CommandType.StoredProcedure);
                ClientEquityBrokerDetails datas = data.Tables[0].AsEnumerable().Select(a => new ClientEquityBrokerDetails
                {
                    ID = a.Field<long>("ID")
                    ,
                    ClientID = a.Field<long>("ClientID")
                    ,
                    BrokerID = a.Field<long>("BrokerID")
                    ,
                    EffectiveFrom = a.Field<string>("EffectiveFrom")
                    ,
                    EffectiveTo = a.Field<string>("EffectiveTo")
                    ,
                    BrokerUCC = a.Field<string>("BrokerUCC")
                }).FirstOrDefault();
                return datas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ClientSecondaryContact GetSecondaryContactDetailData(long ID)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", ID);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetSecondaryContactDetailsByID, param, CommandType.StoredProcedure);
                ClientSecondaryContact datas = data.Tables[0].AsEnumerable().Select(a => new ClientSecondaryContact
                {
                    ID = a.Field<long>("ID")
                    ,
                    ClientID = a.Field<long>("ClientID")
                    ,
                    Name = a.Field<string>("Name")
                    ,
                    MobileNumber = a.Field<string>("MobileNumber")
                    ,
                    EmailId = a.Field<string>("EmailId")
                    ,
                    Relationship = a.Field<int>("Relationship")
                    ,
                    EffectiveFrom = a.Field<string>("EffectiveFrom")
                    ,
                    EffectiveTo = a.Field<string>("EffectiveTo")

                }).FirstOrDefault();
                return datas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ClientDepositoryDetails GetDepositryDetailData(long ID)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", ID);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetDepositoryDetailsByID, param, CommandType.StoredProcedure);
                ClientDepositoryDetails datas = data.Tables[0].AsEnumerable().Select(a => new ClientDepositoryDetails
                {
                    ID = a.Field<long>("ID")
                    ,
                    ClientID = a.Field<long>("ClientID")
                    ,
                    DpId = a.Field<long>("DpId")
                    ,
                    EffectiveFrom = a.Field<string>("EffectiveFrom")
                    ,
                    EffectiveTo = a.Field<string>("EffectiveTo")
                    ,
                    AccountNumber = a.Field<string>("AccountNumber")
                }).FirstOrDefault();
                return datas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResponse GetAddressesDropDowns(string Procedure, long Parameter, string Flag)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Parameters", Parameter);
                param[1] = new SqlParameter("@Flag", Flag);
                data = new ADODataFunction().ExecuteDataset(Procedure, param, CommandType.StoredProcedure);
                resp.Data = data.Tables[0].AsEnumerable().Select(a => new SelectListItem
                {
                    Text = a.Field<string>("Name"),
                    Value = Convert.ToString(a.Field<long>("ID"))
                }).ToList();
                resp.Status = Constants.ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                resp.Status = Constants.ResponseStatus.Failed;
            }
            return resp;
        }

        public IEnumerable<SelectListItem> GetDropDownList(string Procedure)
        {
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Procedure, null, CommandType.StoredProcedure);
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

        public IEnumerable<SelectListItem> FillEnumList(string EnumType)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@EnumType", EnumType);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetEnumList, param, CommandType.StoredProcedure);
                IEnumerable<SelectListItem> listItems = data.Tables[0].AsEnumerable().Select(a => new SelectListItem
                {
                    Text = a.Field<string>("Text"),
                    Value = Convert.ToString(a.Field<int>("Value"))
                }).ToList();
                return listItems;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public ClientMainViewModel GetDatas(long ID)
        //{
        //    //ClientMainViewModel model = new ClientMainViewModel
        //    {
        //        clientMain = GetData(ID),
        //        clientAddresses = GetAddressData(ID),
        //        clientBankDetails = GetBankDetailsData(ID),
        //        clientDepositry = GetDepositryDetailData(ID),
        //        clientEquityBrokerDetails = GetEquityBrokersData(ID),
        //        clientPersonalDetails = GetPersonalDetailData(ID),
        //        clientMappingDetails = GetMappingDetailData(ID),
        //        clientSecondaryContactDetails= GetSecondaryContactDetailData(ID)
        //    };
        //    return model;
        //}

        public JsonResponse AddUpdate(ClientMainViewModel model)
        {
            try
            {
                bool Valid_Mobile = IsExsits("Mobile", model.ClientDetails.MobileNumber.ToString()),
                    Valid_Email = IsExsits("Email", model.ClientDetails.EmailId),
                    Valid_Pan = IsExsits("PAN", model.ClientDetails.PAN);
                if (!Valid_Mobile && !Valid_Pan && !Valid_Email)
                {


                    long id;
                    SqlParameter[] param = new SqlParameter[16];
                    param[0] = new SqlParameter("@ID", model.ClientDetails.ID);
                    param[1] = new SqlParameter("@FamilyID", model.ClientDetails.FamilyID);
                    param[2] = new SqlParameter("@Title", model.ClientDetails.Title);
                    param[3] = new SqlParameter("@FirstName", model.ClientDetails.FirstName);
                    param[4] = new SqlParameter("@MiddleName", model.ClientDetails.MiddleName);
                    param[5] = new SqlParameter("@LastName", model.ClientDetails.LastName);
                    param[6] = new SqlParameter("@ShortName", model.ClientDetails.ShortName);
                    param[7] = new SqlParameter("@Gender", model.ClientDetails.Gender);
                    param[8] = new SqlParameter("@DOB", model.ClientDetails.DOB);
                    param[9] = new SqlParameter("@OccupationID", model.ClientDetails.OccupationID);
                    param[10] = new SqlParameter("@PAN", model.ClientDetails.PAN);
                    param[11] = new SqlParameter("@IntroducerID", model.ClientDetails.IntroducerID);
                    param[12] = new SqlParameter("@UserID", GetUserID());
                    //param[12] = new SqlParameter("@UserID", Convert.ToInt32(GetUserID("RoleID")));
                    param[13] = new SqlParameter("@IsActive", model.ClientDetails.IsActive);
                    param[14] = new SqlParameter("@Mobile", model.ClientDetails.MobileNumber);
                    param[15] = new SqlParameter("@EmailID", model.ClientDetails.EmailId);
                    id = Convert.ToInt64(new ADODataFunction().ExecuteScalar(Constants.Procedures.AddUpdateClientMain, param, CommandType.StoredProcedure));
                    //  If i != 0 means it affect one data So the data was Inserted.
                    if (id != 0)
                    {
                        model.PersonalDetails.ClientID = id;
                        model.AddressDetails.ClientID = id;
                        model.SecondaryContactDetails.ClientID = id;
                        model.EquityBrokerDetails.ClientID = id;
                        model.DepositryDetails.ClientID = id;
                        model.BankDetails.ClientID = id;
                        model.MappingDetails.ClientID = id;
                        int i = AddUpdateClientPersonalDetails(model.PersonalDetails),
                            j = AddUpdateClientAddresses(model.AddressDetails),
                            k = AddUpdateClientSecondaryContactDetails(model.SecondaryContactDetails),
                            l = AddUpdateClientEquityBrokerDetails(model.EquityBrokerDetails),
                            m = AddUpdateClientDepositoryDetails(model.DepositryDetails),
                            n = AddUpdateClientBankDetails(model.BankDetails),
                            o = AddUpdateClientMappingDetails(model.MappingDetails);
                        if (i > 0 && j > 0 && k > 0 && l > 0 && m > 0 && n > 0 && o > 0)
                        {
                            if (model.ClientDetails.ID == 0)
                            {
                                resp.Message = Constants.Service.Data_insert_success;
                            }
                            else
                            {
                                resp.Message = Constants.Service.Data_Update_success;
                            }
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Data = id;
                        }
                        else
                        {
                            resp.Message = Constants.Service.Data_insert_failed;
                        }

                    }
                    //else It gives some error in data insertion.
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
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }
        //public JsonResponse AddUpdateAccountDetails(List<ClientAccountList> model)
        //{
        //    try
        //    {
        //        if (model != null && model.Count > 0)
        //        {
        //            DataTable dt = ConvertToDataTable(model);
        //            SqlParameter[] param = new SqlParameter[3];
        //            param[0] = new SqlParameter("@TableParam", dt);
        //            param[1] = new SqlParameter("@ClientID", model[0].ClientID);
        //            param[2] = new SqlParameter("@UserID", GetUserID());
        //            int i = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.AddUpdateClientsAccounts, param, CommandType.StoredProcedure);
        //            //  If it return was not equal to 0 then it affect some rows means data inserted.
        //            if (i != 0)
        //            {
        //                resp.Status = Constants.ResponseStatus.Success;
        //                resp.Message = Constants.Service.Data_insert_success;
        //            }
        //            // Else Show the error message.
        //            else
        //            {
        //                resp.Status = Constants.ResponseStatus.Failed;
        //                resp.Message = Constants.Service.Data_insert_failed;
        //            }
        //        }
        //        else
        //        {
        //            resp.Message = Constants.Service.Data_insert_failed;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.Message = Constants.Service.Common_message;
        //    }
        //    return resp;
        //}

        public int AddUpdateClientPersonalDetails(ClientPersonal model)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[17];
                param[0] = new SqlParameter("@ID", model.ID);
                param[1] = new SqlParameter("@ClientID", model.ClientID);
                param[2] = new SqlParameter("@MaritalStatusID", model.MaritalStatusID);
                param[3] = new SqlParameter("@MotherFirstName", model.MotherFirstName);
                param[4] = new SqlParameter("@MotherLastName", model.MotherLastName);
                param[5] = new SqlParameter("@AnualIncomeID", model.AnnualIncomeID);
                param[6] = new SqlParameter("@TradingExperienceID", model.TradingExperienceID);
                param[7] = new SqlParameter("@CommodityTradeClassificationID", model.CommodityTradeClassificationID);
                param[8] = new SqlParameter("@IsPoliticalExperienceID", model.IsPoliticalExperienceID);
                param[9] = new SqlParameter("@CitizenshipID", model.CitizenshipID);
                param[10] = new SqlParameter("@ResidentialStatusID", model.ResidentialStatusID);
                param[11] = new SqlParameter("@EducationID", model.EducationID);
                param[12] = new SqlParameter("@SpouseOrFatherFirstName", model.SpouseOrFatherFirstName);
                param[13] = new SqlParameter("@SpouseOrFatherLastName", model.SpouseOrFatherLastName);
                param[14] = new SqlParameter("@MaidenFirstName", model.MaidenFirstName);
                param[15] = new SqlParameter("@MaidenLastName", model.MaidenLastName);
                param[16] = new SqlParameter("@UserID", GetUserID());
                //param[16] = new SqlParameter("@UserID", Convert.ToInt32(GetUserID("RoleID")));
                int i = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.AddUpdateClientPersonalDetails, param, CommandType.StoredProcedure);
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public int AddUpdateClientAddresses(ClientAddresses model)
        {
            try
            {
                int i = 0;
                SqlParameter[] param = new SqlParameter[16];
                param[0] = new SqlParameter("@CurrentID", model.Current_ID);
                param[1] = new SqlParameter("@ClientID", model.ClientID);
                param[2] = new SqlParameter("@CurrentAddressLine1", model.Current_AddressLine1);
                param[3] = new SqlParameter("@CurrentAddressLine2", model.Current_AddressLine2);
                param[4] = new SqlParameter("@CurrentCountryID", model.Current_CountryID);
                param[5] = new SqlParameter("@CurrentStateID", model.Current_StateID);
                param[6] = new SqlParameter("@CurrentCityID", model.Current_CityID);
                param[7] = new SqlParameter("@CurrentPinCode", model.Current_PinCode);
                param[8] = new SqlParameter("@CoresspondingID", model.Coressponding_ID);
                param[9] = new SqlParameter("@CoresspondingAddressLine1", model.Coressponding_AddressLine1);
                param[10] = new SqlParameter("@CoresspondingAddressLine2", model.Coressponding_AddressLine2);
                param[11] = new SqlParameter("@CoresspondingCountryID", model.Coressponding_CountryID);
                param[12] = new SqlParameter("@CoresspondingStateID", model.Coressponding_StateID);
                param[13] = new SqlParameter("@CoresspondingCityID", model.Coressponding_CityID);
                param[14] = new SqlParameter("@CoresspondingPinCode", model.Coressponding_PinCode);
                param[15] = new SqlParameter("@UserID", GetUserID());
                //param[15] = new SqlParameter("@UserID", Convert.ToInt32(GetUserID("RoleID")));
                i = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.AddUpdateClientAddresses, param, CommandType.StoredProcedure);

                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public int AddUpdateClientSecondaryContactDetails(ClientSecondaryContact model)
        {

            try
            {
                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@ID", model.ID);
                param[1] = new SqlParameter("@ClientID", model.ClientID);
                param[2] = new SqlParameter("@Relationship", model.Relationship);
                param[3] = new SqlParameter("@MobileNumber", model.MobileNumber);
                param[4] = new SqlParameter("@EmailId", model.EmailId);
                param[5] = new SqlParameter("@EffectiveFrom", model.EffectiveFrom);
                param[6] = new SqlParameter("@EffectiveTo", model.EffectiveTo);
                param[7] = new SqlParameter("@Name", model.Name);
                param[8] = new SqlParameter("@UserID", GetUserID());
                //param[8] = new SqlParameter("@UserID", Convert.ToInt32(GetUserID("RoleID")));
                int i = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.AddUpdateClientSecondaryContactDetails, param, CommandType.StoredProcedure);
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public int AddUpdateClientEquityBrokerDetails(ClientEquityBrokerDetails model)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@ID", model.ID);
                param[1] = new SqlParameter("@ClientID", model.ClientID);
                param[2] = new SqlParameter("@BrokerID", model.BrokerID);
                param[3] = new SqlParameter("@BrokerUCC", model.BrokerUCC);
                param[4] = new SqlParameter("@EffectiveFrom", model.EffectiveFrom);
                param[5] = new SqlParameter("@EffectiveTo", model.EffectiveTo);
                param[6] = new SqlParameter("@UserID", GetUserID());
                //param[6] = new SqlParameter("@UserID", Convert.ToInt32(GetUserID("RoleID")));
                int i = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.AddUpdateClientEquityBrokerDetails, param, CommandType.StoredProcedure);
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int AddUpdateClientDepositoryDetails(ClientDepositoryDetails model)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@ID", model.ID);
                param[1] = new SqlParameter("@ClientID", model.ClientID);
                param[2] = new SqlParameter("@DpId", model.DpId);
                param[3] = new SqlParameter("@AccountNumber", model.AccountNumber);
                param[4] = new SqlParameter("@EffectiveFrom", model.EffectiveFrom);
                param[5] = new SqlParameter("@EffectiveTo", model.EffectiveTo);
                //param[6] = new SqlParameter("@UserID", Convert.ToInt32(GetUserID("RoleID")));
                param[6] = new SqlParameter("@UserID", GetUserID());
                int i = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.AddUpdateClientDepositoryDetails, param, CommandType.StoredProcedure);
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public int AddUpdateClientMappingDetails(ClientMapping model)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[13];
                param[0] = new SqlParameter("@ID", model.ID);
                param[1] = new SqlParameter("@ClientID", model.ClientID);
                param[2] = new SqlParameter("@SBUID", model.SBUID);
                param[3] = new SqlParameter("@RMId", model.RMId);
                param[4] = new SqlParameter("@SecondaryRMId", model.SecondaryRMId);
                param[5] = new SqlParameter("@BranchId", model.BranchId);
                param[6] = new SqlParameter("@ModelId", model.ModelId);
                param[7] = new SqlParameter("@HeadOfFamily", model.HeadOfFamily);
                param[8] = new SqlParameter("@KYCValid", model.KYCValid);
                param[9] = new SqlParameter("@KYCFormNo", model.KYCFormNo);
                param[10] = new SqlParameter("@EffectiveFrom", model.EffectiveFrom);
                param[11] = new SqlParameter("@EffectiveTo", model.EffectiveTo);
                param[12] = new SqlParameter("@UserID", GetUserID());
                //param[12] = new SqlParameter("@UserID", Convert.ToInt32(GetUserID("RoleID")));
                int i = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.AddUpdateClientMappingDetails, param, CommandType.StoredProcedure);
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int AddUpdateClientBankDetails(ClientBankDetails model)
        {

            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@ID", model.ID);
                param[1] = new SqlParameter("@ClientID", model.ClientID);
                param[2] = new SqlParameter("@AccountType", model.AccountType);
                param[3] = new SqlParameter("@IFSCCode", model.IFSCCode);
                param[4] = new SqlParameter("@BranchMICRCode", model.BranchMICRCode);
                param[5] = new SqlParameter("@BankAccountNumber", model.BankAccountNumber);
                param[6] = new SqlParameter("@UserID", GetUserID());
                //param[6] = new SqlParameter("@UserID", Convert.ToInt32(GetUserID("RoleID")));
                int i = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.AddUpdateClientBankDetails, param, CommandType.StoredProcedure);
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }
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

        public bool IsExsits(string Flag, string Data)
        {
            var Exsits = false;
            var Client = string.Empty;
            if (Flag == "Mobile")
                Client = _context.clientMains.Where(e => e.MobileNumber == Convert.ToInt64(Data)).Select(e => e.FirstName).FirstOrDefault();
            if (Flag == "PAN")
                Client = _context.clientMains.Where(e => e.PAN == Data).Select(e => e.FirstName).FirstOrDefault();
            if (Flag == "Email")
                Client = _context.clientMains.Where(e => e.EmailId == Data).Select(e => e.FirstName).FirstOrDefault();
            if (Client == "" || Client != null)
            {
                Exsits = true;
            }
            return Exsits;
        }
    }
}
