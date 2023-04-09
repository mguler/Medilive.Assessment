using Microsoft.EntityFrameworkCore;

namespace Medilive.Assessment.Affiliate.Data.Model.UserManagement
{
    public class RegistrationReferralCodeAudit : BaseModel
    {
        public string ReferralCode { get; set; }
        public string IdentificationCookie { get; set; }
        public long IpNumber { get; set; }
        public long AttemptOn { get; set; }
        public bool IsSuccessfull { get; set; }
        internal static void FluentInitAndSeed(ModelBuilder modelBuilder)
        {
            FluentInit<RegistrationReferralCodeAudit>(modelBuilder);
            modelBuilder.Entity<RegistrationReferralCodeAudit>(entity =>
            {
                entity.ToTable("RegistrationReferralCodeAudit", "UserManagement");

                #region Property List
                entity.Property(e => e.ReferralCode).IsRequired().HasMaxLength(256);
                entity.Property(e => e.IdentificationCookie).IsRequired().HasMaxLength(64);
                entity.Property(e => e.IpNumber).IsRequired();
                entity.Property(e => e.AttemptOn).IsRequired();
                entity.Property(e => e.IsSuccessfull).IsRequired();
                #endregion

            });
        }
    } 
}
