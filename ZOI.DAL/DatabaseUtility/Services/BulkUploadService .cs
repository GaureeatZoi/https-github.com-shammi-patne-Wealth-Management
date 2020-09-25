using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ZOI.BAL;
using ZOI.BAL.DBContext;
using ZOI.BAL.Models;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.DAL.DatabaseUtility.Services
{
    public class BulkUploadService : IBulkUploadService
    {
        private IConfiguration _configuration;
        private IHostingEnvironment _hostingEnvironment;
        private readonly DatabaseContext _context;

        public BulkUploadService(IConfiguration configuration, IHostingEnvironment hostingEnvironment, DatabaseContext context)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        /// <summary>
        /// Get Table from ExcelSheet
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public DataTable ExcelDataTable(IFormFile file)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads\\");
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            using (var fileStream = new FileStream(Path.Combine(fullPath, file.FileName), FileMode.Create))
            {
                fileStream.Position = 0;
                file.CopyTo(fileStream);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(fileStream);
                var result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });
                excelReader.Close();
                //table.Columns.Add("BatchNumber").DefaultValue = BatchNumber;
                DataTable table = result.Tables[0];
                return table;
            }
        }

        /// <summary>
        /// Fill Rta Dropdown
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> FillRTAs()
        {
            List<SelectListItem> rta = new List<SelectListItem>();
            DataSet dataSet = new ADODataFunction().ExecuteDataset("Admin_GetFilterForBulkUpload", null);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable rtaDataTable = dataSet.Tables[0];
                if (rtaDataTable != null && rtaDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < rtaDataTable.Rows.Count; i++)
                    {
                        rta.Add(new SelectListItem()
                        {
                            Text = string.IsNullOrEmpty(rtaDataTable.Rows[i]["Name"].ToString()) ? "" : Convert.ToString(rtaDataTable.Rows[i]["Name"]),
                            Value = string.IsNullOrEmpty(rtaDataTable.Rows[i]["ID"].ToString()) ? "" : Convert.ToString(rtaDataTable.Rows[i]["ID"]),
                        });
                    }
                }
                else
                {
                    rta.DefaultIfEmpty();
                }
            }
            return rta;
        }

        /// <summary>
        /// Fill FileType Dropdown
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> FillFileType()
        {
            List<SelectListItem> fileType = new List<SelectListItem>();

            DataSet dataSet = new ADODataFunction().ExecuteDataset("Admin_GetFilterForBulkUpload", null);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable fileTypeDataTable = dataSet.Tables[1];
                if (fileTypeDataTable != null && fileTypeDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < fileTypeDataTable.Rows.Count; i++)
                    {
                        fileType.Add(new SelectListItem()
                        {
                            Text = string.IsNullOrEmpty(fileTypeDataTable.Rows[i]["Name"].ToString()) ? "" : Convert.ToString(fileTypeDataTable.Rows[i]["Name"]),
                            Value = string.IsNullOrEmpty(fileTypeDataTable.Rows[i]["ID"].ToString()) ? "" : Convert.ToString(fileTypeDataTable.Rows[i]["ID"]),
                        });
                    }
                }
                else
                {
                    fileType.DefaultIfEmpty();
                }
            }
            return fileType;
        }
     
        /// <summary>
        /// Check condition for upload Excel sheet with combination of rta and filetype
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResponse ExcelUpload(BulkUpload model)
        {
            JsonResponse response = new JsonResponse();
            if (model.File == null)
            {
                response.Status = "F";
                response.Message = "Failed";
                return response;
            }
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                var extension = Path.GetExtension(model.File.FileName);
                if (extension.Equals(".xls") || extension.Equals(".xlsx"))
                {
                    bool FileHeaderMatching = false;
                    string[] FileHeader = null;
                    string[] RtaFileTypeCombo1 = BAL.Utilites.Constants.Message.RtaFileTypeCombination.rtaFileTypecombo1.Split(",");
                    string[] RtaFileTypeCombo2 = BAL.Utilites.Constants.Message.RtaFileTypeCombination.rtaFileTypecombo2.Split(",");
                    string[] RtaFileTypeCombo3 = BAL.Utilites.Constants.Message.RtaFileTypeCombination.rtaFileTypecombo3.Split(",");
                    string[] RtaFileTypeCombo4 = BAL.Utilites.Constants.Message.RtaFileTypeCombination.rtaFileTypecombo4.Split(",");
                    string[] RtaFileTypeCombo5 = BAL.Utilites.Constants.Message.RtaFileTypeCombination.rtaFileTypecombo5.Split(",");
                    string[] RtaFileTypeCombo6 = BAL.Utilites.Constants.Message.RtaFileTypeCombination.rtaFileTypecombo6.Split(",");
                    //string[] RtaFileTypeCombo7 = BAL.Utilites.Constants.Message.RtaFileTypeCombination.rtaFileTypecombo7.Split(",");
                    string ExpectedHeader = string.Empty;
                    string TableName = string.Empty;
                    DataTable table = ExcelDataTable(model.File);
                    //new folio
                     if (model.FileTypeId == Convert.ToInt32(RtaFileTypeCombo1[0]) && model.RTAsId == Convert.ToInt32(RtaFileTypeCombo1[1]) && model.File.FileName == RtaFileTypeCombo1[2])
                    {
                        ExpectedHeader = BAL.Utilites.Constants.Message.UploadFileHeader.CAMSFolioFile;
                        FileHeader = ExpectedHeader.Split(",");
                        TableName = "dbo.tbl_CamsFolio_Staging";
                    }
                    //new format camstran
                    else if (model.FileTypeId == Convert.ToInt32(RtaFileTypeCombo2[0]) && model.RTAsId == Convert.ToInt32(RtaFileTypeCombo2[1]) && model.File.FileName == RtaFileTypeCombo2[2])
                    {
                        ExpectedHeader = BAL.Utilites.Constants.Message.UploadFileHeader.CAMSTransactionFile;
                        FileHeader = ExpectedHeader.Split(",");
                        TableName = "dbo.tbl_CamsTransaction_Staging";
                    }
                    //Franklin tran
                    else if (model.FileTypeId == Convert.ToInt32(RtaFileTypeCombo5[0]) && model.RTAsId == Convert.ToInt32(RtaFileTypeCombo5[1]) && model.File.FileName == RtaFileTypeCombo5[2])
                    {
                        ExpectedHeader = BAL.Utilites.Constants.Message.UploadFileHeader.FranklinTranFileHeader;
                        FileHeader = ExpectedHeader.Split(",");
                        TableName = "dbo.tbl_Franklin_Tran_Staging";
                    }
                    //Franklin AUM
                    else if (model.FileTypeId == Convert.ToInt32(RtaFileTypeCombo4[0]) && model.RTAsId == Convert.ToInt32(RtaFileTypeCombo4[1]) && model.File.FileName == RtaFileTypeCombo4[2])
                    {
                        ExpectedHeader = BAL.Utilites.Constants.Message.UploadFileHeader.FranklinAUMFileHeader;
                        FileHeader = ExpectedHeader.Split(",");
                        TableName = "dbo.tbl_Franklin_AUM_Staging";
                    }
                    //Karvy AUM
                    else if (model.FileTypeId == Convert.ToInt32(RtaFileTypeCombo6[0]) && model.RTAsId == Convert.ToInt32(RtaFileTypeCombo6[1]) && model.File.FileName == RtaFileTypeCombo6[2])
                    {
                        ExpectedHeader = BAL.Utilites.Constants.Message.UploadFileHeader.KarvyAUMFileHeader;
                        FileHeader = ExpectedHeader.Split(",");
                        TableName = "dbo.tbl_karvy_aum_staging";
                    }
                    //Karvy Tran
                    else if (model.FileTypeId == Convert.ToInt32(RtaFileTypeCombo3[0]) && model.RTAsId == Convert.ToInt32(RtaFileTypeCombo3[1]) && model.File.FileName == RtaFileTypeCombo3[2])
                    {
                        ExpectedHeader = BAL.Utilites.Constants.Message.UploadFileHeader.KarvyTranFileHeader;
                        FileHeader = ExpectedHeader.Split(",");
                        TableName = "dbo.tbl_karvy_tran_staging";
                    }
                    else
                    {
                        response.Status = "F";
                        response.Message = "Rta's FileType Combination is mismatched.";
                    }
                    if (FileHeader != null)
                    {
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            if (table.Columns[i].ToString().ToLower() == FileHeader[i].ToLower())
                            {
                                FileHeaderMatching = true;
                            }
                            else
                            {
                                FileHeaderMatching = false;
                                response.Status = "F";
                                response.Message = FileHeader[i] + " column is missing in the selected file";
                                break;
                            }
                        }
                    }
                    if (FileHeaderMatching)
                    {
                        string status=UploadExcelFile(table, TableName, FileHeader);
                        if (status!=null)
                        {
                            model.BatchNumber = status;
                            int i = InsertBulkUploadDetails(model);
                            if (i != 0)
                            {
                                response.Status = "S";
                                response.Message = "Upload Successfully..";
                            }
                            else
                            {
                                response.Status = "F";
                                response.Message = "File is not inserted..";
                            }
                        }
                        else
                        {
                            response.Status = "F";
                            response.Message = "Something went wrong..";
                            
                        }
                      
                    }

                }
                else
                {
                    response.Status = "F";
                    response.Message = "File is not valid";
                }
            }
            catch (Exception ex)
            {
                throw ex;
              
            }
            return response;
        }


        //For identify Which column length is small size(Temporary uses)

        protected string GetBulkCopyColumnException(Exception ex, SqlBulkCopy bulkcopy)

        {
            string message = string.Empty;
            if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))

            {
                string pattern = @"\d+";
                Match match = Regex.Match(ex.Message.ToString(), pattern);
                var index = Convert.ToInt32(match.Value) - 1;

                FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                var sortedColumns = fi.GetValue(bulkcopy);
                var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                var metadata = itemdata.GetValue(items[index]);
                var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                message = (String.Format("Column: {0} contains data with a length greater than: {1}", column, length));
            }
            return message;
        }

        /// <summary>
        /// Upload Excelsheet to db
        /// </summary>
        /// <param name="table"></param>
        /// <param name="tableName"></param>
        /// <param name="ExpectedHeader"></param>
        /// <returns></returns>
        public string UploadExcelFile(DataTable table, string tableName, string[] ExpectedHeader)
        {
            string s = string.Empty;
            string BatchNumber = DateTime.Now.ToString("ddMMyyyyHHmmss");
            table.Columns.Add("BatchNumber").Expression= BatchNumber;
            string connectionstring = _configuration.GetConnectionString("DatabaseConnection");
            var sqlCopy = new SqlBulkCopy(connectionstring);
            try
            {
          
                using (sqlCopy = new SqlBulkCopy(connectionstring))
                {
                    sqlCopy.DestinationTableName = tableName;
                    for (int i = 0; i < ExpectedHeader.Length; i++)
                    {
                        sqlCopy.ColumnMappings.Add(ExpectedHeader[i], ExpectedHeader[i].Replace("-","_").Replace(" ","_"));
                    }
                    sqlCopy.WriteToServer(table);
                    s = BatchNumber;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Empty;
                if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                {
                    errorMessage = GetBulkCopyColumnException(ex, sqlCopy);
                    Exception exInvlidColumn = new Exception(errorMessage, ex);
                    s = errorMessage;
                    //throw ex;
                }
                throw ex;
            }
            return s;
        }

        /// <summary>
        /// Insert ExcelName and BatchNumber to db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertBulkUploadDetails(BulkUpload model)
        {
            model.UploadFile = model.File.FileName;
            model.IsActive = true;
            model.CreatedOn = DateTime.Now;
            model.CreatedBy = 1;
            _context.Set<BulkUpload>().Add(model);
            return _context.SaveChanges();

        }
 
        /// <summary>
        /// Get No of Rows in the Selected Excel sheet
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public JsonResponse GetSheetsRecords(IFormFile file)
        {
            JsonResponse response = new JsonResponse();
            var extension = Path.GetExtension(file.FileName);
            if (extension.Equals(".xls") || extension.Equals(".xlsx"))
            {
                DataTable table = ExcelDataTable(file);
                SqlParameter[] ObjParams = new SqlParameter[] {
                     new SqlParameter("@uploadfile", file.FileName)
                    };
                DataSet data = new ADODataFunction().ExecuteDataset("dbo.Admin_GetUploadedRecord", ObjParams, CommandType.StoredProcedure);
                response.Status = "S";
                response.Message = table.Rows.Count.ToString();
                response.Data = data.Tables[0].Rows[0]["UploadedRecords"].ToString();
            }
            else
            {
                response.Status = "F";
                response.Message = "File is not valid";
            }

            return response;
        }


        /// <summary>
        /// Get Uploaded Records for Export
        /// </summary>
        /// <returns></returns>
        public List<BulkUploadData> GetBulkUploadedData()
        {
            try
            {
                List<BulkUploadData> bulkData = new List<BulkUploadData>();

                DataSet data = new ADODataFunction().ExecuteDataset("dbo.GetBulkUploadedData", null);
                if (data != null)
                {
                    for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                    {
                        bulkData.Add(new BulkUploadData()
                        {
                            amc_code = Convert.ToString(data.Tables[0].Rows[i]["amc_code"]),
                            folio_no = Convert.ToString(data.Tables[0].Rows[i]["folio_no"]),
                            prodcode = Convert.ToString(data.Tables[0].Rows[i]["prodcode"]),
                            scheme = Convert.ToString(data.Tables[0].Rows[i]["scheme"]),
                            inv_name = Convert.ToString(data.Tables[0].Rows[i]["inv_name"]),
                            trxntype = Convert.ToString(data.Tables[0].Rows[i]["trxntype"]),
                            trxnno = Convert.ToString(data.Tables[0].Rows[i]["trxnno"]),
                            trxnmode = Convert.ToString(data.Tables[0].Rows[i]["trxnmode"]),
                            trxnstat = Convert.ToString(data.Tables[0].Rows[i]["trxnstat"]),
                            usercode = Convert.ToString(data.Tables[0].Rows[i]["usercode"]),
                            usrtrxno = Convert.ToString(data.Tables[0].Rows[i]["usrtrxno"]),
                            traddate = Convert.ToString(data.Tables[0].Rows[i]["traddate"]),
                            postdate = Convert.ToString(data.Tables[0].Rows[i]["postdate"]),
                            purprice = Convert.ToString(data.Tables[0].Rows[i]["purprice"]),
                            units = Convert.ToString(data.Tables[0].Rows[i]["units"]),
                            amount = Convert.ToString(data.Tables[0].Rows[i]["amount"]),
                            brokcode = Convert.ToString(data.Tables[0].Rows[i]["brokcode"]),
                            subbrok = Convert.ToString(data.Tables[0].Rows[i]["subbrok"]),
                            brokperc = Convert.ToString(data.Tables[0].Rows[i]["brokperc"]),
                            brokcomm = Convert.ToString(data.Tables[0].Rows[i]["brokcomm"]),
                            altfolio = Convert.ToString(data.Tables[0].Rows[i]["altfolio"]),
                            rep_date = Convert.ToString(data.Tables[0].Rows[i]["rep_date"]),
                            time1 = Convert.ToString(data.Tables[0].Rows[i]["time1"]),
                            trxnsubtyp = Convert.ToString(data.Tables[0].Rows[i]["trxnsubtyp"]),
                            application_no = Convert.ToString(data.Tables[0].Rows[i]["application_no"]),
                            trxn_nature = Convert.ToString(data.Tables[0].Rows[i]["trxn_nature"]),
                            tax = Convert.ToString(data.Tables[0].Rows[i]["tax"]),
                            total_tax = Convert.ToString(data.Tables[0].Rows[i]["total_tax"]),
                            te_15h = Convert.ToString(data.Tables[0].Rows[i]["te_15h"]),
                            micr_no = Convert.ToString(data.Tables[0].Rows[i]["micr_no"]),
                            remarks = Convert.ToString(data.Tables[0].Rows[i]["remarks"]),
                            swflag = Convert.ToString(data.Tables[0].Rows[i]["swflag"]),
                            old_folio = Convert.ToString(data.Tables[0].Rows[i]["old_folio"]),
                            seq_no = Convert.ToString(data.Tables[0].Rows[i]["seq_no"]),
                            reinvest_flag = Convert.ToString(data.Tables[0].Rows[i]["reinvest_flag"]),
                            mult_brok = Convert.ToString(data.Tables[0].Rows[i]["mult_brok"]),
                            stt = Convert.ToString(data.Tables[0].Rows[i]["stt"]),
                            location = Convert.ToString(data.Tables[0].Rows[i]["location"]),
                            scheme_type = Convert.ToString(data.Tables[0].Rows[i]["scheme_type"]),
                            tax_status = Convert.ToString(data.Tables[0].Rows[i]["tax_status"]),
                            load = Convert.ToString(data.Tables[0].Rows[i]["load"]),
                            scanrefno = Convert.ToString(data.Tables[0].Rows[i]["scanrefno"]),
                            pan = Convert.ToString(data.Tables[0].Rows[i]["pan"]),
                            inv_iin = Convert.ToString(data.Tables[0].Rows[i]["inv_iin"]),
                            targ_src_scheme = Convert.ToString(data.Tables[0].Rows[i]["targ_src_scheme"]),
                            trxn_type_flag = Convert.ToString(data.Tables[0].Rows[i]["trxn_type_flag"]),
                            ticob_trtype = Convert.ToString(data.Tables[0].Rows[i]["ticob_trtype"]),
                            ticob_trno = Convert.ToString(data.Tables[0].Rows[i]["ticob_trno"]),
                            ticob_posted_date = Convert.ToString(data.Tables[0].Rows[i]["ticob_posted_date"]),
                            dp_id = Convert.ToString(data.Tables[0].Rows[i]["dp_id"]),
                            trxn_charges = Convert.ToString(data.Tables[0].Rows[i]["trxn_charges"]),
                            eligib_amt = Convert.ToString(data.Tables[0].Rows[i]["eligib_amt"]),
                            src_of_txn = Convert.ToString(data.Tables[0].Rows[i]["src_of_txn"]),
                            trxn_suffix = Convert.ToString(data.Tables[0].Rows[i]["trxn_suffix"]),
                            siptrxnno = Convert.ToString(data.Tables[0].Rows[i]["siptrxnno"]),
                            ter_location = Convert.ToString(data.Tables[0].Rows[i]["ter_location"]),
                            euin = Convert.ToString(data.Tables[0].Rows[i]["euin"]),
                            euin_valid = Convert.ToString(data.Tables[0].Rows[i]["euin_valid"]),
                            euin_opted = Convert.ToString(data.Tables[0].Rows[i]["euin_opted"]),
                            sub_brk_arn = Convert.ToString(data.Tables[0].Rows[i]["sub_brk_arn"]),
                            exch_dc_flag = Convert.ToString(data.Tables[0].Rows[i]["exch_dc_flag"]),
                            src_brk_code = Convert.ToString(data.Tables[0].Rows[i]["src_brk_code"]),
                            sys_regn_date = Convert.ToString(data.Tables[0].Rows[i]["sys_regn_date"]),
                            ac_no = Convert.ToString(data.Tables[0].Rows[i]["ac_no"]),
                            bank_name = Convert.ToString(data.Tables[0].Rows[i]["bank_name"]),
                            reversal_code = Convert.ToString(data.Tables[0].Rows[i]["reversal_code"]),
                            exchange_flag = Convert.ToString(data.Tables[0].Rows[i]["exchange_flag"]),
                            ca_initiated_date = Convert.ToString(data.Tables[0].Rows[i]["ca_initiated_date"]),
                            gst_state_code = Convert.ToString(data.Tables[0].Rows[i]["gst_state_code"]),
                            igst_amount = Convert.ToString(data.Tables[0].Rows[i]["igst_amount"]),
                            cgst_amount = Convert.ToString(data.Tables[0].Rows[i]["cgst_amount"]),
                            sgst_amount = Convert.ToString(data.Tables[0].Rows[i]["sgst_amount"]),
                            rev_remark = Convert.ToString(data.Tables[0].Rows[i]["rev_remark"]),
                            original_trxnno = Convert.ToString(data.Tables[0].Rows[i]["original_trxnno"])
                        });
                    }
                }
                return bulkData;
            }
            catch (Exception ex)
            {
                StackTrace CallStack = new StackTrace(ex, true);
                ex.Data["ErrDescription"] = ex.Data["ErrDescription"] != null ? ex.Data["ErrDescription"] : string.Format("Error captured in {0} on Line No {1} of Method {2}", CallStack.GetFrame(0).GetFileName(), CallStack.GetFrame(0).GetFileLineNumber(), CallStack.GetFrame(0).GetMethod().ToString());
                throw ex;
            }
        }
    }
}