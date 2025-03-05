using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Akatsuki.Framework.GUI.Test {
    public class Test5 : MonoBehaviour {
        public bool showForValue1;

        [field: SerializeField]
        public bool ShowForValue2 { get; private set; }

        public bool ShowForValue3() {
            return ShowForValue2;
        }

        [ShowIf(nameof(showForValue1))]
        [SelectionPath]
        public string value1;

        [ShowIf(ConditionOperator.And, nameof(showForValue1), nameof(ShowForValue2))]
        public int value2;

        [HelpBox("For non-param method with bool return")]
        [ShowIf(nameof(ShowForValue3))]
        [Expendable]
        public ScriptableObject value3;
    }
}
