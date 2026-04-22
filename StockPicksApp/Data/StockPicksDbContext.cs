using Microsoft.EntityFrameworkCore;
public class StockPicksDbContext : DbContext
{
    public StockPicksDbContext(DbContextOptions<StockPicksDbContext> o) 
        : base(o) { }

    public DbSet<StockPickEntity> StockPicks => Set<StockPickEntity>();
    public DbSet<PickReasonEntity> PickReasons => Set<PickReasonEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        SeedData.Seed(builder);
    }
}