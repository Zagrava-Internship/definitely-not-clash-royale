using UnityEngine;

namespace Targeting.Extensions
{
    public static class TeamExtensions
    {
        public static bool IsEnemy(this Team team, Team otherTeam)
        {
            return team != otherTeam;
        }
        // The simplest realization possible, just returns a color based on the team.
        public static Color GetColor(this Team team,bool isDarkMode = false)
        {
            return team switch
            {
                Team.Team2 when isDarkMode => new Color(0.50f, 0.03f, 0.0f), // Darker red
                Team.Team1 when isDarkMode => new Color(0.0f, 0.29f, 0.50f), // Darker blue
                Team.Team2 => Color.red,
                Team.Team1 => new Color(0,0.65f,1.0f), // Lighter blue
                _ => Color.white // Default color if no match
            };
        }
    }
}