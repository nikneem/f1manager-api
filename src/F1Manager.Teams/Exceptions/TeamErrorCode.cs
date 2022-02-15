using F1Manager.Shared.Base;

namespace F1Manager.Teams.Exceptions
{
    public abstract class TeamErrorCode : ErrorCode
    {
        public static readonly TeamErrorCode NameNullOrEmpty = new NameNullOrEmpty();
        public static readonly TeamErrorCode NameAlreadyTaken = new NameAlreadyTaken();
        public static readonly TeamErrorCode InvalidName = new InvalidName();

        public static readonly TeamErrorCode NotYourTeam = new NotYourTeam();
        public static readonly TeamErrorCode UserAlreadyHasTeam = new UserAlreadyHasTeam();

        public static readonly TeamErrorCode NotEnoughMoney = new NotEnoughMoney();
        public static readonly TeamErrorCode InvalidTransfer = new InvalidTransfer();
        public static readonly TeamErrorCode DriverAlreadyInTeam = new DriverAlreadyInTeam();
        public static readonly TeamErrorCode ComponentNotFound = new ComponentNotFound();
        public static readonly TeamErrorCode ComponentAlreadyFilled = new ComponentAlreadyFilled();
        public static readonly TeamErrorCode ComponentNotInPossession = new ComponentNotInPosession();

        public static readonly TeamErrorCode PersistenceFailed = new PersistenceFailed();

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
        public override string Code => "Teams.Errors.ComponentNotInPossession";
        public override string TranslationKey => "Teams.Errors.ComponentNotInPossession";
    }
    
    public class PersistenceFailed : TeamErrorCode
    {
        public override string Code => "Teams.Errors.PersistenceFailed";
        public override string TranslationKey => "Teams.Errors.PersistenceFailed";
    }

    
}
