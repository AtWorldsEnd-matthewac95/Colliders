using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xunit;
using AWE.Math;

namespace AWE.UnitTests.XUnit.Math {

    public static partial class STestHelpers {

        internal class TestPolygon : APolygon2D {

            private readonly List<pair2f> _verticies;
            public Bounds2D _bounds { get; set; }
            public pair2f _center { get; set; }

            public override Bounds2D bounds => this._bounds;
            public override pair2f center => this._center;

            public override ReadOnlyCollection<pair2f> unoffsetVerticies => this._verticies.AsReadOnly ();

            public TestPolygon (float[] verticies) : this (CreatePointListFromFloatArray (verticies)) {}
            public TestPolygon (List<pair2f> verticies) => this._verticies = verticies;

            public override bool IsContainingPoint (pair2f point) => SShapeMath.IsPointInConvexShape (point, this.unoffsetVerticies);

            protected override APolygon2D _CreateOffset (angle offset, pair2f center = default) => throw new System.NotImplementedException ();
            protected override APolygon2D _CreateOffset (pair2f offset, angle rotation, pair2f center = default) => throw new System.NotImplementedException ();

            protected override APolygon2D _CreateOffset (pair2f offset) => this.CreateOffset (offset);
            new public TestPolygon CreateOffset (pair2f offset) => new TestPolygon (this._verticies.Select (point => (point + offset)).ToList ());

        }
    }

    public class tests_SShapeMath_ConvexOverlap {

        [Theory]
        [InlineData(
            new float[] {
                5f, 5f,
                5f, -5f,
                -5f, -5f,
                -5f, 5f
            },
            new float[] {
                -6f, 0f,
                0f, 6f,
                6f, 0f,
                0f, -6f
            },
            new float[] {
                5f, 1f,
                5f, -1f,
                1f, -5f,
                -1f, -5f,
                -5f, -1f,
                -5f, 1f,
                -1f, 5f,
                1f, 5f
            }
        )]
        [InlineData(
            new float[] {
                1f, 2f,
                -4f, 2f,
                -5f, -5f,
                0f, -8f,
                7f, -6f,
                3f, 0f
            },
            new float[] {
                -1f, 5f,
                -3f, -1f,
                -2f, -3f,
                0f, -4f,
                3f, -4f,
                7f, -2f,
                5f, 5f
            },
            new float[] {
                -2f, 2f,
                -3f, -1f,
                -2f, -3f,
                0f, -4f,
                3f, -4f,
                5f, -3f,
                3f, 0f,
                1f, 2f
            }
        )]
        [InlineData(
            new float[] {
                6f, -2f,
                6f, 6f,
                -3f, 6f,
                -3f, -2f
            },
            new float[] {
                5f, 2f,
                3f, 10f,
                -1f, 6f,
                -3f, 0f,
                -1f, -4f,
                4f, -2f
            },
            new float[] {
                4f, 6f,
                -1f, 6f,
                -3f, 0f,
                -2f, -2f,
                4f, -2f,
                5f, 2f
            }
        )]
        [InlineData(
            new float[] {
                3f, 1f,
                1f, 3f,
                -1f, 3f,
                -3f, 1f,
                -3f, -3f,
                3f, -3f
            },
            new float[] {
                3f, 3f,
                -3f, 3f,
                -3f, -1f,
                -1f, -3f,
                1f, -3f,
                3f, -1f
            },
            new float[] {
                1f, 3f,
                -1f, 3f,
                -3f, 1f,
                -3f, -1f,
                -1f, -3f,
                1f, -3f,
                3f, -1f,
                3f, 1f
            }
        )]
        [InlineData(
            new float[] {
                0f, 0f,
                0f, 4f,
                4f, 4f,
                4f, 0f
            },
            new float[] {
                1f, 1f,
                1f, 2f,
                2f, 2f,
                2f, 1f
            },
            new float[] {
                1f, 1f,
                1f, 2f,
                2f, 2f,
                2f, 1f
            }
        )]
        [InlineData(
            new float[] {
                0f, 4f,
                6f, 3f,
                8f, -4f,
                3f, -4f,
                -5f, -2f,
                -4f, 0f
            },
            new float[] {
                8f, -4f,
                0f, -4f,
                -4f, 0f,
                -6f, 5f,
                6f, 3f,
                9f, 0f
            },
            new float[] {
                6f, 3f,
                8f, -4f,
                3f, -4f,
                -1f, -3f,
                -4f, 0f,
                0f, 4f
            }
        )]
        public void FindSpecificOverlaps (float[] polygon1points, float[] polygon2points, float[] expected) {

            var stopwatch = new Stopwatch ();
            var polygon1 = new ConvexPolygon2D (new STestHelpers.TestPolygon (polygon1points));
            var polygon2 = new ConvexPolygon2D (new STestHelpers.TestPolygon (polygon2points));

            stopwatch.Start ();
            var overlap = polygon1.FindOverlap (polygon2);
            stopwatch.Stop ();

            Debug.WriteLine ("Overlap found in {0} ms", stopwatch.ElapsedMilliseconds);

            Assert.True (STestHelpers.PointListsAreEquivalent (
                STestHelpers.CreatePointListFromFloatArray (expected),
                overlap.CreateVertexList ()
            ));

        }
    }
}
