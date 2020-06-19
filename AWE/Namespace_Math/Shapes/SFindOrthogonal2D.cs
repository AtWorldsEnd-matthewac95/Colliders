namespace AWE.Math {

    public static class SFindOrthogonal2D {

        public static pair2f Left (pair2f vector) => new pair2f (-vector.y, vector.x);
        public static pair2f Right (pair2f vector) => new pair2f (vector.y, -vector.x);
        public static pair2f Clockwise (pair2f vector) => Left (vector);
        public static pair2f Counterclockwise (pair2f vector) => Right (vector);

    }
}
