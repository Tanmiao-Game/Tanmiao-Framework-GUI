using UnityEngine;

namespace Akatsuki.Framework.GUI.AttributeTest {
    public class Test7 : MonoBehaviour
    {
        [OnValueChanged(nameof(OnHealthChanged))]
        [ProgressBar(0, 100, System.Drawing.KnownColor.IndianRed)]
        public int health;

        [ProgressBar(0, 200, System.Drawing.KnownColor.SkyBlue)]
        [OnValueChanged(nameof(OnExpChanged))]
        public float exp;

        [OnValueChanged(nameof(OnValueChangedCallback2))]
        public int value2;

        public void OnHealthChanged() {
            Debug.Log($"Health: {health}");
        }

        public void OnExpChanged() {
            Debug.Log($"Exp: {exp}");
        }

        public void OnHealthChanged(int temp) {
            Debug.Log($"2. {temp}");
        }

        public void OnValueChangedCallback2(int value) {
            Debug.Log($"3. {value2}");
        }
    }
}
