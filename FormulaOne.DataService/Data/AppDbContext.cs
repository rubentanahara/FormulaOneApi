using Microsoft.EntityFrameworkCore;
using FormulaOne.Entities.DbSet;

namespace FormulaOne.Entities.Data;

/// <summary>
/// Represents the database context for the Formula One application.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the AppDbContext.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the Drivers DbSet.
    /// </summary>
    public virtual DbSet<Driver> Drivers => Set<Driver>();

    /// <summary>
    /// Gets or sets the Achievements DbSet.
    /// </summary>
    public virtual DbSet<Achievement> Achievements => Set<Achievement>();

    /// <summary>
    /// Configures the model that was discovered by convention from the entity types.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
            throw new ArgumentNullException(nameof(modelBuilder));

        base.OnModelCreating(modelBuilder);

        ConfigureAchievement(modelBuilder);
        ConfigureDriver(modelBuilder);
    }

    private static void ConfigureAchievement(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.ToTable("Achievements");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.RaceWins)
                  .IsRequired()
                  .HasColumnName("RaceWins");

            entity.Property(e => e.PolePositions)
                  .IsRequired()
                  .HasColumnName("PolePositions");

            entity.Property(e => e.FastetLap)
                  .IsRequired()
                  .HasColumnName("FastestLap");

            entity.Property(e => e.WorldChampionships)
                  .IsRequired()
                  .HasColumnName("WorldChampionships");

            entity.Property(e => e.DriverId)
                  .IsRequired()
                  .HasColumnName("DriverId");

            entity.HasOne(e => e.Driver)
                  .WithMany(e => e.Achievements)
                  .HasForeignKey(e => e.DriverId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_Achievement_Driver");
        });
    }

    private static void ConfigureDriver(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Driver>(entity =>
        {
            entity.ToTable("Drivers");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName)
                  .IsRequired()
                  .HasMaxLength(50)
                  .HasColumnName("FirstName");

            entity.Property(e => e.LastName)
                  .IsRequired()
                  .HasMaxLength(50)
                  .HasColumnName("LastName");

            entity.Property(e => e.DriverNumber)
                  .IsRequired()
                  .HasColumnName("DriverNumber");

            entity.Property(e => e.DateOfBirth)
                  .IsRequired()
                  .HasColumnType("date")
                  .HasColumnName("DateOfBirth");

            // Add indexes for frequently queried columns
            entity.HasIndex(e => e.DriverNumber)
                  .HasDatabaseName("IX_Driver_DriverNumber")
                  .IsUnique();
        });
    }
}
