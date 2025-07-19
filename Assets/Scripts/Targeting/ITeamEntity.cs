namespace Targeting
{
    /// <summary>
    /// Identifies the team to which an entity belongs in a match or battle.
    /// Used to distinguish allies from enemies on the battlefield.
    /// </summary>
    public interface ITeamEntity
    {
        // Unique team identifier, e.g. "Team1", "Blue", or a GUID.
        Team Team { get; }
    }
}