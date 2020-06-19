namespace AWE {

    public class DirectionGradient : IGradient<EDirection> {

        public const int DIRECTION_COUNT = 8;
        public const int DIRECTION_HALFCOUNT = (DIRECTION_COUNT / 2);
        public const int CARDINAL_OFFSET = 2;

        private static readonly EDirection[] opposites;
        private static readonly EDirection[,] projectionMatrix;
        private static readonly EDirection[,] disprojectionMatrix;

        static DirectionGradient () {

            opposites = new EDirection[] {
                EDirection.None,
                EDirection.Left, EDirection.DownLeft, EDirection.Down, EDirection.DownRight,
                EDirection.Right, EDirection.UpRight, EDirection.Up, EDirection.UpLeft
            };

            projectionMatrix = new EDirection[,] {
                { EDirection.None,      EDirection.Right,       EDirection.UpRight,     EDirection.Up,          EDirection.UpLeft,      EDirection.Left,        EDirection.DownLeft,    EDirection.Down,        EDirection.DownRight },
                { EDirection.Right,     EDirection.Right,       EDirection.UpRight,     EDirection.UpRight,     EDirection.Up,          EDirection.None,        EDirection.Down,        EDirection.DownRight,   EDirection.DownRight },
                { EDirection.UpRight,   EDirection.UpRight,     EDirection.UpRight,     EDirection.UpRight,     EDirection.Up,          EDirection.Up,          EDirection.None,        EDirection.Right,       EDirection.Right },
                { EDirection.Up,        EDirection.UpRight,     EDirection.UpRight,     EDirection.Up,          EDirection.UpLeft,      EDirection.UpLeft,      EDirection.Left,        EDirection.None,        EDirection.Right },
                { EDirection.UpLeft,    EDirection.Up,          EDirection.Up,          EDirection.UpLeft,      EDirection.UpLeft,      EDirection.UpLeft,      EDirection.Left,        EDirection.Left,        EDirection.None },
                { EDirection.Left,      EDirection.None,        EDirection.Up,          EDirection.UpLeft,      EDirection.UpLeft,      EDirection.Left,        EDirection.DownLeft,    EDirection.DownLeft,    EDirection.Down },
                { EDirection.DownLeft,  EDirection.Down,        EDirection.None,        EDirection.Left,        EDirection.Left,        EDirection.DownLeft,    EDirection.DownLeft,    EDirection.DownLeft,    EDirection.Down },
                { EDirection.Down,      EDirection.DownRight,   EDirection.Right,       EDirection.None,        EDirection.Left,        EDirection.DownLeft,    EDirection.DownLeft,    EDirection.Down,        EDirection.DownRight },
                { EDirection.DownRight, EDirection.DownRight,   EDirection.Right,       EDirection.Right,       EDirection.None,        EDirection.Down,        EDirection.Down,        EDirection.DownRight,   EDirection.DownRight }
            };

            disprojectionMatrix = new EDirection[,] {
                { EDirection.None,      EDirection.Right,       EDirection.UpRight,     EDirection.Up,          EDirection.UpLeft,      EDirection.Left,        EDirection.DownLeft,    EDirection.Down,        EDirection.DownRight },
                { EDirection.Right,     EDirection.None,        EDirection.None,        EDirection.Right,       EDirection.Right,       EDirection.Right,       EDirection.Right,       EDirection.Right,       EDirection.None },
                { EDirection.UpRight,   EDirection.Up,          EDirection.None,        EDirection.Right,       EDirection.Right,       EDirection.UpRight,     EDirection.UpRight,     EDirection.UpRight,     EDirection.Up },
                { EDirection.Up,        EDirection.Up,          EDirection.None,        EDirection.None,        EDirection.None,        EDirection.Up,          EDirection.Up,          EDirection.Up,          EDirection.Up },
                { EDirection.UpLeft,    EDirection.UpLeft,      EDirection.Left,        EDirection.Left,        EDirection.None,        EDirection.Up,          EDirection.Up,          EDirection.UpLeft,      EDirection.UpLeft },
                { EDirection.Left,      EDirection.Left,        EDirection.Left,        EDirection.Left,        EDirection.None,        EDirection.None,        EDirection.None,        EDirection.Left,        EDirection.Left },
                { EDirection.DownLeft,  EDirection.DownLeft,    EDirection.DownLeft,    EDirection.DownLeft,    EDirection.Down,        EDirection.Down,        EDirection.None,        EDirection.Left,        EDirection.Left },
                { EDirection.Down,      EDirection.Down,        EDirection.Down,        EDirection.Down,        EDirection.Down,        EDirection.Down,        EDirection.None,        EDirection.None,        EDirection.None },
                { EDirection.DownRight, EDirection.Down,        EDirection.Down,        EDirection.DownRight,   EDirection.DownRight,   EDirection.DownRight,   EDirection.Right,       EDirection.Right,       EDirection.None }
            };

        }

        public static DirectionGradient operator + (DirectionGradient d, EDirection e) {

            d.Add (e);
            return d;

        }

        public static DirectionGradient operator - (DirectionGradient d, EDirection e) {

            d.Subtract (e);
            return d;

        }

        public EDirection current { get; set; }

        public EDirection horizontal {

            get {

                var horz = EDirection.None;

                switch (this.current) {

                case EDirection.DownRight:
                case EDirection.Right:
                case EDirection.UpRight:
                    horz = EDirection.Right;
                break;

                case EDirection.UpLeft:
                case EDirection.Left:
                case EDirection.DownLeft:
                    horz = EDirection.Left;
                break;

                }

                return horz;

            }
        }

        public EDirection vertical {

            get {

                var vert = EDirection.None;

                switch (this.current) {

                case EDirection.UpRight:
                case EDirection.Up:
                case EDirection.UpLeft:
                    vert = EDirection.Up;
                break;

                case EDirection.DownLeft:
                case EDirection.Down:
                case EDirection.DownRight:
                    vert = EDirection.Down;
                break;

                }

                return vert;

            }
        }

        public EDirection opposite {

            get {

                return opposites[(int)this.current];

            }
        }

        public bool isNone {

            get {

                return (this.current == EDirection.None);

            }
        }

        public bool isCardinal {

            get {

                return (
                    (this.current == EDirection.Right)
                    || (this.current == EDirection.Up)
                    || (this.current == EDirection.Left)
                    || (this.current == EDirection.Down)
                );

            }
        }

        public DirectionGradient (EDirection current = EDirection.None) {

            this.current = current;

        }

        public EDirection Add (EDirection direction) {

            this.current = this.Project (direction);
            return this.current;

        }

        public EDirection Subtract (EDirection direction) {

            this.Add (direction.opposite ());
            return this.current;

        }

        public EDirection Remove (EDirection direction) {

            this.current = this.Disproject (direction);
            return this.current;

        }

        public EDirection Project (EDirection direction) {

            var myInt = (int)this.current;
            var givenInt = (int)direction;

            return projectionMatrix[myInt, givenInt];

        }

        public EDirection Disproject (EDirection direction) {

            var myInt = (int)this.current;
            var givenInt = (int)direction;

            return disprojectionMatrix[myInt, givenInt];

        }

        public bool IsHorizontal (bool strict = true) {

            bool isHorizontal;

            if (strict) {

                isHorizontal = (
                    (this.current == EDirection.Right)
                    || (this.current == EDirection.Left)
                );

            } else {

                isHorizontal = (
                    (this.current != EDirection.None)
                    && (this.current != EDirection.Up)
                    && (this.current != EDirection.Down)
                );

            }

            return isHorizontal;

        }

        public bool IsVertical (bool strict = true) {

            bool isVertical;

            if (strict) {

                isVertical = (
                    (this.current == EDirection.Up)
                    || (this.current == EDirection.Down)
                );

            } else {

                isVertical = (
                    (this.current != EDirection.None)
                    && (this.current != EDirection.Right)
                    && (this.current != EDirection.Left)
                );

            }

            return isVertical;

        }

        public bool IsRight (bool strict = false) {

            bool isRight;

            if (strict) {

                isRight = (this.current == EDirection.Right);

            } else {

                isRight = (
                    (this.current == EDirection.Right)
                    || (this.current == EDirection.UpRight)
                    || (this.current == EDirection.DownRight)
                );

            }

            return isRight;

        }

        public bool IsUp (bool strict = false) {

            bool isUp;

            if (strict) {

                isUp = (this.current == EDirection.Up);

            } else {

                isUp = (
                    (this.current == EDirection.Up)
                    || (this.current == EDirection.UpRight)
                    || (this.current == EDirection.UpLeft)
                );

            }

            return isUp;

        }

        public bool IsLeft (bool strict = false) {

            bool isLeft;

            if (strict) {

                isLeft = (this.current == EDirection.Left);

            } else {

                isLeft = (
                    (this.current == EDirection.Left)
                    || (this.current == EDirection.UpLeft)
                    || (this.current == EDirection.DownLeft)
                );

            }

            return isLeft;

        }

        public bool IsDown (bool strict = false) {

            bool isDown;

            if (strict) {

                isDown = (this.current == EDirection.Down);

            } else {

                isDown = (
                    (this.current == EDirection.Down)
                    || (this.current == EDirection.DownLeft)
                    || (this.current == EDirection.DownRight)
                );

            }

            return isDown;

        }
    }
}
