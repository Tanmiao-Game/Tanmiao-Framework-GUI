using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// 是否显示，default无参构造是不显示
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ShowIfAttribute : ConditionAttribute {
        public ShowIfAttribute() : base(ConditionOperator.And, default(string)) { }
        public ShowIfAttribute(bool value) : base(ConditionOperator.And, value.ToString()) {}
        public ShowIfAttribute(string value) : base(ConditionOperator.And, value) { }
        public ShowIfAttribute(ConditionOperator condition, params string[] values) : base(condition, values) { }
    }
}