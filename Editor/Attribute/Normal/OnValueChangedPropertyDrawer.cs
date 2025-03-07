using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    [CustomPropertyDrawer(typeof(OnValueChangedAttribute))]
    public class OnValueChangedPropertyDrawer : PropertyDrawer {
        protected static BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var container = new VisualElement();

            var methodName = ((OnValueChangedAttribute)attribute).MethodName;
            var method = property.serializedObject.targetObject.GetType().GetMethods(Flags).SingleOrDefault(method => method.Name == methodName && method.GetParameters().Length == 0);
            if (method == null) {
                container.AddHelpBoxToProperty(property, $"Cant find method named {methodName} with non-parameter");
            } else {
                var field = new PropertyField(property, $"[Callback]{property.displayName}");
                container.Add(field);
                field.TrackPropertyValue(property, _ => method.Invoke(property.serializedObject.targetObject, null));
            }

            return container;
        }
    }
}