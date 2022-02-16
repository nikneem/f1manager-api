using F1Manager.Shared.Enums;

namespace F1Manager.Shared.Base;

public class ValueObject
{
    public TrackingState TrackingState { get; private set; }

    protected void SetState(TrackingState newState)
    {
        if (TrackingState.CanChangeTo(newState))
        {
            TrackingState = newState;
        }
    }

    protected ValueObject(TrackingState initialState = null)
    {
        TrackingState = initialState ?? TrackingState.Pristine;
    }

}