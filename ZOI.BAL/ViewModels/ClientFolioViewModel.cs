using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public  class ClientFolioViewModel : BaseViewModel
    {
        public ClientFolioViewModel()
        {
            this.clientFolio = new ClientFolio();
        }

        public ClientFolio clientFolio { get; set; }

        public IEnumerable<SelectListItem> CLientList { get; set; }
        
        public IEnumerable<SelectListItem> AccountTypeList { get; set; }

        public IEnumerable<SelectListItem> AccountList { get; set; }
        
        public IEnumerable<SelectListItem> AMCList { get; set; }
        
        public IEnumerable<SelectListItem> SchemaList { get; set; }        


    }
}
