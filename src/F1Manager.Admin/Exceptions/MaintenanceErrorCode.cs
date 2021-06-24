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
        

        static MaintenanceErrorCode()
        {
            UserIsNotAnAdmin = new UserIsNotAnAdmin();
            ObjectIsDeleted = new ObjectIsDeleted();
            NameIsNullOrEmpty = new NameIsNullOrEmpty();
            CountryIsNullOrEmpty = new CountryIsNullOrEmpty();
            ValueOutOfRange = new ValueOutOfRange();
            ObjectActiveRangeInvalid = new ObjectActiveRangeInvalid();
            PictureUrlInvalid = new PictureUrlInvalid();
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
}
