namespace AWE.IntExtensions {

    public static class SIntExtensions {

        public static bool IsEven (this int i) {

            return ((i % 2) == 0);

        }

        public static bool IsOdd (this int i) {

            return ((i % 2) == 1);

        }
    }
}