using System;
using AWE.Math;

namespace AWE.Moving.Moving2D {

    public interface ITransform2DState : ITransformState<float, pair2f, angle, pair2f>, IComparable<ITransform2DState> {

        bool IsEquivalent (ITransform2DState other);

    }
}