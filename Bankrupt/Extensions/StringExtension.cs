namespace Bankrupt.Extensions
{
    public static class StringExtension
    {
        public static bool IsEmptyNullWhitespace(this string s)
        {
            return string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s);
        }
    }
}