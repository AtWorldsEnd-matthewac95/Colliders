using System.Collections.Generic;

namespace AWE.Moving {

    public interface ITransform {

        ITransformState state { get; }

        void AddListener (ITransformListener listener);

    }

    public interface ITransform<TValueType> : ITransform where TValueType : struct {

        new ITransformState<TValueType> state { get; }
        IReadOnlyList<TValueType> position { get; }
        IReadOnlyList<TValueType> rotation { get; }
        IReadOnlyList<TValueType> dilation { get; }

        void AddListener (ITransformListener<TValueType> listener);
        BooleanNote TransformBy (ITransformation<TValueType> transformation);
        BooleanNote TranslateBy (IReadOnlyList<TValueType> translation);
        BooleanNote RotateBy (IReadOnlyList<TValueType> rotation);
        BooleanNote TransformTo (ITransformState<TValueType> state);
        BooleanNote TransformTo (ITransformation<TValueType> transformation);
        BooleanNote TranslateTo (IReadOnlyList<TValueType> position);
        BooleanNote RotateTo (IReadOnlyList<TValueType> rotation);
        BooleanNote DilateTo (IReadOnlyList<TValueType> dilation);

    }

    public interface ITransform<TValueType, TTranslation, TRotation, TDilation>
        : ITransform<TValueType>
        where TValueType : struct
        where TTranslation : IReadOnlyList<TValueType>
        where TRotation : IReadOnlyList<TValueType>
        where TDilation : IReadOnlyList<TValueType>
    {

        new ITransformState<TValueType, TTranslation, TRotation, TDilation> state { get; }
        new TTranslation position { get; }
        new TRotation rotation { get; }
        new TDilation dilation { get; }

        void AddListener (ITransformListener<TValueType, TTranslation, TRotation, TDilation> listener);
        BooleanNote TransformBy (ITransformation<TValueType, TTranslation, TRotation, TDilation> transformation);
        BooleanNote TranslateBy (TTranslation translation);
        BooleanNote RotateBy (TRotation rotation);
        BooleanNote TransformTo (ITransformState<TValueType, TTranslation, TRotation, TDilation> state);
        BooleanNote TransformTo (ITransformation<TValueType, TTranslation, TRotation, TDilation> transformation);
        BooleanNote TranslateTo (TTranslation position);
        BooleanNote RotateTo (TRotation rotation);
        BooleanNote DilateTo (TDilation dilation);

    }
}