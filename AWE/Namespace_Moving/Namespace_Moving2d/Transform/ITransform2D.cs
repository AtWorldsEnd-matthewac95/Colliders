using AWE.Math;

namespace AWE.Moving.Moving2D {

    public interface ITransform2D : ITransform, IReadOnlyTransform2D {

        new IReadOnlyTransform2D AsReadOnly ();

        BooleanNote TransformBy (Transformation2D transformation);
        BooleanNote TranslateBy (pair2f translation);
        BooleanNote RotateBy (angle rotation);
        BooleanNote TransformTo (Transform2DState state);
        BooleanNote TransformTo (Transformation2D transformation);
        BooleanNote TranslateTo (pair2f position);
        BooleanNote RotateTo (angle rotation);
        BooleanNote DilateTo (pair2f dilation);

    }
}
