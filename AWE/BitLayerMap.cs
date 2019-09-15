using System.Collections.Generic;

namespace AWE {

    public class BitLayerMap {

        public const int MAX_LAYER_COUNT = 64;
        public const ulong ALL = 0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111;
        public const ulong NONE = ~ALL;

        protected const ulong ONE = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001;

        protected readonly Dictionary<string, int> layers;

        public string name { get; }
        public int count { get; protected set; }

        public virtual ulong this [string layerName] {

            get {

                this.layers.TryGetValue (layerName, out int position);
                return (ONE << position);

            }
        }

        public BitLayerMap (string name = null) {

            if ((name != null) && (name.Length > 0)) {

                this.name = name;

            } else {

                this.name = "Unnamed Bit Layer Mapping";

            }

            this.layers = new Dictionary<string, int> ();
            this.count = 0;

        }

        public ulong Add (string layerName) {

            var value = NONE;

            if (this.count < MAX_LAYER_COUNT) {

                value = (ONE << this.count);
                this.layers.Add (layerName, this.count);
                this.count++;

            }

            return value;

        }

        public ulong Rename (string oldLayerName, string newLayerName) {

            var value = NONE;

            if (this.layers.ContainsKey (oldLayerName)) {

                var position = this.layers[oldLayerName];
                this.layers.Remove (oldLayerName);
                this.layers.Add (newLayerName, position);

                value = (ONE << position);

            } else if (this.count < MAX_LAYER_COUNT) {

                value = this.Add (newLayerName);

            }

            return value;

        }

        public ulong Combine (params string[] layers) {

            var value = NONE;

            for (int i = 0; i < layers.Length; i++) {

                value |= this[layers[i]];

            }

            return value;

        }

        // TODO - Override ToString
    }
}