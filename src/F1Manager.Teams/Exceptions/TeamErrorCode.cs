using F1Manager.Shared.Base;

namespace F1Manager.Teams.Exceptions
{
    public abstract class TeamErrorCode : ErrorCode
    {
        public static readonly TeamErrorCode NameNullOrEmpty;
        public static readonly TeamErrorCode InvalidName;
        public static readonly TeamErrorCode NameAlreadyTaken;

        public static readonly TeamErrorCode NotYourTeam;
        public static readonly TeamErrorCode UserAlreadyHasTeam;

        public static readonly TeamErrorCode NotEnoughMoney;
        public static readonly TeamErrorCode InvalidTransfer;
        public static readonly TeamErrorCode DriverAlreadyInTeam;
        public static readonly TeamErrorCode ComponentNotFound;
        public static readonly TeamErrorCode ComponentAlreadyFilled;
        public static readonly TeamErrorCode ComponentNotInPosession;

        public static readonly TeamErrorCode PersistenceFailed;

        static TeamErrorCode()
        {
            NameNullOrEmpty = new NameNullOrEmpty();
            NameAlreadyTaken = new NameAlreadyTaken();
            InvalidName = new InvalidName();

            NotYourTeam = new NotYourTeam();
            UserAlreadyHasTeam = new UserAlreadyHasTeam();

            NotEnoughMoney = new NotEnoughMoney();
            InvalidTransfer = new InvalidTransfer();
            DriverAlreadyInTeam = new DriverAlreadyInTeam();
            ComponentNotFound = new ComponentNotFound();
            ComponentAlreadyFilled = new ComponentAlreadyFilled();
            ComponentNotInPosession = new ComponentNotInPosession();

            PersistenceFailed = new PersistenceFailed();
        }
    }

    public class NameNullOrEmpty : TeamErrorCode
    {
        public override string Code => "Teams.Errors.NameNullOrEmpty";
        public override string TranslationKey => "Teams.Errors.NameNullOrEmpty";
    }
    public class NameAlreadyTaken : TeamErrorCode
    {
        public override string Code => "Teams.Errors.NameAlreadyTaken";
        public override string TranslationKey => "Teams.Errors.NameAlreadyTaken";
    }
    public class InvalidName : TeamErrorCode
    {
        public override string Code => "Teams.Errors.InvalidName";
        public override string TranslationKey => "Teams.Errors.InvalidName";
    }
    public class UserAlreadyHasTeam : TeamErrorCode
    {
        public override string Code => "Teams.Errors.UserAlreadyHasTeam";
        public override string TranslationKey => "Teams.Errors.UserAlreadyHasTeam";
    }
    public class NotYourTeam : TeamErrorCode
    {
        public override string Code => "Teams.Errors.NotYourTeam";
        public override string TranslationKey => "Teams.Errors.NotYourTeam";
    }
    public class NotEnoughMoney : TeamErrorCode
    {
        public override string Code => "Teams.Errors.NotEnoughMoney";
        public override string TranslationKey => "Teams.Errors.NotEnoughMoney";
    }
    public class InvalidTransfer : TeamErrorCode
    {
        public override string Code => "Teams.Errors.InvalidTransfer";
        public override string TranslationKey => "Teams.Errors.InvalidTransfer";
    }
    public class DriverAlreadyInTeam : TeamErrorCode
    {
        public override string Code => "Teams.Errors.DriverAlreadyInTeam";
        public override string TranslationKey => "Teams.Errors.DriverAlreadyInTeam";
    }
    public class ComponentNotFound : TeamErrorCode
    {
        public override string Code => "Teams.Errors.ComponentNotFound";
        public override string TranslationKey => "Teams.Errors.ComponentNotFound";
    }
    public class ComponentAlreadyFilled : TeamErrorCode
    {
        public override string Code => "Teams.Errors.ComponentAlreadyFilled";
        public override string TranslationKey => "Teams.Errors.ComponentAlreadyFilled";
    }
    public class ComponentNotInPosession : TeamErrorCode
    {
        public override string Code => "Teams.Errors.ComponentNotInPosession";
        public override string TranslationKey => "Teams.Errors.ComponentNotInPosession";
    }
    
    public class PersistenceFailed : TeamErrorCode
    {
        public override string Code => "Teams.Errors.PersistenceFailed";
        public override string TranslationKey => "Teams.Errors.PersistenceFailed";
    }

    
}
