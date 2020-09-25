using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.Services.Interface
{
    public interface IClientFamilyService
    {
        IEnumerable<ClientFamily> Summary();

        JsonResponse AddUpdate(ClientFamily model);

        ClientFamily GetData(long ID);

        JsonResponse Deactivate(long ID ,bool Status);

        IEnumerable<SelectListItem> GetDropdownGroups();

        bool IsExsits(string name);
    }
}
