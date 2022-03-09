using System.Diagnostics;
using F1Manager.Shared.Base;

namespace F1Manager.Email.Exceptions;

public abstract class F1ManagerErrorCode: ErrorCode
{
    public static readonly F1ManagerErrorCode TemplateNotFound = new TemplateNotFound();
}

public  class TemplateNotFound : F1ManagerErrorCode
{
    public override string Code => "Email.Errors.TemplateNotFound";
    public override string TranslationKey => Code;
}