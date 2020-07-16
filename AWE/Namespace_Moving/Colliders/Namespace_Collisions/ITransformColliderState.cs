using System.Collections.Generic;

namespace AWE.Moving.Collisions {

    public interface ITransformColliderState<TTransformState> : IColliderState where TTransformState : ITransformState {

        TTransformState transformState { get; }

        List<float> CreateInterpolationSteps (TTransformState state);
        float FindTranslationCollisionDistance (TTransformState point);

    }
}
