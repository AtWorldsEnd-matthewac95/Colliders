using System.Collections.ObjectModel;

namespace AWE.Moving.Collisions {

    public class ReadOnlyColliderAgentPathCursor<TTransformState> : ReadOnlyTransformPathCursor<TTransformState>, IReadOnlyColliderAgentPathCursor<TTransformState> where TTransformState : ITransformState {

        private readonly ColliderAgentPathCursor<TTransformState> cursor;

        public bool isInterpolatingWithAnchors => this.cursor.isInterpolatingWithAnchors;

        public ReadOnlyColliderAgentPathCursor (ColliderAgentPathCursor<TTransformState> cursor) : base (cursor) => this.cursor = cursor;

        public ReadOnlyCollection<WeightedTransformState<TTransformState>> CreateInterpolationCollection () => this.cursor.CreateInterpolationCollection ();
        public ReadOnlyCollection<WeightedTransformState<TTransformState>> CreateInterpolationCollection (float cursorSpeed) => this.cursor.CreateInterpolationCollection (cursorSpeed);
        public WeightedTransformState<TTransformState> CreateInterpolatedState (float interpolation) => this.cursor.CreateInterpolatedState (interpolation);
        public WeightedTransformState<TTransformState> CreateInterpolatedState (float interpolation, float cursorSpeed) => this.cursor.CreateInterpolatedState (interpolation, cursorSpeed);

        public override bool Equals (object other) {

            bool isEqual;

            if (other is ReadOnlyColliderAgentPathCursor<TTransformState> otherReadOnly) {

                isEqual = this.Equals (otherReadOnly);

            } else if (other is ColliderAgentPathCursor<TTransformState> otherCursor) {

                isEqual = this.Equals (otherCursor);

            } else {

                isEqual = base.Equals (other);

            }

            return isEqual;

        }
        public override int GetHashCode () => this.cursor.GetHashCode ();
        public virtual bool Equals (ReadOnlyColliderAgentPathCursor<TTransformState> other) => (this.cursor == other.cursor);
        public virtual bool Equals (ColliderAgentPathCursor<TTransformState> other) => (this.cursor == other);

    }
}
