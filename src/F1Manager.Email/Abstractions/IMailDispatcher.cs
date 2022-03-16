using F1Manager.Email.Enums;
using HexMaster.Email.DomainModels;

namespace F1Manager.Email.Abstractions;

public interface IMailDispatcher
{
    Task<bool> Dispatch(Subjects subject, string preferredLanguage, Recipient recipient);
//    Task Dispatch(Subjects subject, string preferredLanguage, List<Recipient> recipients);
}