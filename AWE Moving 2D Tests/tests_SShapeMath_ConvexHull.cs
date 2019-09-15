using Xunit;
using System.Diagnostics;
using System.Collections.Generic;
using AWE.Math;
using System;

namespace AWE.UnitTests.XUnit.Math {

    public static partial class STestHelpers {

        internal static List<pair2f> CreatePointListFromFloatArray (float[] floats) {

            var list = new List<pair2f> ();

            int i;
            for (i = 0; (i + 1) < floats.Length; i += 2) {

                list.Add (new pair2f (floats[i], floats[i + 1]));

            }

            if (i < floats.Length) {

                // The array was of odd length, so create the last point using the last element and 0f.
                list.Add (new pair2f (floats[i], 0f));

            }

            return list;

        }

        internal static bool PointListsAreEquivalent (List<pair2f> list1, List<pair2f> list2) {

            var listContentsAreEquivalent = true;
            var listsAreSameSize = (list1.Count == list2.Count);

            if (listsAreSameSize) {

                for (int i = 0; i < list1.Count; i++) {

                    listContentsAreEquivalent &= list1[i].Equals (list2[i]);

                    if (!listContentsAreEquivalent) {

                        break;

                    }
                }
            }

            return (listsAreSameSize && listContentsAreEquivalent);

        }

        internal static List<pair2f> CreateRegularPolygonPointList (float radius, int count) {

            var polygon = new List<pair2f> (count);

            for (int i = 0; i < count; i++) {

                var theta = new angle ((float)i / count, EAngleMode.Percent);
                polygon.Add (new pair2f (SFloatMath.Cosine (theta), SFloatMath.Sine (theta)));

            }

            return polygon;

        }

        internal static List<pair2f> CreateRandomPointList (float range, int count) {

            var list = new List<pair2f> ();
            var rng = new Random ();
            var range2 = (range * 2f);

            for (int i = 0; i < count; i++) {

                list.Add (new pair2f (
                    ((range2 * (float)rng.NextDouble ()) - range),
                    ((range2 * (float)rng.NextDouble ()) - range)
                ));

            }

            return list;

        }
    }

    public class tests_SShapeMath_ConvexHull {

        [Theory]
        [InlineData(1, 4, new float[] {
            0f, 0f,
            1f, 1f,
            0f, 1f,
            1f, 0f,
            0f, 0f,
            1f, 1f,
            0f, 1f,
            1f, 0f,
            0.5f, 0.5f
        })]
        [InlineData(2, 3, new float[] {
            1f, 2f,
            1f, 2f,
            1f, 2f,
            1f, 2f,
            1f, 2f,
            1f, 0f,
            0f, 0f,
            2f, 0f,
            0f, 0f,
            2f, 0f,
            1f, 1f,
            1f, 2f,
            1f, 0f,
            0f, 0f,
            2f, 0f
        })]
        public void ConvexHullRemovesDuplicates (int testCaseNumber, int expected, float[] shape) {

            var inputShape = STestHelpers.CreatePointListFromFloatArray (shape);
            var convexHull = SShapeMath.GetConvexHull (inputShape.AsReadOnly ()).AsReadOnly ();

            var convexHullShape = new ConvexPolygon2D (convexHull);

            Debug.WriteLine ("Convex Hull from Test Case #{0}: {1}", testCaseNumber, convexHullShape.ToString ());

            Assert.Equal (expected, convexHull.Count);

        }

        [Theory]
        [InlineData(
            new float[] {
                0f, 0f,
                1f, 0f,
                0f, 1f
            },
            new float[] {
                1f, 0f,
                0f, 0f,
                0f, 1f
            }
        )]
        [InlineData(
            new float[] {
                3f, 0f,
                1f, 0f,
                0f, 2f,
                2f, 3f,
                4f, 2f
            },
            new float[] {
                4f, 2f,
                3f, 0f,
                1f, 0f,
                0f, 2f,
                2f, 3f
            }
        )]
        [InlineData(
            new float[] {
                -3f, -4f,
                -1f, -6f,
                1f, -4f,
                1f, 1f,
                -3f, 1f,
                -1f, 3f
            },
            new float[] {
                1f, 1f,
                1f, -4f,
                -1f, -6f,
                -3f, -4f,
                -3f, 1f,
                -1f, 3f
            }
        )]
        [InlineData(
            new float[] {
                4f, -1f,
                4f, 1f,
                1f, 4f,
                -1f, 4f,
                -4f, 1f,
                -4f, -1f,
                -1f, -4f,
                1f, -4f
            },
            new float[] {
                4f, 1f,
                4f, -1f,
                1f, -4f,
                -1f, -4f,
                -4f, -1f,
                -4f, 1f,
                -1f, 4f,
                1f, 4f
            }
        )]
        public void ConvexHullCreatesClockwiseShape (float[] shape, float[] expected) {

            var convexHull = SShapeMath.GetConvexHull (STestHelpers.CreatePointListFromFloatArray (shape).AsReadOnly ());
            Assert.True (STestHelpers.PointListsAreEquivalent (convexHull, STestHelpers.CreatePointListFromFloatArray (expected)));

        }

        [Theory]
        [InlineData(true, new float[] {
            0f, 0f,
            1f, 1f,
            0f, 1f
        })]
        [InlineData(false, new float[] {
            0f, 0f
        })]
        [InlineData(true, new float[] {
            10f, 0f,
            6f, 1f,
            3f, 3f,
            1f, 6f,
            0f, 10f,
            1f, 14f,
            3f, 17f,
            6f, 19f,
            10f, 20f,
            14f, 19f,
            17f, 17f,
            19f, 14f,
            20f, 10f,
            19f, 6f,
            17f, 3f,
            14f, 1f,
        })]
        [InlineData(false, new float[] {
            17f, 3f,
            3f, 3f,
            1f, 6f,
            6f, 1f,
            0f, 10f,
            20f, 10f,
            19f, 14f,
            3f, 17f,
            6f, 19f,
            10f, 0f,
            10f, 20f,
            14f, 19f,
            14f, 1f,
            17f, 17f,
            19f, 6f,
            1f, 14f,
        })]
        [InlineData(true, new float[] {
            0f, 0f,
            0f, 2f,
            1f, 1f,
            1f, 0f
        })]
        [InlineData(false, new float[] {
            0f, 0f,
            0f, 1f,
            0f, 2f,
            1f, 1f,
            1f, 0f
        })]
        [InlineData(false, new float[] {
            0f, 0f,
            0f, 1f,
            0f, 2f,
            0f, 3f,
            0f, 4f
        })]
        [InlineData(true, new float[] {
            -687.34f, -9.8901f,
            -1498.1f, 20f,
            84.84f, 4560f,
            765.111f, -9f
        })]
        public void IsConvexValid (bool expected, float[] shape) {

            Assert.Equal (expected, SShapeMath.IsConvex (STestHelpers.CreatePointListFromFloatArray (shape)));

        }
    }
}