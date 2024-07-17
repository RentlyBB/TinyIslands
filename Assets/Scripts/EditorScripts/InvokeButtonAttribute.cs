using System;
using UnityEngine;

namespace EditorScripts {
    [AttributeUsage(AttributeTargets.Method)]
    public class InvokeButtonAttribute : PropertyAttribute {
        public readonly string ButtonName;

        public InvokeButtonAttribute(string buttonName = null) {
            ButtonName = buttonName;
        }
    }
}