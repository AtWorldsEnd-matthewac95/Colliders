namespace AWE.Math {

    public sealed class ConvexHull2DBuilderOptions {

        static ConvexHull2DBuilderOptions () {

            clockwise = new ConvexHull2DBuilderOptions (DirectionSpectrum2D.clockwise, SFindOrthogonal2D.Clockwise);
            counterclockwise = new ConvexHull2DBuilderOptions (DirectionSpectrum2D.counterclockwise, SFindOrthogonal2D.Counterclockwise);

        }

        public static ConvexHull2DBuilderOptions clockwise { get; }
        public static ConvexHull2DBuilderOptions counterclockwise { get; }

        public DirectionSpectrum2D directionalSearch { get; }
        public DFindOrthogonal2D DelegateFindOrthogonal { get; }

        internal ConvexHull2DBuilderOptions (DirectionSpectrum2D directionalSearch, DFindOrthogonal2D DelegateFindOrthogonal) {

            this.directionalSearch = directionalSearch;
            this.DelegateFindOrthogonal = DelegateFindOrthogonal;

        }
    }
}
