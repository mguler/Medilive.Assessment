using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Medilive.Assessment.Affiliate.Data.Model.AuthenticationManagement
{
    public class RouteAccess
    {
        public int Id { get; set; }
        public Values V { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum Values
        {
            NONE = 0,
            AUTHENTICATED_USER = 1,
            UNAUTHENTICATED_USER = 2
        }

        internal static EnumToStringConverter<Values> FluentInitAndSeed(ModelBuilder modelBuilder)
        {
            var converter = new EnumToStringConverter<Values>();

            modelBuilder.Entity<RouteAccess>(entity =>
            {
                entity.ToTable("RouteAccess", "AuthenticationManagement");

                entity.HasKey(e => e.V);
                entity.Property(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(32);
                entity.Property(e => e.V).HasColumnName("Value").HasMaxLength(32).HasConversion(converter);
            });

            var values = Enum.GetValues(typeof(Values)).Cast<Values>();

            modelBuilder.Entity<RouteAccess>(entity =>
            {
                entity.HasData(new RouteAccess()
                {
                    Id = (int)Values.NONE,
                    V = Values.NONE,
                    Name = "Undefined"
                });
                entity.HasData(new RouteAccess()
                {
                    Id = (int)Values.AUTHENTICATED_USER,
                    V = Values.AUTHENTICATED_USER,
                    Name = "Authenticated"
                });
                entity.HasData(new RouteAccess()
                {
                    Id = (int)Values.UNAUTHENTICATED_USER,
                    V = Values.UNAUTHENTICATED_USER,
                    Name = "Unauthenticated"
                });
            });
            return converter;
        }
    }
}
