using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(SelectionPathAttribute))]
    public class SelectionPathAttributeDrawer : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var container = new VisualElement() {
                name = "path-selection",
                style = {
                    flexDirection = FlexDirection.Row,
                    flexGrow = 1f,
                    flexShrink = 1f,
                }
            };

            if (property.propertyType == SerializedPropertyType.String) {
                var value = property.stringValue;
                var pathTextField = new TextField() {
                    name = "path-text-field",
                    label = property.displayName,
                    value = value,
                    style = {
                        flexGrow = 1f,
                        flexShrink = 1f,
                    }
                };
                container.Add(pathTextField);

                var chooseBtn = new Button(() => {
                    // 来源: https://forum.unity.com/threads/editorutility-openfilepanel-causes-error-log-for-endlayoutgroup.1389873/
                    EditorApplication.delayCall += () => {
                        value = EditorUtility.OpenFolderPanel($"Choose {property.displayName} Path", string.IsNullOrEmpty(value) ? Application.dataPath : value, "");
                        if (value.Contains(Application.dataPath))
                            value = value.GetRelativeAssetsPath();
                        property.stringValue = value;
                        pathTextField.value = value;
                        property.serializedObject.ApplyModifiedProperties();
                        property.serializedObject.SetIsDifferentCacheDirty();
                        AssetDatabase.SaveAssets();
                    };
                }) {
                    name = "choose-btn",
                    text = "Choose",
                };
                container.Add(chooseBtn);

            } else {
                container.style.flexDirection = FlexDirection.Column;
                container.AddHelpBoxToProperty(property, $"{typeof(SelectionPathAttribute)} is only support for {typeof(string)}");
                // container.Add(new HelpBox($"{typeof(SelectionPathAttribute)} is only support for {typeof(string)}", HelpBoxMessageType.Warning));
            }

            return container;
        }
    }
}