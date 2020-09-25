using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;
namespace ZOI.BAL.ViewModels
{
   public class BankAccountTypeViewModel : BaseViewModel
 
    {
       public  BankAccountTypeViewModel()
        {
            this.bankAccountType = new BankAccountType();
        }

        public BankAccountType bankAccountType { get; set; }

    }
}
