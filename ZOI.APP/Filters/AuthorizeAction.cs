using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System.Linq;
using ZOI.DAL.DatabaseUtility.Services;
using ZOI.BAL.Models;
using ZOI.DAL.DatabaseUtility.Interface;
using System.Security.Claims;
using System.Collections.Generic;
using System;

namespace ZOI.APP.Filters
{
    public class AuthorizeAction : ActionFilterAttribute, IActionFilter
    {     

        private IServiceFactory ServiceFactory { get; }

        protected readonly IConfiguration configuration;


        private readonly string Key;


        public AuthorizeAction(string KEY, IServiceFactory service)
        {
            this.Key = KEY;
            ServiceFactory = service;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.RouteValues["Controller"];

            string actionName = filterContext.ActionDescriptor.RouteValues["Action"];

            if (filterContext.HttpContext.Request.Headers["Referer"].ToString() == "" && actionName != null && controllerName !=null)
            {
                filterContext.HttpContext.Response.StatusCode = 405;
            }
            Controller controller = filterContext.Controller as Controller;
            if (controller != null)
            {
                if (filterContext.HttpContext.User.Claims.Count() == 0)
                {
                    filterContext.HttpContext.Response.StatusCode = 400;
                    filterContext.Result =
                           new RedirectToRouteResult(
                               new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    if (controllerName!= "Home")
                    {
                        int RoleID = Convert.ToInt32(GetUserID("RoleID"));
                        MenuPermission permission = ServiceFactory.GetService<BaseService>().CurrentMenuPermission(controllerName, RoleID);
                        if (Key == "Read")
                        {
                            if (!permission.Read) 
                            {
                                filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary { { "controller", "Home" }, { "action", "NotAuthorize" } });
                            }
                        }
                        else if (Key == "Write")
                        {
                            if (!permission.Write)
                            {
                                filterContext.Result = new RedirectToRouteResult(
                                 new RouteValueDictionary { { "controller", "Home" }, { "action", "NotAuthorize" } });
                            }
                        }
                        else
                        {
                            filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary { { "controller", "Home" }, { "action", "NotAuthorize" } });
                        }
                    }                    
                    base.OnActionExecuting(filterContext);
                }
            }
            string GetUserID(string key)
            {
                var identity = (ClaimsIdentity)filterContext.HttpContext.User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                return identity.Claims.Where(c => c.Type == key)
                      .Select(c => c.Value).SingleOrDefault();
            }
        }
    }
}
