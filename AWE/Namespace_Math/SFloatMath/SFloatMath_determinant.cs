using AWE.IntExtensions;

namespace AWE.Math {

    public static partial class SFloatMath {

        public static float GetCrossProduct (pair2f p, pair2f q) {

            return GetDeterminant (p, q);

        }

        public static float GetDeterminant (pair2f p, pair2f q) {

            return (
                (p.x * q.y)
                - (q.x * p.y)
            );

        }

        public static float GetDeterminant (float[,] matrix) {

            float determinant = 0f;
            int length = matrix.GetLength (0);

            float subdeterminant;
            int rowIndex, colIndex;

            for (int i = 0; i < length; i++) {

                subdeterminant = 1f;
                rowIndex = 0;
                colIndex = i;

                while (rowIndex < length) {

                    subdeterminant *= matrix[rowIndex, colIndex];

                    rowIndex++;
                    colIndex++;
                    colIndex = (
                        (colIndex < length)
                        ? colIndex
                        : 0
                    );

                }

                if (i.IsEven ()) {

                    determinant += subdeterminant;

                } else {

                    determinant -= subdeterminant;

                }

                subdeterminant = 1f;
                rowIndex--;
                colIndex--;
                colIndex = (
                    (colIndex >= 0)
                    ? colIndex
                    : (length - 1)
                );

                while (rowIndex >= 0) {

                    subdeterminant *= matrix[rowIndex, colIndex];

                    rowIndex--;
                    colIndex++;
                    colIndex = (
                        (colIndex < length)
                        ? colIndex
                        : 0
                    );

                }

                if (i.IsEven ()) {

                    determinant -= subdeterminant;

                } else {

                    determinant += subdeterminant;

                }
            }

            return determinant;

        }
    }
}
