using Medilive.Assessment.Affiliate.Data.Model.AuthenticationManagement;
using Medilive.Assessment.Affiliate.Data.Model.UserManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medilive.Assessment.Affiliate.Data
{
    public class MediliveAffiliateDatabaseContext : DbContext
    {
        public MediliveAffiliateDatabaseContext(DbContextOptions<MediliveAffiliateDatabaseContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Converters
            var genderConverter = Gender.FluentInitAndSeed(modelBuilder);
            var userTypeConverter = UserType.FluentInitAndSeed(modelBuilder);
            var routeAccessConverter = RouteAccess.FluentInitAndSeed(modelBuilder);
            #endregion End Of Converters

            #region User Management
            AffiliateUser.FluentInitAndSeed(modelBuilder, userTypeConverter, genderConverter);
            RegistrationReferralCodeAudit.FluentInitAndSeed(modelBuilder);
            ClientBlock.FluentInitAndSeed(modelBuilder);
            Route.FluentInitAndSeed(modelBuilder, routeAccessConverter);
            #endregion End Of User Management
        }
    }
}