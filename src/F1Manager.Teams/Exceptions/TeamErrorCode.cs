using F1Manager.Shared.Base;

namespace F1Manager.Teams.Exceptions
{
    public abstract class TeamErrorCode : ErrorCode
    {
        public static TeamErrorCode NameNullOrEmpty;
        public static TeamErrorCode InvalidName;
        public static TeamErrorCode NameAlreadyTaken;

        public static TeamErrorCode NotEnoughMoney;
        public static TeamErrorCode InvalidTransfer;
        public static TeamErrorCode DriverAlreadyInTeam;
        public static TeamErrorCode ComponentNotFound;
        public static TeamErrorCode ComponentAlreadyFilled;

        static TeamErrorCode()
        {
            NameNullOrEmpty = new NameNullOrEmpty();
            NameAlreadyTaken = new NameAlreadyTaken();
            InvalidName = new InvalidName();

            NotEnoughMoney = new NotEnoughMoney();
            InvalidTransfer = new InvalidTransfer();
            DriverAlreadyInTeam = new DriverAlreadyInTeam();
            ComponentNotFound = new ComponentNotFound();
            ComponentAlreadyFilled = new ComponentAlreadyFilled();
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
}
