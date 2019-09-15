namespace AWE.Moving {

    public interface IFrameMovement<out TTransformation> : IMovement<int, TTransformation> where TTransformation : ITransformation {

        int elapsed { get; }
        float completion { get; }

    }
}