using System.Collections.Generic;

namespace AWE.Moving {

    public interface IEnumerableMovement<out TTransformation> : IMovement<TTransformation>, IEnumerable<TTransformation>
        where TTransformation : ITransformation
    {

        new TTransformation MoveNext ();

    }
}