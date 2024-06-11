using UnityEngine;
using System;

namespace EditorScripts {
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class InvokeButtonAttribute : PropertyAttribute {
        public readonly string ButtonName;

        public InvokeButtonAttribute(string buttonName = null) {
            this.ButtonName = buttonName;
        }
    }
}