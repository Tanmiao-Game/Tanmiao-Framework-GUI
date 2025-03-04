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
            
            // params
            var parameters = method.GetParameters();
            values = new object[parameters.Length];
            if (parameters.Length > 0) {
                this.BuildFrameboxStyle();
                for (int i = 0; i < parameters.Length; i++) {
                    var index = i;
                    var parameter = parameters[index];
                    this.Add(parameter.ParameterType.CreatePropertyField(parameter.Name, evt => values[index] = evt));
                }
            }

            this.Insert(0, new Button(() => method.Invoke(target.targetObject, values)) { text = this.name });

            SetEnabled(attribute.Mode.IsOnCondition());
        }
    }
}