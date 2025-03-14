using UnityEngine;

namespace Akatsuki.Framework.GUI.AttributeTest {
    public class Test3 : MonoBehaviour {
        [Selection(typeof(CommonForTest))]
        public int testForSelection1;
        [Selection(typeof(CommonForTest))]
        public string textForSelection2;
        [Selection(typeof(CommonForTest.CommonForTestOfSub), true)]
        public string textForSelection3;

        [Space]
        [Selection]
        [SerializeReference]
        public ICommonForTest testInterfaceSelection;

        [Selection]
        [SerializeReference]
        public AbstractForTest testAbstractSelection;

        [Space]
        [SelectionPath]
        public int testForPath1;
        [SelectionPath]
        public string testForPath2;
    }
}
