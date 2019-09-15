namespace AWE {

    public static class SEDirectionExtensions {

        private static DirectionGradient gradient;

        static SEDirectionExtensions () {

            gradient = new DirectionGradient ();

        }

        public static EDirection horizontal (this EDirection value) {

            gradient.current = value;
            return gradient.horizontal;

        }

        public static EDirection vertical (this EDirection value) {

            gradient.current = value;
            return gradient.vertical;

        }

        public static EDirection opposite (this EDirection value) {

            gradient.current = value;
            return gradient.opposite;

        }
        
        public static bool IsCardinal (this EDirection value) {

            gradient.current = value;
            return gradient.isCardinal;

        }

        public static bool IsNone (this EDirection value) {

            return (value == EDirection.None);

        }

        public static EDirection Add (this EDirection value, EDirection other) {

            gradient.current = value;
            gradient.Add (other);
            return gradient.current;

        }

        public static EDirection Subtract (this EDirection value, EDirection other) {

            gradient.current = value;
            gradient.Subtract (other);
            return gradient.current;

        }

        public static EDirection Remove (this EDirection value, EDirection other) {

            gradient.current = value;
            gradient.Remove (other);
            return gradient.current;

        }

        public static bool IsHorizontal (this EDirection value, bool strict = true) {

            gradient.current = value;
            return gradient.IsHorizontal (strict);

        }

        public static bool IsVertical (this EDirection value, bool strict = true) {

            gradient.current = value;
            return gradient.IsVertical (strict);

        }

        public static bool IsRight (this EDirection value, bool strict = false) {

            gradient.current = value;
            return gradient.IsRight (strict);

        }

        public static bool IsUp (this EDirection value, bool strict = false) {

            gradient.current = value;
            return gradient.IsUp (strict);

        }

        public static bool IsLeft (this EDirection value, bool strict = false) {

            gradient.current = value;
            return gradient.IsLeft (strict);

        }

        public static bool IsDown (this EDirection value, bool strict = false) {

            gradient.current = value;
            return gradient.IsDown (strict);

        }
    }
}