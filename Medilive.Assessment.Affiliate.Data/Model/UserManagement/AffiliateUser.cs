using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Medilive.Assessment.Affiliate.Data.Model.UserManagement
{
    public class AffiliateUser : BaseModel
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public Gender.Values Gender { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public UserType.Values UserType { get; set; }
        internal static void FluentInitAndSeed(ModelBuilder modelBuilder, EnumToStringConverter<UserType.Values> memberStatusConverter, EnumToStringConverter<Gender.Values> genderConverter)
        {
            FluentInit<AffiliateUser>(modelBuilder);
            modelBuilder.Entity<AffiliateUser>(entity =>
            {
                entity.ToTable("AffiliateUser", "UserManagement");

                #region Property List
                entity.Property(e => e.Name).IsRequired().HasMaxLength(64);
                entity.Property(e => e.Lastname).IsRequired().HasMaxLength(32);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(16);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(128);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(64);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(16);
                entity.Property(e => e.UserType).HasConversion(memberStatusConverter);
                entity.Property(e => e.Gender).HasConversion(genderConverter);
                #endregion

                #region Relation List
                entity.HasOne<Gender>().WithMany().HasForeignKey(e => e.Gender).OnDelete(DeleteBehavior.Restrict);
                #endregion

                #region Data
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    entity.HasData(new AffiliateUser()
                    {
                        Id = 1,
                        Username = "Administrator",
                        Password = Convert.ToBase64String(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes("123456789Aa!"))), 
                        Name = "System",
                        Lastname = "Administrator",
                        Email = "admin@host.com",
                        Phone = "5321111111",
                        Gender = UserManagement.Gender.Values.NONE,
                        UserType = Model.UserManagement.UserType.Values.ADMINISTRATOR
                    });
                }
                #endregion
            });
        }
    }
}