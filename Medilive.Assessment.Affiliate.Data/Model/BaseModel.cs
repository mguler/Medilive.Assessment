using Microsoft.EntityFrameworkCore;

namespace Medilive.Assessment.Affiliate.Data.Model
{
    public class BaseModel
    {
        public long Id { get; set; }

        public bool IsDeleted { get; set; }

        public static void FluentInit<T>(ModelBuilder modelBuilder) where T : BaseModel
        {
            modelBuilder.Entity<T>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id);
                entity.Property(e => e.IsDeleted).IsRequired();
            });
        }

    }
}
