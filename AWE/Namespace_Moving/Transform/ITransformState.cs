using System;
using System.Collections.Generic;

namespace AWE.Moving {

    public interface ITransformState : IComparable, IComparable<ITransformState> {

        IReadOnlyTransform transform { get; }
        TransformStateIndex index { get; }

        ITransformState Add (ITransformation transformation);
        ITransformState Subtract (ITransformation transformation);
        ITransformation FindDifference (ITransformState other);

    }
}