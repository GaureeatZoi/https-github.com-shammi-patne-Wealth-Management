using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;

namespace ZOI.BAL.Models
{
    public class BulkUploadData
    {
        public string amc_code { get; set; }
        public string folio_no { get; set; }
        public string prodcode { get; set; }
        public string scheme { get; set; }
        public string inv_name { get; set; }
        public string trxntype { get; set; }
        public string trxnno { get; set; }
        public string trxnmode { get; set; }
        public string trxnstat { get; set; }
        public string usercode { get; set; }
        public string usrtrxno { get; set; }
        public string traddate { get; set; }
        public string postdate { get; set; }
        public string purprice { get; set; }
        public string units { get; set; }
        public string amount { get; set; }
        public string brokcode { get; set; }
        public string subbrok { get; set; }
        public string brokperc { get; set; }
        public string brokcomm { get; set; }
        public string altfolio { get; set; }
        public string rep_date { get; set; }
        public string time1 { get; set; }
        public string trxnsubtyp { get; set; }
        public string application_no { get; set; }
        public string trxn_nature { get; set; }
        public string tax { get; set; }
        public string total_tax { get; set; }
        public string te_15h { get; set; }
        public string micr_no { get; set; }
        public string remarks { get; set; }
        public string swflag { get; set; }
        public string old_folio { get; set; }
        public string seq_no { get; set; }
        public string reinvest_flag { get; set; }
        public string mult_brok { get; set; }
        public string stt { get; set; }
        public string location { get; set; }
        public string scheme_type { get; set; }
        public string tax_status { get; set; }
        public string load { get; set; }
        public string scanrefno { get; set; }
        public string pan { get; set; }
        public string inv_iin { get; set; }
        public string targ_src_scheme { get; set; }
        public string trxn_type_flag { get; set; }
        public string ticob_trtype { get; set; }
        public string ticob_trno { get; set; }
        public string ticob_posted_date { get; set; }
        public string dp_id { get; set; }
        public string trxn_charges { get; set; }
        public string eligib_amt { get; set; }
        public string src_of_txn { get; set; }
        public string trxn_suffix { get; set; }
        public string siptrxnno { get; set; }
        public string ter_location { get; set; }
        public string euin { get; set; }
        public string euin_valid { get; set; }
        public string euin_opted { get; set; }
        public string sub_brk_arn { get; set; }
        public string exch_dc_flag { get; set; }
        public string src_brk_code { get; set; }
        public string sys_regn_date { get; set; }
        public string ac_no { get; set; }
        public string bank_name { get; set; }
        public string reversal_code { get; set; }
        public string exchange_flag { get; set; }
        public string ca_initiated_date { get; set; }
        public string gst_state_code { get; set; }
        public string igst_amount { get; set; }
        public string cgst_amount { get; set; }
        public string sgst_amount { get; set; }
        public string rev_remark { get; set; }
        public string original_trxnno { get; set; }

        public List<BulkUploadData> ListBulkData { get; set; }
    }

    public class RtaFileTypeCombo1
    {
        public string amc_code { get; set; }
        public string folio_no { get; set; }
        public string prodcode { get; set; }
        public string scheme { get; set; }
        public string inv_name  { get; set; }
        public string trxntype  { get; set; }
        public string trxnno   { get; set; }

    }
    public class RtaFileTypeCombo2
    {
        public string amc_code { get; set; }
        public string folio_no { get; set; }
        public string prodcode { get; set; }

    }
}
