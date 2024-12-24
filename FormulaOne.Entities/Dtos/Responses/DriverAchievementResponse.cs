/// <summary>
/// Represents the response of a driver achievement.
/// </summary>
public class DriverAchievementResponse
{
    /// <summary>
    /// The driver's ID.
    /// </summary>
    public Guid DriverId { get; set; }
    /// <summary>
    /// The driver's World Championship count.
    /// </summary>
    public int Worldchampionship { get; set; }
    /// <summary>
    /// The driver's Fastest Lap count.
    /// </summary>
    public int FastestLap { get; set; }
    /// <summary>
    /// The driver's Pole Position count.
    /// </summary>
    public int PolePosition { get; set; }
    /// <summary>
    /// The driver's Wins count.
    /// </summary>
    public int Wins { get; set; }
}

