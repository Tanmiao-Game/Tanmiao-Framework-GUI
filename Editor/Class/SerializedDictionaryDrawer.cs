using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections.Generic;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(SerializedDictionary<,>))]
    public class SerializedDictionaryDrawer : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            // 获取 keys 和 values
            var keyProperty = property.FindPropertyRelative("keys");
            var valProperty = property.FindPropertyRelative("vals");

            // 折叠面板
            var sessionKey = property.displayName;
            var container = new Foldout {
                name = sessionKey,
                text = $"{sessionKey}[{keyProperty.arraySize}]",
                value = SessionState.GetBool(sessionKey, true),
            };
            container.RegisterValueChangedCallback(evt => SessionState.SetBool(sessionKey, evt.newValue));

            var foldoutToggle = container.Q<Toggle>();
            var foldoutHeader = foldoutToggle.Q<Toggle>();
            foldoutHeader[0].style.maxWidth = 200f;

            // force callback
            foldoutToggle.RegisterCallback<MouseEnterEvent>(_ => foldoutToggle.BuildSelectedBackgroundColor());
            foldoutToggle.RegisterCallback<MouseLeaveEvent>(_ => foldoutToggle.BuildDefaultBackgroundColor());
            container.TrackPropertyValue(keyProperty, _ => FreshContainer());

            // 创建 ListView
            container.Add(CreateListView(keyProperty, valProperty, FreshContainer));

            #region addition header
            var additionHeader = new VisualElement() {
                name = "addition",
                style = {
                    flexDirection = FlexDirection.Row,
                    flexGrow = 1f,
                }
            };
            foldoutHeader.Add(additionHeader);

            var type = fieldInfo.FieldType.GenericTypeArguments[0];
            object keyAddValue = null;
            var keyAddField = type.CreatePropertyField(default, v => keyAddValue = v);
            keyAddField.RemoveTextElementStyle();
            additionHeader.Add(keyAddField);

            var additionBtn = new Button(() => {
                try {
                    if (keyAddValue == null) {
                        Debug.LogError("Key cant be null!");
                        return;
                    }

                    for (int i = 0; i < keyProperty.arraySize; i++) {
                        var value = keyProperty.GetArrayElementAtIndex(i).GetPropertyValue();
                        if (EqualityComparer<object>.Default.Equals(value, keyAddValue)) {
                            Debug.LogError("A same key has been added!");
                            return;
                        }
                    }

                    keyProperty.arraySize++;
                    valProperty.arraySize++;
                    keyProperty.GetArrayElementAtIndex(keyProperty.arraySize - 1).SetPropertyValue(keyAddValue);
                    property.serializedObject.ApplyModifiedProperties();
                } catch (Exception ex) {
                    Debug.LogError(ex);
                } finally {
                    FreshContainer();
                }
            }) {
                name = "additionbtn",
                tooltip = "add key to dictionary",
                style = {
                    backgroundSize = new StyleBackgroundSize(new BackgroundSize(BackgroundSizeType.Contain)),
                    backgroundImage = Utility.GetEditorUtilityIcon("d_Toolbar Plus@2x"),
                    minWidth = 25f,
                    unityFontStyleAndWeight = FontStyle.Bold,
                }
            };
            additionBtn.RemoveTextElementStyle();
            additionHeader.Add(additionBtn);
            #endregion

            // fresh
            void FreshContainer() {
                container.schedule.Execute(() => {
                    container.text = $"{sessionKey}[{keyProperty.arraySize}]";
                    container.Q<ListView>().Rebuild();
                }).ExecuteLater(100);
            }

            return container;
        }

        /// <summary>
        /// 创建 ListView
        /// </summary>
        private ListView CreateListView(SerializedProperty keysProp, SerializedProperty valsProp, Action onRemoved) {
            // 创建 ListView
            ListView listView = new ListView {
                showBoundCollectionSize = false,
                showAddRemoveFooter = false,
                reorderable = true,
                reorderMode = ListViewReorderMode.Animated,
                makeItem = CreateRow,
                bindItem = (element, index) => BindRow(element, keysProp, valsProp, index, onRemoved),
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight
            };

            listView.BindProperty(keysProp);

             // 监听拖拽排序事件
            listView.itemIndexChanged += (int a, int b) => {
                // reorder and update value
                var fromValue = valsProp.GetArrayElementAtIndex(a).GetPropertyValue();
                var toValue   = valsProp.GetArrayElementAtIndex(b).GetPropertyValue();

                valsProp.GetArrayElementAtIndex(a).SetPropertyValue(toValue);
                valsProp.GetArrayElementAtIndex(b).SetPropertyValue(fromValue);

                valsProp.serializedObject.ApplyModifiedProperties();
            };

            return listView;
        }

        /// <summary>
        /// 创建 ListView 行
        /// </summary>
        private VisualElement CreateRow() {
            var row = new VisualElement { style = { flexDirection = FlexDirection.Row } };

            // Key
            var keyField = new PropertyField
            {
                label = "",
                style = {
                    flexGrow = 1f,
                    flexShrink = 1f,
                    maxWidth = 150f,
                }
            };
            row.Add(keyField);

            // Value
            var valueField = new PropertyField
            {
                label = "",
                style = {
                    flexGrow = 1f,
                    flexShrink = 1f,
                }
            };
            row.Add(valueField);

            // remove
            var removeBtn = new Button() {
                style = {
                    width = 20f,
                    backgroundImage = Utility.GetEditorUtilityIcon("d_Toolbar Minus@2x"),
                }
            };
            row.Add(removeBtn);

            return row;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindRow(VisualElement element, SerializedProperty keysProp, SerializedProperty valuesProp, int index, Action onRemoved) {
            var keyField = element.ElementAt(0) as PropertyField;
            var valueField = element.ElementAt(1) as PropertyField;
            var removeBtn = element.ElementAt(2) as Button;

            keyField.BindProperty(keysProp.GetArrayElementAtIndex(index));
            valueField.BindProperty(valuesProp.GetArrayElementAtIndex(index));
            removeBtn.clicked -= RemoveElement;
            removeBtn.clicked += RemoveElement;

            void RemoveElement() {
                keysProp.DeleteArrayElementAtIndex(index);
                valuesProp.DeleteArrayElementAtIndex(index);
                keysProp.serializedObject.ApplyModifiedProperties();
                // valuesProp.serializedObject.ApplyModifiedProperties();
                onRemoved?.Invoke();
            } 
        }
    }
}