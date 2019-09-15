using System;
using System.Collections.Generic;

namespace AWE.Math {

    public static partial class SShapeMath {

        private const int REG_POLYGON_VERT_THRES = 36;

        internal static List<pair2f> CreatePolygon2DVerticies (Polygon2DTemplate template)
            => CreatePolygon2DVerticies (template, out _, out _);

        internal static List<pair2f> CreatePolygon2DVerticies (Polygon2DTemplate template, out pair2f center, out Bounds2D bounds) {

            /*
             TODO - What should this function do if the user gives an unsupported polygon type?

             1. Return an empty list?
             2. Return null?
             3. Throw an exception?

             Currently we implement 2, but another implementation may be better.
             */

            List<pair2f> verticies = null;

            switch (template.polygonType) {

            case EPolygon2DType.Rectangle:
                var right = (float)(template.parameters[EPolygon2DParameter.right]);
                var top = (float)(template.parameters[EPolygon2DParameter.top]);
                var left = (float)(template.parameters[EPolygon2DParameter.left]);
                var bottom = (float)(template.parameters[EPolygon2DParameter.bottom]);
                verticies = CreateBox2DVerticies (right, top, left, bottom, out center, out bounds);
            break;

            case EPolygon2DType.Regular:
                center = (pair2f)(template.parameters[EPolygon2DParameter.center]);

                verticies = CreateRegularPolygon2DVerticies (
                    center,
                    (int)(template.parameters[EPolygon2DParameter.edgeCount]),
                    (
                        template.parameters.ContainsKey (EPolygon2DParameter.radius)
                        ? (float)(template.parameters[EPolygon2DParameter.radius])
                        : 1f
                    ),
                    (
                        template.parameters.ContainsKey (EPolygon2DParameter.startingAngle)
                        ? (angle)(template.parameters[EPolygon2DParameter.startingAngle])
                        : new angle (0f, EAngleMode.Degree)
                    ),
                    out bounds
                );
            break;

            default:
                center = pair2f.nan;
                bounds = new Bounds2D (Single.NaN, Single.NaN, Single.NaN, Single.NaN);
            break;

            }

            return verticies;

        }

        public static List<pair2f> CreateBox2DVerticies (float right, float top, float left, float bottom)
            => CreateBox2DVerticies (right, top, left, bottom, out _, out _);

        public static List<pair2f> CreateBox2DVerticies (
            float right,
            float top,
            float left,
            float bottom,
            out pair2f center,
            out Bounds2D bounds
        ) {

            center = new pair2f (
                ((right + left) / 2f),
                ((top + bottom) / 2f)
            );
            bounds = new Bounds2D (right, top, left, bottom);

            var verticies = new List<pair2f> () {
                new pair2f (right, top),
                new pair2f (left, top),
                new pair2f (left, bottom),
                new pair2f (right, bottom)
            };

            return verticies;

        }

        public static List<pair2f> CreateRegularPolygon2DVerticies (pair2f center, int edgeCount, float radius = 1f)
            => CreateRegularPolygon2DVerticies (center, edgeCount, radius, new angle (0f, EAngleMode.Degree), out _);

        public static List<pair2f> CreateRegularPolygon2DVerticies (pair2f center, int edgeCount, angle startingAngle, float radius = 1f)
            => CreateRegularPolygon2DVerticies (center, edgeCount, radius, startingAngle, out _);

        public static List<pair2f> CreateRegularPolygon2DVerticies (pair2f center, int edgeCount, out Bounds2D bounds, float radius = 1f)
            => CreateRegularPolygon2DVerticies (center, edgeCount, radius, new angle (0f, EAngleMode.Degree), out bounds);

        public static List<pair2f> CreateRegularPolygon2DVerticies (
            pair2f center,
            int edgeCount,
            float radius,
            angle startingAngle,
            out Bounds2D bounds
        ) {

            var verticies = new List<pair2f> ();

            for (int i = 0; i < edgeCount; i++) {

                var radialAngle = new angle (
                    (startingAngle.degree + ((i * SFloatMath.DEGREE_MAX_VALUE) / edgeCount)),
                    EAngleMode.Degree
                );

                verticies.Add (new pair2f (
                    (center.x + (radius * SFloatMath.Cosine (radialAngle))),
                    (center.y + (radius * SFloatMath.Sine (radialAngle)))
                ));

            }

            if (edgeCount < REG_POLYGON_VERT_THRES) {

                bounds = new Bounds2D (verticies.AsReadOnly ());

            } else {

                bounds = new Bounds2D (
                    (center.x + radius),
                    (center.y + radius),
                    (center.x - radius),
                    (center.y - radius)
                );

            }

            return verticies;

        }
    }
}