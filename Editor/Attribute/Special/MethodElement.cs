using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    /// <summary>
    /// 方法元素
    /// </summary>
    public class MethodElement : VisualElement {
        // 参数值
        private object[] values;

        public MethodElement(SerializedObject target, MethodInfo method) {
            var attribute = method.GetCustomAttribute<MethodAttribute>();
            if (string.IsNullOrEmpty(attribute.MethodName))
                this.name = method.Name;
            else
                this.name = attribute.MethodName;
            
            // parameters
            var parameters = method.GetParameters();
            values = new object[parameters.Length];

            // foldout
            var sessionKey = method.Name;
            var foldout = new Foldout() {
                text  = $"[Method]{this.name}",
                value = SessionState.GetBool(sessionKey, true),
                style = {
                    flexGrow = 1f,
                }
            };
            var toggle = foldout.Q<Toggle>();
            toggle.Add(new Button(() => method.Invoke(target.targetObject, values)) {
                text = "Invoke",
                style = {
                    width = 100,
                    unityFontStyleAndWeight = FontStyle.Bold
                },
            });
            this.Add(foldout);

            // force callback
            foldout.RegisterValueChangedCallback(evt => SessionState.SetBool(sessionKey, evt.newValue));
            toggle.RegisterCallback<MouseEnterEvent>(_ => toggle.BuildSelectedBackgroundColor());
            toggle.RegisterCallback<MouseLeaveEvent>(_ => toggle.BuildDefaultBackgroundColor());

            // parameters fields
            for (int i = 0; i < parameters.Length; i++) {
                var index = i;
                var parameter = parameters[index];
                foldout.Add(parameter.ParameterType.CreatePropertyField(parameter.Name, evt => values[index] = evt));
            }

            // if there is no parameters, hide toggle
            foldout.Q("unity-checkmark").visible = parameters.Length > 0;

            SetEnabled(attribute.Mode.IsOnCondition());
        }
    }
}