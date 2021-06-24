using System;
using F1Manager.Shared.Base;

namespace F1Manager.Admin.Exceptions
{
    public class F1ManagerMaintenanceException : F1ManagerException
    {
        public F1ManagerMaintenanceException(MaintenanceErrorCode code, string message, Exception ex = null) : base(
            code, message, ex)
        {
        }
    }
}
