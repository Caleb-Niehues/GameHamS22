using System;
using System.Collections.Generic;
using System.Text;

namespace TimeGame.Scoring
{
    /// <summary>
    /// Represents a LeaderboardEntry for a leaderboard
    /// </summary>
    public class LeaderboardEntry : IComparable<LeaderboardEntry>
    {
        private string initials_;
        private int timeLived_;

        /// <summary>
        /// The initials of the user playing the game
        /// </summary>
        public string Initials { get => initials_; set => initials_ = value; }

        /// <summary>
        /// The time in seconds that the player lived
        /// </summary>
        public int TimeLived { get => timeLived_; set => timeLived_ = value; }

        public LeaderboardEntry(string initials, int timeLived)
        {
            this.Initials = initials;
            this.TimeLived = timeLived;
        }

        /// <summary>
        /// Format the leaderboard output
        /// </summary>
        /// <returns>the formatted string for the leaderboard output</returns>
        public string Formatted()
        {
            return string.Format("{0} - {0}", initials_, SecondsToTime());
        }

        /// <summary>
        /// Converters the seconds lived for the game into a formatted string
        /// </summary>
        /// <returns>Formatted string from the seconds lived</returns>
        public string SecondsToTime()
        {
            var time = TimeSpan.FromSeconds(timeLived_);
            return time.ToString(@":mm\:ss");
        }

        /// <summary>
        /// Compares the current LeaderboardEntry to the other
        /// </summary>
        /// <param name="other">The other LeaderboardEntry</param>
        /// <returns>Comparing results between the two LeaderboardEntries</returns>
        public int CompareTo(LeaderboardEntry other)
        {
            if (other == null) return 1;
            if (this.timeLived_ > other.timeLived_) return 1;
            if (this.timeLived_ < other.timeLived_) return -1;
            return 0;
        }
    }
}
