using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class AssetClassViewModel : BaseViewModel
    {
        public AssetClassViewModel()
        {
            this.assetClass = new AssetClass();
        }

        public AssetClass assetClass { get; set; }       

    }

}

