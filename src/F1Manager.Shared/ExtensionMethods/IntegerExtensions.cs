namespace F1Manager.Shared.ExtensionMethods
{
    public static class IntegerExtensions
    {

        public static int CalculatePages(this int pageSize, int totalEntries)
        {
            return (totalEntries + pageSize - 1) / pageSize;
        }

    }
}
