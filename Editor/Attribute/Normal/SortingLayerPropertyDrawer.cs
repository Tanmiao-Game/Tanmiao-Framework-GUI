using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(SortingLayerAttribute))]
    public class SortingLayerPropertyDrawer : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var propertyType = property.propertyType;
            if (propertyType != SerializedPropertyType.Integer && propertyType != SerializedPropertyType.String) {
                return new PropertyField(property);
            }

            // Get Layer
            List<SortingLayer> layers = SortingLayer.layers.ToList();
            int defaultIndex = Int32.MinValue;
            if (propertyType == SerializedPropertyType.Integer) defaultIndex = layers.FindIndex(x => x.id == property.intValue);
            if (propertyType == SerializedPropertyType.String) defaultIndex = layers.FindIndex(x => x.name == property.stringValue);
            defaultIndex = defaultIndex == -1 ? 0 : defaultIndex;

            void SetLayerValue(SortingLayer sortingLayer) {
                if (propertyType == SerializedPropertyType.Integer) property.intValue = sortingLayer.id;
                else if (propertyType == SerializedPropertyType.String) property.stringValue = sortingLayer.name;
                property.serializedObject.ApplyModifiedProperties();
            }

            var dropDown = new DropdownField(property.displayName, layers.Select(x => x.name).ToList(), defaultIndex).BuildFieldAlignStyle();
            dropDown.RegisterValueChangedCallback(evt => SetLayerValue(layers[layers.FindIndex(x => x.name == evt.newValue)]));

            // check value and if value is empty, set value directly
            if ((propertyType == SerializedPropertyType.Integer && property.intValue != layers[defaultIndex].id) ||
                (propertyType == SerializedPropertyType.String  && property.stringValue != layers[defaultIndex].name))
                SetLayerValue(layers[defaultIndex]);

            return dropDown;
        }
    }
}