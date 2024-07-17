using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ScriptableObjectArchitecture {
    public class StackTraceEntry : IEquatable<StackTraceEntry> {
        private readonly bool _constructedWithValue;
        private readonly int _frameCount;

        private readonly int _id;
        private readonly string _stackTrace;
        private readonly object _value;
        private StackTraceEntry(string trace) {
            _id = Random.Range(int.MinValue, int.MaxValue);
            _stackTrace = trace;

            if (Application.isPlaying) {
                _frameCount = Time.frameCount;
            }
        }
        private StackTraceEntry(string trace, object value) {
            _value = value;
            _constructedWithValue = true;
            _id = Random.Range(int.MinValue, int.MaxValue);
            _stackTrace = trace;

            if (Application.isPlaying) {
                _frameCount = Time.frameCount;
            }
        }
        public bool Equals(StackTraceEntry other) {
            return other._id == _id;
        }

        public static StackTraceEntry Create(object obj) {
            return new StackTraceEntry(Environment.StackTrace, obj);
        }
        public static StackTraceEntry Create() {
            return new StackTraceEntry(Environment.StackTrace);
        }
        public override bool Equals(object obj) {
            if (obj == null)
                return false;

            if (obj is StackTraceEntry) {
                return Equals(obj as StackTraceEntry);
            }

            return false;
        }
        public override int GetHashCode() {
            return _id;
        }
        public override string ToString() {
            if (_constructedWithValue) {
                return string.Format("{1}   [{0}] {2}", _value == null ? "null" : _value.ToString(), _frameCount, _stackTrace);
            }
            return string.Format("{0} {1}", _frameCount, _stackTrace);
        }

        public static implicit operator string(StackTraceEntry trace) {
            return trace.ToString();
        }
    }
}