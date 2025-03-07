using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarPropertyDrawer : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            if (property.numericType == SerializedPropertyNumericType.Unknown) {
                return new PropertyField(property);
            }

            var attr = (ProgressBarAttribute)attribute;
            var color = System.Drawing.Color.FromKnownColor(attr.Color);
            float value = 0;
            if (property.propertyType == SerializedPropertyType.Float) value = property.floatValue;
            if (property.propertyType == SerializedPropertyType.Integer) value = property.intValue;

            // 进度表现
            var progressBar = new ProgressBar() {
                name = property.displayName,
                title = $"{property.displayName}({value})",
                lowValue = attr.Min,
                highValue = attr.Max,
                value = value,
            };
            var actualBar = progressBar.Q(className: "unity-progress-bar__progress");
            actualBar.style.backgroundColor = new Color(color.R / 255f, color.G / 255f, color.B / 255f, 1f);
            var label = progressBar.Q<Label>();
            var edit  = new FloatField() {
                style = {
                    display = DisplayStyle.None,
                    minWidth = 50,
                }
            };
            label.parent.Add(edit);

            // 进度条事件
            bool isDown = false;
            // 鼠标位置计算数值
            void CalculateProgressValue(Vector2 mousePosition) {
                var local = progressBar.WorldToLocal(mousePosition);
                var precent = local.x / progressBar.worldBound.width;
                var value = precent * (attr.Max - attr.Min) + attr.Min;
                SetProgressValue(value);
            }
            // 设置进度条数值
            void SetProgressValue(float value) {
                progressBar.value = value;
                if (property.propertyType == SerializedPropertyType.Float) {
                    property.floatValue = value;
                    progressBar.title = $"{property.displayName}({System.Math.Round(value, 2)})";
                }
                if (property.propertyType == SerializedPropertyType.Integer) {
                    property.intValue = (int)value;
                    progressBar.title = $"{property.displayName}({(int)value})";
                }
                property.serializedObject.ApplyModifiedProperties();
            }

            // 鼠标事件回调
            progressBar.RegisterCallback<MouseDownEvent>(evt => {
                if (evt.button == 0) {
                    // left button down
                    isDown = true;
                    CalculateProgressValue(evt.mousePosition);
                } else if (evt.button == 1) {
                    // right button down
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Edit"), false, () => {
                        if (edit.style.display == DisplayStyle.Flex) return;
                        label.ActiveOrNot(false);
                        edit.value = progressBar.value;
                        edit.ActiveOrNot(true);
                        edit.Focus();
                    });
                    menu.AddItem(new GUIContent("Set To Min Value"), false, () => SetProgressValue(attr.Min));
                    menu.AddItem(new GUIContent("Set To Max Value"), false, () => SetProgressValue(attr.Max));

                    if (property.numericType == SerializedPropertyNumericType.Float) {
                        menu.AddItem(new GUIContent("Convert To Int"), false, () => SetProgressValue((int)progressBar.value));
                    }

                    menu.ShowAsContext();
                }
            });
            progressBar.RegisterCallback<MouseUpEvent>(_ => isDown = false);
            progressBar.RegisterCallback<MouseMoveEvent>(evt => {
                if (isDown) {
                    CalculateProgressValue(evt.mousePosition);
                }
            });
            // 鼠标离开直接退出按下状态
            progressBar.RegisterCallback<MouseLeaveEvent>(_ => isDown = false);

            // 编辑文本框的回调
            edit.RegisterCallback<BlurEvent>(_ => {
                label.ActiveOrNot(true);
                edit.ActiveOrNot(false);
                SetProgressValue(edit.value);
            });

            return progressBar;
        }
    }
}