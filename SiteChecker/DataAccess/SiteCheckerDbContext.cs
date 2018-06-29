namespace SiteChecker
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;

    /// <summary>
    /// Контекст хранилища данных
    /// </summary>
    public sealed class SiteCheckerDbContext : DbContext
    {
        /// <summary>
        /// Задачи проверки доступности URL-адреса по протоколу HTTP
        /// </summary>
        public DbSet<UrlCheckTask> UrlCheckTasks { get; set; }
        public DbSet<UrlCheckTaskResult> UrlCheckTaskResults { get; set; }

        public SiteCheckerDbContext(DbContextOptions<SiteCheckerDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public SiteCheckerDbContext ()
        {
            Database.EnsureCreated();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=UrlCheckTaskDb;Trusted_Connection=True;");
            }

            optionsBuilder.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UrlCheckTask>().HasIndex(x => x.Url).IsUnique();

            builder.Entity<UrlCheckTaskResult>()
                .HasOne(x => x.UrlCheckTask)
                .WithMany()
                .HasForeignKey(x => x.UrlCheckTaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}