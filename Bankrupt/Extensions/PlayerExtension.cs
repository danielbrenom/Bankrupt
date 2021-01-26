using Bankrupt.Models;

namespace Bankrupt.Extensions
{
    public static class PlayerExtension
    {
        public static void SetNewPosition(this Player player, int adder)
        {
            for (var i = 0; i < adder; i++)
            {
                player.PlayerPosition++;
                if (player.PlayerPosition != 20) continue;
                player.PlayerPosition = 0;
                player.Coins += 100;
                player.TimesAroundBoard++;
            }
        }
    }
}