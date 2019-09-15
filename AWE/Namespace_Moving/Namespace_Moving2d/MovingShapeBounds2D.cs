using AWE.Math;

namespace AWE.Moving.Moving2D {

    public class MovingShapeBounds2D : IGradient<TranslatingBounds2D>, IBounds2D {

        private readonly ATransform2D transform;
        private readonly DPointCollection GetReferencePoints;

        private TranslatingBounds2D value;
        private TransformStateIndex currentStateIndex;

        internal TranslatingBounds2D current {

            get {

                if (this.value == null) {

                    this.value = DetermineValue ();

                }

                return this.value;

            }
        }

        TranslatingBounds2D IGradient<TranslatingBounds2D>.current => this.current;

        public float right => this.current.right;
        public float top => this.current.top;
        public float left => this.current.left;
        public float bottom => this.current.bottom;
        public float width => this.current.width;
        public float height => this.current.height;

        public float this [EDirection direction] => this.current[direction];

        public MovingShapeBounds2D (
            ATransform2D transform,
            DPointCollection GetReferencePoints,
            bool evaluateImmediately = true
        ) {

            //transform.OnRotation += this.Clear;
            //transform.OnDilation += this.Clear;

            this.transform = transform;
            this.GetReferencePoints = GetReferencePoints;

            if (evaluateImmediately) {

                this.value = this.DetermineValue ();

            }
        }

        private TranslatingBounds2D DetermineValue () {

            var extrema = new Bounds2D (this.GetReferencePoints ());

            return new TranslatingBounds2D (
                this.transform,
                (extrema.right - this.transform.position.x),
                (extrema.top - this.transform.position.y),
                (extrema.right - extrema.left),
                (extrema.top - extrema.bottom)
            );

        }

        private void Clear (Transform2DState resultantState) {

            if ((this.transform == resultantState.transform)
                && (this.currentStateIndex < resultantState.index)) {

                this.currentStateIndex = resultantState.index;
                this.value = null;

            }
        }

        public bool IsContaining (pair2f point) => this.current.IsContaining (point);

    }
}
