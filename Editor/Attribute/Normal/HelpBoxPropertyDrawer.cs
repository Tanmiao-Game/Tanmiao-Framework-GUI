using UnityEditor;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxPropertyDrawer : PropertyDrawer {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var container = new VisualElement();

            var attr = (HelpBoxAttribute)attribute;
            container.AddHelpBoxToProperty(property, attr.Message, attr.Type);

            return container;
        }
    }
}