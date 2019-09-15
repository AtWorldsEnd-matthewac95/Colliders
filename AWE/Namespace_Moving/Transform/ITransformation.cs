using System;
using System.Collections.Generic;

namespace AWE.Moving {

    public interface ITransformation {

        ITransformation Add (ITransformation other);
        ITransformation Subtract (ITransformation other);

    }

    public interface ITransformation<TValueType> : ITransformation, IEquatable<ITransformation<TValueType>>
        where TValueType : struct
    {

        IReadOnlyList<TValueType> translation { get; }
        IReadOnlyList<TValueType> rotation { get; }
        IReadOnlyList<TValueType> dilation { get; }

        ITransformation<TValueType> Add (ITransformation<TValueType> other);
        ITransformation<TValueType> Subtract (ITransformation<TValueType> other);

    }

    public interface ITransformation<TValueType, TTranslation, TRotation, TDilation> :
        ITransformation<TValueType>,
        IEquatable<ITransformation<TValueType, TTranslation, TRotation, TDilation>>
        where TValueType : struct
        where TTranslation : IReadOnlyList<TValueType>
        where TRotation : IReadOnlyList<TValueType>
        where TDilation : IReadOnlyList<TValueType>
    {

        new TTranslation translation { get; }
        new TRotation rotation { get; }
        new TDilation dilation { get; }

        ITransformation<TValueType, TTranslation, TRotation, TDilation> Add (
            ITransformation<TValueType, TTranslation, TRotation, TDilation> other
        );
        ITransformation<TValueType, TTranslation, TRotation, TDilation> Subtract (
            ITransformation<TValueType, TTranslation, TRotation, TDilation> other
        );

    }
}