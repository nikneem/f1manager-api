using F1Manager.Shared.Base;

namespace F1Manager.Leagues.Exceptions;

public abstract class LeagueErrorCode : ErrorCode
{
    public static readonly LeagueErrorCode NameNullOrEmpty = new NameNullOrEmpty();
    public static readonly LeagueErrorCode NameInvalid = new NameInvalid();
    public static readonly LeagueErrorCode NameAlreadyTaken = new NameAlreadyTaken();
    public static readonly LeagueErrorCode AlreadyMemberOfLeague = new AlreadyMemberOfLeague();
    public static readonly LeagueErrorCode InvitationAlreadyPending = new InvitationAlreadyPending();
    public static readonly LeagueErrorCode RequestAlreadyPending = new RequestAlreadyPending();
    public static readonly LeagueErrorCode InvitationAlreadyAccepted = new InvitationAlreadyAccepted();
    public static readonly LeagueErrorCode InvitationAlreadyDeclined = new InvitationAlreadyDeclined();
    public static readonly LeagueErrorCode InvitationExpired = new InvitationExpired();
    public static readonly LeagueErrorCode RequestAlreadyAccepted = new RequestAlreadyAccepted();
    public static readonly LeagueErrorCode RequestAlreadyDeclined = new RequestAlreadyDeclined();
    public static readonly LeagueErrorCode RequestExpired = new RequestExpired();
}

public  class NameNullOrEmpty : LeagueErrorCode
{
    public override string Code => "Leagues.Errors.NameNullOrEmpty";
    public override string TranslationKey => Code;
}
public class NameInvalid : LeagueErrorCode
{
    public override string Code => "Leagues.Errors.NameInvalid";
    public override string TranslationKey => Code;
}
public class NameAlreadyTaken : LeagueErrorCode
{
    public override string Code => "Leagues.Errors.NameAlreadyTaken";
    public override string TranslationKey => Code;
}

public class AlreadyMemberOfLeague : LeagueErrorCode
{
    public override string Code => "Leagues.Errors.AlreadyMemberOfLeague";
    public override string TranslationKey => Code;
}
public class InvitationAlreadyPending : LeagueErrorCode
{
    public override string Code => "Leagues.Errors.InvitationAlreadyPending";
    public override string TranslationKey => Code;
}
public class RequestAlreadyPending : LeagueErrorCode
{
    public override string Code => "Leagues.Errors.RequestAlreadyPending";
    public override string TranslationKey => Code;
}
public class InvitationAlreadyAccepted : LeagueErrorCode
{
    public override string Code => "Leagues.Errors.InvitationAlreadyAccepted";
    public override string TranslationKey => Code;
}
public class InvitationAlreadyDeclined : LeagueErrorCode
{
    public override string Code => "Leagues.Errors.InvitationAlreadyDeclined";
    public override string TranslationKey => Code;
}
public class InvitationExpired : LeagueErrorCode
{
    public override string Code => "Leagues.Errors.InvitationExpired";
    public override string TranslationKey => Code;
}
public class RequestAlreadyAccepted : LeagueErrorCode
{
    public override string Code => "Leagues.Errors.RequestAlreadyAccepted";
    public override string TranslationKey => Code;
}
public class RequestAlreadyDeclined : LeagueErrorCode
{
    public override string Code => "Leagues.Errors.RequestAlreadyDeclined";
    public override string TranslationKey => Code;
}
public class RequestExpired : LeagueErrorCode
{
    public override string Code => "Leagues.Errors.RequestExpired";
    public override string TranslationKey => Code;
}