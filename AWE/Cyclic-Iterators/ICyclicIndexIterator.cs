namespace AWE {

    public interface ICyclicIndexIterator<out T> : ICyclicIterator<T> {

        int currentIndex { get; set; }
        int nextIndex { get; }
        int previousIndex { get; }
        int count { get; }

        ICyclicIndexIterator<T> Copy (bool copyCycles = false, int startingIndex = -1);


    }
}
