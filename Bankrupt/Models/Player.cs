using Bankrupt.Enums;

namespace Bankrupt.Models
{
    public class Player
    {
        public string Id { get; set; }
        public int Coins { get; set; }
        public PlayerStrategy Strategy { get; set; }
        public int PlayerPosition { get; set; }
        public int TimesAroundBoard { get; set; }
    }

    
}