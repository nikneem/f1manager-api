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
    }


    public sealed class UserIsNotAnAdmin : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.UserIsNotAnAdmin";
        public override string TranslationKey => "Maintenance.Errors.UserIsNotAnAdmin";
    }
    public sealed class ObjectIsDeleted : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ObjectIsDeleted";
        public override string TranslationKey => "Maintenance.Errors.ObjectIsDeleted";
    }
    public sealed class NameIsNullOrEmpty : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.NameIsNullOrEmpty";
        public override string TranslationKey => "Maintenance.Errors.NameIsNullOrEmpty";
    }
    public sealed class CountryIsNullOrEmpty : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.CountryIsNullOrEmpty";
        public override string TranslationKey => "Maintenance.Errors.CountryIsNullOrEmpty";
    }
    public sealed class ValueOutOfRange : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ValueOutOfRange";
        public override string TranslationKey => "Maintenance.Errors.ValueOutOfRange";
    }
    public sealed class ObjectActiveRangeInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ObjectActiveRangeInvalid";
        public override string TranslationKey => "Maintenance.Errors.ObjectActiveRangeInvalid";
    }
    public sealed class PictureUrlInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.PictureUrlInvalid";
        public override string TranslationKey => "Maintenance.Errors.PictureUrlInvalid";
    }


    public sealed class ManufacturerNullOrEmpty : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ManufacturerNullOrEmpty";
        public override string TranslationKey => "Maintenance.Errors.ManufacturerNullOrEmpty";
    }
    public sealed class ModelNullOrEmpty : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.ModelNullOrEmpty";
        public override string TranslationKey => "Maintenance.Errors.ModelNullOrEmpty";
    }
    public sealed class WeeklyWearOffInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.WeeklyWearOffInvalid";
        public override string TranslationKey => "Maintenance.Errors.WeeklyWearOffInvalid";
    }
    public sealed class MaxWearOffInvalid : MaintenanceErrorCode
    {
        public override string Code => "Maintenance.Errors.MaxWearOffInvalid";
        public override string TranslationKey => "Maintenance.Errors.MaxWearOffInvalid";
    }


}


