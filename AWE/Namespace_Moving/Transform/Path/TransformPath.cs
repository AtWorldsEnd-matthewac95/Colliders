using System.Collections.Generic;
using AWE.Math.FloatExtensions;

/*
 * TODO
 *
 * Anchor points definitely need to be defined in their own class.
 *
 * Current unaddressed problem:
 *     If the path is intended to go in infinitely, for instance a circle,
 *     anchor points should probably be defined in a periodic manner so any
 *     indefinite stretch of the path can have anchor points.
 */
namespace AWE.Moving {

    public class TransformPath<TTransformState> where TTransformState : ITransformState {

        protected readonly DTransformPath<TTransformState> path;

        /*
         * TODO
         *
         *
         * The transform path pipeline should be redesigned as follows:
         *
         *
         *     1. A TransformPath object provides a TransformPathAnchors object, giving the **default anchors** of that path.
         *
         *    2A. The TransformPathAnchors object can then be modified by either requesting more path samples to make new anchor points,
         *        or by adding interpolations between existing anchors.
         *        The latter would have to be done at the ITransformState implementation level.
         *
         *    3A. The TransformPathAnchors object provides a TransformPathCursor, which gets the anchor points from the anchors object.
         *
         * !! 2B. ALTERNATIVE TO 2A, the TransformPath will also be able to provide an anchorless TransformPathCursor.
         *        This path's anchor object would be null, and therefore would have no anchors.
         *
         *
         * Creating the TransformPathAnchors object allows the problems outlined in this file to be tackled in an organized manner.
         *     Problems such as algorithmically determining anchors for loops and infinite paths, and anchor modifications.
         *
         * It also allows the anchors to be more freely customized to user needs while still putting some of the anchor groundwork in
         *     the path creator's hands, as is sensible since they should know best what anchor points will work in their path.
         */
        protected List<WeightedTransformState<TTransformState>> anchors;

        public float defaultCursorSpeed { get; protected set; }

        public TTransformState this [float position] => this.GetState (position);

        public TransformPath (DTransformPath<TTransformState> path, float defaultCursorSpeed = 0f) : this (
            path,
            new float[] {},
            defaultCursorSpeed,
            sortAnchors: false
        ) {
        }

        public TransformPath (
            DTransformPath<TTransformState> path,
            IEnumerable<float> anchorPositions,
            float defaultCursorSpeed = 0f,
            bool sortAnchors = true
        ) {

            this.path = path;
            this.anchors = new List<WeightedTransformState<TTransformState>> ();
            this.defaultCursorSpeed = defaultCursorSpeed;

            foreach (var position in anchorPositions) {

                this.anchors.Add (new WeightedTransformState<TTransformState> (path (position), position));

            }

            if (sortAnchors) {

                this.anchors.Sort ((a, b) => a.weight.CompareTo (b.weight));

            }
        }

        public TTransformState GetState (float position) => this.path (position);

        /*
         * TODO
         *
         * What if the path is supposed to loop??? This class & function need a revisit to account for this case.
         *
         * Another problem:
         *     If the path is infinite (say parabola) and includeEndpoints = true, BOTH endpoints should
         *     be returned REGARDLESS if both endpoints exceed the furthest anchor.
         */
        public List<WeightedTransformState<TTransformState>> FindAnchorsInRange (float min, float max, bool includeEndpoints = true) {

            var anchors = new List<WeightedTransformState<TTransformState>> ();
            var reachedMin = false;

            for (int i = 0; i < this.anchors.Count; i++) {

                var anchor = this.anchors[i];

                if (anchor.weight >= min) {

                    if (anchor.weight > max) {

                        if (includeEndpoints) {

                            anchors.Add (new WeightedTransformState<TTransformState> (this.path (max), max));

                        }

                        break;

                    } else if ((max - anchor.weight).IsNegligible ()) {

                        anchors.Add (anchor.Clone ());
                        break;

                    } else if (reachedMin) {

                        anchors.Add (anchor.Clone ());

                    } else {

                        // Notice the !. If position - min is NOT negligible...
                        if (includeEndpoints && !(anchor.weight - min).IsNegligible ()) {

                            anchors.Add (new WeightedTransformState<TTransformState> (this.path (min), min));

                        }

                        anchors.Add (anchor.Clone ());
                        reachedMin = true;

                    }

                } else if (reachedMin) {

                    // This would be a very weird case, it's not clear what the user was intending. Just end the loop.
                    break;

                }
            }

            return anchors;

        }

        public TransformPathCursor<TTransformState> CreateCursor (float speed = 0f, float position = 0f, bool evaluateCurrent = false) {

            if (speed <= 0f) {

                speed = this.defaultCursorSpeed;

            }

            return new TransformPathCursor<TTransformState> (this, speed, position, evaluateCurrent);

        }
    }
}