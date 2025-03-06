using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// Sorting Layer
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SortingLayerAttribute : PropertyAttribute {
        public SortingLayerAttribute() { }
    }
}