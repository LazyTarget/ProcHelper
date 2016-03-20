using System.Collections;
using System.Collections.Generic;

namespace Remotus.Base
{
    public class ParameterCollection : IParameterCollection
    {
        private readonly IDictionary<string, IParameter> _data = new Dictionary<string, IParameter>();

        
        // todo: support multiple parameters with same key?, for multiple types...

        #region IDictionary

        public IEnumerator<KeyValuePair<string, IParameter>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _data).GetEnumerator();
        }

        public void Add(KeyValuePair<string, IParameter> item)
        {
            _data.Add(item);
        }

        public void Clear()
        {
            _data.Clear();
        }

        public bool Contains(KeyValuePair<string, IParameter> item)
        {
            return _data.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, IParameter>[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, IParameter> item)
        {
            return _data.Remove(item);
        }

        public int Count
        {
            get { return _data.Count; }
        }

        public bool IsReadOnly
        {
            get { return _data.IsReadOnly; }
        }

        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }

        public void Add(string key, IParameter value)
        {
            _data.Add(key, value);
        }

        public bool Remove(string key)
        {
            return _data.Remove(key);
        }

        public bool TryGetValue(string key, out IParameter value)
        {
            return _data.TryGetValue(key, out value);
        }

        public IParameter this[string key]
        {
            get { return _data[key]; }
            set { _data[key] = value; }
        }

        public ICollection<string> Keys
        {
            get { return _data.Keys; }
        }

        public ICollection<IParameter> Values
        {
            get { return _data.Values; }
        }

        #endregion

    }
}