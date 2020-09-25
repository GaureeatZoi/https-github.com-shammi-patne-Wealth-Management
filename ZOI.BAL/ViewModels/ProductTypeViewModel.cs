using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class ProductTypeViewModel : BaseViewModel
    {
        public ProductTypeViewModel()
        {
            this.productTypeMaster = new ProductTypeMaster();
        }

        public ProductTypeMaster productTypeMaster { get; set; }

       

    }

}

