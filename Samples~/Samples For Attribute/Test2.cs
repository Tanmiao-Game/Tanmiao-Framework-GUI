using UnityEngine;

namespace Akatsuki.Framework.GUI.AttributeTest {
    public class Test2 : MonoBehaviour
    {
        [Expendable]
        public TestScriptObject scriptObject;

        [Expendable]
        public Animator animator;

        [Expendable]
        public Test1 test;
    }
}
