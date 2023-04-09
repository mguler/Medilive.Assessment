using Microsoft.EntityFrameworkCore;

namespace Medilive.Assessment.Affiliate.Data.Model.UserManagement
{
    public class ClientBlock : BaseModel
    {
        public long IpNumber { get; set; }
        public string IdentificationCookie { get; set; }
        public long BlockedUntil { get; set; }
        internal static void FluentInitAndSeed(ModelBuilder modelBuilder)
        {
            FluentInit<ClientBlock>(modelBuilder);
            modelBuilder.Entity<ClientBlock>(entity =>
            {
                entity.ToTable("ClientBlock", "UserManagement");

                #region Property List
                entity.Property(e => e.IdentificationCookie).IsRequired().HasMaxLength(64);
                entity.Property(e => e.IpNumber).IsRequired();
                entity.Property(e => e.BlockedUntil).IsRequired();
                #endregion

            });
        }
    }
}
