namespace AWE.Moving {

    public delegate TTransformState DTransformPath<out TTransformState> (float position) where TTransformState : ITransformState;

}