using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// Tag
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class TagAttribute : PropertyAttribute {
        public TagAttribute() { }
    }
}