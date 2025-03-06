using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// 是否可编辑，default无参构造是不可编辑
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class EnableIfAttribute : ConditionAttribute {
        public EnableIfAttribute() : base(ConditionOperator.And, default(string)) { }
        public EnableIfAttribute(bool value) : base(ConditionOperator.And, value.ToString()) {}
        public EnableIfAttribute(string value) : base(ConditionOperator.And, value) { }
        public EnableIfAttribute(ConditionOperator condition, params string[] values) : base(condition, values) { }
    }
}