

// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Options;

// namespace AccountService.API
// {
//     public class DatabaseContext(DbContextOptions<DatabaseContext> options, IOptions<DbSettings> dbOptions) : DbContext(options)
//     {
//         private readonly DbSettings _dbSettings = dbOptions.Value;
//     }

// protected override void OnModelCreating(ModelBuilder modelBuilder)
// {
//     modelBuilder.HasDefaultSchema("auth");
//     base.OnModelCreating(modelBuilder);
// }
// }