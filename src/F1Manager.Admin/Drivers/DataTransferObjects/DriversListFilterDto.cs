using F1Manager.Shared.Base;

namespace F1Manager.Admin.Drivers.DataTransferObjects
{
    public class DriversListFilterDto : PageFilterBase
    {
        public string Name { get; set; }
        public bool IncludeDeleted { get; set; }
    }
}