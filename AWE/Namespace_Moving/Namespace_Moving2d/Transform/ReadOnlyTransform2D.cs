using AWE.Math;

namespace AWE.Moving.Moving2D {

    public class ReadOnlyTransform2D : ReadOnlyTransform<float, pair2f, angle, pair2f, Transformation2D, Transform2DState>, IReadOnlyTransform2D {

        private readonly ATransform2D transform;

        public event DTransform2DUpdate OnAnyChange {

            add => this.transform.OnAnyChange += value;
            remove => this.transform.OnAnyChange -= value;

        }
        
        public event DTransform2DTransformation OnTransformation {

            add => this.transform.OnTransformation += value;
            remove => this.transform.OnTransformation -= value;

        }

        public event DTransform2DTranslation OnTranslation {

            add => this.transform.OnTranslation += value;
            remove => this.transform.OnTranslation -= value;

        }

        public event DTransform2DRotation OnRotation {

            add => this.transform.OnRotation += value;
            remove => this.transform.OnRotation -= value;

        }

        public event DTransform2DDilation OnDilation {

            add => this.transform.OnDilation += value;
            remove => this.transform.OnDilation -= value;

        }

        public ReadOnlyTransform2D (ATransform2D transform) : base (transform) => this.transform = transform;

        public void AddListener (ITransform2DListener listener) => this.transform.AddListener (listener);
        public void AddListener (ATransform2DListener listener) => this.transform.AddListener (listener);

    }
}