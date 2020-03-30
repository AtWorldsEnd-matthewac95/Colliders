using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AWE.Moving {

    public interface IReadOnlyTransform {

        ITransformState state { get; }

        void AddListener (ITransformListener listener);

    }

    public interface IReadOnlyTransform<out TTransformState> : IReadOnlyTransform where TTransformState : ITransformState {

        new TTransformState state { get; }

    }

    public interface IReadOnlyTransform<TValueType, TPosition, TRotation, TDilation, TTransformation, TTransformState> : IReadOnlyTransform<TTransformState>
        where TValueType : struct
        where TPosition : IReadOnlyList<TValueType>
        where TRotation : IReadOnlyList<TValueType>
        where TDilation : IReadOnlyList<TValueType>
        where TTransformation : ATransformation<TValueType, TPosition, TRotation, TDilation>
        where TTransformState : ATransformState<TValueType, TPosition, TRotation, TDilation, TTransformation>
    {

        new TTransformState state { get; }
        TPosition position { get; }
        TRotation rotation { get; }
        TDilation dilation { get; }

        void AddListener (ITransformListener<TValueType, TPosition, TRotation, TDilation, TTransformation, TTransformState> listener);

    }
}