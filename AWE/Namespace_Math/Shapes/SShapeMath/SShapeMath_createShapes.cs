using System;
using System.Collections.Generic;

namespace AWE.Math {

    public static partial class SShapeMath {

        private const int REG_POLYGON_VERT_THRES = 36;

        internal static List<pair2f> CreatePolygon2DVerticies (Polygon2DTemplate template)
            => CreatePolygon2DVerticies (template, out _, out _, out _, out _);

        internal static List<pair2f> CreatePolygon2DVerticies (
            Polygon2DTemplate template,
            out pair2f center,
            out Bounds2D bounds,
            out float minimalRadius,
            out float maximalRadius
        ) {

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
                verticies = CreateBox2DVerticies (right, top, left, bottom, out center, out bounds, out minimalRadius, out maximalRadius);
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
                    out bounds,
                    out minimalRadius,
                    out maximalRadius
                );
            break;

            default:
                center = pair2f.nan;
                bounds = new Bounds2D (Single.NaN, Single.NaN, Single.NaN, Single.NaN);
                minimalRadius = Single.NaN;
                maximalRadius = Single.NaN;
            break;

            }

            return verticies;

        }

        public static List<pair2f> CreateBox2DVerticies (float right, float top, float left, float bottom)
            => CreateBox2DVerticies (right, top, left, bottom, out _, out _, out _, out _);

        public static List<pair2f> CreateBox2DVerticies (
            float right,
            float top,
            float left,
            float bottom,
            out pair2f center,
            out Bounds2D bounds,
            out float minimalRadius,
            out float maximalRadius
        ) {

            center = new pair2f (
                ((right + left) / 2f),
                ((top + bottom) / 2f)
            );
            bounds = new Bounds2D (right, top, left, bottom);
            minimalRadius = System.Math.Min (
                ((right - left) / 2f),
                ((top - bottom) / 2f)
            );
            maximalRadius = ((new pair2f (right, top) - new pair2f (left, bottom)).magnitude / 2f);

            var verticies = new List<pair2f> () {
                new pair2f (right, top),
                new pair2f (left, top),
                new pair2f (left, bottom),
                new pair2f (right, bottom)
            };

            return verticies;

        }

        public static List<pair2f> CreateRegularPolygon2DVerticies (pair2f center, int edgeCount, float radius = 1f)
            => CreateRegularPolygon2DVerticies (center, edgeCount, radius, new angle (0f, EAngleMode.Degree), out _, out _, out _);

        public static List<pair2f> CreateRegularPolygon2DVerticies (pair2f center, int edgeCount, angle startingAngle, float radius = 1f)
            => CreateRegularPolygon2DVerticies (center, edgeCount, radius, startingAngle, out _, out _, out _);

        public static List<pair2f> CreateRegularPolygon2DVerticies (pair2f center, int edgeCount, out Bounds2D bounds, float radius = 1f)
            => CreateRegularPolygon2DVerticies (center, edgeCount, radius, new angle (0f, EAngleMode.Degree), out bounds, out _, out _);

        public static List<pair2f> CreateRegularPolygon2DVerticies (
            pair2f center,
            int edgeCount,
            float radius,
            angle startingAngle,
            out Bounds2D bounds,
            out float minimalRadius,
            out float maximalRadius
        ) {

            maximalRadius = radius;

            var verticies = new List<pair2f> ();
            var incrementalAngle = (SFloatMath.DEGREE_MAX_VALUE / edgeCount);

            for (int i = 0; i < edgeCount; i++) {

                var radialAngle = new angle (
                    (startingAngle.degree + (i * incrementalAngle)),
                    EAngleMode.Degree
                );

                verticies.Add (new pair2f (
                    (center.x + (radius * SFloatMath.Cosine (radialAngle))),
                    (center.y + (radius * SFloatMath.Sine (radialAngle)))
                ));

            }

            if (edgeCount < REG_POLYGON_VERT_THRES) {

                bounds = new Bounds2D (verticies.AsReadOnly ());
                minimalRadius = (radius * SFloatMath.Cosine (new angle ((incrementalAngle / 2f), EAngleMode.Degree)));

            } else {

                bounds = new Bounds2D (
                    (center.x + radius),
                    (center.y + radius),
                    (center.x - radius),
                    (center.y - radius)
                );
                minimalRadius = radius;

            }

            return verticies;

        }
    }
}