using AWE.Math;

namespace AWE.Moving.Moving2D {

    public interface ITransform2DListener {

        bool hasOnAnyChange { get; }
        bool hasOnTransformation { get; }
        bool hasOnTranslation { get; }
        bool hasOnRotation { get; }
        bool hasOnDilation { get; }

        void OnAnyChange (Transform2DState resultantState);
        void OnTransformation (Transform2DState resultantState, Transformation2D transformation);
        void OnTranslation (Transform2DState resultantState, pair2f translation);
        void OnRotation (Transform2DState resultantState, angle rotation);
        void OnDilation (Transform2DState resultantState, pair2f dilation);

    }
}
