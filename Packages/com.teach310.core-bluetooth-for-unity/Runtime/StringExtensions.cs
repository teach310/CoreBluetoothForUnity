using System.Text.RegularExpressions;

namespace CoreBluetooth
{
    internal static class StringExtensions
    {
        private static readonly Regex CBUUIDRegex = new Regex(@"^([0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}|[0-9a-f]{8}|[0-9a-f]{4})$", RegexOptions.IgnoreCase);

        public static bool IsCBUUID(this string uuidString)
        {
            return CBUUIDRegex.IsMatch(uuidString);
        }
    }
}
