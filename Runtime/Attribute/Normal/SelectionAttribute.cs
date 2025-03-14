using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// <para>string 选择面板</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class SelectionAttribute : PropertyAttribute {
        /// <summary>
        /// 类型
        /// </summary>
        public Type type { get; protected set; }

        /// <summary>
        /// 是否可以自己编辑
        /// </summary>
        public bool editable { get; protected set; }

        /// <summary>
        /// <para>类型获得抽象类或接口</para>
        /// <para>直接获得变量类型，所以Type已经不需要了</para>
        /// </summary>
        public SelectionAttribute() {}

        /// <summary>
        /// 类型中获得常量请注意！
        /// </summary>
        /// <param name="type"></param>
        public SelectionAttribute(Type type) : this(type, false) {}
        
        /// <summary>
        /// 类型中获得常量请注意！
        /// </summary>
        /// <param name="type"></param>
        /// <param name="editable">是否可以自己编辑</param>
        /// <returns></returns>
        public SelectionAttribute(Type type, bool editable) {
            this.type = type;
            this.editable = editable;
        }
    }
}