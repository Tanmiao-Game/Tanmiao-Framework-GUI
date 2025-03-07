using UnityEngine;

namespace Akatsuki.Framework.GUI.AttributeTest {
    [CreateAssetMenu(fileName = "TestScriptObject", menuName = "Test/TestScriptObject", order = 0)]
    public class TestScriptObject : ScriptableObject {
        public int value;
        public GameObject prefab;

        [TextArea]
        public string text;        
    }
}