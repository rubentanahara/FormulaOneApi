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
