using System;
using System.Text.RegularExpressions;
using F1Manager.Shared.Base;
using F1Manager.Shared.Constants;
using F1Manager.Shared.Enums;
using F1Manager.Shared.Helpers;

namespace F1Manager.Shared.ValueObjects;

public class EmailAddress : ValueObject
{
    public string Value { get; private set; }
    public string VerificationCode { get; private set; }
    public DateTimeOffset VerificationDueOn { get; private set; }
    public DateTimeOffset? VerificationOn { get; private set; }

    public void SetValue(string value)
    {
        if (!Equals(Value, value))
        {
            Value= value;
            SetState(TrackingState.Modified);
        }
    }

    public EmailAddress(string value, string verification, DateTimeOffset? verifiedOn, DateTimeOffset verificationDue) :
        base(TrackingState.New)
    {
        Value = value;
        VerificationCode = verification;
        VerificationOn = verifiedOn;
        VerificationDueOn = verificationDue;
    }

    private EmailAddress() : base(TrackingState.New)
    {
        VerificationCode = Randomize.GenerateEmailVerificationCode();
        VerificationDueOn = DateTimeOffset.UtcNow.AddDays(Defaults.EmailVerificationPeriodInDays);
    }

    public static EmailAddress Create(string value)
    {
        var emailAddress = new EmailAddress();
        emailAddress.SetValue(value);
        return emailAddress;
    }
}