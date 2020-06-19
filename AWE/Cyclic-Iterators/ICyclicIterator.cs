namespace AWE {

    public interface ICyclicIterator<out T> {

        T current { get; }
        T next { get; }
        T previous { get; }
        int cycles { get; }

        bool MoveNext ();
        bool MovePrevious ();
        ICyclicIterator<T> Copy (bool copyCycles = false);
        int ResetCycles ();

    }
}
