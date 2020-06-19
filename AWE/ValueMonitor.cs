using System;

namespace AWE {

    public sealed class ValueMonitor<T> {

        public static ValueMonitor<TStatic> Create<TStatic> (TStatic value) => new ValueMonitor<TStatic> (value);

        public event Action<T> OnSet;
        public event Action<T> OnSetInitialValue;
        public event Action<T, int> OnSetDifferentValue;
        public event Action<T, int> OnSetSameValue;
        public event Action<T, int, int> OnClear;

        private T _value;

        public bool isSet { get; private set; }
        public int transitionCount { get; private set; }
        public int consecutiveCount { get; private set; }

        public T value {

            get => this._value;
            set => this.Set (value);

        }

        public ValueMonitor () {

            this.isSet = false;
            this._value = default;
            this.transitionCount = 0;
            this.consecutiveCount = 0;

        }

        public ValueMonitor (T value) {

            this.isSet = true;
            this._value = value;
            this.transitionCount = 0;
            this.consecutiveCount = 0;

        }

        public bool Set (T value) {

            this.OnSet (value);

            var isDifferentValue = false;

            if (this.isSet) {

                isDifferentValue = !this._value.Equals (value);

                if (isDifferentValue) {

                    this.consecutiveCount = 0;
                    this.transitionCount++;
                    this.OnSetDifferentValue (value, this.transitionCount);

                } else {

                    this.consecutiveCount++;
                    this.OnSetSameValue (value, this.consecutiveCount);

                }

            } else {

                this.isSet = true;
                this.OnSetInitialValue (value);

            }

            this._value = value;
            return isDifferentValue;

        }

        public void Clear () {

            if (this.isSet) {

                this.OnClear (this.value, this.transitionCount, this.consecutiveCount);
                this.isSet = false;
                this._value = default;
                this.transitionCount = 0;
                this.consecutiveCount = 0;

            }
        }
    }
}
