using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZOI.BAL.Models;

namespace ZOI.APP.Models.Base
{
    public class BaseViewModel
    {
        public int Id { get; set; }

        public IEnumerable<Menu> Menus { get; set; }

        public bool IsCreateAllowed { get; set; }

        public bool IsEditAllowed { get; set; }

        public bool IsViewAllowed { get; set; }

        public bool ForEdit { get; set; }

        public bool ForView { get; set; }
    }
}
