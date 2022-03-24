using F1Manager.Shared.Base;

namespace F1Manager.Admin.Exceptions
{
    public abstract class MaintenanceErrorCode : ErrorCode
    {
        public static readonly MaintenanceErrorCode UserIsNotAnAdmin = new UserIsNotAnAdmin();
        public static readonly MaintenanceErrorCode ObjectIsDeleted = new ObjectIsDeleted();
        public static readonly MaintenanceErrorCode NameIsNullOrEmpty = new NameIsNullOrEmpty();
        public static readonly MaintenanceErrorCode CountryIsNullOrEmpty = new CountryIsNullOrEmpty();
        public static readonly MaintenanceErrorCode ValueOutOfRange = new ValueOutOfRange();
        public static readonly MaintenanceErrorCode ObjectActiveRangeInvalid = new ObjectActiveRangeInvalid();
        public static readonly MaintenanceErrorCode PictureUrlInvalid = new PictureUrlInvalid();
        public static readonly MaintenanceErrorCode ManufacturerNullOrEmpty = new ManufacturerNullOrEmpty();
        public static readonly MaintenanceErrorCode ModelNullOrEmpty = new ModelNullOrEmpty();
        public static readonly MaintenanceErrorCode WeeklyWearOffInvalid = new WeeklyWearOffInvalid();
        public static readonly MaintenanceErrorCode MaxWearOffInvalid = new MaxWearOffInvalid();

        public static readonly ActualTeamBaseInvalid ActualTeamBaseInvalid = new ActualTeamBaseInvalid();
        public static readonly ActualTeamNameInvalid ActualTeamNameInvalid = new ActualTeamNameInvalid();
        public static readonly ActualTeamPrincipalInvalid ActualTeamPrincipalInvalid = new ActualTeamPrincipalInvalid();
        public static readonly ActualTeamTechChiefInvalid ActualTeamTechChiefInvalid = new ActualTeamTechChiefInvalid();
        public static readonly ActualTeamFirstDriverInvalid ActualTeamFirstDriverInvalid = new ActualTeamFirstDriverInvalid();
        public static readonly ActualTeamSecondDriverInvalid ActualTeamSecondDriverInvalid = new ActualTeamSecondDriverInvalid();
        public static readonly ActualTeamEngineInvalid ActualTeamEngineInvalid = new ActualTeamEngineInvalid();
        public static readonly ActualTeamChassisInvalid ActualTeamChassisInvalid = new ActualTeamChassisInvalid();
    }


    public sealed class UserIsNotAnAdmin : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.UserIsNotAnAdmin";
        public override string TranslationKey => Code;
    }
    public sealed class ObjectIsDeleted : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ObjectIsDeleted";
        public override string TranslationKey => Code;
    }
    public sealed class NameIsNullOrEmpty : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.NameIsNullOrEmpty";
        public override string TranslationKey => Code;
    }
    public sealed class CountryIsNullOrEmpty : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.CountryIsNullOrEmpty";
        public override string TranslationKey => Code;
    }
    public sealed class ValueOutOfRange : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ValueOutOfRange";
        public override string TranslationKey => Code;
    }
    public sealed class ObjectActiveRangeInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ObjectActiveRangeInvalid";
        public override string TranslationKey => Code;
    }
    public sealed class PictureUrlInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.PictureUrlInvalid";
        public override string TranslationKey => Code;
    }


    public sealed class ManufacturerNullOrEmpty : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ManufacturerNullOrEmpty";
        public override string TranslationKey => Code;
    }
    public sealed class ModelNullOrEmpty : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ModelNullOrEmpty";
        public override string TranslationKey => Code;
    }
    public sealed class WeeklyWearOffInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.WeeklyWearOffInvalid";
        public override string TranslationKey => Code;
    }
    public sealed class MaxWearOffInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.MaxWearOffInvalid";
        public override string TranslationKey => Code;
    }

    public sealed class ActualTeamNameInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ActualTeamNameInvalid";
        public override string TranslationKey => Code;
    }
    public sealed class ActualTeamBaseInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ActualTeamBaseInvalid";
        public override string TranslationKey => Code;
    }
    public sealed class ActualTeamPrincipalInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ActualTeamPrincipalInvalid";
        public override string TranslationKey => Code;
    }
    public sealed class ActualTeamTechChiefInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ActualTeamTechChiefInvalid";
        public override string TranslationKey => Code;
    }
    public sealed class ActualTeamFirstDriverInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ActualTeamFirstDriverInvalid";
        public override string TranslationKey => Code;
    }
    public sealed class ActualTeamSecondDriverInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ActualTeamSecondDriverInvalid";
        public override string TranslationKey => Code;
    }
    public sealed class ActualTeamEngineInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ActualTeamEngineInvalid";
        public override string TranslationKey => Code;
    }
    public sealed class ActualTeamChassisInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ActualTeamChassisInvalid";
        public override string TranslationKey => Code;
    }

}


