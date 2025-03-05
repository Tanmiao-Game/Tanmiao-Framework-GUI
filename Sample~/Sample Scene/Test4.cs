using UnityEngine;

namespace Akatsuki.Framework.GUI.Test {
    public class Test4 : MonoBehaviour {
        public bool enableForValue1;

        [field: SerializeField]
        public bool EnableForValue2 { get; private set; }

        public bool EnableForValue3() {
            return EnableForValue2;
        }

        [EnableIf(nameof(enableForValue1))]
        [SelectionPath]
        public string value1;

        [EnableIf(ConditionOperator.Or, nameof(enableForValue1), nameof(EnableForValue2))]
        public int value2;

        [HelpBox("For non-param method with bool return")]
        [EnableIf(nameof(EnableForValue3))]
        [Expendable]
        public ScriptableObject value3;

        [Space(10)]
        public bool showForValue1;

        [field: SerializeField]
        public bool ShowForValue2 { get; private set; }

        public bool ShowForValue3() {
            return ShowForValue2;
        }

        [ShowIf(nameof(showForValue1))]
        [SelectionPath]
        public string value4;

        [ShowIf(ConditionOperator.And, nameof(showForValue1), nameof(ShowForValue2))]
        public int value5;

        [HelpBox("For non-param method with bool return")]
        [ShowIf(nameof(ShowForValue3))]
        [Expendable]
        public ScriptableObject value6;
    }
}
