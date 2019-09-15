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

    public interface ITransformListener<TValueType> where TValueType : struct {

        bool hasOnAnyChange { get; }
        bool hasOnTransformation { get; }
        bool hasOnTranslation { get; }
        bool hasOnRotation { get; }
        bool hasOnDilation { get; }

        void OnAnyChange (ITransformState<TValueType> resultantState);
        void OnTransformation (ITransformState<TValueType> resultantState, ITransformation<TValueType> transformation);
        void OnTranslation (ITransformState<TValueType> resultantState, IReadOnlyList<TValueType> translation);
        void OnRotation (ITransformState<TValueType> resultantState, IReadOnlyList<TValueType> rotation);
        void OnDilation (ITransformState<TValueType> resultantState, IReadOnlyList<TValueType> dilation);

    }

    public interface ITransformListener<TValueType, TTranslation, TRotation, TDilation>
        where TValueType : struct
        where TTranslation : IReadOnlyList<TValueType>
        where TRotation : IReadOnlyList<TValueType>
        where TDilation : IReadOnlyList<TValueType>
    {

        bool hasOnAnyChange { get; }
        bool hasOnTransformation { get; }
        bool hasOnTranslation { get; }
        bool hasOnRotation { get; }
        bool hasOnDilation { get; }
        
        void OnAnyChange (
            ITransformState<TValueType, TTranslation, TRotation, TDilation> resultantState
        );
        void OnTransformation (
            ITransformState<TValueType, TTranslation, TRotation, TDilation> resultantState,
            ITransformation<TValueType, TTranslation, TRotation, TDilation> transformation
        );
        void OnTranslation (
            ITransformState<TValueType, TTranslation, TRotation, TDilation> resultantState,
            TTranslation translation
        );
        void OnRotation (
            ITransformState<TValueType, TTranslation, TRotation, TDilation> resultantState,
            TRotation rotation
        );
        void OnDilation (
            ITransformState<TValueType, TTranslation, TRotation, TDilation> resultantState,
            TDilation dilation
        );

    }
}