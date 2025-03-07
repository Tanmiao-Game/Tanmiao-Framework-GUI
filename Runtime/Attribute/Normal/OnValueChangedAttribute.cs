using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// Valued changed callback, editor only
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class OnValueChangedAttribute : PropertyAttribute {
        public string MethodName { get; private set; }

        public OnValueChangedAttribute(string methodName) {
            MethodName = methodName;
        }
    }
}