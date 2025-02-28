using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// 动画变量，在原来面板的基础上寻找 Animator 的变量
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class AnimatorParamAttribute : PropertyAttribute {
        public string AnimatorName { get; private set; }
        public AnimatorControllerParameterType? Type { get; private set; }

        public AnimatorParamAttribute(string animatorName) {
            AnimatorName = animatorName;
            Type = default;
        }

        public AnimatorParamAttribute(string animatorName, AnimatorControllerParameterType type) {
            AnimatorName = animatorName;
            Type = type;
        }
    }
}