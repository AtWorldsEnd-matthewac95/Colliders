using System;
using System.Collections.Generic;

namespace AWE.Moving {

    public interface ITransformState : IComparable, IComparable<ITransformState> {

        TransformStateIndex index { get; }

    }

    public interface ITransformState<TValueType> : ITransformState where TValueType : struct
    {

        IReadOnlyList<TValueType> position { get; }
        IReadOnlyList<TValueType> rotation { get; }
        IReadOnlyList<TValueType> dilation { get; }

        bool IsEquivalent (ITransformState<TValueType> other);

    }

    public interface ITransformState<TValueType, TPosition, TRotation, TDilation> : ITransformState<TValueType>
        where TValueType : struct
        where TPosition : IReadOnlyList<TValueType>
        where TRotation : IReadOnlyList<TValueType>
        where TDilation : IReadOnlyList<TValueType>
    {

        new TPosition position { get; }
        new TRotation rotation { get; }
        new TDilation dilation { get; }

        bool IsEquivalent (ITransformState<TValueType, TPosition, TRotation, TDilation> other);

    }
}