using System;

namespace AWE.Moving {

    public interface ITransformation : IEquatable<ITransformation> {

        ITransformation Add (ITransformation other);
        ITransformation Subtract (ITransformation other);

    }
}