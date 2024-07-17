using System;
using UnityEngine;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class AudioClipReference : BaseReference<AudioClip, AudioClipVariable> {
        public AudioClipReference() { }
        public AudioClipReference(AudioClip value) : base(value) { }
    }
}