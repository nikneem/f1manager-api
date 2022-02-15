using System;
using F1Manager.Shared.Base;

namespace F1Manager.Users.Exceptions;

[Serializable]
public class F1ManagerUserException : F1ManagerException
{
    internal F1ManagerUserException(UserErrorCode code, string message, Exception ex = null) : base(code, message, ex)
    {
    }
}