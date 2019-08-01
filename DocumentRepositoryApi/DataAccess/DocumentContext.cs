using DocumentRepositoryApi.DataAccess.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.DataAccess
{
    public class DocumentContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }

        public DocumentContext(DbContextOptions<DocumentContext> options)
       : base(options)
        {
        }
    
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.DetectChanges();
            var changedEntries = ChangeTracker.Entries();

            var addedEntities = changedEntries.Where(x => x.State == EntityState.Added);
            foreach (var entity in addedEntities)
            {
                if (entity.Entity is BaseEntity baseEntity)
                {
                    baseEntity.Id = Guid.NewGuid();
                    baseEntity.CreatedDate = DateTime.Now;
                }
            }

            var updatedEntities = changedEntries.Where(x => x.State == EntityState.Modified);
            foreach (var entity in updatedEntities)
            {
                if (entity.Entity is BaseEntity baseEntity)
                {
                    baseEntity.UpdatedDate = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
