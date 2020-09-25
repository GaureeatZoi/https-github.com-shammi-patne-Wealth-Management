using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class ClientAccountMappingViewModel : BaseViewModel
    {
        public ClientAccountMappingViewModel()
        {
            AccountsMapping = new ClientAccountsMapping();
        }

        public ClientAccountsMapping AccountsMapping { get; set; }
        
        public IEnumerable<SelectListItem> AccountTypeList { get; set; }
        
        public IEnumerable<SelectListItem> ClientList { get; set; }
        
        public IEnumerable<SelectListItem> EmployeList { get; set; }


    }
}
