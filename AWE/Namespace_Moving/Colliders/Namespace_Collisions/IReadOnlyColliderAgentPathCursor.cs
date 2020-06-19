using System.Collections.ObjectModel;

namespace AWE.Moving.Collisions {

    public interface IReadOnlyColliderAgentPathCursor<TTransformState> : IReadOnlyTransformPathCursor<TTransformState> where TTransformState : ITransformState {

        bool isInterpolatingWithAnchors { get; }

        ReadOnlyCollection<WeightedTransformState<TTransformState>> CreateInterpolationCollection ();
        ReadOnlyCollection<WeightedTransformState<TTransformState>> CreateInterpolationCollection (float cursorSpeed);
        WeightedTransformState<TTransformState> CreateInterpolatedState (float interpolation);
        WeightedTransformState<TTransformState> CreateInterpolatedState (float interpolation, float cursorSpeed);

    }
}