using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;
using ZOI.BAL.Models;
using ZOI.DAL.DatabaseUtility.Interface;
using ZOI.BAL.Utilites;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZOI.BAL.ViewModels;
using ZOI.BAL.DBContext;
using ZOI.BAL;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ZOI.DAL.DatabaseUtility.Services
{
    public class EntityService : IEntityService
    {
        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        JsonResponse resp = new JsonResponse();

        public EntityService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public long GetUserID()
        {
            var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            return Convert.ToInt64(identity.Claims.Where(c => c.Type == "RoleID")
                  .Select(c => c.Value).SingleOrDefault());
        }

        public JsonResponse Summary()
        {   //Used for Entity Listing 
            //Stored Procedure Used for listing  Admin_GetEmployee
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetEntityList, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new Entity
                {
                    ID = a.Field<long>("ID"),
                    EntityTypeName = a.Field<string>("EntityType"),
                    EntityCode = a.Field<string>("EntityCode"),
                    FirstName = a.Field<string>("EntityName"),
                    AddressLine1 = a.Field<string>("AddressLine1"),
                    AddressLine2 = a.Field<string>("AddressLine2"),
                    Pincode = a.Field<long>("Pincode"),
                    CountryName = a.Field<string>("Country"),
                    StateName = a.Field<string>("State"),
                    CityName = a.Field<string>("City"),
                    ContactPersonName = a.Field<string>("ContactPersonName"),
                    ContactNumber = a.Field<string>("ContactNumber"),
                    ContactEmail = a.Field<string>("ContactEmail"),
                    ManagerName = a.Field<string>("ManagerName"),
                    IsHO = a.Field<string>("IsHO"),
                    SebiRegistration = a.Field<string>("SEBIRegistrationNo"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate"),
                    IsActiveText = a.Field<string>("Status")
                }).ToList();
            }
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }



        public JsonResponse AddUpdate(EntityViewModel model)
        {
            //Used for inserting and updating Entity
            //Stored Procedure Used for insert Admin_InsertEntity
            //Stored Procedure Used for update Admin_UpdateEntity
            // Createdby and Modifiedby parameters are hardcoded as of now             
            if (model.entity.EntityTypeID != 3)
            {
                model.subbrokerMapping.SEBIRegistrationDate = DateTime.Today;
            }
            if (model.entity.ID == 0)
            {
                try
                {
                    SqlParameter[] ObjParams = new SqlParameter[] {
                         new SqlParameter("@EntityType",model.entity.EntityTypeID),
                         new SqlParameter("@FirstName", model.entity.FirstName),
                         new SqlParameter("@LastName", model.entity.LastName),
                         new SqlParameter("@AddressLine1", model.entity.AddressLine1),
                         new SqlParameter("@AddressLine2", model.entity.AddressLine2),
                         new SqlParameter("@CountryID", model.entity.CountryID),
                         new SqlParameter("@StateID", model.entity.StateID),
                         new SqlParameter("@CityID",model.entity.CityID),
                         new SqlParameter("@Pincode", model.entity.Pincode),
                         new SqlParameter("@ContactPersonName", model.entity.ContactPersonName),
                         new SqlParameter("@ContactNumber", model.entity.ContactNumber),
                         new SqlParameter("@ContactEmail", model.entity.ContactEmail),
                         new SqlParameter("@ManagerID", model.branchMapping.ManagerID),
                         new SqlParameter("@IsHO", model.branchMapping.IsHO),
                         new SqlParameter("@SEBIRegistrationNo", model.subbrokerMapping.SEBIRegistrationNo),
                          new SqlParameter("@SEBIRegistrationDate", model.subbrokerMapping.SEBIRegistrationDate),
                         new SqlParameter("@CreatedBy",GetUserID()),
                         new SqlParameter("@IsActive",model.entity.IsActive)
                    };

                    new ADODataFunction().ExecuteNonQuery(Constants.Procedures.InsertEntity, ObjParams, CommandType.StoredProcedure);
                    ObjParams = null;
                    resp.Status = "S";
                    resp.Message = "Data inserted successfully";

                }
                catch (Exception ex)
                {
                    resp.Status = "F";
                    resp.Message = "Data not insert";
                    throw ex;
                }
            }
            else
            {
                resp.Message = "Data updated failed";
                try
                {
                    long mappingid = 0;
                    if (model.entity.EntityTypeID == 1)
                    {
                        mappingid = model.branchMapping.ID;
                    }
                    else if (model.entity.EntityTypeID == 2)
                    {
                        mappingid = model.franchiseMapping.ID;
                    }
                    else if (model.entity.EntityTypeID == 3)
                    {
                        mappingid = model.subbrokerMapping.ID;
                    }

                    SqlParameter[] ObjParams = new SqlParameter[] {
                        new SqlParameter("@Id ",model.entity.ID),
                        new SqlParameter("@MappingId ",mappingid),
                        new SqlParameter("@EntityType",model.entity.EntityTypeID),
                        new SqlParameter("@LastName", model.entity.LastName),
                        new SqlParameter("@AddressLine1", model.entity.AddressLine1),
                        new SqlParameter("@AddressLine2", model.entity.AddressLine2),
                        new SqlParameter("@CountryID", model.entity.CountryID),
                        new SqlParameter("@StateID", model.entity.StateID),
                        new SqlParameter("@CityID",model.entity.CityID),
                        new SqlParameter("@Pincode", model.entity.Pincode),
                        new SqlParameter("@ContactPersonName", model.entity.ContactPersonName),
                        new SqlParameter("@ContactNumber", model.entity.ContactNumber),
                        new SqlParameter("@ContactEmail", model.entity.ContactEmail),
                        new SqlParameter("@ManagerID", model.branchMapping.ManagerID),
                        new SqlParameter("@IsHO", model.branchMapping.IsHO),
                        new SqlParameter("@SEBIRegistrationNo", model.subbrokerMapping.SEBIRegistrationNo),
                        new SqlParameter("@SEBIRegistrationDate", model.subbrokerMapping.SEBIRegistrationDate),
                        new SqlParameter("@ModifiedBy",GetUserID()),
                        new SqlParameter("@IsActive",model.entity.IsActive)
                                       };

                    new ADODataFunction().ExecuteNonQuery(Constants.Procedures.UpdateEntity, ObjParams, CommandType.StoredProcedure);
                    ObjParams = null;
                    resp.Status = Constants.ResponseStatus.Success;
                    resp.Message = "Data updated Successfully";
                }
                catch (Exception ex)
                {
                    resp.Status = "F";
                    resp.Message = "Data not Updated";
                    throw ex;
                }
            }
            return resp;
        }

        public IEnumerable<Entity> ListAll()
        {
            //Used for Entity Listing 
            //Stored Procedure Used for listing  Admin_GetEmployee
            DataSet data = new DataSet();
            IEnumerable<Entity> EntityList;
            try
            {
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetEntityList, null, CommandType.StoredProcedure);
                EntityList = data.Tables[1].AsEnumerable().Select(a => new Entity
                {
                    ID = a.Field<long>("ID"),
                    EntityTypeName = a.Field<string>("EntityType"),
                    EntityCode = a.Field<string>("EntityCode"),
                    FirstName = a.Field<string>("EntityName"),
                    AddressLine1 = a.Field<string>("AddressLine1"),
                    AddressLine2 = a.Field<string>("AddressLine2"),
                    Pincode = a.Field<long>("Pincode"),
                    CountryName = a.Field<string>("Country"),
                    StateName = a.Field<string>("State"),
                    CityName = a.Field<string>("City"),
                    ContactPersonName = a.Field<string>("ContactPersonName"),
                    ContactNumber = a.Field<string>("ContactNumber"),
                    ContactEmail = a.Field<string>("ContactEmail"),
                    ManagerName = a.Field<string>("ManagerName"),
                    IsHO = a.Field<string>("IsHO"),
                    SebiRegistration = a.Field<string>("SEBIRegistrationNo"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate"),
                    IsActiveText = a.Field<string>("Status")
                }).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
            return EntityList;
        }

        public bool IsExsits(string name, long ID)
        {  // For Unique Validation of Entity Name
            bool IsExsits = true;
            if (_context.Entity.Where(e => e.FirstName == name && e.ID != ID).FirstOrDefault() == null)
            {
                IsExsits = false;
            }
            return IsExsits;
        }




        public EntityViewModel GetData(long ID)
        {  //Used for finding Entity by Id
            EntityViewModel model = new EntityViewModel();
            if (ID != 0)
            {
                try
                {
                    model.entity = _context.Entity.AsNoTracking().Where(e => (e.ID) == ID).FirstOrDefault();

                    if (model.entity.EntityTypeID == 1)
                    {
                        model.entity.EntityTypeName = "Branch";
                        model.branchMapping = _context.BranchMapping.AsNoTracking().Where(e => (e.EntityID) == ID).FirstOrDefault();
                    }
                    if (model.entity.EntityTypeID == 2)
                    {
                        model.entity.EntityTypeName = "Franchise";
                        model.franchiseMapping = _context.FranchiseMapping.AsNoTracking().Where(e => (e.EntityID) == ID).FirstOrDefault();
                    }
                    if (model.entity.EntityTypeID == 3)
                    {
                        model.entity.EntityTypeName = "SubBroker";
                        model.subbrokerMapping = _context.SubbrokerMapping.AsNoTracking().Where(e => (e.EntityID) == ID).FirstOrDefault();
                    }
                    return model;
                }
                catch (Exception ex)
                {
                    return null;
                    throw ex;
                }
            }
            else
            {
                return null;
            }
        }

        public JsonResponse Deactivate(long ID, bool Status)
        {
            if (ID != 0)
            {
                resp.Message = "Data not deleted";
                try
                {
                    var model = GetData(ID);
                    model.entity.IsActive = Status;
                    model.entity.ModifiedOn = DateTime.Now;
                    model.entity.ModifiedBy = GetUserID();
                    if (model.entity.EntityTypeID == 1)
                    {
                        model.branchMapping.IsActive = Status;
                        model.branchMapping.ModifiedOn = DateTime.Now;
                        model.branchMapping.ModifiedBy = 1;
                        _context.Set<BranchMapping>().Update(model.branchMapping);
                    }
                    if (model.entity.EntityTypeID == 2)
                    {
                        model.franchiseMapping.IsActive = Status;
                        model.franchiseMapping.ModifiedOn = DateTime.Now;
                        model.franchiseMapping.ModifiedBy = 1;
                        _context.Set<FranchiseMapping>().Update(model.franchiseMapping);
                    }
                    if (model.entity.EntityTypeID == 3)
                    {
                        model.subbrokerMapping.IsActive = Status;
                        model.subbrokerMapping.ModifiedOn = DateTime.Now;
                        model.subbrokerMapping.ModifiedBy = 1;
                        _context.Set<SubbrokerMapping>().Update(model.subbrokerMapping);
                    }
                    _context.Set<Entity>().Update(model.entity);
                    int i = _context.SaveChanges();
                    if (i != 0)
                    {
                        resp.Status = "S";
                        resp.Message = "Data deleted successfully";
                    }
                }
                catch (Exception ex)
                {
                    resp.Status = "F";
                    resp.Message = "Data not deleted";
                    throw ex;
                }
            }
            else
            {
                resp.Message = "ID was not found";
            }
            return resp;
        }


        public IEnumerable<SelectListItem> GetCountryList()
        { //Country Dropdown
            return _context.Country.AsNoTracking().OrderBy(e => e.CountryName).Where(e => e.IsActive == true).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.CountryName
            }).ToList();
        }
        public IEnumerable<SelectListItem> GetStateList(long? CountryId)
        { //State Dropdown

            if (CountryId == null)
            {
                var d = _context.State.AsNoTracking().OrderBy(e => e.StateName).Where(e => e.IsActive).Select(e => new SelectListItem()
                {
                    Value = (e.Id).ToString(),
                    Text = e.StateName
                }).ToList();
                return d;
            }
            return _context.State.AsNoTracking().OrderBy(e => e.StateName).Where(e => e.CountryId == CountryId && e.IsActive).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.StateName
            }).ToList();

        }
        public IEnumerable<SelectListItem> GetCityList(long? StateID)
        {//City Dropdown
            if (StateID == null)
            {
                return _context.City.AsNoTracking().OrderBy(e => e.CityName).Where(e => e.IsActive).Select(e => new SelectListItem()
                {
                    Value = (e.Id).ToString(),
                    Text = e.CityName
                }).ToList();
            }

            return _context.City.AsNoTracking().OrderBy(e => e.CityName).Where(e => e.IsActive && e.StateId == StateID).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.CityName
            }).ToList();

        }
        public IEnumerable<SelectListItem> GetManagerList()
        {// REporting to Dropdown

            return _context.Employee.AsNoTracking().OrderBy(e => e.EmployeeCode).Where(e => e.IsActive == true).Select(e => new SelectListItem()
            {
                Value = (e.ID).ToString(),
                Text = e.FName.Trim() + " " + e.MName.Trim() + " " + e.LName.Trim()
            }).ToList();


        }
        public IEnumerable<SelectListItem> GetEntityTypeList()
        {//EntityType dropdown
            return _context.Enum.AsNoTracking().OrderBy(e => e.EnumCode).Where(e => e.EnumType == "EntityType" && e.IsActive == true).Select(e => new SelectListItem()
            {
                Value = (e.EnumValue).ToString(),
                Text = e.EnumCode
            }).ToList();
        }

    }
}
