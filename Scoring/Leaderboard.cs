using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace TimeGame.Scoring
{
    /// <summary>
    /// Represents the Leaderboard object for the game
    /// </summary>
    public class Leaderboard
    {
        /// <summary>
        /// List of LeaderboardEntries
        /// </summary>
        public List<LeaderboardEntry> Entries = new List<LeaderboardEntry>();

        public Leaderboard()
        {

        }

        /// <summary>
        /// Adds a new entry to the leaderboard
        /// </summary>
        /// <param name="name">the name of the player playing</param>
        /// <param name="time">the time in seconds that the player lived</param>
        public void AddEntry(string name, int time)
        {
            Entries.Add(new LeaderboardEntry(name, time));
        }

        /// <summary>
        /// Sort the leaderboard entires
        /// </summary>
        /// <returns>A sorted copy of the leaderboard</returns>
        public List<LeaderboardEntry> Sorted()
        {
            var tmp = Entries;
            return tmp.OrderByDescending(entry => entry.TimeLived).ToList();
        }

        public string[] Formatted()
        {
            var tmp = new string[5];

            var sorted = Sorted();

            tmp[0] = sorted[0].Formatted();
            tmp[1] = sorted[1].Formatted();
            tmp[2] = sorted[2].Formatted();
            tmp[3] = sorted[3].Formatted();
            tmp[4] = sorted[4].Formatted();

            return tmp;
        }

        /// <summary>
        /// Saves the leaderboard file
        /// </summary>
        public void Save()
        {
            TextWriter writer = null;
            try
            {
                var data = JsonConvert.SerializeObject(this.Entries);
                writer = new StreamWriter("leaderboard.json", false);
                writer.Write(data);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }


        /// <summary>
        /// Loads the leaderboard from a file
        /// </summary>
        public void Load()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader("leaderboard.json");
                var data = reader.ReadToEnd();
                if (data.Length != 0)
                {
                    this.Entries = JsonConvert.DeserializeObject<List<LeaderboardEntry>>(data);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
