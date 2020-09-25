using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ZOI.BAL.Utilites
{
    public class Constants
    {
        public const string APP_NAME = "ZOI APP";
        public const string Select = "--Select--";
        public const string Dashboard = "Dashboard";

        public class DboObjectNames
        {
            public const string Dbo1 = "dbo";
            public const string Dbo2 = "zoi_2FA";
            public const string Dbo3 = "zoi_wealth";
        }

        public class Procedures
        {

            //-------------------------------Dropdown Procedures---------------------------------------\\

            public const string GetAccountTypesDropDown = "Admin_Get_AccountTypes";
            public const string FillStateDropDown = "dbo.Admin_FillStateDropDown";
            public const string GetCountryDropDown = "dbo.Admin_GetCountryDropDown";
            public const string FillSchemesDropdown = "Admin_FillSchemeMasterDropDown";
            public const string FillCityDropDown = "dbo.Admin_FillCityDropDown";
            public const string GetAddressesDropDowns = "dbo.Admin_GetAddressesDropDowns";
            public const string GetTransactionTypeDropDown = "Admin_Get_TransactionTypeDropDown";
            public const string GetSchemaDropDownByAMC = "Admin_GetSchemaDropDownByAMC";
            public const string GetAMCDropDownByRTA = "Admin_GetAMCDropDownByRTA";
            public const string GetRTADropDown = "Admin_GetRTADropDown";
            public const string GetClientDropDown = "Admin_GetClientDropDown";


            //-------------------------CLient Folio---------------------------------------------\\

            public const string GetL4AccountTypeDropdown= "Admin_L4_AccountTypeDropdown";

            public const string GetL5AccountsDropdown= "Admin_L5_AccountsDropdown";

            //----------------------------------------------------------------------\\

            public const string GetSchemeNAV = "Admin_GetSchemeNAV";

            public const string ZOI_AddUser = "ZOI_AddUser";
            public const string GetRTAsSummary = "Admin_GetRTAsSummary";
            public const string GetAssetClassSummary = "Admin_GetAssetClassSummary";
            public const string GetProductTypeSummary = "Admin_GetProductTypeSummary";
            public const string GetSBUSummary = "Admin_GetSBUSummary";
            public const string GetInvestorCategorySummary = "Admin_GetInvestorCategorySummary";
            public const string GetAMCsSummary = "Admin_GetAMCsSummary";
            public const string GetBankSummary = "Admin_GetBankSummary";
            public const string GetMenus = "Admin_GetMenuList";
            public const string GetBankBranchSummary = "Admin_GetBankBranchSummary";
            public const string GetEquityBrokerSummary = "Admin_GetEquityBrokerSummary";
            public const string GetDipositorySummary = "Admin_GetDipository";

            // PinCodeMaster
            public const string GetPincodeSummary = "Admin_GetPincode";
            public const string InsertPincode = "Admin_InsertPincode";
            public const string UpdatePincode = "Admin_UpdatePinCode";

            //Holiday
            public const string GetHoliday = "Admin_GetHoliday";
            public const string InsertHoliday = "Admin_InsertHoliday";
            public const string UpdateHoliday = "Admin_UpdateHoliday";
            public const string RoleMenuPermission = "Admin_RoleMenuPermission";
            public const string AddUpdateClientsAccounts = "dbo.Admin_AddUpdate_ClientAccounts";
            public const string GetMenusToSetPermission = "Admin_GetMenusToSetPermission";
        //Menu Master
            public const string InsertMenu = "Admin_InsertMenu";
            public const string UpdateMenu = "Admin_UpdateMenu";
            public const string GetMenuList = "Admin_GetMenu";
            public const string GetCurrentUserMenu = "dbo.Admin_GetCurrentUserMenu";
        //Role Master
            public const string GetRoleList = "Admin_GetRole";
            public const string UpdateRole = "Admin_UpdateRole";
            public const string InsertRole = "Admin_InsertRole";
        //Employee Master
            public const string GetEmpList = "Admin_GetEmployee";
            public const string InsertEmployee = "Admin_InsertEmployee";
            public const string UpdateEmployee = "Admin_UpdateEmployee";

            //Enum Master
            public const string GetEnumList = "Admin_GetEnumList";
            public const string InsertEnum = "Admin_InsertEnum";
            public const string UpdateEnum = "Admin_UpdateEnum";

            //Entity Master
            public const string GetEntityList = "Admin_GetEntityData";
            public const string InsertEntity = "Admin_InsertEntity";
            public const string UpdateEntity = "Admin_UpdateEntity";
           
        //Country Master
            public const string GetCountryList = "SP_GetCountry";
            public const string FindCountry = "SP_FindCountry";
            public const string UpdateCountry = "SP_UpdateCountry";
            public const string InsertCountry = "SP_SaveCountry";
            public const string IsCoutryExist = "SP_IsCountryExists";
        //State Master
            public const string GetStateList = "SP_GetState";
            public const string FindState = "SP_FindState";
            public const string UpdateState = "SP_UpdateState";
            public const string InsertState = "SP_SaveState";
            public const string IsStateExist = "SP_IsStateExists";
            //City Master
            public const string GetCityList = "SP_GetCity";
            public const string FindCity = "SP_FindCity";
            public const string UpdateCity = "SP_UpdateCity";
            public const string InsertCity = "SP_SaveCity";
            public const string IsCityExist = "SP_IsCityExists";
        //Relationship
            public const string GetRelationShipData = "Admin_GetRelationshipMasterData";
        //Scheme Master
            public const string GetSchemeMasterData = "dbo.Admin_GetSchemeMasterData";
            public const string GetNationalityAndInvesterType = "Admin_GetRestrictedNationalAndInvestorType";
        //--Client Main
            public const string AddUpdateClientMain = "Admin_AddUpdate_ClientMain";
            public const string AddUpdateClientPersonalDetails = "Admin_AddUpdate_ClientPersonalDetails";
            public const string AddUpdateClientAddresses = "Admin_AddUpdate_ClientAddresses";
            public const string AddUpdateClientSecondaryContactDetails = "Admin_AddUpdate_ClientSecondaryContactDetails";
            public const string AddUpdateClientEquityBrokerDetails = "Admin_AddUpdate_ClientEquityBrokerDetails";
            public const string AddUpdateClientDepositoryDetails = "Admin_AddUpdate_ClientDepositoryDetails";
            public const string AddUpdateClientBankDetails = "Admin_AddUpdate_ClientBankDetails";
            public const string AddUpdateClientMappingDetails = "Admin_AddUpdate_Client_MappingDetails";
            public const string GetClientMain = "Admin_GetClientMain";
            public const string AddUpdateClientAccounts = "Admin_AddUpdate_ClientAccounts";
        //--CLient Master
            public const string EmployeeList = "Admin_Get_EmployeeList";
            public const string EquityBrokersList = "Admin_Get_EquityBrokersList";
            public const string MICRList = "Admin_Get_MICRList";
            public const string DipositoriesList = "Admin_Get_DipositoriesList";
            public const string EntityBranchList = "Admin_Get_EntityBranchList";
            public const string SBUList = "Admin_Get_SBUList";
            public const string ClienFamilyList = "Admin_Get_ClienFamilyList";

            public const string GetClientMainByID = "Admin_GetClientMainByID";           
            public const string GetBankDetailsByID = "Admin_GetBankDetailsByID";
            public const string GetDepositoryDetailsByID = "Admin_GetDepositoryDetailsByID";
            public const string GetEquityBrokerDetailsByID = "Admin_GetEquityBrokerDetailsByID";
            public const string GetSecondaryContactDetailsByID = "Admin_GetSecondaryContactDetailsByID";
            public const string GetMappingDetailsByID = "Admin_GetMappingDetailsByID";
            public const string GetAddressDetailsByID = "Admin_GetClientAddressesByID";
            public const string GetPersonalDetailsByID = "Admin_GetPersonalDetailsByID";

            public const string GetBankAccountType = "Admin_GetBankAccountType";
            public const string GetSchemePlan = "Admin_GetSchemePlan";
            public const string GetSchemeType = "Admin_GetSchemeType";

        //-- Client Folios
            public const string AddUpdateClientFolios = "Admin_AddUpdate_ClientFolios";
            public const string GetClientFoliosByID = "Admin_GetClientFoliosByID";
            public const string GetClientFolioSummary = "Admin_GetClientFolioSummary";
            public const string GetInvestorDetail = "Admin_GetInverstordetail";
            public const string IsClientFolioExsits = "Admin_IsClientFolioExsits";
        //-- Transaction Type
            public const string GetTransactionTypeByID = "Admin_GetTransactionTypeByID";
            public const string AddUpdateTransactionType = "Admin_AddUpdate_TransactionType";
            public const string TransactionTypeSummary = "Admin_Summary_TransactionType";
        //-- RTA Transaction Type
            public const string GetRTATransactionTypeByID = "Admin_GetRTATransactionTypesByID";
            public const string GetClientAccountsByID = "Admin_Get_ClientAccountsByID";
            public const string AddUpdateRTATransactionType = "Admin_AddUpdate_RTATransactionTypes";
            public const string RTATransactionTypeSummary = "Admin_Summary_RTATransactionTypes";            
         //   public const string GetTransactionTypeDropDown = "Admin_Get_TransactionTypeDropDown";
            public const string Getunmappedtransactiontypes = "Admin_Unmmaped_RTATransactionTypes";
            public const string UpdateMapping = "Admin_Update_Mapping";

            // L5 CLient
            public const string GetClientAccountSumary = "Admin_GetClientAccountSumary";
            
            public const string GetClientAccountByID = "Admin_GetClientAccountByID";

            public const string ClientAccontIsExists = "Admin_ClientAccountIsExists";

            public const string ChangeStatusClientAccount = "Admin_ChangeStatus_ClientAccounts";

            // MENU MASTER :-AMAR

            //public const string AddUpdateMenu = "Admin_Get_AccountTypes";

            //public const string GetMenuSumary = "Admin_GetClientAccountSumary";
            
        }

        public class ControllerMessage 
        {
            public const string  All_Fields_Mandatory= "All fields are mandatory.";            
            public const string  Data_Exsists= "This data already exists.";        
            public const string  Upload_Failed= "File Upload Failed.";            
            public const string  Upload_Needed= "Upload the Image file.";            
            public const string  Permission_Changed_Success= "Permission Changed Successfully.";            
            public const string  Permission_Changed_Failed= "Permission Changed Failed.";
            public const string  TransactionType_Mapping_Failed = "TransactionType Mapping Not Saved.";
            public const string TransactionType_Mapping_Success = "TransactionType Mapping Saved Successfully.";
        }

        public class ResponseStatus 
        {
            public const string  Success= "S";

            public const string  Failed= "F";

        }

        public class Service 
        {
            public const string  Data_insert_success= "Data inserted successfully";
            
            public const string  Data_insert_failed= "Data not insert";

             public const string  Data_Update_success= "Data updated successfully";
            
            public const string  Common_message= "Something went wrong";
            
            public const string  Status_changed_success= "Status changed successfully";
            
            public const string Data_Update_failed = "Data updated failed";

            public const string  Failed= "F";

            public const string No_Changes = "There is no changes in Permission.";
        }

        public class HttpStatusCode
        {
            //Success Responses : Level - 200
            public const int SUCCESS = 200;
            public const int CREATED = 201;
            public const int NON_AUTHORITATIVE_INFORMATION = 203;
            public const int NO_CONTENT = 204;

            //Warning Responses : Level - 400
            public const int BAD_REQUEST = 400;
            public const int UN_AUTHORIZED = 401;
            public const int FORBIDDEN = 403;
            public const int NOT_FOUND = 404;
            public const int CONFLICT = 409;

            //Error Responses : Level - 500
            public const int INTERNAL_SERVER_ERROR = 500;
            public const int NOT_IMPLEMENTED = 501;
            public const int BAD_GATEWAY = 502;
            public const int SERVICE_UNAVIALABLE = 503;
            public const int GATEWAY_TIMEOUT = 504;
            public const int NETWORK_TIMEOUT = 599;
        }

        public enum Number
        {
            One = 1
            , Two = 2
            , Three = 3
        }

        public enum ExportTypes
        {
            Excel = 1
            , CSV = 2
            , PDF = 3
        }

        public class ExportProperties
        {
            public const string PDF_Extension = ".pdf";
            public const string PDF_ContentType = "application/pdf";
        }

        public class FileExtension
        {
            public const string CSV = ".csv";
            public const string Excelx = ".xlsx";
            public const string Excel = ".xls";
            public const string PDF = ".pdf";
            public const string ZIP = ".zip";
            public const string ZIP_Merge = ".zip_merge";
        }

        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = System.IO.Path.GetExtension(path).ToLowerInvariant();

            return types[ext];
        }

        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".js", "application/javascript"},
                {".zip", "application/x-zip-compressed"},
                {".msg","application/vnd.ms-outlook"}
            };
        }

        public class Message
        {
            public class InvestorCategorys
            {
                //  Investor Categorys Code messages.

                public const string Code_Required = "Please enter the Investor Category Code";
                
                public const string Code_Expression = "Investor Category Name only support the Alphanumeric with space.";

                public const string Code_MinLength = "Investor Category Code should not be exists 5 characters.";

                public const string Code_MaxLength = " Investor Category Code should not be exists 10 characters.";

                //  Investor Categorys Name Messages.

                public const string Name_Required = "Please enter the Investor Category Name";
                                
                public const string Name_Expression = "Investor Category Name only support the Alphabets with space.";
                                
                public const string Name_MinLength = "Investor Category Name should not be exists 5 characters.";
                                
                public const string Name_MaxLength = " Investor Category Name should not be exists 500 characters.";


            }

            public static class UploadFileHeader
            {
                //public const string CAMSFolioFileFileFormat2 = "foliochk,inv_name,address1,address2,address3,city,pincode,product,sch_name,rep_date,clos_bal,rupee_bal,jnt_name1,jnt_name2,phone_off,phone_res,email,holding_na,uin_no,pan_no,joint1_pan,joint2_pan,guard_pan,tax_status,broker_cod,subbroker,reinv_flag,bank_name,branch,ac_type,ac_no,b_address1,b_address2,b_address3,b_city,b_pincode,inv_dob,mobile_no,occupation,inv_iin,nom_name,relation,nom_addr1,nom_addr2,nom_addr3,nom_city,nom_state,nom_pincod,nom_ph_off,nom_ph_res,nom_email,nom_percen,nom2_name,nom2_relat,nom2_addr1,nom2_addr2,nom2_addr3,nom2_city,nom2_state,nom2_pinco,nom2_ph_of,nom2_ph_re,nom2_email,nom2_perce,nom3_name,nom3_relat,nom3_addr1,nom3_addr2,nom3_addr3,nom3_city,nom3_state,nom3_pinco,nom3_ph_of,nom3_ph_re,nom3_email,nom3_perce,ifsc_code,dp_id,demat,guard_name,brokcode,folio_date,aadhaar,tpa_linked,fh_ckyc_no,jh1_ckyc,jh2_ckyc,g_ckyc_no,jh1_dob,jh2_dob,guardian_d,amc_code,gst_state_";
                //karvy tran
                // public const string RTAFile = "FMCODE,TD_FUND,TD_SCHEME,TD_PLAN,TD_ACNO,SCHPLN,DIVOPT,FUNDDESC,TD_PURRED,TD_TRNO,SMCODE,CHQNO,INVNAME,TRNMODE,TRNSTAT,TD_BRANCH,ISCTRNO,TD_TRDT,TD_PRDT,TD_UNITS,TD_AMT,TD_AGENT,TD_BROKER,BROKPER,BROKCOMM,INVID,CRDATE,CRTIME,TRNSUB,TD_APPNO,UNQNO,TRDESC,TD_TRTYPE,NAVDATE,PORTDT,ASSETTYPE,SUBTRTYPE,CITYCATEG0,EUIN,TRCHARGES,CLIENTID,DPID,STT,IHNO,BRANCHCODE,INWARDNUM1,PAN1,PAN2,PAN3,TDSAMOUNT,CHQDATE,CHQBANK,TRFLAG,LOAD1,BROK_ENTDT,NCTREMARKS,PRCODE1,STATUS,SCHEMEISIN,TD_NAV,INSAMOUNT,REJTRNOOR2,EVALID,EDECLFLAG,SUBARNCODE,ATMCARDRE3,ATMCARDST4,SCH1,PLN1,TD_TRXNMO5,NEWUNQNO,SIPREGDT,DIVPER,TD_POP,ELECTRXNF6,STAMPDUTY";
                // public const string RtaFileTypeComboHeader1 = "amc_code,folio_no,prodcode,scheme,inv_name,trxntype,trxnno";
                // public const string RtaFileTypeComboHeader2 = "amc_code,folio_no,prodcode";
                // public const string CAMSTransactionFileFormat2 = "AMC_CODE,FOLIO_NO,PRODCODE,SCHEME,INV_NAME,TRXNTYPE,TRXNNO,TRXNMODE,TRXNSTAT,USERCODE,USRTRXNO,TRADDATE,POSTDATE,PURPRICE,UNITS,AMOUNT,BROKCODE,SUBBROK,BROKPERC,BROKCOMM,ALTFOLIO,REP_DATE,TIME1,TRXNSUBTYP,APPLICATIO,TRXN_NATUR,TAX,TOTAL_TAX,TE_15H,MICR_NO,REMARKS,SWFLAG,OLD_FOLIO,SEQ_NO,REINVEST_F,MULT_BROK,STT,LOCATION,SCHEME_TYP,TAX_STATUS,LOAD,SCANREFNO,PAN,INV_IIN,TARG_SRC_S,TRXN_TYPE_,TICOB_TRTY,TICOB_TRNO,TICOB_POST,DP_ID,TRXN_CHARG,ELIGIB_AMT,SRC_OF_TXN,TRXN_SUFFI,SIPTRXNNO,TER_LOCATI,EUIN,EUIN_VALID,EUIN_OPTED,SUB_BRK_AR,EXCH_DC_FL,SRC_BRK_CO,SYS_REGN_D,AC_NO,BANK_NAME,REVERSAL_C,EXCHANGE_F,CA_INITIAT,GST_STATE_,IGST_AMOUN,CGST_AMOUN,SGST_AMOUN,REV_REMARK,ORIGINAL_T,STAMP_DUTY";
                //new folio file(cams 03082020112950_106234361R9.xls)
                //new transaction(CAMS 03082020112841_106234332R2.xls)
                //public const string KarvyTranFileHeaders = "FMCODE,TD_FUND,TD_SCHEME,TD_PLAN,TD_ACNO,SCHPLN,DIVOPT,FUNDDESC,TD_PURRED,TD_TRNO,SMCODE,CHQNO,INVNAME,TRNMODE,TRNSTAT,TD_BRANCH,ISCTRNO,TD_TRDT,TD_PRDT,TD_UNITS,TD_AMT,TD_AGENT,TD_BROKER,BROKPER,BROKCOMM,INVID,CRDATE,CRTIME,TRNSUB,TD_APPNO,UNQNO,TRDESC,TD_TRTYPE,NAVDATE,PORTDT,ASSETTYPE,SUBTRTYPE,CityCategory,eUIN,Trcharges,clientid,Dpid,STT,IHNO,BranchCode,InwardNumber,PAN1,PAN2,PAN3,TdsAmount,chqdate,chqbank,trflag,load1,BROK_ENTDT,NCTREMARKS,PRCODE1,Status,SchemeISIN,td_nav,InsAmount,RejTrnoOrgNo,eValid,eDeclFlag,SubArnCode,ATMCardRemarks,ATMCardStatus,Sch1,Pln1,td_trxnmode,NewUnqno,SipRegdt,sipregslno,DivPer,CAN,ExchOrgTrType,ElecTrxnFlag,cleared,Brok_ValueDt,td_pop,InvState,StampDuty";

                public const string CAMSTransactionFile = "amc_code,folio_no,prodcode,scheme,inv_name,trxntype,trxnno,trxnmode,trxnstat,usercode,usrtrxno,traddate,postdate,purprice,units,amount,brokcode,subbrok,brokperc,brokcomm,altfolio,rep_date,time1,trxnsubtyp,application_no,trxn_nature,tax,total_tax,te_15h,micr_no,remarks,swflag,old_folio,seq_no,reinvest_flag,mult_brok,stt,location,scheme_type,tax_status,load,scanrefno,pan,inv_iin,targ_src_scheme,trxn_type_flag,ticob_trtype,ticob_trno,ticob_posted_date,dp_id,trxn_charges,eligib_amt,src_of_txn,trxn_suffix,siptrxnno,ter_location,euin,euin_valid,euin_opted,sub_brk_arn,exch_dc_flag,src_brk_code,sys_regn_date,ac_no,bank_name,reversal_code,exchange_flag,ca_initiated_date,gst_state_code,igst_amount,cgst_amount,sgst_amount,rev_remark,original_trxnno,BatchNumber";
                public const string CAMSFolioFile = "foliochk,inv_name,address1,address2,address3,city,pincode,product,sch_name,rep_date,clos_bal,rupee_bal,jnt_name1,jnt_name2,phone_off,phone_res,email,holding_nature,uin_no,pan_no,joint1_pan,joint2_pan,guard_pan,tax_status,broker_code,subbroker,reinv_flag,bank_name,branch,ac_type,ac_no,b_address1,b_address2,b_address3,b_city,b_pincode,inv_dob,mobile_no,occupation,inv_iin,nom_name,relation,nom_addr1,nom_addr2,nom_addr3,nom_city,nom_state,nom_pincode,nom_ph_off,nom_ph_res,nom_email,nom_percentage,nom2_name,nom2_relation,nom2_addr1,nom2_addr2,nom2_addr3,nom2_city,nom2_state,nom2_pincode,nom2_ph_off,nom2_ph_res,nom2_email,nom2_percentage,nom3_name,nom3_relation,nom3_addr1,nom3_addr2,nom3_addr3,nom3_city,nom3_state,nom3_pincode,nom3_ph_off,nom3_ph_res,nom3_email,nom3_percentage,ifsc_code,dp_id,demat,guard_name,brokcode,folio_date,aadhaar,tpa_linked,fh_ckyc_no,jh1_ckyc,jh2_ckyc,g_ckyc_no,jh1_dob,jh2_dob,guardian_dob,amc_code,gst_state_code,BatchNumber";
                public const string FranklinAUMFileHeader = "BROKERCODE,SUBBROKER0,PRODCODE,SCHEME,PLAN_OPTI1,ACCNO,INVNAME,UNITBALAN2,NAV,NAVDATE,CURVALUE,INVADD1,INVADD2,INVADD3,INVCITY,INVPIN,EMAIL,OFFPH,HOMEPH,REPDATE,CUSTOMER_3,DIVIDEND_4,ISIN,BatchNumber";
                public const string FranklinTranFileHeader = "COMP_CODE,SCHEME_CO0,SCHEME_NA1,FOLIO_NO,TRXN_TYPE,TRXN_NO,INVESTOR_2,TRXN_DATE,POSTDT_DA3,NAV,POP,UNITS,AMOUNT,CHECK_NO,DIVIDEND_4,JOINT_NAM1,JOINT_NAM2,ADDRESS1,ADDRESS2,ADDRESS3,CITY,PIN_CODE,STATE,COUNTRY,D_BIRTH,PHONE_RES,PHONE_OFF,TAX_STATUS,OCCU_CODE,EMAIL,TRXN_MODE,TRXN_STAT,ISC_CODE,BROK_CODE,SUB_BROKE5,EUIN,BROK_PERC,BROK_COMM,INVEST_ID,CREA_DATE,CREA_TIME,TRXN_SUB,APPL_NO,TRXN_ID,STT,CUSTOMER_6,DIRECT_FL7,IT_PAN_NO1,IT_PAN_NO2,IT_PAN_NO3,PAN_STATU8,PAN_STATU9,PAN_STAT10,REGD_DATE,ISIN,FAMILY_S11,GOAL_SHEET,GOAL,GOAL_DET12,TXN_CHG,GROSS_AM13,ALLOTMEN14,SIP_INST15,ACCOUNT_16,FUND_OPT17,REMARKS,SOCIAL_S18,KYC_ID,KYC_STATUS,HOLDING_19,FOLIO_SO20,PLAN_TYPE,BRANCH_C21,KARVY_AC22,PBANK_NAME,PERSONAL23,PAYMENT_24,ACCOUNT_25,BRANCH_N26,PAYMENT_27,CUSTOMER28,DD_PAYAB29,BANK_CODE,IFSC_CODE,PAYMENT_30,NEFT_CODE,DEFAULT_31,SUB_BROK32,EUIN_INV33,B15_T15_34,COMMISSI35,SWITCH_R36,LOAD_AMO37,TDS_AMOUNT,ELECMODE38,NOMINEE1,NOMINEE2,NOMINEE3,NOMINEE_39,DP_ID,BENF_ID,STP_REGN40,TRANSFER41,SIP_UNIQ42,BatchNumber";
                public const string KarvyAUMFileHeader = "Product Code,Fund,Folio Number,Scheme Code,Fund Description,Balance,Pledged,Transaction Date,Transaction Type,Hold Mode,Agent Code,Broker Code,P-OUT Code,Investor ID,Investor Name,Address #1,Address #2,Address #3,City,Pincode,Phone Residence,Phone Office,Fax,Email,AUM,NAV,Report Date,Report Time,Dividend Option,PldgBank,PAN,Plan,SchemeISIN,ClientID,DPID,To Date,Mobile No,BatchNumber";
                public const string KarvyTranFileHeader = "fmcode,td_fund,td_scheme,td_plan,td_acno,schpln,divopt,funddesc,td_purred,td_trno,smcode,chqno,invname,trnmode,trnstat,td_branch,isctrno,td_trdt,td_prdt,td_units,td_amt,td_agent,td_broker,brokper,brokcomm,invid,crdate,crtime,trnsub,td_appno,unqno,trdesc,td_trtype,navdate,portdt,assettype,subtrtype,CityCategory,eUIN,Trcharges,clientid,Dpid,STT,IHNO,BranchCode,InwardNumber,PAN1,PAN2,PAN3,TdsAmount,chqdate,chqbank,trflag,load1,brok_entdt,nctremarks,prcode1,Status,SchemeISIN,td_nav,InsAmount,RejTrnoOrgNo,eValid,eDeclFlag,SubArnCode,ATMCardRemarks,ATMCardStatus,Sch1,Pln1,td_trxnmode,NewUnqno,SipRegdt,sipregslno,DivPer,CAN,ExchOrgTrType,ElecTrxnFlag,cleared,Brok_ValueDt,td_pop,InvState,StampDuty,BatchNumber";
            }

            public static class RtaFileTypeCombination
            {
                //public const string rtaFileTypecombo1 = "1,1,WBR2.xls";
                //public const string rtaFileTypecombo2 = "1,2,RtasCompination1.xls";
                //public const string rtaFileTypecombo3 = "2,1,RtasCompination2.xls";
                //public const string rtaFileTypecombo4 = "3,3,cams r 9 29072020090848_103920455R9.xlsx";
                //public const string rtaFileTypecombo5 = "3,1,cams r2 29072020072129_103920500R2.xlsx";
                //public const string rtaFileTypecombo6 = "2,4,WBR9__For L3  and Folio Tag master.xls";
                //public const string rtaFileTypecombo7 = "3,4,karvy MFSD307-7779797778845955W5446166.xlsx";

                public const string rtaFileTypecombo1 = "2,1,cams 03082020112950_106234361R9.xls";//foliofile
                public const string rtaFileTypecombo2 = "1,1,CAMS 03082020112841_106234332R2.xls";//tran
                public const string rtaFileTypecombo3 = "1,2,karvy tran 114010942876684RN5137109.xls";
                public const string rtaFileTypecombo4 = "2,3,Franklin AUM PFCTAsmmlpb8HTkbdJlZINg20200803114138000353ILAgZ0H.xls";
                public const string rtaFileTypecombo5 = "1,3,Franklin TRAN QoHmCWqvJNmG4dfq320200803114138000301Hd3K5RajA67o1.xls";
                public const string rtaFileTypecombo6 = "2,2,karvy AUM 203WBCUM122115251.xls";
                //public const string rtaFileTypecombo7 = "1,2,karvy tran 114010942876684RN5137109.xls";
            }

             public class ProductTypeMaster
             {
                //  Product Type Master Code messages.

                public const string Code_Required = "Please enter the Product Type Code";
                
                public const string Code_Expression = "Product Type Code only support the Alphanumeric with space.";

                public const string Code_MinLength = "Product Type Code should not be exists 5 characters.";

                public const string Code_MaxLength = "Product Type Code should not be exists 10 characters.";

                //  Product Type Master Name Messages.

                public const string Name_Required = "Please enter the Product Type Name";
                                
                public const string Name_Expression = "Product Type Name only support the Alphabets with space.";
                                
                public const string Name_MinLength = "Product Type Name should not be exists 5 characters.";
                                
                public const string Name_MaxLength = "Product Type Name should not be exists 500 characters.";

                //  Product Type Master Name Messages.

                public const string Description_Required = "Please enter the Product Description";
                                
                //public const string Description_Expression = "Product Description only support the Aplhabets with space.";
                                
                public const string Description_MinLength = "Product Description should not be exists 5 characters.";
                                
                public const string Description_MaxLength = "Product Description should not be exists 500 characters.";
                
                //  Product Type Master Name Messages.

                public const string KYCDescription_Required = "Please enter the Product KYC Description";
                                
                //public const string KCYDescription_Expression = "Product Description only support the Aplhabets with space.";
                                
                public const string KYCDescription_MinLength = "Product KYC Description should not be exists 5 characters.";
                                
                public const string KYCDescription_MaxLength = "Product KYC Description should not be exists 500 characters.";


             }

        }
    }

    public static class ExtensionMethods
    {
        public static List<Dictionary<string, object>> ToDynamicList(this DataTable dt)
        {
            var dynamicDt = new List<Dictionary<string, object>>();

            foreach (DataRow row in dt.Rows)
            {
                var dic = new Dictionary<string, object>();
                foreach (DataColumn column in dt.Columns)
                {
                    dic.Add(column.ColumnName, row[column]);
                }
                dynamicDt.Add(dic);
            }
            return dynamicDt;
        }
    }
  
}
