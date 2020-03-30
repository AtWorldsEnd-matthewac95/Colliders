using System.Collections.Generic;

namespace AWE.Moving {

    public interface ITransformListener {

        bool hasOnAnyChange { get; }
        bool hasOnTransformation { get; }
        bool hasOnTranslation { get; }
        bool hasOnRotation { get; }
        bool hasOnDilation { get; }

        void OnAnyChange (ITransformState resultantState);
        void OnTransformation (ITransformState resultantState, ITransformation transformation);
        void OnTranslation (ITransformState resultantState, ITransformation transformation);
        void OnRotation (ITransformState resultantState, ITransformation transformation);
        void OnDilation (ITransformState resultantState, ITransformation transformation);

    }

    public interface ITransformListener<in TValueType, in TTranslation, in TRotation, in TDilation, in TTransformation, in TTransformState>
        where TValueType : struct
        where TTranslation : IReadOnlyList<TValueType>
        where TRotation : IReadOnlyList<TValueType>
        where TDilation : IReadOnlyList<TValueType>
        where TTransformation : ATransformation<TValueType, TTranslation, TRotation, TDilation>
        where TTransformState : ATransformState<TValueType, TTranslation, TRotation, TDilation, TTransformation>
    {

        bool hasOnAnyChange { get; }
        bool hasOnTransformation { get; }
        bool hasOnTranslation { get; }
        bool hasOnRotation { get; }
        bool hasOnDilation { get; }

        void OnAnyChange (TTransformState resultantState);
        void OnTransformation (TTransformState resultantState, TTransformation transformation);
        void OnTranslation (TTransformState resultantState, TTranslation translation);
        void OnRotation (TTransformState resultantState, TRotation rotation);
        void OnDilation (TTransformState resultantState, TDilation dilation);

    }
}