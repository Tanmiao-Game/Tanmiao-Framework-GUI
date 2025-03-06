using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// Layer
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class LayerAttribute : PropertyAttribute {
        public LayerAttribute() { }
    }
}