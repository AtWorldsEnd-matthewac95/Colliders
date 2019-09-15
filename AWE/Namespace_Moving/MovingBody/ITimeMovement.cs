namespace AWE.Moving {

    public interface ITimeMovement<out TTransformation> : IMovement<float, TTransformation> where TTransformation : ITransformation {

        float elapsed { get; }
        float completion { get; }

    }
}