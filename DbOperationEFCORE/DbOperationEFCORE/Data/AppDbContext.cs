using Microsoft.EntityFrameworkCore;

namespace DbOperationEFCORE.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
