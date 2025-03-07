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
        protected List<SerializedProperty> serializers = new();
        protected IEnumerable<FieldInfo> fields;
        protected IEnumerable<MethodInfo> methods;
        
        protected static BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        private void OnEnable() {
            InitSerializeProperty();
            InitReflectionValues();
        }

        /// <summary>
        /// 初始化 序列化 字段
        /// </summary>
        protected virtual void InitSerializeProperty() {
            serializers.Clear();
            using var iterator = serializedObject.GetIterator();
            if (iterator.NextVisible(true)) {
                do {
                    // skip children serializer
                    if (iterator.depth > 0) continue;
                    serializers.Add(iterator.Copy());
                } while (iterator.NextVisible(true));
            }
        }

        /// <summary>
        /// 初始化特殊的字段，属性，方法
        /// </summary>
        protected virtual void InitReflectionValues() {
            var type = target.GetType();
            fields = type.GetFields(Flags);
            // var properties = type.GetProperties(Flags);
            methods = type.GetMethods(Flags).Where(method => method.GetCustomAttribute<MethodAttribute>() != null);
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
            // serializedObject.Update();
            foreach (var property in serializers) {
                var field = new PropertyField(property);
                field.Bind(serializedObject);
                if (property.name.Equals("m_Script", StringComparison.Ordinal))
                    field.SetEnabled(false);

                container.Add(field);
            }
            // serializedObject.ApplyModifiedProperties();
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