using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;
namespace ZOI.BAL.ViewModels
{
    public class   HolidayViewModel : BaseViewModel
    {
        public HolidayViewModel()
        {
            this.holyday = new Holidays();
        }

        public Holidays holyday { get; set; }
    }
}
