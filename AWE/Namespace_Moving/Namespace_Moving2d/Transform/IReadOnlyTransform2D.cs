using AWE.Math;

namespace AWE.Moving.Moving2D {

    public interface IReadOnlyTransform2D : IReadOnlyTransform<float, pair2f, angle, pair2f, Transformation2D, Transform2DState> {

        new Transform2DState state { get; }

        event DTransform2DUpdate OnAnyChange;
        event DTransform2DTransformation OnTransformation;
        event DTransform2DTranslation OnTranslation;
        event DTransform2DRotation OnRotation;
        event DTransform2DDilation OnDilation;

        void AddListener (ITransform2DListener listener);

    }
}
