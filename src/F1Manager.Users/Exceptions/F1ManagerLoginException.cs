using System;
using F1Manager.Shared.Base;

namespace F1Manager.Users.Exceptions
{
    public class F1ManagerLoginException : F1ManagerException
    {
        internal F1ManagerLoginException(LoginErrorCode code, string message, Exception ex = null) : base(code, message, ex)
        {
        }
    }
}
