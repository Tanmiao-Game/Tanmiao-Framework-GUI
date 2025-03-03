using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// 按钮
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MethodAttribute : PropertyAttribute {
        public string MethodName { get; private set; }

        public MethodAttribute() : this(default) {}
        public MethodAttribute(string methodName) {
            MethodName = methodName;
        }
    }
}