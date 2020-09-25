using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ZOI.BAL.Services.Interface
{
    public interface IStateService
    {
        public JsonResponse Summary();
        IEnumerable<State> ListAll();
        IEnumerable<SelectListItem> InitStateView();
        State Find(long id);
        public JsonResponse Deactivate(long ID, bool Status);
        bool IsStateExists(string StateName);
        public bool IsgstcodeExists(int name);
        public JsonResponse AddUpdate(State State);
    } 
}
