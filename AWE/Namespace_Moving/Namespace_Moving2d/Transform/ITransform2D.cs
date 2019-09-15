using AWE.Math;

namespace AWE.Moving.Moving2D {

    public interface ITransform2D : ITransform<float, pair2f, angle, pair2f> {

        new Transform2DState state { get; }

        event DTransform2DUpdate OnAnyChange;
        event DTransform2DTransformation OnTransformation;
        event DTransform2DTranslation OnTranslation;
        event DTransform2DRotation OnRotation;
        event DTransform2DDilation OnDilation;

        void AddListener (ITransform2DListener listener);

    }
}
