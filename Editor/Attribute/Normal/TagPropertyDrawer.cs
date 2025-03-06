using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagPropertyDrawer : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var container = new VisualElement();

            var propertyType = property.propertyType;
            if (propertyType != SerializedPropertyType.String) {
                container.Add(new PropertyField(property));
                return container;
            }

            // Get tags
            List<string> tags = InternalEditorUtility.tags.ToList();
            int defaultIndex = tags.FindIndex(x => x == property.stringValue);
            defaultIndex = defaultIndex == -1 ? 0 : defaultIndex;

            void SetTagValue(string value) {
                property.stringValue = value;
                property.serializedObject.ApplyModifiedProperties();
            }

            var dropDown = new DropdownField(property.displayName, tags, defaultIndex).BuildFieldAlignStyle();
            dropDown.RegisterValueChangedCallback(evt => SetTagValue(evt.newValue));
            container.Add(dropDown);

            // check value and if value is empty, set value directly
            if (property.stringValue != tags[defaultIndex])
                SetTagValue(tags[defaultIndex]);

            return container;
        }
    }
}