using Bankrupt.Enums;

namespace Bankrupt.Models
{
    public class GameResult
    {
        public PlayerStrategy WinnerStrategy { get; set; }
        public int TurnCount { get; set; }
    }
}