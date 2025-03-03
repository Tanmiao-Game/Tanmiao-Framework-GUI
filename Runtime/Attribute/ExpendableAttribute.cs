using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// UnityEngine.Object 面板扩展
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ExpendableAttribute : PropertyAttribute {
        public ExpendableAttribute() { }
    }
}