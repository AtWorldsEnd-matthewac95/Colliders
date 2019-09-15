//using UnityTransform = UnityEngine.Transform;

namespace AWE.Unity.Moving2D {

    public class UnityTransform2D {// : ATransform2D {

        /*
        private UnityTransform unityTransform;

        public override Transform2DState state {

            get {

                return new Transform2DState (this);

            }
        }

        public override pair2f position {

            get {

                return new pair2f (
                    this.unityTransform.position.x,
                    this.unityTransform.position.y
                );

            }
        }

        public override angle rotation {

            get {

                return new angle (
                    this.unityTransform.eulerAngles.z,
                    EAngleMode.Radian
                );

            }
        }

        public override pair2f dilation {

            get {

                return new pair2f (
                    this.unityTransform.lossyScale.x,
                    this.unityTransform.lossyScale.y
                );

            }
        }

        public UnityTransform2D (UnityTransform unityTransform) {

            this.unityTransform = unityTransform;

        }

        public override BooleanNote TranslateBy (pair2f relativeDeltaPosition) {

            var translation = new Vector3 (
                relativeDeltaPosition.x,
                relativeDeltaPosition.y,
                0f
            );

            this.unityTransform.Translate (translation);

            var onTranslation = this.OnTranslation;

            if (onTranslation != null) {

                onTranslation (this.state, this);

            }

            var onAnyChange = this.OnAnyChange;

            if (onAnyChange != null) {

                onAnyChange (this.state, this);

            }

            return new BooleanNote (true);

        }

        public override BooleanNote RotateBy (angle relativeDeltaRotation) {

            var rotation = new Vector3 (
                0f,
                0f,
                relativeDeltaRotation.radian
            );

            this.unityTransform.Rotate (rotation);

            var onRotation = this.OnRotation;

            if (onRotation != null) {

                onRotation (this.state, this);

            }

            var onAnyChange = this.OnAnyChange;

            if (onAnyChange != null) {

                onAnyChange (this.state, this);

            }

            return new BooleanNote (true);

        }

        public override BooleanNote TranslateTo (pair2f relativePosition) {

            var position = new Vector3 (
                relativePosition.x,
                relativePosition.y,
                0f
            );

            this.unityTransform.position = position;

            var onTranslation = this.OnTranslation;

            if (onTranslation != null) {

                onTranslation (this.state, this);

            }

            var onAnyChange = this.OnAnyChange;

            if (onAnyChange != null) {

                onAnyChange (this.state, this);

            }

            return new BooleanNote (true);

        }

        public override BooleanNote RotateTo (angle relativeRotation) {

            var rotation = new Vector3 (
                0f,
                0f,
                relativeRotation.radian
            );

            this.unityTransform.eulerAngles = rotation;

            var onRotation = this.OnRotation;

            if (onRotation != null) {

                onRotation (this.state, this);

            }

            var onAnyChange = this.OnAnyChange;

            if (onAnyChange != null) {

                onAnyChange (this.state, this);

            }

            return new BooleanNote (true);

        }

        public override BooleanNote DilateTo (pair2f relativeDilation) {

            var dilation = new Vector3 (
                relativeDilation.x,
                relativeDilation.y,
                0f
            );

            this.unityTransform.localScale = dilation;

            var onDilation = this.OnDilation;

            if (onDilation != null) {

                onDilation (this.state, this);

            }

            var onAnyChange = this.OnAnyChange;

            if (onAnyChange != null) {

                onAnyChange (this.state, this);

            }

            return new BooleanNote (true);

        }
        */
    }
}
