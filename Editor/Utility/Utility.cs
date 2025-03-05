using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using UnityEngine;
using System.IO;
using System.Linq;

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

        #region Property
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

        /// <summary>
        /// Framebox
        /// </summary>
        public static T BuildFrameboxStyle<T>(this T element, float borderWidth = 2f, float corner = 5f) where T : VisualElement {
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