using System.Collections.Generic;

namespace AWE.Moving.Collisions {

    public class ColliderProperties {

        private readonly Dictionary<string, string> map;

        public bool isSolid { get; set; }

        public string this [string key] {

            get => this.GetMappableProperty (key).note;
            set => this.SetMappableProperty (key, value);

        }

        public ColliderProperties (bool isSolid, params KeyValuePair<string, string>[] mappableProperties) : this (mappableProperties, isSolid) {}
        public ColliderProperties (IEnumerable<KeyValuePair<string, string>> mappableProperties, bool isSolid) {

            this.isSolid = isSolid;
            this.map = new Dictionary<string, string> ();

            foreach (var pair in mappableProperties) {

                this.map[pair.Key] = pair.Value;

            }
        }

        public BooleanNote GetMappableProperty (string key) {

            var property = new BooleanNote (false);

            if (this.map.ContainsKey (key)) {

                property = new BooleanNote (true, this.map[key]);

            }

            return property;

        }

        public BooleanNote SetMappableProperty (string key, string value) {

            var oldValue = (
                this.map.ContainsKey (key)
                ? new BooleanNote (true, this.map[key])
                : new BooleanNote (false)
            );

            this.map[key] = value;

            return oldValue;

        }

        public virtual ReadOnlyColliderProperties AsReadOnly () => new ReadOnlyColliderProperties (this);

    }
}
