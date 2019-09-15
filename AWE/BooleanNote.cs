using System;
using System.Text;

namespace AWE {

    public struct BooleanNote {

        public static implicit operator bool (BooleanNote b) => b.value;

        public bool value { get; }
        public string note { get; }

        public BooleanNote (bool value, string note = "") : this () {

            this.value = value;
            this.note = note;

        }

        public override string ToString () {

            var builder = new StringBuilder (this.value.ToString ());

            if (!String.IsNullOrWhiteSpace (this.note)) {

                builder.Append (" - ")
                    .Append (this.note);

            }

            return builder.ToString ();

        }
    }
}
