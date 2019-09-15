using System.Collections.Generic;

namespace AWE {

    public interface IPair<T> : IReadOnlyList<T> {

        T first { get; }
        T second { get; }

        T this[bool getSecond] { get; }

        T opposite (int index);
        void Deconstruct (out T first, out T second);

    }
}
