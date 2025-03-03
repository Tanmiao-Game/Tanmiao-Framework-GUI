using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.UIElements;
using System;
using System.Reflection;
using System.Linq;

namespace Akatsuki.Framework.GUI.Editor {
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class CustomCommonEditor : global::UnityEditor.Editor {
        protected List<SerializedProperty> properties = new();
        protected IEnumerable<MethodInfo> methods;

        private void OnEnable() {
            InitSerializeProperty();
            InitMethod();
        }

        /// <summary>
        /// 初始化 序列化 字段
        /// </summary>
        protected virtual void InitSerializeProperty() {
            properties.Clear();
            using (var iterator = serializedObject.GetIterator()) {
                if (iterator.NextVisible(true)) {
                    do {
                        properties.Add(iterator.Copy());
                    } while(iterator.NextVisible(true));
                }
            }
        }

        /// <summary>
        /// 初始化特殊的字段，属性，方法
        /// </summary>
        protected virtual void InitMethod() {
            var type = target.GetType();
            // var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);
            // var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);
            this.methods = methods.Where(method => method.GetCustomAttribute<MethodAttribute>() != null);
        }

        public override VisualElement CreateInspectorGUI() {
            var container = new VisualElement() { name = serializedObject.targetObject.name };
            DrawDefaultField(container);
            DrawMethodButton(container);
            return container;
        }

        /// <summary>
        /// 绘制 默认 序列化字段
        /// </summary>
        /// <param name="container"></param>
        protected virtual void DrawDefaultField(VisualElement container) {
            foreach (var property in properties)
                container.Add(new PropertyField(property));
        }

        /// <summary>
        /// 绘制 特殊 序列化字段
        /// </summary>
        protected virtual void DrawMethodButton(VisualElement container) {
            foreach (var method in methods) {
                container.Add(new MethodElement(serializedObject, method));
            }
        }
    }
}