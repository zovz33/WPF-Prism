using Microsoft.EntityFrameworkCore;
using WPFPrism.Infrastructure.Models;

namespace WPFPrism.Infrastructure.Database
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Service> Services { get; set; } 
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
