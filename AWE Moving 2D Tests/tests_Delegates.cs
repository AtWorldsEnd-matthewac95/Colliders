using Xunit;
using System;
using System.Diagnostics;

namespace AWE.UnitTests.XUnit {

    public class DummyValue {

        public int number { get; set; }
        public string message { get; set; }
        public object other { get; set; }

        public override string ToString () => String.Format ("{0}: {1}, {2} -> {3}", this.number, this.message, this.other.GetType (), this.other);

    }

    public delegate void DTest (DummyValue d);

    public class TestEventHolder {

        public event DTest OnTrigger;

        public int TriggerEvent (DummyValue d) {

            var r = 0;

            if (OnTrigger != null) {

                this.OnTrigger (d);
                r = 1;

            }

            return r;

        }

        public DTest GimmieDatDelegate () => this.OnTrigger;

    }

    public class DelegateAccumulator {

        public static DelegateAccumulator operator + (DelegateAccumulator ac, DTest t) {

            ac.Add (t);
            return ac;

        }

        private DTest accumulated;

        public void RunAccumulated (DummyValue d) => this.accumulated?.Invoke (d);

        public void Add (DTest t) => accumulated += t;

    }

    public class tests_Delegates {

        private TestEventHolder Initialize (DTest t) {

            var ev = new TestEventHolder ();
            ev.OnTrigger += t;
            return ev;

        }

        private void ExampleMethod (DummyValue d) {

            d.other = new DummyValue { number = -1, message = "This is an example", other = new DummyValue { number = -2, message = "How deep does this gooooooooo", other = 0.999999999999999999999999999999999999999999999999999999999f } };

        }

        [Theory]
        [InlineData (1, "Hi")]
        [InlineData (5, "Hello")]
        [InlineData (15, "Fourteen was simply one short I'm afraid")]
        [InlineData (6, "Let's try with a period and space at the end. ")]
        [InlineData (10, "Are we human??? Or are we dancer?????")]
        [InlineData (22, "Twenty less than fourty-two. ")]
        public void SingleHolderTest (int number, string message) {

            var d = new DummyValue { number = number, message = message };
            DTest t = null;
            var ev = this.Initialize (t);

            ev.TriggerEvent (d);

            t += dd => Debug.WriteLine (String.Format ("{0}: {1}", dd.number, dd.message));

            ev.TriggerEvent (d);

            t += dd => Debug.WriteLine (String.Format ("{0}", (dd.number + dd.message.Length)));

            ev.TriggerEvent (d);

            t += this.ExampleMethod;
            t += dd => Debug.WriteLine ((dd.other == null) ? "other is empty. " : String.Format ("other evaluates to: {0}", dd.other));

            ev.TriggerEvent (d);

            Assert.NotNull (t);

        }

        [Theory]
        [InlineData (1, "Hi")]
        [InlineData (5, "Hello")]
        [InlineData (15, "Fourteen was simply one short I'm afraid")]
        [InlineData (6, "Let's try with a period and space at the end. ")]
        [InlineData (10, "Are we human??? Or are we dancer?????")]
        [InlineData (22, "Twenty less than fourty-two. ")]
        public void ComparisonTest (int number, string message) {

            var d = new DummyValue { number = number, message = message };
            DTest t = null;
            var ev = this.Initialize (t);

            ev.TriggerEvent (d);

            t += dd => Debug.WriteLine (String.Format ("{0}: {1}", dd.number, dd.message));

            ev.TriggerEvent (d);

            t += dd => Debug.WriteLine (String.Format ("{0}", (dd.number + dd.message.Length)));

            ev.TriggerEvent (d);

            t += this.ExampleMethod;
            t += dd => Debug.WriteLine ((dd.other == null) ? "other is empty. " : String.Format ("other evaluates to: {0}", dd.other));

            var ev2 = this.Initialize (t);
            ev.OnTrigger += ev2.GimmieDatDelegate ();

            Assert.True (ev.TriggerEvent (d) == ev2.TriggerEvent (d));

        }

        [Theory]
        [InlineData (1, "Hi")]
        [InlineData (5, "Hello")]
        [InlineData (15, "Fourteen was simply one short I'm afraid")]
        [InlineData (6, "Let's try with a period and space at the end. ")]
        [InlineData (10, "Are we human??? Or are we dancer?????")]
        [InlineData (22, "Twenty less than fourty-two. ")]
        public void AccumulatorTest (int number, string message) {

            var d = new DummyValue { number = number, message = message };
            var ac = new DelegateAccumulator ();
            var ev = this.Initialize (ac.RunAccumulated);

            ev.TriggerEvent (d);

            ac += dd => Debug.WriteLine (String.Format ("{0}: {1}", dd.number, dd.message));

            ev.TriggerEvent (d);

            ac += dd => Debug.WriteLine (String.Format ("{0}", (dd.number + dd.message.Length)));

            ev.TriggerEvent (d);

            ac += this.ExampleMethod;
            ac += dd => Debug.WriteLine ((dd.other == null) ? "other is empty. " : String.Format ("other evaluates to: {0}", dd.other));

            Assert.True (ev.TriggerEvent (d) > 0);

        }
    }
}