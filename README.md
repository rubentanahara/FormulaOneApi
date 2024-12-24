<!--toc:start-->

- [Project setup](#project-setup)
- [Entites](#entites)
- [Add packages](#add-packages)
- [Add the DbContext](#add-the-dbcontext)
<!--toc:end-->

**Technologies used:**

- .NET 8.0

### Project setup

1. Create a folder for the project.

```bash
mkdir FormulaOne
cd FormulaOne
git init
dotnet new gitignore
dotnet new globaljson --sdk-version 8.0.111
```

2. Navigate to the project folder and create a new solution.

```bash
dotnet new sln -n FormulaOne.
```

3. Create a web api project

```bash
dotnet new webapi -n FormulaOne.Api
```

4. Add the project to the solution

```bash
dotnet sln FormulaOne.sln add FormulaOne.Api/FormulaOne.Api.csproj
```

5. Create a class library project

```bash
dotnet new classlib -n FormulaOne.Entites
```

6. Add the project to the solution

```bash
dotnet sln FormulaOne.sln add FormulaOne.Entites/FormulaOne.Entites.csproj
```

7. Create a class library project

```bash
dotnet new classlib -n FormulaOne.DataService
```

8. Add the project to the solution

```bash
dotnet sln FormulaOne.sln add FormulaOne.DataService/FormulaOne.DataService.csproj

```

8.5 Add references

```bash
dotnet add FormulaOne.DataService/FormulaOne.DataService.csproj reference FormulaOne.Entites/FormulaOne.Entites.csproj
dotnet add FormulaOne.Api/FormulaOne.Api.csproj reference FormulaOne.DataService/FormulaOne.DataService.csproj
```

9. Run the project

```bash
dotnet run --project FormulaOne.Api/FormulaOne.Api.csproj
```

10. Open the browser and navigate to the url below
    <localhost:5038>

## Entites

1. Create a folder on the FormulaOne.Entites project called DbSet.
2. Create a class called **BaseEntity.cs**

```cs
namespace FormulaOne.Entities.DbSet;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
```

3. Create a class called **Driver.cs**

```cs
namespace FormulaOne.Entities.DbSet;

public class Driver : BaseEntity
{
    public Driver() => Achievements = new HashSet<Achievement>();

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int DriverNumber { get; set; }
    public DateTime DateOfBirth { get; set; }

    public virtual ICollection<Achievement> Achievements { get; set; }
}
```

4. Create a class called **Achievement.cs**

```cs
namespace FormulaOne.Entities.DbSet;

public class Achievement : BaseEntity
{
    public int RaceWins { get; set; }
    public int PolePositions { get; set; }
    public int FastetLap { get; set; }
    public int WorldChampionships { get; set; }
    public Guid DriverId { get; set; }

    // Foreign Key
    public virtual Driver? Driver { get; set; }
}

```

## Add packages

1. Add the following packages to the FormulaOne.DataService project

```bash
dotnet add FormulaOne.DataService/FormulaOne.DataService.csproj package Microsoft.EntityFrameworkCore
dotnet add FormulaOne.DataService/FormulaOne.DataService.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add FormulaOne.DataService/FormulaOne.DataService.csproj package Microsoft.EntityFrameworkCore.Tools
dotnet add FormulaOne.DataService/FormulaOne.DataService.csproj package Microsoft.EntityFrameworkCore.Sqlite
```

2. Add the following packages to the FormulaOne.Api project

```bash
dotnet add FormulaOne.Api/FormulaOne.Api.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add FormulaOne.Api/FormulaOne.Api.csproj package Microsoft.EntityFrameworkCore.Tools
dotnet add FormulaOne.Api/FormulaOne.Api.csproj package AutoMapper
dotnet add FormulaOne.Api/FormulaOne.Api.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection
```

Note: For AutoMapper the version should be the same as the AutoMapper.Extensions.Microsoft.DependencyInjection

````bash


3. Install dotnet ef tools

```bash
dotnet tool install --global dotnet-ef --version 8.0.0
````

4. Add it to the PATH

```bash
export PATH="$PATH:$HOME/.dotnet/tools"
source ~/.zshrc
dotnet ef --version
ls -la $HOME/.dotnet/tools
```

## Add the DbContext

1. Create a folder called **DbContext** in the FormulaOne.DataService project.
2. Create a class called **AppDbContext.cs**

```cs
using Microsoft.EntityFrameworkCore;
using FormulaOne.Entities.DbSet;
using System.Reflection;

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

        // Apply all configurations from current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

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
```

2. Update Program.cs in the FormulaOne.Api project

```cs
using FormulaOne.DataService.Repositories;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Register the DbContext with the connection string in the DI container
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
```

3. Update appsettings.json in the FormulaOne.Api project

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "DataSource=FormulaOne.db"
  }
}
```

4. Run Migrations

```bash
dotnet ef migrations add InitialCreate --project FormulaOne.DataService/FormulaOne.DataService.csproj --startup-project FormulaOne.Api/FormulaOne.Api.csproj
dotnet ef database update --project FormulaOne.DataService/FormulaOne.DataService.csproj --startup-project FormulaOne.Api/FormulaOne.Api.csproj
```

## Repository Pattern

1. Create a folder called **Repositories** in the FormulaOne.DataService project.
2. Create a folder called **Interfaces** in the FormulaOne.DataService project.
3. Create a class called **IGenericRepository.cs**

```cs
namespace FormulaOne.DataService.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(Guid id);
}
```

4. Create a class called **GenericRepository.cs**

```cs
namespace FormulaOne.DataService.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    public readonly ILogger _logger;
    protected AppDbContext _context;
    internal DbSet<T> _dbSet;

    public GenericRepository(
        AppDbContext context,
        ILogger logger
        )
    {
        _context = context;
        _logger = logger;

        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> All()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<bool> Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> Update(T entity)
    {
        await Task.CompletedTask;
        return false;
    }

    public virtual async Task<bool> Delete(Guid id)
    {
        await Task.CompletedTask;
        return false;
    }
}
```

5. Create a class called **IDriverRepository.cs**
6. Create a class called **DriverRepository.cs**
7. Create a class called **IAchievementRepository.cs**

```cs
public interface IAchievementsRepository : IGenericRepository<Achievement>
{
    Task<Achievement?> GetDriverAchievementAsync(Guid driverId);
}

```

8. Create a class called **AchievementsRepository.cs**

```cs

```
