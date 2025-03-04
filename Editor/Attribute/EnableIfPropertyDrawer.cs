using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(EnableIfAttribute))]
    public class EnableIfPropertyDrawer : PropertyDrawer {
        protected static BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var container = new VisualElement();

            var element = new PropertyField(property);
            container.Add(element);

            element.schedule.Execute(() => element.SetEnabled(IsEnabledField(property))).Every(1);

            return container;
        }

        protected bool IsEnabledField(SerializedProperty property) {
            var instance = property.serializedObject.targetObject;
            var type  = instance.GetType();
            var values = ((EnableIfAttribute)attribute).Values;
            var result = ((EnableIfAttribute)attribute).Condition == ConditionOperator.And;
            foreach (var value in values) {
                bool thisResult;
                if (string.IsNullOrEmpty(value))
                    thisResult = false;
                else if (value.Equals(true.ToString()))
                    thisResult = true;
                else if (value.Equals(false.ToString()))
                    thisResult = false;
                else {
                    var tryFindField = type.GetField(value, Flags);
                    if (tryFindField == null || tryFindField.FieldType != typeof(bool)) {
                        var tryFindProperty = type.GetProperty(value, Flags);
                        if (tryFindProperty == null || tryFindProperty.PropertyType != typeof(bool)) {
                            var tryFindMethod = type.GetMethod(value, Flags);
                            if (tryFindMethod == null || tryFindMethod.GetParameters().Length > 0 || tryFindMethod.ReturnType != typeof(bool)) {
                                thisResult = true;
                            } else {
                                thisResult = (bool)tryFindMethod.Invoke(instance, null);
                            }
                        } else {
                            thisResult = (bool)tryFindProperty.GetValue(instance);
                        }
                    } else {
                        thisResult = (bool)tryFindField.GetValue(instance);
                    }
                }

                if (((EnableIfAttribute)attribute).Condition == ConditionOperator.And) {
                    result &= thisResult;
                    if (!result) break;
                } else {
                    result |= thisResult;
                }
            }

            return result;
        }
    }
}