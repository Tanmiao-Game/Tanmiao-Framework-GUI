using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// 方法按钮
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MethodAttribute : PropertyAttribute {
        public string MethodName { get; private set; }
        public ConditionMode Mode { get; private set; }

        public MethodAttribute(ConditionMode mode = ConditionMode.Always) : this(default, mode) {}
        public MethodAttribute(string methodName, ConditionMode mode = ConditionMode.Always) {
            MethodName = methodName;
            Mode = mode;
        }
    }
}