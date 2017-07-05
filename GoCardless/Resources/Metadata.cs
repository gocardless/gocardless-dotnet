using System;
using System.Collections;
using System.Collections.Generic;

namespace GoCardless.Resources
{
    /// <summary>
    /// Key-value store of custom data. Up to 3 keys are permitted, with key names up to 50 characters and values up to 500 characters
    /// </summary>
    public class Metadata : IDictionary<string, string>
    {
        public void Add(string key, string value)
        {
            if (key == null || key.Length > 50) throw new ArgumentException(nameof(key) + " is required and must be less than 50 characters");
            if (value == null || value.Length > 500) throw new ArgumentException(nameof(value) + " is required and must be less than 500 characters");
            if (this._items.Count >= 3) throw new InvalidOperationException("Only 3 keys are permitted");
            _items.Add(key, value);
        }

        private readonly IDictionary<string, string> _items = new Dictionary<string, string>();
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _items.GetEnumerator();
        public void Add(KeyValuePair<string, string> item) => this.Add(item.Key, item.Value);
        public void Clear() => _items.Clear();
        public bool Contains(KeyValuePair<string, string> item) => _items.Contains(item);
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
        public bool Remove(KeyValuePair<string, string> item) => _items.Remove(item);
        public int Count => _items.Count;
        public bool IsReadOnly => _items.IsReadOnly;
        

        public bool ContainsKey(string key) => _items.ContainsKey(key);

        public bool Remove(string key) => _items.Remove(key);

        public bool TryGetValue(string key, out string value) => _items.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        public string this[string key]
        {
            get => _items[key];
            set
            {
                this.Remove(key);
                this.Add(key, value);
            }
        }

        public ICollection<string> Keys => _items.Keys;

        public ICollection<string> Values => _items.Values;

        
 
    }
}
