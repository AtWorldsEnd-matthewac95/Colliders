namespace AWE.Math {

    public class BoxShape2D : ConvexPolygon2D {

        public float right => this.bounds.right;
        public float top => this.bounds.top;
        public float left => this.bounds.left;
        public float bottom => this.bounds.bottom;

        public BoxShape2D (
            float right,
            float top,
            float left,
            float bottom
        ) : base (
            (new Polygon2DTemplate ()).SetAsBox (
                right,
                top,
                left,
                bottom
            )
        ) {
        }

        public BoxShape2D FindOverlap (BoxShape2D other) {

            BoxShape2D overlap = null;

            float right = System.Math.Min (this.right, other.right);
            float left = System.Math.Max (this.left, other.left);

            if ((right - left) > SFloatMath.MINIMUM_DIFFERENCE) {

                float top = System.Math.Min (this.top, other.top);
                float bottom = System.Math.Max (this.bottom, other.bottom);

                if ((top - bottom) > SFloatMath.MINIMUM_DIFFERENCE) {

                    overlap = new BoxShape2D (right, top, left, bottom);

                }
            }

            return overlap;

        }

        protected override APolygon2D _CreateOffset (pair2f offset) => this.CreateOffset (offset);

        new public BoxShape2D CreateOffset (pair2f offset) => new BoxShape2D (
            (this.right + offset.x),
            (this.top + offset.y),
            (this.left + offset.x),
            (this.bottom + offset.y)
        );

    }
}