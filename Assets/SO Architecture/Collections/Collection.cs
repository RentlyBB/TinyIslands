using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    public class Collection<T> : BaseCollection, IEnumerable<T> {
        [SerializeField]
        private List<T> _list = new();

        public new T this[int index] {
            get => _list[index];
            set => _list[index] = value;
        }

        public override IList List => _list;

        public override Type Type => typeof(T);

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator() {
            return _list.GetEnumerator();
        }

        public void Add(T obj) {
            if (!_list.Contains(obj))
                _list.Add(obj);
        }

        public void Remove(T obj) {
            if (_list.Contains(obj))
                _list.Remove(obj);
        }

        public void Clear() {
            _list.Clear();
        }

        public bool Contains(T value) {
            return _list.Contains(value);
        }

        public int IndexOf(T value) {
            return _list.IndexOf(value);
        }

        public void RemoveAt(int index) {
            _list.RemoveAt(index);
        }

        public void Insert(int index, T value) {
            _list.Insert(index, value);
        }

        public override string ToString() {
            return "Collection<" + typeof(T) + ">(" + Count + ")";
        }

        public T[] ToArray() {
            return _list.ToArray();
        }
    }
}