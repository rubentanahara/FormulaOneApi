// CreateDriverAchievementRequest.cs is a class that represents the request to create a driver achievement.

/// <summary>
/// Represents the request to create a driver achievement.
/// </summary>
public class UpdateDriverAchievementRequest
{
    /// <summary>
    /// The driver's ID.
    /// </summary>
    public Guid DriverId { get; set; }
    /// <summary>
    /// The driver's World Championship count.
    /// </summary>
    public int Worldchampionships { get; set; }
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
