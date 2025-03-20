using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Akatsuki.Framework.GUI.Editor {
    public static class Utility {
        /// <summary>
        /// 在原来面板的基础上增加一个 HelpBox
        /// </summary>
        public static void AddHelpBoxToProperty(this VisualElement element, SerializedProperty property, string message, HelpBoxMessageType type = HelpBoxMessageType.Info) {
            if (property != null)
                element.Add(new PropertyField(property));
            element.Add(new HelpBox(message, type));
        }

        /// <summary>
        /// <para>获得编辑器内置 Icon，并转换到 Texture2D</para>
        /// <para>名称参考 https://github.com/halak/unity-editor-icons </para>
        /// </summary>
        /// <returns></returns>
        public static Texture2D GetEditorUtilityIcon(string name) {
            var self = EditorGUIUtility.IconContent(name).image;
            var sw = self.width;
            var sh = self.height;
            var format = TextureFormat.RGBA32;
            var result = new Texture2D( sw, sh, format, false );
            var currentRT = RenderTexture.active;
            var rt = new RenderTexture( sw, sh, 32 );
            Graphics.Blit( self, rt );
            RenderTexture.active = rt;
            var source = new Rect( 0, 0, rt.width, rt.height );
            result.ReadPixels( source, 0, 0 );
            result.Apply();
            RenderTexture.active = currentRT;
            return result;
        }

        #region Property
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
                case SerializedPropertyType.Rect:               return property.rectValue;
                case SerializedPropertyType.RectInt:            return property.rectIntValue;
                case SerializedPropertyType.Bounds:             return property.boundsValue;
                case SerializedPropertyType.BoundsInt:          return property.boundsIntValue;
                case SerializedPropertyType.Quaternion:         return property.quaternionValue;
                case SerializedPropertyType.ObjectReference:    return property.objectReferenceValue;
                case SerializedPropertyType.AnimationCurve:     return property.animationCurveValue;
                case SerializedPropertyType.Enum:               return property.enumValueIndex;
                case SerializedPropertyType.ManagedReference:   return property.managedReferenceValue;
                default:    return null;
            }
        }

        /// <summary>
        /// 塞入绑定值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(this SerializedProperty property, object value) {
            switch (property.propertyType) {
                case SerializedPropertyType.Integer:            property.intValue = (int)value;                             break;
                case SerializedPropertyType.Float:              property.floatValue = (float)value;                         break;
                case SerializedPropertyType.Boolean:            property.boolValue = (bool)value;                           break;
                case SerializedPropertyType.String:             property.stringValue = (string)value;                       break;
                case SerializedPropertyType.Color:              property.colorValue = (Color)value;                         break;
                case SerializedPropertyType.Vector2:            property.vector2Value = (Vector2)value;                     break;
                case SerializedPropertyType.Vector3:            property.vector3Value = (Vector3)value;                     break;
                case SerializedPropertyType.Vector4:            property.vector4Value = (Vector4)value;                     break;
                case SerializedPropertyType.Vector2Int:         property.vector2IntValue = (Vector2Int)value;               break;
                case SerializedPropertyType.Vector3Int:         property.vector3IntValue = (Vector3Int)value;               break;
                case SerializedPropertyType.Rect:               property.rectValue = (Rect)value;                           break;
                case SerializedPropertyType.RectInt:            property.rectIntValue = (RectInt)value;                     break;
                case SerializedPropertyType.Bounds:             property.boundsValue = (Bounds)value;                       break;
                case SerializedPropertyType.BoundsInt:          property.boundsIntValue = (BoundsInt)value;                 break;
                case SerializedPropertyType.Quaternion:         property.quaternionValue = (Quaternion)value;               break;
                case SerializedPropertyType.Enum:               property.enumValueIndex = (int)value;                       break;
                case SerializedPropertyType.AnimationCurve:     property.animationCurveValue = (AnimationCurve)value;       break;
                case SerializedPropertyType.ObjectReference:    property.objectReferenceValue = (UnityEngine.Object)value;  break;
                default:    break;
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
            object defaultValue;
            if (type == typeof(int)) {
                result = new IntegerField(label);
                (result as IntegerField).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = 0;
            }
            else if (type == typeof(float)) {
                result = new FloatField(label);
                (result as FloatField).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = 0;
            }
            else if (type == typeof(bool)) {
                result = new Toggle(label);
                (result as Toggle).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = false;
            }
            else if (type == typeof(Color)) {
                result = new ColorField(label);
                (result as ColorField).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = Color.black;
            }
            else if (type == typeof(Vector2)) {
                result = new Vector2Field(label);
                (result as Vector2Field).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = Vector2.zero;
            }
            else if (type == typeof(Vector3)) {
                result = new Vector3Field(label);
                (result as Vector3Field).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = Vector3.zero;
            }
            else if (type == typeof(Vector4)) {
                result = new Vector4Field(label);
                (result as Vector4Field).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = Vector4.zero;
            }
            else if (type == typeof(Vector2Int)) {
                result = new Vector2IntField(label);
                (result as Vector2IntField).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = Vector2Int.zero;
            }
            else if (type == typeof(Vector3Int)) {
                result = new Vector3IntField(label);
                (result as Vector3IntField).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = Vector3Int.zero;
            }
            else if (type.IsSubclassOf(typeof(UnityEngine.Object))) {
                result = new ObjectField(label) { objectType = type };
                (result as ObjectField).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = null;
            }
            else if (type == typeof(AnimationCurve)) {
                result = new CurveField(label);
                (result as CurveField).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = new AnimationCurve();
            }
            else if (type == typeof(Rect)) {
                result = new RectField(label);
                (result as RectField).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = new Rect();
            }
            else if (type == typeof(RectInt)) {
                result = new RectIntField(label);
                (result as RectIntField).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = new RectInt();
            }
            else if (type == typeof(Bounds)) {
                result = new BoundsField(label);
                (result as BoundsField).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = new Bounds();
            }
            else if (type == typeof(BoundsInt)) {
                result = new BoundsIntField(label);
                (result as BoundsIntField).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = new BoundsInt();
            }
            else if (type == typeof(Quaternion)) {
                result = new Vector3Field(label);
                (result as Vector3Field).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(Quaternion.Euler(evt.newValue)));
                defaultValue = Quaternion.identity;
            }
            else if (type == typeof(string)) {
                result = new TextField(label);
                (result as TextField).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(evt.newValue));
                defaultValue = string.Empty;
            }
            else if (type.IsEnum) {
                defaultValue = Enum.GetValues(type).GetValue(0);
                result = new EnumField(label, (Enum)defaultValue);
                // convert to int(enum.index) to compare
                defaultValue = Convert.ToInt32(defaultValue);
                (result as EnumField).RegisterValueChangedCallback(evt => onValueCallback?.Invoke(Convert.ToInt32(evt.newValue)));
            }
            else {
                var wrapper = SerializedObjectWrapper.Create(defaultValue = Activator.CreateInstance(type));
                var serialized = new SerializedObject(wrapper);

                result = new PropertyField(serialized.FindProperty("data"), label);
                result.Bind(serialized);
                result.RegisterCallback<MouseLeaveEvent>(evt => onValueCallback(wrapper.data));
            }
            
            result.name = label;
            result.style.flexGrow = 1f;
            onValueCallback?.Invoke(defaultValue);
            return result;
        }
        #endregion

        #region Style
        public static T BuildDefaultBackgroundColor<T>(this T element) where T : VisualElement {
            const float value = 56f / 255;
            element.style.backgroundColor = new Color(value, value, value, 1);
            return element;
        }

        public static T BuildSelectedBackgroundColor<T>(this T element) where T : VisualElement {
            const float value = 91f / 255;
            element.style.backgroundColor = new Color(value, value, value, 1);
            return element;
        }

        public static T BuildDarkBackgroundColor<T>(this T element) where T : VisualElement {
            const float value = 40f / 255;
            element.style.backgroundColor = new Color(value, value, value, 1);
            return element;
        }

        /// <summary>
        /// 去除样式 unity-text-element
        /// </summary>
        /// <param name="element"></param>
        /// <typeparam name="T"></typeparam>
        public static void RemoveTextElementStyle<T>(this T element) where T : VisualElement {
            const string className = "unity-text-element";
            element.Q(classes: className)?.RemoveFromClassList(className);
        }

        /// <summary>
        /// Framebox
        /// </summary>
        public static T BuildFrameboxStyle<T>(this T element, float borderWidth = 2f, float corner = 5f) where T : VisualElement {
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

        /// <summary>
        /// 输入字段对齐
        /// </summary>
        /// <param name="element"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T BuildFieldAlignStyle<T>(this T element) where T : VisualElement {
            element.AddToClassList("unity-base-field__aligned");
            return element;
        }

        /// <summary>
        /// style.display 控制显示与隐藏
        /// </summary>
        /// <param name="element"></param>
        /// <param name="active"></param>
        public static void ActiveOrNot(this VisualElement element, bool active) {
            element.style.display = active ? DisplayStyle.Flex : DisplayStyle.None;
        }
        #endregion

        #region File
        /// <summary>
        /// 获得文件路径
        /// </summary>
        /// <param name="name"></param>
        /// <param name="extension">后缀，不需要带 . </param>
        public static string GetFileLocation(this string name, string extension) {
            var guids = AssetDatabase.FindAssets(name);
            foreach (var guid in guids) {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.Split('/').Last().Equals($"{name}.{extension}")) {
                    return path;
                }
            }
            return default;
        }

        /// <summary>
        /// 是否是有效路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsValidPath(this string path) {
            return !string.IsNullOrEmpty(path) && Directory.Exists(path);
        }

        /// <summary>
        /// 获得资源路径
        /// </summary>
        /// <param name="path">
        ///     例：C://Demo/Assets/Res/Textures/Test.png => Assets/Res/Textures/Test.png
        /// </param>
        /// <returns></returns>
        public static string GetRelativeAssetsPath(this string path) {
            return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
        }

        /// <summary>
        /// 获得详细路径
        /// </summary>
        /// <param name="path">
        ///     例：Assets/Res/Textures/Test.png => C://Demo/Assets/Res/Textures/Test.png
        /// </param>
        /// <returns></returns>
        public static string GetFullAssetsPath(this string path) {
            return Path.Combine(Application.dataPath, path.Replace("Assets/", ""));
        }
        #endregion

    }
}