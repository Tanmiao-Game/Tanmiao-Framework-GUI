using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(SelectionAttribute))]
    public class SelectionPopAttributeDrawer : PropertyDrawer {
        private static readonly Dictionary<Type, List<string>> caches = new();

        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var container = new VisualElement() {
                name = property.displayName,
                style = {
                    flexDirection = FlexDirection.Row,
                    flexGrow = 1f,
                    flexShrink = 1f,
                }
            };

            if (property.propertyType == SerializedPropertyType.String) {
                var selection = attribute as SelectionAttribute;
                var type = selection.type;

                // check cache
                List<string> names = default;
                if (!caches.ContainsKey(type)) {
                    var values = type.GetFields();
                    names = new() { "" };
                    for (int i = 0; i < values.Length; i++) {
                        var name = values[i].GetRawConstantValue().ToString();
                        if (string.IsNullOrEmpty(name))
                            continue;
                        names.Add(name);
                    }
                    caches[type] = names;
                } else {
                    names = caches[type];
                }

                var dropDown = new DropdownField(property.displayName) {
                    value = property.stringValue,
                    formatListItemCallback = FormatItem,
                    formatSelectedValueCallback = FormatItem,
                    style = {
                        flexGrow = 1f,
                    }
                };
                dropDown.AddToClassList("unity-base-field__aligned");
                dropDown.RegisterCallback<ClickEvent>(evt => {
                    var field = new AdvanceDropField(type.FullName, names, item => {
                        dropDown.value = item.fullName;
                        // property.serializedObject.Update();
                        property.stringValue = item.fullName;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                    field.Show(dropDown.worldBound);
                });
                
                container.Add(dropDown);

                // editable
                if (selection.editable) {
                    var editTextField = new TextField() {
                        name = "edit-text-field",
                        label = property.displayName,
                        value = property.stringValue,
                        style = {
                            flexGrow = 1f,
                            flexShrink = 1f,
                            display = names.Contains(property.stringValue) ? DisplayStyle.None : DisplayStyle.Flex,
                        }
                    };
                    editTextField.RegisterValueChangedCallback(evt => {
                        property.stringValue = evt.newValue;
                        property.serializedObject.ApplyModifiedProperties(); 
                    });
                    editTextField.AddToClassList("unity-base-field__aligned");
                    container.Add(editTextField);

                    dropDown.style.display = editTextField.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;

                    var editBtn = new Button(() => {
                        if (editTextField.style.display == DisplayStyle.None) {
                            // 显示编辑，隐藏下拉
                            editTextField.style.display = DisplayStyle.Flex;
                            dropDown.style.display = DisplayStyle.None;
                            editTextField.value = property.stringValue;
                        } else {
                            // 隐藏编辑，显示下拉
                            editTextField.style.display = DisplayStyle.None;
                            dropDown.style.display = DisplayStyle.Flex;
                            property.stringValue = dropDown.value;
                            property.serializedObject.ApplyModifiedProperties();
                        }
                    }) {
                        name = "edit-btn",
                        tooltip = "edit",
                        style = {
                            minWidth = 30,
                            backgroundSize = new StyleBackgroundSize(new BackgroundSize(BackgroundSizeType.Contain)),
                            backgroundImage = Utility.GetEditorUtilityIcon("editicon.sml"),
                        },
                    };
                    container.Add(editBtn);
                }

            } else {
                container.style.flexDirection = FlexDirection.Column;
                container.AddHelpBoxToProperty(property, $"{typeof(SelectionAttribute)} is only support for {typeof(string)}");
                // container.Add(new HelpBox($"{typeof(SelectionAttribute)} is only support for {typeof(string)}", HelpBoxMessageType.Info));
            }

            return container;
        }

        private string FormatItem(string value) {
            if (string.IsNullOrEmpty(value))
                return "(null)";
            else
                return value;
        }
    }
}