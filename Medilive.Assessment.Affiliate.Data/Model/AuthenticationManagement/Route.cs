using Medilive.Assessment.Affiliate.Data.Model.UserManagement;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;

namespace Medilive.Assessment.Affiliate.Data.Model.AuthenticationManagement
{
    public class Route : BaseModel
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string RouteTemplate { get; set; }

        public RouteAccess.Values Access { get; set; }

        internal static void FluentInitAndSeed(ModelBuilder modelBuilder, EnumToStringConverter<RouteAccess.Values> routeAccessConverter)        {
            FluentInit<Route>(modelBuilder);
            modelBuilder.Entity<Route>(entity =>
            {
                entity.ToTable("Route", "AuthenticationManagement");

                #region Property List
                entity.Property(e => e.Controller).IsRequired().HasMaxLength(64);
                entity.Property(e => e.Action).IsRequired().HasMaxLength(64);
                entity.Property(e => e.RouteTemplate).IsRequired().HasMaxLength(64);
                entity.Property(e => e.Access).HasConversion(routeAccessConverter);
                #endregion

                #region Relation List
                entity.HasOne<RouteAccess>().WithMany().HasForeignKey(e => e.Access).OnDelete(DeleteBehavior.Restrict);
                #endregion

                #region Data

                #region Home
                entity.HasData(new Route()
                {
                    Id = 1,
                    Controller = "Home",
                    Action = "Index",
                    RouteTemplate = "/",
                    Access = RouteAccess.Values.UNAUTHENTICATED_USER
                });

                entity.HasData(new Route()
                {
                    Id = 2,
                    Controller = "Home",
                    Action = "Privacy",
                    RouteTemplate = "/privacy",
                    Access = RouteAccess.Values.NONE
                });
                #endregion End Of Home

                #region Reference Data
                entity.HasData(new Route()
                {
                    Id = 3,
                    Controller = "ReferenceData",
                    Action = "GetGenderList",
                    RouteTemplate = "/gender-list",
                    Access = RouteAccess.Values.NONE
                });
                #endregion End Of Reference Data

                #region User Management

                entity.HasData(new Route()
                {
                    Id = 4,
                    Controller = "User",
                    Action = "Register",
                    RouteTemplate = "/new-user-registration",
                    Access = RouteAccess.Values.UNAUTHENTICATED_USER
                });

                entity.HasData(new Route()
                {
                    Id = 5,
                    Controller = "User",
                    Action = "UserHome",
                    RouteTemplate = "/user-home",
                    Access = RouteAccess.Values.AUTHENTICATED_USER
                });

                entity.HasData(new Route()
                {
                    Id = 6,
                    Controller = "User",
                    Action = "GetUserinfo",
                    RouteTemplate = "/get-user-info",
                    Access = RouteAccess.Values.AUTHENTICATED_USER
                });

                entity.HasData(new Route()
                {
                    Id = 7,
                    Controller = "User",
                    Action = "GetUserSummary",
                    RouteTemplate = "/get-user-summary",
                    Access = RouteAccess.Values.AUTHENTICATED_USER
                });
                #endregion End Of User Management

                #region Authentication Management
                entity.HasData(new Route()
                {
                    Id = 8,
                    Controller = "Authentication",
                    Action = "Login",
                    RouteTemplate = "/user-login",
                    Access = RouteAccess.Values.UNAUTHENTICATED_USER
                });

                entity.HasData(new Route()
                {
                    Id = 9,
                    Controller = "Authentication",
                    Action = "Logout",
                    RouteTemplate = "/user-logout",
                    Access = RouteAccess.Values.AUTHENTICATED_USER
                });
                #endregion End Of Authentication Management

                #endregion Data

            });
        }
    }
}
