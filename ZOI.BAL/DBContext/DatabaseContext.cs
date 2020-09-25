using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.DBContext
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<RTAs> RTA { get; set; }

        public DbSet<AssetClass> AssetClass { get; set; }
        
        public DbSet<ClientAccountsMapping> ClientAccounts { get; set; }
        
        public DbSet<AccountTypes> AccountTypes { get; set; }
        
        public DbSet<BankAccountType> BankAccountType { get; set; }
        
        public DbSet<SchemePlan>  SchemePlans{ get; set; }
        
        public DbSet<SchemeType> SchemeType { get; set; }

        public DbSet<ClientMain> clientMains { get; set; }
        
        public DbSet<RTATransactionTypes> rtaTransactionTypes { get; set; }
        
        public DbSet<ClientDepositoryDetails> clientDepositryDetails { get; set; }
        
        public DbSet<ClientAddresses> clientAddresses { get; set; }
        
        public DbSet<ClientPersonal> ClientPersonalDetails { get; set; }
        
        public DbSet<ClientBankDetails> clientBankDetails { get; set; }
        
        public DbSet<ClientEquityBrokerDetails> clientEquityBrokerDetails { get; set; }
        
        public DbSet<ClientSecondaryContact> clientSecondaryContactDetails { get; set; }
        
        public DbSet<ClientMapping> clientMappingDetails { get; set; }

        public DbSet<ProductTypeMaster> ProductType { get; set; }
        
        public DbSet<SBU> SBU { get; set; }
        
        public DbSet<InvestorCategorys> InvestorCategory { get; set; }

        public DbSet<AMCs> AMC { get; set; }

        public DbSet<Bank> Bank { get; set; }

        public DbSet<City> City { get; set; }

        public DbSet<State> State { get; set; }

        public DbSet<Country> Country { get; set; }

        public DbSet<Dipository> Dipository { get; set; }

        public DbSet<Holidays> Holiday { get; set; }

        public DbSet<MenuPermission> RolePermission { get; set; }

        public DbSet<BankBranch> BankBranch { get; set; }

        public DbSet<EquityBrokers> EquityBrokers { get; set; }

        public DbSet<Menu> Menu { get; set; }
        
        public DbSet<MenuType> MenuType { get; set; }
        
        public DbSet<MenuGroup> MenuGroup { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<ClientGroup> ClientGroup { get; set; }

        public DbSet<ClientFamily> ClientFamily { get; set; }
      
        public DbSet<Employee> Employee { get; set; }

        public DbSet<Department> Department { get; set; }
        
        public DbSet<SubDepartment> SubDepartment { get; set; }
        
        public DbSet<Designation> Designation { get; set; }
        
        public DbSet<Models.Enum> Enum { get; set; }

        public DbSet<PincodeMaster> PincodeMaster { get; set; }

        public DbSet<Models.TimeZone> TimeZone { get; set; }

        public DbSet<FileType> FileType { get; set; }

        public DbSet<BulkUpload> BulkUpload { get; set; }
        
        public DbSet<Entity> Entity { get; set; }
        
        public DbSet<BranchMapping> BranchMapping { get; set; }
        
        public DbSet<SubbrokerMapping> SubbrokerMapping { get; set; }
        
        public DbSet<FranchiseMapping> FranchiseMapping { get; set; }
        
        public DbSet<Schema> SchemeMaster { get; set; }

        public DbSet<Application> Application { get; set; }

        public DbSet<SchemeTransaction> SchemeTransactionDetails { get; set; }
        
        public DbSet<SchemeRegistration> SchemeRegistrationDetails { get; set; }
        
        public DbSet<Frequency> Frequency { get; set; }
        
        public DbSet<TransactionTypes> TransactionTypes { get; set; }

        public DbSet<SchemeRestrictedNationality> SchemeRestrictedNationalities { get; set; }
        
        public DbSet<SchemeRestrictedInvestorType> SchemeRestrictedInvestorTypes { get; set; }
        
        public DbSet<OccupationMaster> OccupationMaster { get; set; }
        
        public DbSet<HoldingNature> HoldingNature { get; set; }
        
        public DbSet<Relationship> Relationship { get; set; }

        public DbSet<CompanyMaster> CompanyMaster { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Ignore<SelectListItem>();
            builder.Entity<ApplicationUser>().ToTable("tbl_Users", "dbo");
            builder.Entity<IdentityRole>().ToTable("tbl_Roles", "dbo");
            builder.Entity<IdentityUserRole<string>>().ToTable("tbl_UserRoles", "dbo");
            builder.Entity<IdentityUserClaim<string>>().ToTable("tbl_UserClaims", "dbo");
            builder.Entity<IdentityUserLogin<string>>().ToTable("tbl_UserLogins", "dbo");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("tbl_RoleClaims", "dbo");
            builder.Entity<IdentityUserToken<string>>().ToTable("tbl_UserTokens", "dbo");
           
         // Please dont add Country here   
            builder.Entity<ClientFamily>()
               .HasNoKey();
            
            builder.Entity<ClientAddresses>()
               .HasNoKey();
        }
    }
}
