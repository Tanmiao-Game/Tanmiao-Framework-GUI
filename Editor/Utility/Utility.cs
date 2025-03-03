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

        #region Property
        /// <summary>
        /// 获得 SerializedProperty 的值
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this SerializedProperty property) {
            switch (property.propertyType) {
                case SerializedPropertyType.Integer:            return property.intValue;
                case SerializedPropertyType.Float:              return property.floatValue;
                case SerializedPropertyType.Boolean:            return property.boolValue;
                case SerializedPropertyType.String:             return property.stringValue;
                case SerializedPropertyType.Color:              return property.colorValue;
                case SerializedPropertyType.Vector2:            return property.vector2Value;
                case SerializedPropertyType.Vector3:            return property.vector3Value;
                case SerializedPropertyType.Vector4:            return property.vector4Value;
                case SerializedPropertyType.Vector2Int:         return property.vector2IntValue;
                case SerializedPropertyType.Vector3Int:         return property.vector3IntValue;
                case SerializedPropertyType.ObjectReference:    return property.objectReferenceValue;
                case SerializedPropertyType.LayerMask:          return property.intValue;
                case SerializedPropertyType.AnimationCurve:     return property.animationCurveValue;
                case SerializedPropertyType.Rect:               return property.rectValue;
                case SerializedPropertyType.RectInt:            return property.rectIntValue;
                case SerializedPropertyType.Bounds:             return property.boundsValue;
                case SerializedPropertyType.BoundsInt:          return property.boundsIntValue;
                case SerializedPropertyType.Quaternion:         return property.quaternionValue;
                default:                                        return null;
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
                    case SerializedPropertyType.Integer:            property.intValue = (int)value; break;
                    case SerializedPropertyType.Float:              property.floatValue = (float)value; break;
                    case SerializedPropertyType.Boolean:            property.boolValue = (bool)value; break;
                    case SerializedPropertyType.String:             property.stringValue = (string)value; break;
                    case SerializedPropertyType.Color:              property.colorValue = (Color)value; break;
                    case SerializedPropertyType.Vector2:            property.vector2Value = (Vector2)value; break;
                    case SerializedPropertyType.Vector3:            property.vector3Value = (Vector3)value; break;
                    case SerializedPropertyType.Vector4:            property.vector3Value = (Vector4)value; break;
                    case SerializedPropertyType.Vector2Int:         property.vector2IntValue = (Vector2Int)value; break;
                    case SerializedPropertyType.Vector3Int:         property.vector3IntValue = (Vector3Int)value; break;
                    case SerializedPropertyType.ObjectReference:    property.objectReferenceValue = (UnityEngine.Object)value; break;
                    case SerializedPropertyType.LayerMask:          property.intValue = (int)value; break;
                    case SerializedPropertyType.AnimationCurve:     property.animationCurveValue = (AnimationCurve)value; break;
                    case SerializedPropertyType.Rect:               property.rectValue = (Rect)value; break;
                    case SerializedPropertyType.RectInt:            property.rectIntValue = (RectInt)value; break;
                    case SerializedPropertyType.Bounds:             property.boundsValue = (Bounds)value; break;
                    case SerializedPropertyType.BoundsInt:          property.boundsIntValue = (BoundsInt)value; break;
                    case SerializedPropertyType.Quaternion:         property.quaternionValue = (Quaternion)value; break;

                    default: return;
                }
            } catch {
                throw new ArgumentException($"无法设置 {property.name} 的值 {value}");
            }
        }

        /// <summary>
        /// 创建一个绑定值的元素
        /// </summary>
        /// <param name="type"></param>
        /// <param name="label"></param>
        /// <param name="onValueCallback"></param>
        /// <returns></returns>
        public static VisualElement CreatePropertyField(this Type type, string label, Action<object> onValueCallback) {
            VisualElement result = new();
            if (type == typeof(int)) {
                result = new IntegerField(label);
                (result as IntegerField).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else if (type == typeof(float)) {
                result = new FloatField(label);
                (result as FloatField).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else if (type == typeof(bool)) {
                result = new Toggle(label);
                (result as Toggle).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else if (type == typeof(Color)) {
                result = new ColorField(label);
                (result as ColorField).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else if (type == typeof(Vector2)) {
                result = new Vector2Field(label);
                (result as Vector2Field).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else if (type == typeof(Vector3)) {
                result = new Vector3Field(label);
                (result as Vector3Field).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else if (type == typeof(Vector4)) {
                result = new Vector4Field(label);
                (result as Vector4Field).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else if (type == typeof(Vector2Int)) {
                result = new Vector2IntField(label);
                (result as Vector2IntField).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else if (type == typeof(Vector3Int)) {
                result = new Vector3IntField(label);
                (result as Vector3IntField).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else if (type.IsSubclassOf(typeof(UnityEngine.Object))) {
                result = new ObjectField(label) { objectType = type };
                (result as ObjectField).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else if (type == typeof(AnimationCurve)) {
                result = new CurveField(label);
                (result as CurveField).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else if (type == typeof(Rect)) {
                result = new RectField(label);
                (result as RectField).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else if (type == typeof(RectInt)) {
                result = new RectIntField(label);
                (result as RectIntField).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else if (type == typeof(Bounds)) {
                result = new BoundsField(label);
                (result as BoundsField).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else if (type == typeof(BoundsInt)) {
                result = new BoundsIntField(label);
                (result as BoundsIntField).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            else {
                result = new TextField(label);
                (result as TextField).RegisterValueChangedCallback(evt => onValueCallback(evt.newValue));
            }
            
            return result;
        }
        #endregion

        #region Style
        /// <summary>
        /// Framebox
        /// </summary>
        public static VisualElement BuildFrameboxStyle(this VisualElement element, float borderWidth = 2f, float corner = 5f) {
            element.style.backgroundColor = Color.gray / 3f;

            element.style.borderTopWidth  = borderWidth;
            element.style.borderBottomWidth = borderWidth;
            element.style.borderLeftWidth  = borderWidth;
            element.style.borderRightWidth = borderWidth;

            element.style.borderTopLeftRadius = corner;
            element.style.borderTopRightRadius = corner;
            element.style.borderBottomLeftRadius = corner;
            element.style.borderBottomRightRadius = corner;

            element.style.borderTopColor  = Color.black;
            element.style.borderBottomColor = Color.black;
            element.style.borderLeftColor  = Color.black;
            element.style.borderRightColor = Color.black;
            
            return element;
        }
        #endregion

    }
}