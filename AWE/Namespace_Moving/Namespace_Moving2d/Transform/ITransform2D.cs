using AWE.Math;

namespace AWE.Moving.Moving2D {

    public interface ITransform2D : ITransform<float, pair2f, angle, pair2f, Transformation2D, Transform2DState>, IReadOnlyTransform2D {

        new IReadOnlyTransform2D AsReadOnly ();

    }
}
