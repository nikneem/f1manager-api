using F1Manager.Shared.Enums;

namespace F1Manager.Shared.Base
{
    public abstract class DomainModel<T>
    {

        public T Id { get; }
        public TrackingState TrackingState { get; private set; }

        protected void SetState(TrackingState newState)
        {
            if (TrackingState.CanChangeTo(newState))
            {
                TrackingState = newState;
            }
        }

        protected DomainModel(T id, TrackingState initialState = null)
        {
            Id = id;
            TrackingState = initialState?? TrackingState.Pristine;
        }

    }
}
