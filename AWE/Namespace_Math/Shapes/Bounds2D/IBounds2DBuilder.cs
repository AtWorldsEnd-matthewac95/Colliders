using System.Collections.ObjectModel;

namespace AWE.Math {

    public interface IBounds2DBuilder<TBounds2D> : IGradient<TBounds2D> where TBounds2D : IBounds2D {

        bool isEmpty { get; }

        TBounds2D ToBounds ();
        IBounds2DBuilder<TBounds2D> Add (TBounds2D bounds);
        IBounds2DBuilder<TBounds2D> AddAll (ReadOnlyCollection<TBounds2D> bounds);
        IBounds2DBuilder<TBounds2D> Clear ();

    }

    public interface IBounds2DBuilder : IBounds2DBuilder<Bounds2D> {}

}
