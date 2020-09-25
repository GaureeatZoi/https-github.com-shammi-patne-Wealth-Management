using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZOI.BAL.Models
{
    public class MenuPermissionList
    {
        public bool ExportCSV { get; set; }

        public bool ExportExcel { get; set; }

        public bool ExportPDF { get; set; }

        public int MenuID { get; set; }

        public bool ReadAccess { get; set; }

        public int RoleID { get; set; }

        public bool SettingsAccess { get; set; }

        public bool WriteAccess { get; set; }

    }
}
