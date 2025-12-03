

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectService.Infrastructure.Config;

namespace ProjectService.Infrastructure
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options, IOptions<DbSettings> dbOptions) : DbContext(options)
    {
        private readonly DbSettings _dbSettings = dbOptions.Value;
    }
}