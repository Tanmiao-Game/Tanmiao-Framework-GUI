using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// 条件特性
    /// </summary>
    public abstract class ConditionAttribute : PropertyAttribute {
        public string[] Values { get; protected set; }
        public ConditionOperator Condition { get; private set; }

        public ConditionAttribute(ConditionOperator condition, params string[] values) {
            Condition = condition;
            Values = values;
        }
    }
}