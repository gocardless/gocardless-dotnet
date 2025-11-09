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
        /// <summary>Add a new key-value pair</summary>
        public void Add(string key, string value)
        {
            if (key == null || key.Length > 50)
                throw new ArgumentException(
                    nameof(key) + " is required and must be less than 50 characters"
                );
            if (value == null || value.Length > 500)
                throw new ArgumentException(
                    nameof(value) + " is required and must be less than 500 characters"
                );
            if (this._items.Count >= 3)
                throw new InvalidOperationException("Only 3 keys are permitted");
            _items.Add(key, value);
        }

        private readonly IDictionary<string, string> _items = new Dictionary<string, string>();

        /// <summary>Returns an enumerator for the metadata collection</summary>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _items.GetEnumerator();

        /// <summary>Add a new key-value pair</summary>
        public void Add(KeyValuePair<string, string> item) => this.Add(item.Key, item.Value);

        /// <summary>Remove all key-value pairs</summary>
        public void Clear() => _items.Clear();

        /// <summary>Check whether a key-value pair exists</summary>
        public bool Contains(KeyValuePair<string, string> item) => _items.Contains(item);

        /// <summary>Copies the elements of the collection to an array, starting at the specified array index</summary>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) =>
            _items.CopyTo(array, arrayIndex);

        /// <summary>Remove a key-value pair by key-value pair</summary>
        public bool Remove(KeyValuePair<string, string> item) => _items.Remove(item);

        /// <summary>Returns the count of metadata items</summary>
        public int Count => _items.Count;

        /// <summary>Gets a value indicating whether the collection is read-only</summary>
        public bool IsReadOnly => _items.IsReadOnly;

        /// <summary>Check whether a key exists</summary>
        public bool ContainsKey(string key) => _items.ContainsKey(key);

        /// <summary>Remove a key-value pair by key</summary>
        public bool Remove(string key) => _items.Remove(key);

        /// <summary>Get the value associated with the specified key</summary>
        public bool TryGetValue(string key, out string value) => _items.TryGetValue(key, out value);

        /// <summary>Returns an enumerator for the metadata collection</summary>
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

        /// <summary>Get a collection of the metadata keys</summary>
        public ICollection<string> Keys => _items.Keys;

        /// <summary>Get a collection of the metadata values</summary>
        public ICollection<string> Values => _items.Values;
    }
}
