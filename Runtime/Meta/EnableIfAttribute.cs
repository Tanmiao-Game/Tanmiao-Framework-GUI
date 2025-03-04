using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// 是否可编辑，default无参构造是不可编辑
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class EnableIfAttribute : PropertyAttribute {
        public string[] Values { get; private set; }
        public ConditionOperator Condition { get; private set; }

        public EnableIfAttribute() : this(ConditionOperator.And, default(string)) { }
        public EnableIfAttribute(bool value) : this(ConditionOperator.And, value.ToString()) {}
        public EnableIfAttribute(string value) : this(ConditionOperator.And, value) { }
        public EnableIfAttribute(ConditionOperator condition, params string[] values) {
            Condition = condition;
            Values = values;
        }
    }
}