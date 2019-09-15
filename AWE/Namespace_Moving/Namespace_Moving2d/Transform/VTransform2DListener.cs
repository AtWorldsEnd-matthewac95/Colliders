using AWE.Math;

namespace AWE.Moving.Moving2D {

    public class VTransform2DListener : ATransform2DListener {

        public override bool hasOnAnyChange => false;
        public override bool hasOnTransformation => false;
        public override bool hasOnTranslation => false;
        public override bool hasOnRotation => false;
        public override bool hasOnDilation => false;

        public override void OnAnyChange (Transform2DState resultantState) {}
        public override void OnTransformation (Transform2DState resultantState, Transformation2D transformation) {}
        public override void OnTranslation (Transform2DState resultantState, pair2f translation) {}
        public override void OnRotation (Transform2DState resultantState, angle rotation) {}
        public override void OnDilation (Transform2DState resultantState, pair2f dilation) {}

    }
}