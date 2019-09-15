using AWE;
using AWE.Math.FloatExtensions;

namespace AWE.Math.DirectionMathExtensions {

    public static class SDirectionMathExtensions {

        private readonly static DirectionGradient gradient;

        static SDirectionMathExtensions () {

            gradient = new DirectionGradient ();

        }

        public static EDirection AddHorizontal (
            this DirectionGradient direction,
            float value,
            bool strict = false,
            bool rightIsPositive = true
        ) {

            if (strict || !value.IsNegligible ()) {

                if (value > 0f) {

                    direction.Add (
                        rightIsPositive
                        ? EDirection.Right
                        : EDirection.Left
                    );

                } else if (value < 0f) {

                    direction.Add (
                        rightIsPositive
                        ? EDirection.Left
                        : EDirection.Right
                    );

                }
            }

            return direction.current;

        }

        public static EDirection AddVertical (
            this DirectionGradient direction,
            float value,
            bool strict = false,
            bool upIsPositive = true
        ) {

            if (strict || !value.IsNegligible ()) {

                if (value > 0f) {

                    direction.Add (
                        upIsPositive
                        ? EDirection.Up
                        : EDirection.Down
                    );

                } else if (value < 0f) {

                    direction.Add (
                        upIsPositive
                        ? EDirection.Down
                        : EDirection.Up
                    );

                }
            }

            return direction.current;

        }

        public static EDirection Add (
            this DirectionGradient direction,
            pair2f point,
            bool strict = false,
            bool rightIsPositive = true,
            bool upIsPositive = true
        ) {

            direction.AddHorizontal (
                point.x, strict, rightIsPositive
            );
            direction.AddVertical (
                point.y, strict, upIsPositive
            );

            return direction.current;

        }

        public static EDirection AddHorizontal (
            this EDirection direction,
            float value,
            bool strict = false,
            bool rightIsPositive = true
        ) {

            gradient.current = direction;
            return gradient.AddHorizontal (value, strict, rightIsPositive);

        }

        public static EDirection AddVertical (
            this EDirection direction,
            float value,
            bool strict = false,
            bool upIsPositive = true
        ) {

            gradient.current = direction;
            return gradient.AddVertical (value, strict, upIsPositive);

        }

        public static EDirection Add (
            this EDirection direction,
            pair2f point,
            bool strict = false,
            bool rightIsPositive = true,
            bool upIsPositive = true
        ) {

            gradient.current = direction;
            return gradient.Add (point, strict, rightIsPositive, upIsPositive);

        }

        public static angle ToAngle (this EDirection value) => new angle (value, EAngleMode.Degree);

        public static pair2f ToPair (this EDirection value) {

            float x = 0f, y = 0f;

            if (value.IsRight ()) {

                x += 1f;

            } else if (value.IsLeft ()) {

                x -= 1f;

            }

            if (value.IsUp ()) {

                y += 1f;

            } else if (value.IsDown ()) {

                y -= 1f;

            }

            return new pair2f (x, y);

        }
    }
}