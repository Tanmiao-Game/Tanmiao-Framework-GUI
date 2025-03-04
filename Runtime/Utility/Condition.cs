using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    public enum ConditionMode {
        Always,
        Editor,
        Play,
    }

    public enum ConditionOperator {
        And,
        Or,
    }

    public static class ConditionUtility {
        public static bool IsOnCondition(this ConditionMode mode) {
            return mode == ConditionMode.Always || (Application.isPlaying && mode == ConditionMode.Play) || (!Application.isPlaying && mode == ConditionMode.Editor);
        }
    }
}