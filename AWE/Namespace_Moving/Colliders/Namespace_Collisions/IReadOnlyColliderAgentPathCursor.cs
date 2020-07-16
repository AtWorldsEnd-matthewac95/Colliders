using System.Collections.ObjectModel;

namespace AWE.Moving.Collisions {

    public interface IReadOnlyColliderAgentPathCursor<TTransformState> : IReadOnlyTransformPathCursor<TTransformState> where TTransformState : ITransformState {

        bool isInterpolatingWithAnchors { get; }

        ReadOnlyCollection<WeightedTransformState<TTransformState>> CreateInterpolationCollection (bool includeEndpoints = true);
        ReadOnlyCollection<WeightedTransformState<TTransformState>> CreateInterpolationCollection (float cursorSpeed, bool includeEndpoints = true);
        WeightedTransformState<TTransformState> CreateInterpolatedState (float interpolation);
        WeightedTransformState<TTransformState> CreateInterpolatedState (float interpolation, float cursorSpeed);

    }
}