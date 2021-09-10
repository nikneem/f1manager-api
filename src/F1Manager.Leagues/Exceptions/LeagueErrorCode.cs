using F1Manager.Shared.Base;

namespace F1Manager.Leagues.Exceptions
{
    public abstract class LeagueErrorCode: ErrorCode
    {
        public static readonly LeagueErrorCode NameNullOrEmpty;
        public static readonly LeagueErrorCode NameInvalid;
        public static readonly LeagueErrorCode NameAlreadyTaken;
        public static readonly LeagueErrorCode AlreadyMemberOfLeague;
        public static readonly LeagueErrorCode InvitationAlreadyPending;
        public static readonly LeagueErrorCode RequestAlreadyPending;

        static LeagueErrorCode()
        {
            NameNullOrEmpty = new NameNullOrEmpty();
            NameInvalid = new NameInvalid();
            NameAlreadyTaken = new NameAlreadyTaken();
            AlreadyMemberOfLeague = new AlreadyMemberOfLeague();
            InvitationAlreadyPending = new InvitationAlreadyPending();
            RequestAlreadyPending = new RequestAlreadyPending();
        }
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
}
