using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class ScenePropertyDrawer : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var container = new VisualElement();

            var propertyType = property.propertyType;
            if (propertyType != SerializedPropertyType.String && propertyType != SerializedPropertyType.Integer) {
                container.Add(new PropertyField(property));
                return container;
            }

            // get scenes
            List<string> scenes = new();
            int defaultIndex = 0;
            if (((SceneAttribute)attribute).BuildSceneOnly) {
                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                    scenes.Add(SceneUtility.GetScenePathByBuildIndex(i));
            } else {
                scenes = AssetDatabase.FindAssets("t:Scene").Select(x => AssetDatabase.GUIDToAssetPath(x))
                                                            .Where(x => !x.StartsWith("Package")).ToList();
            }
            scenes = scenes.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();

            if (scenes.Count == 0) {
                container.AddHelpBoxToProperty(property, "不存在场景");
                return container;
            }

            // change direction
            container.style.flexDirection = FlexDirection.Row;

            // get default index
            if (propertyType == SerializedPropertyType.String) defaultIndex = scenes.FindIndex(x => x == property.stringValue);
            if (propertyType == SerializedPropertyType.Integer) defaultIndex = property.intValue > scenes.Count - 1 ? -1 : property.intValue;
            defaultIndex = defaultIndex == -1 ? 0 : defaultIndex;

            void SetSceneValue(string value) {
                defaultIndex = scenes.IndexOf(value);
                if (propertyType == SerializedPropertyType.Integer) property.intValue = scenes.IndexOf(value);
                else if (propertyType == SerializedPropertyType.String) property.stringValue = value;
                property.serializedObject.ApplyModifiedProperties();
                FreshLoadButton();
            }

            var dropDown = new DropdownField(property.displayName, scenes, defaultIndex) {
                style = {
                    flexGrow = 1f,
                }
            }.BuildFieldAlignStyle();
            dropDown.RegisterValueChangedCallback(evt => SetSceneValue(evt.newValue));
            container.Add(dropDown);

            // check value and if value is empty, set value directly
            if ((propertyType == SerializedPropertyType.Integer && property.intValue != defaultIndex) ||
                (propertyType == SerializedPropertyType.String  && property.stringValue != scenes[defaultIndex]))
                SetSceneValue(scenes[defaultIndex]);

            // add operation buttons
            void FreshLoadButton() {
                var btn = container.Q("load");
                if (btn != null) container.Remove(btn);

                string selected = scenes[defaultIndex];
                if (SceneManager.GetActiveScene().name == selected) return;
                // cur load scenes in hierarchy
                string[] loaded = new string[SceneManager.sceneCount];
                for (int i = 0; i < loaded.Length; i++)
                    loaded[i] = SceneManager.GetSceneAt(i).name;
                
                if (loaded.Contains(selected)) {
                    container.Add(new Button(() => {
                        var scene = EditorSceneManager.GetSceneByPath(GetScenePathByName(selected));
                        EditorSceneManager.CloseScene(scene, true);
                        FreshLoadButton();
                    }) { name = "load", text = "Close" });
                } else {
                    container.Add(new Button(() => {
                        EditorSceneManager.OpenScene(GetScenePathByName(selected), OpenSceneMode.Additive);
                        FreshLoadButton();
                    }) { name = "load", text = "Open" });
                }
            }

            if (((SceneAttribute)attribute).ShowOperator) {
                container.Add(new Button(() => GUIUtility.systemCopyBuffer = GetScenePathByName(scenes[defaultIndex])) { name = "copy", text = "Copy" });
                FreshLoadButton();
            }

            return container;
        }

        /// <summary>
        /// 通过名字获得场景
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetScenePathByName(string name) {
            return AssetDatabase.FindAssets("t:Scene").Select(x => AssetDatabase.GUIDToAssetPath(x))
                                                            .SingleOrDefault(x => Path.GetFileNameWithoutExtension(x) == name);
        }
    }
}