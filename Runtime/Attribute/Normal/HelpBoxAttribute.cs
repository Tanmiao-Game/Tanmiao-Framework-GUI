using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// 帮助提示框
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HelpBoxAttribute : PropertyAttribute {
        public string Message { get; private set; }
        public HelpBoxMessageType Type { get; private set; }

        public HelpBoxAttribute(string message) : this(message, HelpBoxMessageType.Info) { }

        public HelpBoxAttribute(string message, HelpBoxMessageType type) {
            Message = message;
            Type = type;
        }
    }
}