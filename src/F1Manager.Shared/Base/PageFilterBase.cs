using F1Manager.Shared.Constants;

namespace F1Manager.Shared.Base
{
    public abstract class PageFilterBase
    {

        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public int Skip => CalculatedPage * CalculatedPageSize;

        public int CalculatedPage => Page.HasValue && Page.Value >= 0 ? Page.Value : 0;

        public int CalculatedPageSize => PageSize.HasValue && PageSize.Value > 1 && PageSize.Value <= 250
            ? PageSize.Value
            : Defaults.PageSize;
    }
}