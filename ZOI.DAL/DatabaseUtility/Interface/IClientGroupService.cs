using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.Services.Interface
{
    public interface IClientGroupService
    {
        IEnumerable<SelectListItem> GetDropdownCountry();
        IEnumerable<SelectListItem> GetDropdownState(long countryId);
        IEnumerable<SelectListItem> GetDropdownCity(long stateId);

        JsonResponse FillStates(long countryId);

        JsonResponse FillCities(long stateId);

        ClientGroup GetData(long ID);

        JsonResponse AddUpdate(ClientGroup model);

        IEnumerable<ClientGroup> Summary();

        JsonResponse Deactivate(long ID,bool Status);

        bool IsExsits(string name);
    }
}
