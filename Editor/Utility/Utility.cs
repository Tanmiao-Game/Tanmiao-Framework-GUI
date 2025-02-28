using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI.Editor {
    public static class Utility {
        /// <summary>
        /// 在原来面板的基础上增加一个 HelpBox
        /// </summary>
        public static void AddHelpBoxToProperty(this VisualElement element, SerializedProperty property, string message, HelpBoxMessageType type = HelpBoxMessageType.Info) {
            element.Add(new PropertyField(property));
            element.Add(new HelpBox(message, type));
        }

        /// <summary>
        /// 获得 SerializedProperty 的值
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this SerializedProperty property) {
            switch (property.propertyType) {
                case SerializedPropertyType.Integer: return property.intValue;
                case SerializedPropertyType.Float: return property.floatValue;
                case SerializedPropertyType.Boolean: return property.boolValue;
                case SerializedPropertyType.String: return property.stringValue;
                case SerializedPropertyType.Color: return property.colorValue;
                case SerializedPropertyType.Vector2: return property.vector2Value;
                case SerializedPropertyType.Vector3: return property.vector3Value;
                case SerializedPropertyType.Vector4: return property.vector4Value;
                case SerializedPropertyType.Vector2Int: return property.vector2IntValue;
                case SerializedPropertyType.Vector3Int: return property.vector3IntValue;
                case SerializedPropertyType.ObjectReference: return property.objectReferenceValue;
                case SerializedPropertyType.LayerMask: return property.intValue;
                case SerializedPropertyType.AnimationCurve: return property.animationCurveValue;
                case SerializedPropertyType.Rect: return property.rectValue;
                case SerializedPropertyType.RectInt: return property.rectIntValue;
                case SerializedPropertyType.Bounds: return property.boundsValue;
                case SerializedPropertyType.BoundsInt: return property.boundsIntValue;
                case SerializedPropertyType.Quaternion: return property.quaternionValue;
                default: return null;
            }
        }

        /// <summary>
        /// 设置 SerializedProperty 的值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(this SerializedProperty property, object value) {
            try {
                switch (property.propertyType) {
                    case SerializedPropertyType.Integer: property.intValue = (int)value; break;
                    case SerializedPropertyType.Float: property.floatValue = (float)value; break;
                    case SerializedPropertyType.Boolean: property.boolValue = (bool)value; break;
                    case SerializedPropertyType.String: property.stringValue = (string)value; break;
                    case SerializedPropertyType.Color: property.colorValue = (Color)value; break;
                    case SerializedPropertyType.Vector2: property.vector2Value = (Vector2)value; break;
                    case SerializedPropertyType.Vector3: property.vector3Value = (Vector3)value; break;
                    case SerializedPropertyType.Vector4: property.vector3Value = (Vector4)value; break;
                    case SerializedPropertyType.Vector2Int: property.vector2IntValue = (Vector2Int)value; break;
                    case SerializedPropertyType.Vector3Int: property.vector3IntValue = (Vector3Int)value; break;
                    case SerializedPropertyType.ObjectReference: property.objectReferenceValue = (UnityEngine.Object)value; break;
                    case SerializedPropertyType.LayerMask: property.intValue = (int)value; break;
                    case SerializedPropertyType.AnimationCurve: property.animationCurveValue = (AnimationCurve)value; break;
                    case SerializedPropertyType.Rect: property.rectValue = (Rect)value; break;
                    case SerializedPropertyType.RectInt: property.rectIntValue = (RectInt)value; break;
                    case SerializedPropertyType.Bounds: property.boundsValue = (Bounds)value; break;
                    case SerializedPropertyType.BoundsInt: property.boundsIntValue = (BoundsInt)value; break;
                    case SerializedPropertyType.Quaternion: property.quaternionValue = (Quaternion)value; break;

                    default: return;
                }
            } catch {
                throw new ArgumentException($"无法设置 {property.name} 的值 {value}");
            }
        }
    }
}