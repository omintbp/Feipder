namespace Feipder.Tools.Extensions
{
    public static class StringAsArrayContains
    {
        public static bool ContainsId(this string str, int? id)
        {
            try
            {
                var ids = str.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                return ids.Contains(id ?? 0);

            }catch(Exception e)
            {
                throw;
            }
        }

        public static IEnumerable<int> ToIntArray(this string str)
        {
            if(str == null)
            {
                return new List<int>();
            }
            var parts = str.Split(',').Select(x => Convert.ToInt32(x));
            return parts;
        }
    }
}
