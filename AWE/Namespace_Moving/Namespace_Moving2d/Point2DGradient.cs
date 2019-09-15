using AWE.Math;
using AWE.Math.FloatExtensions;

namespace AWE.Moving.Moving2D {

    public class Point2DGradient : IGradient<pair2f> {

        private pair2f baseOffset;
        private pair2f? offset;
        private ATransform2D transform;
        private TransformStateIndex currentStateIndex;

        public pair2f current {

            get {

                var position = this.transform.position;

                if (!this.offset.HasValue) {

                    this.offset = this.CalculateOffset ();

                }

                position += this.offset.Value;

                return position;

            }
        }

        public Point2DGradient (ATransform2D transform) : this (transform, pair2f.origin) {}

        public Point2DGradient (
            ATransform2D transform,
            pair2f offset
        ) {

            //transform.OnRotation += this.Clear;
            //transform.OnDilation += this.Clear;

            this.transform = transform;
            this.currentStateIndex = transform.state.index;
            this.baseOffset = offset;
            this.offset = this.CalculateOffset ();

        }

        private void Clear (Transform2DState resultantState) {

            if ((this.transform == resultantState.transform)
                && (this.currentStateIndex < resultantState.index)
            ) {

                this.currentStateIndex = resultantState.index;
                this.offset = null;

            }
        }

        private pair2f CalculateOffset () {

            var dilation = this.transform.dilation;
            var direction = new pair2f (
                (this.baseOffset.x * dilation.x),
                (this.baseOffset.y * dilation.y)
            );
            var magnitude = direction.magnitude;

            if (magnitude.IsNegligible ()) {

                direction = pair2f.origin;
                magnitude = 0f;

            } else {

                var angle = (
                    this.transform.rotation
                    + SFloatMath.Arctangent (direction)
                );
                direction = new pair2f (
                    SFloatMath.Cosine (angle),
                    SFloatMath.Sine (angle)
                );

            }

            return (direction * magnitude);

        }

        public bool Contains (ATransform2D transform) {

            return (this.transform == transform);

        }
    }
}
