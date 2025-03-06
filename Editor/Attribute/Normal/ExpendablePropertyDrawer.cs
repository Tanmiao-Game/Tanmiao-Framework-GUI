using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(ExpendableAttribute))]
    public class ExpendablePropertyDrawer : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
                return new PropertyField(property);

            var container = new Foldout() { value = false };
            var toggle = container.Q<Toggle>();

            // force callback
            toggle.RegisterCallback<MouseEnterEvent>(_ => toggle.BuildSelectedBackgroundColor());
            toggle.RegisterCallback<MouseLeaveEvent>(_ => toggle.BuildDefaultBackgroundColor());

            var propertyField = new PropertyField(property) { style = {
                flexGrow = 1,
            } };
            toggle[0].style.flexGrow = 0;
            toggle.Add(propertyField);

            var editorElement = new VisualElement();
            container.Add(editorElement);

            propertyField.RegisterValueChangeCallback(evt => {
                var newValue = evt.changedProperty.objectReferenceValue;
                var visible  = newValue != null;
                toggle[0].visible = visible;
                editorElement.ActiveOrNot(newValue != null);
                if (visible) {
                    editorElement.Clear();
                    editorElement.Add(UnityEditor.Editor.CreateEditor(newValue, typeof(CustomCommonEditor)).CreateInspectorGUI());
                }
            });

            return container;
        }
    }
}