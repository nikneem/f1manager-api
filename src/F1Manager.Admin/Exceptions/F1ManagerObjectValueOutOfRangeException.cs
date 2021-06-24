using System;

namespace F1Manager.Admin.Exceptions
{
    public class F1ManagerObjectValueOutOfRangeException : F1ManagerMaintenanceException
    {
        public string ObjectType { get; }
        public decimal MinValue { get; }
        public decimal MaxValue { get; }
        public decimal Value { get; }

        public F1ManagerObjectValueOutOfRangeException(string objectType, decimal minValue, decimal maxValue, decimal value, Exception ex = null) :
            base(MaintenanceErrorCode.ValueOutOfRange, 
                $"Value of bject {objectType} must be between {minValue} and {maxValue}, current value is {value}"
                , ex)
        {
            ObjectType = objectType;
            MinValue = minValue;
            MaxValue = maxValue;
            Value = value;
        }
    }
}