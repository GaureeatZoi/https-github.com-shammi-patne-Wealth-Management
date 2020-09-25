using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class SchemePlanViewModel : BaseViewModel
    {
        public SchemePlanViewModel()
        {
            this.schemePlan = new SchemePlan();
        }

        public SchemePlan schemePlan { get; set; }

    }
}
