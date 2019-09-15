using AWE.Math;

namespace AWE.Moving.Moving2D {

    public abstract class ATransform2DListener : ITransform2DListener {

        public abstract bool hasOnAnyChange { get; }
        public abstract bool hasOnTransformation { get; }
        public abstract bool hasOnTranslation { get; }
        public abstract bool hasOnRotation { get; }
        public abstract bool hasOnDilation { get; }

        public abstract void OnAnyChange (Transform2DState resultantState);
        public abstract void OnTransformation (Transform2DState resultantState, Transformation2D transformation);
        public abstract void OnTranslation (Transform2DState resultantState, pair2f translation);
        public abstract void OnRotation (Transform2DState resultantState, angle rotation);
        public abstract void OnDilation (Transform2DState resultantState, pair2f dilation);

    }
}
