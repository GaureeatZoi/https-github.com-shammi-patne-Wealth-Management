using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class RelationshipViewModel:BaseViewModel
    {
        public RelationshipViewModel()
        {
            Relationship = new Relationship();
        }

        public Relationship Relationship { get; set; }
    }
}
