using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Claims;
using ZOI.BAL;
using ZOI.BAL.Configuration;
using ZOI.BAL.DBContext;
using ZOI.BAL.Models;
using ZOI.BAL.Services;
using ZOI.BAL.Services.Interface;
using ZOI.DAL;
using ZOI.DAL.DatabaseUtility.Interface;
using ZOI.DAL.DatabaseUtility.Services;

namespace ZOI.APP
{
    public static class SameSite
    {
        public static void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                if (!BrowserSupportsSameSiteNone(httpContext.Request.Headers["User-Agent"].ToString()))
                {
                    // Unspecified - no SameSite will be included in the Set-Cookie.
                    options.SameSite = (SameSiteMode)(-1);
                }
            }
        }

        private static bool BrowserSupportsSameSiteNone(string userAgent)
        {
            // iOS 12 browsers don't support SameSite=None.
            if (userAgent.Contains("CPU iPhone OS 12") || userAgent.Contains("iPad; CPU OS 12"))
            {
                return false;
            }

            // macOS 10.14 Mojave browsers don't support SameSite=None.
            if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") && userAgent.Contains("Version/") && userAgent.Contains("Safari"))
            {
                return false;
            }

            // Old versions of Chrome don't support SameSite=None.
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return false;
            }

            return true;
        }
    }
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // SameSiteMode.None is required to support SAML SSO.
                options.MinimumSameSitePolicy = SameSiteMode.None;
                // Some older browsers don't support SameSiteMode.None.
                options.OnAppendCookie = cookieContext => SameSite.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext => SameSite.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.ConsentCookie.Expiration = TimeSpan.FromHours(360);
            });

            //services.AddDbContext<ApplicationContext>(options =>
            //   options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection")));

            // aspnetrun dependencies
            ConfigureAspnetRunServices(services);

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie();

            services.AddCors();

            services.AddMvc(option =>
            {
                option.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "ExampleServiceProvider.Identity";
            });

            services.AddScoped<IMenuService, MenuService>();

            //services.AddScoped<IClaimsPrincipal, ClaimsPrincipal>();

            services.AddSingleton<IADODataFuntion, ADODataFunction>();

            services.AddScoped<ICityService, CityService>();
            
            services.AddScoped<IClientAccountMappingService, ClientAccountMappingService>();

            services.AddScoped<ICountryService, CountryService>();

            services.AddScoped<IStateService, StateService>();

            services.AddScoped<IDepositoryService, DepositoryService>();

            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IHolidayService, HolidayService>();

            services.AddScoped<IClientGroupService, ClientGroupService>();

            services.AddScoped<IClientFamilyService, ClientFamilyService>();

            services.AddScoped<IRTAService, RTAService>();

            services.AddScoped<IAMCService, AMCService>();

            services.AddScoped<IBankService, BankService>();

            services.AddScoped<IBankBranchService, BankBranchService>();

            services.AddScoped<IAssetClassService, AssetClassService>();

            services.AddScoped<ISBUService, SBUService>();

            services.AddScoped<IEquityBrokerService, EquityBrokerService>();

            services.AddScoped<IInvestorCategoryService, InvestorCategoryService>();

            services.AddScoped<IProductTypeService, ProductTypeService>();

         //   services.AddScoped<IDipositoryService, DipositoryService>();

            services.AddScoped<IHolidayService, HolidayService>();

            services.AddScoped<IRolePermissionService, RolePermissionService>();

            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddScoped<IServiceFactory, ServiceFactory>();

            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddScoped<IBulkUploadService, BulkUploadService>();

            services.AddScoped<IEntityService, EntityService>();
            services.AddScoped<IBankAccountTypeService, BankAccountTypeService>(); 
            services.AddScoped<ISchemeTypeService, SchemeTypeService>();
            services.AddScoped<ISchemePlanService, SchemePlanService>();
            services.AddScoped<IEnumMasterService, EnumMasterService>();
            services.AddScoped<IPincodeService, PincodeService>();
            services.AddScoped<ISchemeNAVService, SchemeNAVService>();
           
            services.AddScoped<IClientFolioService, ClientFolioService>();

            services.AddScoped<ITransactionTypeService, TransactionTypeService>();

            services.AddScoped<IRTATransactionTypeService, RTATransactionTypeService>();

            services.AddScoped<IClientMainService, ClientMainService>();

            services.AddScoped<IOccupationService, OccupationService>();

            services.AddScoped<IHoldingNatureService, HoldingNatureService>();

            services.AddScoped<IRelationshipService, RelationshipService>();

            services.AddScoped<ISchemeService, SchemeService>();

            services.AddScoped<ICompanyService, CompanyService>();

            services.AddTransient<ClaimsPrincipal>(s => s.GetService<IHttpContextAccessor>().HttpContext.User);

            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<DatabaseContext>();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddControllersWithViews();

            services.AddSaml(Configuration.GetSection("SAML"));

            services.AddHttpContextAccessor();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/TEST/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

            // Use SAML middleware.
            //app.UseSaml();

            //// Specify the display name and return URL for logout.
            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path.Value.Equals("/Account/LogOut", StringComparison.OrdinalIgnoreCase) &&
            //    string.IsNullOrEmpty(context.Request.Query["logoutId"]))
            //    {
            //        var identityServerInteractionService =
            //        context.RequestServices.GetRequiredService<IIdentityServerInteractionService>();
            //        var logoutMessageStore =
            //        context.RequestServices.GetRequiredService<IMessageStore<LogoutMessage>>();
            //        var logoutMessage = new Message<LogoutMessage>(new LogoutMessage
            //        {
            //            ClientName = "JADE",
            //            PostLogoutRedirectUri = "/Account/LogOut"
            //        },
            //        DateTime.UtcNow);
            //        var logoutId = await logoutMessageStore.WriteAsync(logoutMessage);
            //        context.Request.QueryString = context.Request.QueryString.Add("logoutId", logoutId);
            //    }
            //    await next();
            //});

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                pattern: "{controller=ClientFolio}/{action=Index}/{id?}");



                //endpoints.MapControllerRoute(
                //    name: "default",
                // pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }

        private void ConfigureAspnetRunServices(IServiceCollection services)
        {
            // Add Core Layer            
            services.Configure<AspnetRunSettings>(Configuration);

            // Add Infrastructure Layer
            ConfigureDatabases(services);

            // Add Miscellaneous
            services.AddHttpContextAccessor();
            services.AddSession();
            services.AddMvc();
        }

        public void ConfigureDatabases(IServiceCollection services)
        {
            // use real database
            services.AddDbContext<DatabaseContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection")));
        }
    }
}
