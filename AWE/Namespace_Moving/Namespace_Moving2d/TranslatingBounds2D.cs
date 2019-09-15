using System;
using AWE.Math;

namespace AWE.Moving.Moving2D {

    public class TranslatingBounds2D : IBounds2D {

        private readonly ATransform2D transform;
        private readonly float rightOffset;
        private readonly float topOffset;

        public float width { get; }
        public float height { get; }

        public pair2f position => this.transform.position;

        public float left => (
            this.transform.position.x
            + this.rightOffset
            - this.width
        );

        public float right => (
            this.transform.position.x
            + this.rightOffset
        );

        public float bottom => (
            this.transform.position.y
            + this.topOffset
            - this.height
        );

        public float top => (
            this.transform.position.y
            + this.topOffset
        );

        public float this [EDirection direction] {

            get {

                float value = Single.NaN;

                switch (direction) {

                case EDirection.Right:
                    value = this.right;
                break;

                case EDirection.Up:
                    value = this.top;
                break;

                case EDirection.Left:
                    value = this.left;
                break;

                case EDirection.Down:
                    value = this.bottom;
                break;

                default:
                    // TODO - Throw an exception
                break;

                }

                return value;

            }
        }

        public TranslatingBounds2D (
            ATransform2D transform,
            float rightOffset,
            float topOffset,
            float width,
            float height
        ) {

            if (width < SFloatMath.MINIMUM_DIFFERENCE) {

                // TODO - Throw an exception

            }

            if (height < SFloatMath.MINIMUM_DIFFERENCE) {

                // TODO - Throw an exception

            }

            this.transform = transform;
            this.rightOffset = rightOffset;
            this.topOffset = topOffset;
            this.width = width;
            this.height = height;

        }

        public bool IsContaining (pair2f point) {

            var isContainingPoint = false;
            float horizontalBound = this.right;
            float verticalBound = this.top;

            if (point.x <= horizontalBound) {

                horizontalBound -= this.width;

                if ((point.x >= horizontalBound)
                    && (point.y <= verticalBound)
                ) {

                    verticalBound -= this.height;
                    isContainingPoint = (point.y >= verticalBound);

                }
            }

            return isContainingPoint;

        }
    }
}
