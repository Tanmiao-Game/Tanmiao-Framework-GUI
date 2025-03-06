using System.Collections;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(PreviewAttribute))]
    public class PreviewPropertyDrawer : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var container = new VisualElement();

            var field = new PropertyField(property);
            container.Add(field);

            var iconField = new VisualElement() { style = { alignSelf = Align.Center } };
            container.Add(iconField);

            if (property.propertyType == SerializedPropertyType.ObjectReference) {
                field.RegisterValueChangeCallback(_ => GetPreviewIcon(iconField, property.objectReferenceValue));
                GetPreviewIcon(iconField, property.objectReferenceValue);
            }

            return container;
        }
        
        private async void GetPreviewIcon(VisualElement element, UnityEngine.Object value) {
            var icon = AssetPreview.GetAssetPreview(value);
            for (int i = 0; i < 50; i++) {
                if (icon != null) break;
                await Task.Delay(10);
                icon = AssetPreview.GetAssetPreview(value);
            }
            
            if (element != null && icon != null) {
                element.style.backgroundImage = icon;
                element.style.height = Mathf.Min(icon.height, 64);
                element.style.width = Mathf.Min(icon.width, 64);
            }
        }
    }
}