using System.Text.RegularExpressions;

namespace LondonStock.Resources
{
    public static class Helpers
    {
        public static bool IsGuid(Guid guid)
        {

            var regex = @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$";

            if (guid != Guid.Empty && Regex.IsMatch(guid.ToString(), regex))
            {
                return true;
            }

            return false;
        }

        public static List<string> GetFilterValues(string filter)
        {
            var query = Regex.Split(filter, "in", RegexOptions.IgnoreCase);
            StringSplitOptions splitOptions = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
            return query[1].Replace("(","").Replace(")","").Replace("\"","").Split(',', splitOptions).ToList();
        }
    }
}
