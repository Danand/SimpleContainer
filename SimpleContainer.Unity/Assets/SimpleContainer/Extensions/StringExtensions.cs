namespace SimpleContainer.Extensions
{
    internal static class StringExtensions
    {
        public static string Repeat(this string source, int amount)
        {
            var result = string.Empty;

            for (int i = 0; i < amount; i++)
            {
                result += source;
            }

            return result;
        }
    }
}