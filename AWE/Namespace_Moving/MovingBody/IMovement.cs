namespace AWE.Moving {

    public interface IMovement {

        ITransformation total { get; }
        EMovementState state { get; }
        bool cancelWhenPaused { get; }
        bool pauseWhenCancelled { get; }

        ITransformation MoveNext ();
        void Reset ();

    }

    public interface IMovement<out TTransformation> : IMovement where TTransformation : ITransformation {

        new TTransformation total { get; }

    }

    public interface IMovement<in TUnit, out TTransformation> : IMovement<TTransformation> where TTransformation : ITransformation {

        TTransformation MoveNext (TUnit units);

    }
}
