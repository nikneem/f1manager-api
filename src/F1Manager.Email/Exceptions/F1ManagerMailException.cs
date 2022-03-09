using F1Manager.Shared.Base;

namespace F1Manager.Email.Exceptions;

public class F1ManagerMailException : F1ManagerException
{
    internal F1ManagerMailException(F1ManagerErrorCode code, string message, Exception ex = null) : base(code, message, ex)
    {
    }
}