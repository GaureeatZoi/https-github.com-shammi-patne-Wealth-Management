using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;


namespace ZOI.BAL.ViewModels
{
    public class SchemeTypeViewModel : BaseViewModel

    {
        public SchemeTypeViewModel()
        {
            this.schemeType = new SchemeType();
        }

      public  SchemeType schemeType { get; set; }
    }
}
