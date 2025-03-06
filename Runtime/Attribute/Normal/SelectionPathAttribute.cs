using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// <para>string 路径面板</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class SelectionPathAttribute : PropertyAttribute {
        /// <summary>
        /// 路径选择特性
        /// </summary>
        public SelectionPathAttribute() { }
    }
}