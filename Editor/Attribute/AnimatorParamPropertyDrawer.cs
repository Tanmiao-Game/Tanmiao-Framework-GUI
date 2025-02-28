using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(AnimatorParamAttribute))]
    public class AnimatorParamPropertyDrawer : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var container = new VisualElement();

            var attr = (AnimatorParamAttribute)attribute;
            var animatorName = attr.AnimatorName;
            var type = attr.Type;

            // find animator
            var animatorProperty = property.serializedObject.FindProperty(animatorName);
            if (animatorProperty == null) {
                container.AddHelpBoxToProperty(property, $"找不到 Animator {animatorName}");
                return container;
            }

            // check parameter type
            var propertyType = property.propertyType;
            if (propertyType != SerializedPropertyType.Integer && propertyType != SerializedPropertyType.String) {
                container.AddHelpBoxToProperty(property, "Animator Param 不支持类型");
                return container;
            }

            var animatorRef = animatorProperty.objectReferenceValue as Animator;
            List<string> choices = new();
            int defaultIndex = 0;
            for (int i = 0; i < animatorRef.parameterCount; i++) {
                var parameter = animatorRef.GetParameter(i);
                if (type != null && parameter.type != type) continue;
                choices.Add(parameter.name);
                if (defaultIndex != 0) continue;
                if (propertyType == SerializedPropertyType.Integer && property.intValue == parameter.nameHash) defaultIndex = choices.Count - 1;
                if (propertyType == SerializedPropertyType.String && property.stringValue == parameter.name) defaultIndex = choices.Count - 1;
            }

            // cant find parameter type match target type
            if (choices.Count == 0) {
                container.AddHelpBoxToProperty(property, $"找不到匹配的参数 {type}");
                return container;
            }

            void SetAnimatorParamValue(string value) {
                if (propertyType == SerializedPropertyType.Integer)
                    property.SetPropertyValue(Animator.StringToHash(value));
                else if (propertyType == SerializedPropertyType.String)
                    property.SetPropertyValue(value);
                property.serializedObject.ApplyModifiedProperties();
            }

            var dropDownField = new DropdownField(property.displayName, choices, defaultIndex);
            dropDownField.RegisterValueChangedCallback(evt => SetAnimatorParamValue(evt.newValue));
            container.Add(dropDownField);

            // check value and if value is empty, set value directly
            if ((propertyType == SerializedPropertyType.Integer && property.intValue != Animator.StringToHash(choices[defaultIndex])) ||
                (propertyType == SerializedPropertyType.String  && property.stringValue != choices[defaultIndex]))
                SetAnimatorParamValue(choices[defaultIndex]);

            return container;
        }
    }
}