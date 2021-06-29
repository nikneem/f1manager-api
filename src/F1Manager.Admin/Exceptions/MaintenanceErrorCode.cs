using F1Manager.Shared.Base;

namespace F1Manager.Admin.Exceptions
{
    public abstract class MaintenanceErrorCode : ErrorCode
    {
        public static MaintenanceErrorCode UserIsNotAnAdmin;
        public static MaintenanceErrorCode ObjectIsDeleted;
        public static MaintenanceErrorCode NameIsNullOrEmpty;
        public static MaintenanceErrorCode CountryIsNullOrEmpty;
        public static MaintenanceErrorCode ValueOutOfRange;
        public static MaintenanceErrorCode ObjectActiveRangeInvalid;
        public static MaintenanceErrorCode PictureUrlInvalid;
        public static MaintenanceErrorCode ManufacturerNullOrEmpty;
        public static MaintenanceErrorCode ModelNullOrEmpty;
        public static MaintenanceErrorCode WeeklyWearOffInvalid;
        public static MaintenanceErrorCode MaxWearOffInvalid;


        static MaintenanceErrorCode()
        {
            UserIsNotAnAdmin = new UserIsNotAnAdmin();
            ObjectIsDeleted = new ObjectIsDeleted();
            NameIsNullOrEmpty = new NameIsNullOrEmpty();
            CountryIsNullOrEmpty = new CountryIsNullOrEmpty();
            ValueOutOfRange = new ValueOutOfRange();
            ObjectActiveRangeInvalid = new ObjectActiveRangeInvalid();
            PictureUrlInvalid = new PictureUrlInvalid();
            ManufacturerNullOrEmpty = new ManufacturerNullOrEmpty();
            ModelNullOrEmpty = new ModelNullOrEmpty();
            WeeklyWearOffInvalid  =new WeeklyWearOffInvalid();
            MaxWearOffInvalid = new MaxWearOffInvalid();
        }
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


