using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Medilive.Assessment.Affiliate.Data.Model.UserManagement
{
    public class UserType
    {
        public int Id { get; set; }
        public Values V { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum Values
        {
            NORMAL_USER =  1,
            ADMINISTRATOR = 2
        }

        internal static EnumToStringConverter<Values> FluentInitAndSeed(ModelBuilder modelBuilder)
        {
            var converter = new EnumToStringConverter<Values>();

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("UserType", "UserManagement");

                entity.HasKey(e => e.V);
                entity.Property(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(32);
                entity.Property(e => e.V).HasColumnName("Value").HasMaxLength(16).HasConversion(converter);
            });

            var values = Enum.GetValues(typeof(Values)).Cast<Values>();

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.HasData(new UserType()
                {
                    Id = (int)Values.ADMINISTRATOR,
                    V = Values.ADMINISTRATOR,
                    Name = "Administrator"
                });
                entity.HasData(new UserType()
                {
                    Id = (int)Values.NORMAL_USER,
                    V = Values.NORMAL_USER,
                    Name = "Normal User"
                });
            });
            return converter;
        }
    }
}
