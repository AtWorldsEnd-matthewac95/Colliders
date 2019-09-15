using System;
using System.Collections.Generic;

namespace AWE.Math {

    internal sealed class Polygon2DTemplate {

        internal EPolygon2DType polygonType { get; set; }
        internal Dictionary<EPolygon2DParameter, ValueType> parameters { get; set; }

        internal Polygon2DTemplate () {

            this.polygonType = EPolygon2DType.None;
            this.parameters = null;

        }

        internal Polygon2DTemplate (
            EPolygon2DType polygonType,
            params KeyValuePair<EPolygon2DParameter, ValueType>[] parameters
        ) {

            this.polygonType = polygonType;

            var length = parameters.Length;
            this.parameters = new Dictionary<EPolygon2DParameter, ValueType> (length);

            for (int i = 0; i < length; i++) {

                var pair = parameters[i];
                this.parameters[pair.Key] = pair.Value;

            }
        }

        internal Polygon2DTemplate SetAsBox (
            float right,
            float top,
            float left,
            float bottom
        ) {

            this.polygonType = EPolygon2DType.Box;
            this.parameters = new Dictionary<EPolygon2DParameter, ValueType> () {
                { EPolygon2DParameter.right, right },
                { EPolygon2DParameter.top, top },
                { EPolygon2DParameter.left, left },
                { EPolygon2DParameter.bottom, bottom }
            };

            return this;

        }

        internal Polygon2DTemplate SetAsRegular (pair2f center, int edgeCount, float radius = 1f) =>
            this.SetAsRegular (center, edgeCount, radius, new angle (0f, EAngleMode.Degree));

        internal Polygon2DTemplate SetAsRegular (
            pair2f center,
            int edgeCount,
            float radius,
            angle startingAngle
        ) {

            this.polygonType = EPolygon2DType.Regular;
            this.parameters = new Dictionary<EPolygon2DParameter, ValueType> () {
                { EPolygon2DParameter.center, center },
                { EPolygon2DParameter.edgeCount, edgeCount },
                { EPolygon2DParameter.radius, radius },
                { EPolygon2DParameter.startingAngle, startingAngle }
            };

            return this;

        }
    }
}