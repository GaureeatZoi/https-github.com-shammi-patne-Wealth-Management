using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZOI.APP.Models
{
    public class GetGeneralFilters
    {
        public string InceptionDate { get; set; }
        public IEnumerable<SelectListItem> data { get; set; }
    }
}
