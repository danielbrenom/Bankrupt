namespace Bankrupt.Architecture
{
    public class DiceRoller
    {
        private const int DiceFaces = 6;
        public static int Roll()
        {
            return ThreadSafeRandom.ThisThreadsRandom.Next (1, DiceFaces + 1);
        }
    }
}