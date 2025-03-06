using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(EnableIfAttribute))]
    public class EnableIfPropertyDrawer : ConditionPropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var container = new VisualElement();

            var element = new PropertyField(property);
            container.Add(element);

            element.schedule.Execute(() => element.SetEnabled(GetResultForConditionField(property))).Every(1);

            return container;
        }
    }
}