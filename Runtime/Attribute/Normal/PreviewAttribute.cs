using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// Preview from unity-folder icon
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class PreviewAttribute : PropertyAttribute {
        public PreviewAttribute() { }
    }
}