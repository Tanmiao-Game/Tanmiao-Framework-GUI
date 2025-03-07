using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(SerializedDirectory))]
    public class SerializedDirectoryInspector : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            // properties
            var projectPathProperty = property.FindPropertyRelative($"<{nameof(SerializedDirectory.ProjectPath)}>k__BackingField");
            var foldoutProperty = property.FindPropertyRelative("foldout");

            var container = new Foldout() {
                name = property.displayName,
                text = property.displayName,
                value = foldoutProperty.boolValue,
            };
            var toggle = container.Q<Toggle>();
            toggle.style.flexGrow = 0;
            toggle.RegisterCallback<MouseEnterEvent>(_ => toggle.BuildSelectedBackgroundColor());
            toggle.RegisterCallback<MouseLeaveEvent>(_ => toggle.BuildDefaultBackgroundColor());
            container.RegisterValueChangedCallback(evt => {
                foldoutProperty.boolValue = evt.newValue;
                property.serializedObject.ApplyModifiedProperties();
            });

            // path field
            var pathField = new Label() {
                text = projectPathProperty.stringValue,
                style = {
                    flexGrow = 1f,
                    width = 200f,
                    alignSelf = Align.Center,
                    overflow = Overflow.Hidden,
                }
            };
            pathField.TrackPropertyValue(projectPathProperty, prop => pathField.text = prop.stringValue);
            pathField.RemoveFromClassList("unity-text-element");
            toggle.Add(pathField);
            toggle.Add(new Button(() => GUIUtility.systemCopyBuffer = pathField.text) {
                name = "copy",
                tooltip = "Copy path to clipboard",
                style = {
                    minWidth = 30,
                    minHeight = 20,
                    backgroundSize = new StyleBackgroundSize(new BackgroundSize(BackgroundSizeType.Contain)),
                    backgroundImage = Utility.GetEditorUtilityIcon("winbtn_win_restore@2x"),
                },
            });

            // drag field
            void OnDropItem(Object[] objects) {
                if (objects.Length == 1) {
                    var dropObject = objects[0];
                    if (dropObject is DefaultAsset) {
                        projectPathProperty.stringValue = AssetDatabase.GetAssetPath(dropObject);
                        property.serializedObject.ApplyModifiedProperties();
                    } else {
                        Debug.LogError("Drop object is not a foldout");
                    }
                } else {
                    Debug.LogError("Drop only one object or drop object is not in project");
                }
            }
            container.Add(new DragAndDropElement("Drop Foldout Here") { onDropEvent = OnDropItem });

            return container;
        }
    }
}