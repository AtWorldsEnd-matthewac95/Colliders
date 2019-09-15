namespace AWE.Math {

    public static class SEQuadrantExtensions {

        public static int x (this EQuadrant q) => (
            ((q == EQuadrant.PP) || (q == EQuadrant.PN))
            ? 1
            : -1
        );
        
        public static int y (this EQuadrant q) => (
            ((q == EQuadrant.PP) || (q == EQuadrant.NP))
            ? 1
            : -1
        );
    }
}