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

            var propertyField = new PropertyField(property) { style = {
                flexGrow = 1,
            } };
            toggle[0].style.flexGrow = 0;
            toggle.Add(propertyField);

            var imguiContainer = new IMGUIContainer();
            container.Add(imguiContainer);

            propertyField.RegisterValueChangeCallback(evt => {
                var newValue = evt.changedProperty.objectReferenceValue;
                var visible  = newValue != null;
                toggle[0].visible = visible;
                imguiContainer.ActiveOrNot(newValue != null);
                if (visible) {
                    imguiContainer.onGUIHandler = () => UnityEditor.Editor.CreateEditor(newValue).DrawDefaultInspector();
                } else {
                    // container.Clear();
                }
            });

            return container;
        }
    }
}