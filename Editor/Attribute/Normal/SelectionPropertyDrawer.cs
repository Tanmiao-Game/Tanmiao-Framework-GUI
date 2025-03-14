using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(SelectionAttribute))]
    public class SelectionPopAttributeDrawer : PropertyDrawer {
        private static readonly Dictionary<Type, List<string>> caches = new();
        private static readonly Dictionary<string, Assembly> dllCaches = new();

        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var container = new VisualElement() {
                name = property.displayName,
                style = {
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
                        bool isConstant = values[i].IsLiteral && !values[i].IsInitOnly;
                        if (!isConstant)
                            continue;
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
                    container.style.flexDirection = FlexDirection.Row;
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

            }
            else if (property.propertyType == SerializedPropertyType.ManagedReference) {
                // get dll and types
                var fullNames = property.managedReferenceFieldTypename.Split(" ");
                var dllName = fullNames[0];
                var inrName = fullNames[1];
                var types = GetTypes(dllName, inrName);
                
                if (types == default || types.Length == 0) {
                    // add help box
                    container.AddHelpBoxToProperty(property, $"{inrName} found no types", HelpBoxMessageType.Warning);
                } else {
                    // parse to string values
                    // add first value "Null"
                    var values = new string[types.Length + 1];
                    values[0] = "";
                    for (int i = 0; i < types.Length; i++) values[i + 1] = types[i].Name;

                    PropertyField managerReferenceField = new(property, $"[Serialized]{property.displayName}");
                    managerReferenceField.ActiveOrNot(property.managedReferenceValue != null);
                    managerReferenceField.BuildFrameboxStyle();
                    managerReferenceField.RegisterCallback<GeometryChangedEvent>(_ => {
                        var toggle = managerReferenceField.Q<Toggle>();
                        toggle.RegisterCallback<MouseEnterEvent>(_ => toggle.BuildSelectedBackgroundColor());
                        toggle.RegisterCallback<MouseLeaveEvent>(_ => toggle.BuildDefaultBackgroundColor());
                    });

                    var dropFied = new DropdownField(property.displayName) {
                        value = property.managedReferenceValue?.GetType().Name ?? "Null",
                        style = { flexGrow = 1f, flexShrink = 1f, } };
                    dropFied.AddToClassList("unity-base-field__aligned");
                    dropFied.RegisterCallback<ClickEvent>(@event => {
                        var field = new AdvanceDropField(inrName, values, item => {
                            bool isNull = string.IsNullOrEmpty(item.fullName);
                            property.managedReferenceValue = isNull ? null : GetTypeByAssembley(dllName, inrName, item.fullName);
                            property.serializedObject.ApplyModifiedProperties();
                            dropFied.value = property.managedReferenceValue?.GetType().Name ?? "Null";
                        });
                        field.Show(dropFied.worldBound);
                    });
                    container.Add(dropFied);
                    container.Add(managerReferenceField);
                }
            }
            else {
                container.AddHelpBoxToProperty(property, $"{typeof(SelectionAttribute)} is not support for {property.propertyType}");
            }

            return container;
        }

        private string FormatItem(string value) {
            if (string.IsNullOrEmpty(value))
                return "(null)";
            else
                return value;
        }

        private Assembly GetDLL(string dllName) {
            if (dllCaches.ContainsKey(dllName)) return dllCaches[dllName];
            return dllCaches[dllName] = Assembly.Load(dllName);
        }

        private Type[] GetTypes(string dllName, string specialName) {
            var dll = GetDLL(dllName);
            var list = dll.GetTypes().Where(t => !t.IsAbstract && !t.IsInterface);
            var type = dll.GetType(specialName);
            if (type.IsInterface)
                return list.Where(t => t.GetInterface(specialName) != null)?.ToArray();
            else if (type.IsAbstract)
                return list.Where(t => t.IsSubclassOf(type))?.ToArray();
            return default;
        }

        private object GetTypeByAssembley(string dllName, string specialName, string className) {
            var type = GetTypes(dllName, specialName).SingleOrDefault(t => t.Name == className);
            return type == null ? throw new Exception($"type {className} not found in {dllName}") : GetDLL(dllName).CreateInstance(type.FullName);
        }
    }
}