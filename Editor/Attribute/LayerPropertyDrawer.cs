using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerPropertyDrawer : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var container = new VisualElement();

            var propertyType = property.propertyType;
            if (propertyType != SerializedPropertyType.Integer && propertyType != SerializedPropertyType.String) {
                container.Add(new PropertyField(property));
                return container;
            }

            // Get Layer
            List<string> layers = InternalEditorUtility.layers.ToList();
            int defaultIndex = 0;
            if (propertyType == SerializedPropertyType.Integer) defaultIndex = layers.FindIndex(x => x == LayerMask.LayerToName(property.intValue));
            if (propertyType == SerializedPropertyType.String) defaultIndex = layers.FindIndex(x => x == property.stringValue);
            defaultIndex = defaultIndex == -1 ? 0 : defaultIndex;

            void SetLayerValue(string value) {
                if (propertyType == SerializedPropertyType.Integer) property.intValue = LayerMask.NameToLayer(value);
                else if (propertyType == SerializedPropertyType.String) property.stringValue = value;
                property.serializedObject.ApplyModifiedProperties();
            }

            var dropDown = new DropdownField(property.displayName, layers, defaultIndex).BuildFieldAlignStyle();
            dropDown.RegisterValueChangedCallback(evt => SetLayerValue(evt.newValue));
            container.Add(dropDown);

            // check value and if value is empty, set value directly
            if ((propertyType == SerializedPropertyType.Integer && property.intValue != LayerMask.NameToLayer(layers[defaultIndex])) ||
                (propertyType == SerializedPropertyType.String  && property.stringValue != layers[defaultIndex]))
                SetLayerValue(layers[defaultIndex]);

            return container;
        }
    }
}