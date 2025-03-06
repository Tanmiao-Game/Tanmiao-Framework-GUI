using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(ConditionAttribute), true)]
    public abstract class ConditionPropertyDrawer : PropertyDrawer {
        protected static BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        protected virtual bool GetResultForConditionField(SerializedProperty property) {
            var instance = property.serializedObject.targetObject;
            var type  = instance.GetType();
            var values = ((ConditionAttribute)attribute).Values;
            var result = ((ConditionAttribute)attribute).Condition == ConditionOperator.And;
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

                if (((ConditionAttribute)attribute).Condition == ConditionOperator.And) {
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