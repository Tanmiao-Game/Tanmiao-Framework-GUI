using UnityEngine;

namespace Akatsuki.Framework.GUI.AttributeTest {
    public class Test3 : MonoBehaviour {
        [Selection(typeof(CommonForTest))]
        public int testForSelection1;
        [Selection(typeof(CommonForTest))]
        public string textForSelection2;
        [Selection(typeof(CommonForTest.CommonForTestOfSub), true)]
        public string textForSelection3;

        [SelectionPath]
        public int testForPath1;
        [SelectionPath]
        public string testForPath2;
    }
}
