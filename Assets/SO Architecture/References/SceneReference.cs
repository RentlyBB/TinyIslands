using System;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public sealed class SceneReference : BaseReference<SceneInfo, SceneVariable> {
        public SceneReference() { }

        public SceneReference(SceneInfo value) : base(value) { }
    }
}