using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Medilive.Assessment.Affiliate.Data.Model.UserManagement
{
    public class Gender
    {
        public int Id { get; set; }
        public Values V { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum Values
        {
            NONE = 0,
            FEMALE =1,
            MALE=2
        }

        internal static EnumToStringConverter<Values> FluentInitAndSeed(ModelBuilder modelBuilder)
        {
            var converter = new EnumToStringConverter<Values>();

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender", "UserManagement");

                entity.HasKey(e => e.V);
                entity.Property(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(32);
                entity.Property(e => e.V).HasColumnName("Value").HasMaxLength(16).HasConversion(converter);
            });

            var values = Enum.GetValues(typeof(Values)).Cast<Values>();
            
            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasData(new Gender()
                {
                    Id = (int)Values.NONE,
                    V = Values.NONE,
                    Name = "Secilmemis"
                });
                entity.HasData(new Gender()
                {
                    Id= (int)Values.FEMALE,
                    V = Values.FEMALE,
                    Name = "Kadin"
                });
                entity.HasData(new Gender()
                {
                    Id = (int)Values.MALE,
                    V = Values.MALE,
                    Name = "Erkek"
                });
            });
            return converter;
        }
    }
}
