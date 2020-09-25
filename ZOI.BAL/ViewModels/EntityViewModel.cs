using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
   public  class EntityViewModel : BaseViewModel
    {
        public  EntityViewModel()
        {
            this.entity = new Entity();
            this.branchMapping = new BranchMapping();
            this.subbrokerMapping = new SubbrokerMapping();
            this.franchiseMapping = new FranchiseMapping();


        }


        public Entity entity { get; set; }
        public SubbrokerMapping subbrokerMapping { get; set; }
        public FranchiseMapping franchiseMapping { get; set; }
        public BranchMapping branchMapping { get; set; }

        public IEnumerable<SelectListItem> EntityType { get; set; }
        public IEnumerable<SelectListItem> Country { get; set; }
        public IEnumerable<SelectListItem> State { get; set; }
        public IEnumerable<SelectListItem> City { get; set; }
        public IEnumerable<SelectListItem> Manager { get; set; }


    }
    
}
