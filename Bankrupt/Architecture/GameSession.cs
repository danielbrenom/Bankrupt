using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Bankrupt.Enums;
using Bankrupt.Models;

namespace Bankrupt.Architecture
{
    public class GameSession
    {
        public int SessionId { get; set; }
        public int TurnCount { get; set; }
        public List<Player> Players { get; set; }
        public Player Winner { get; set; }
        public List<Property> Properties { get; set; } = new List<Property>();

        public GameSession()
        {
            InitializePlayers();
            InitializeProperties();
        }

        private void InitializePlayers()
        {
            Players = new List<Player>
            {
                new Player {Id = System.Guid.NewGuid().ToString("N"), Coins = 300, Strategy = PlayerStrategy.Impulsive},
                new Player {Id = System.Guid.NewGuid().ToString("N"), Coins = 300, Strategy = PlayerStrategy.Demanding},
                new Player {Id = System.Guid.NewGuid().ToString("N"), Coins = 300, Strategy = PlayerStrategy.Cautious},
                new Player {Id = System.Guid.NewGuid().ToString("N"), Coins = 300, Strategy = PlayerStrategy.Random},
            };
            Players.Shuffle();
        }

        private void InitializeProperties()
        {
            var file = File.ReadAllLines("gameConfig.txt");
            foreach (var property in file)
            {
                var info = Regex.Match(property, @"(\d+)\s+(\d+)");
                if (!info.Success)
                    throw new System.ArgumentNullException(nameof(Property),"A property was not found in the configuration file, simulation can't continue");
                var prop = new Property
                {
                    Order = Properties.Count,
                    BuyValue = int.Parse(info.Groups[1].Value),
                    RentValue = int.Parse(info.Groups[2].Value)
                };
                Properties.Add(prop);
            }
        }
    }
}