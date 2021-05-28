using System;
using System.Collections.Generic;
using F1Manager.Shared.DataTransferObjects;

namespace F1Manager.Shared.Base
{
    public class F1ManagerException: Exception
    {
        public ErrorCode ErrorCode { get; }
        public List<ErrorSubstituteDto> Substitutes { get; }

        protected F1ManagerException(ErrorCode code, string message, Exception ex = null)
            : base(message, ex)
        {
            ErrorCode = code;
            Substitutes = new List<ErrorSubstituteDto>();
        }
    }
}