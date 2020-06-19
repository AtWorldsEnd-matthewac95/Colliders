using System.Collections.Generic;

namespace AWE.Moving {

    public interface ITransform : IReadOnlyTransform {

        IReadOnlyTransform AsReadOnly ();
        BooleanNote TransformBy (ITransformation transformation);
        BooleanNote TransformTo (ITransformState state);
        BooleanNote TransformTo (ITransformation transformation);

    }

    public interface ITransform<TValueType, TTranslation, TRotation, TDilation, TTransformation, TTransformState>
        : ITransform,
        IReadOnlyTransform<TValueType, TTranslation, TRotation, TDilation, TTransformation, TTransformState>
        where TValueType : struct
        where TTranslation : IReadOnlyList<TValueType>
        where TRotation : IReadOnlyList<TValueType>
        where TDilation : IReadOnlyList<TValueType>
        where TTransformation : ATransformation<TValueType, TTranslation, TRotation, TDilation>
        where TTransformState : ATransformState<TValueType, TTranslation, TRotation, TDilation, TTransformation>
    {

        new IReadOnlyTransform<TValueType, TTranslation, TRotation, TDilation, TTransformation, TTransformState> AsReadOnly ();
        BooleanNote TransformBy (TTransformation transformation);
        BooleanNote TranslateBy (TTranslation translation);
        BooleanNote RotateBy (TRotation rotation);
        BooleanNote TransformTo (TTransformState state);
        BooleanNote TransformTo (TTransformation transformation);
        BooleanNote TranslateTo (TTranslation position);
        BooleanNote RotateTo (TRotation rotation);
        BooleanNote DilateTo (TDilation dilation);

    }
}
