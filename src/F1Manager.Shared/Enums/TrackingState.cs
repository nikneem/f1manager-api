using F1Manager.Shared.Constants;

namespace F1Manager.Shared.Enums
{
    public abstract class TrackingState
    {

        public static TrackingState Pristine;
        public static TrackingState New;
        public static TrackingState Modified;
        public static TrackingState Deleted;
        public static TrackingState Touched;
        public static TrackingState[] All;


        public abstract string Status { get; }

        public virtual bool CanChangeTo(TrackingState targetState)
        {
            return false;
        }

        static TrackingState()
        {
            All = new[]
            {
                Pristine = new TrackingStatePristine(),
                New = new TrackingStateNew(),
                Modified = new TrackingStateModified(),
                Deleted = new TrackingStateDeleted(),
                Touched = new TrackingStateTouched()
            };
        }

    }

    public class TrackingStatePristine : TrackingState
    {
        public override string Status => TrackingStateName.Pristine;
        public override bool CanChangeTo(TrackingState targetState)
        {
            if (targetState == New)
            {
                return false;
            }
            return true;
        }
    }

    public class TrackingStateNew : TrackingState
    {
        public override string Status => TrackingStateName.New;
        public override bool CanChangeTo(TrackingState targetState)
        {
            return false;
        }
    }
    public class TrackingStateModified : TrackingState
    {
        public override string Status => TrackingStateName.Modified;
        public override bool CanChangeTo(TrackingState targetState)
        {
            if (targetState == Deleted)
            {
                return true;
            }

            return base.CanChangeTo(targetState);
        }
    }

    public class TrackingStateDeleted : TrackingState
    {
        public override string Status => TrackingStateName.Deleted;
    }
    public class TrackingStateTouched : TrackingState
    {
        public override string Status => TrackingStateName.Touched;

        public override bool CanChangeTo(TrackingState targetState)
        {
            if (targetState == New)
            {
                return false;
            }
            return true;
        }
    }
}
