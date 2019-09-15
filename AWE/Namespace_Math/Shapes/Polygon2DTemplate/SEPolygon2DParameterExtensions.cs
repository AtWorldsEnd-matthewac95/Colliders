using System;
using System.Collections.Generic;

namespace AWE.Math {

    internal static class SEPolygon2DParameterExtensions {

        internal static KeyValuePair<EPolygon2DParameter, ValueType> PairWith (
            this EPolygon2DParameter parameter,
            ValueType value
        ) {

            // Possible TODO - Throw an exception if the given value doesn't match
            // the intended parameter type?

            return new KeyValuePair<EPolygon2DParameter, ValueType> (parameter, value);

        }
    }
}