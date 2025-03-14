using System;

namespace Akatsuki.Framework.GUI.AttributeTest {
    public interface ICommonForTestEmpty {}

    public interface ICommonForTest {}

    public interface ICommonForTestOfSub : ICommonForTest {}

    [Serializable]
    public class CommonForTest : ICommonForTest {
        public const string A = "A";
        public const string B = "B";
        public const string C = "C";

        public string value;

        public class CommonForTestOfSub {
            public const string SubA = "SubA";
            public const string SubB = "SubB";
            public const string SubC = "SubC";
        }
    }

    [Serializable]
    public class CommonForTestSecond : CommonForTest {
        public CommonForTest testA;
        public CommonForTest textB;
    }

    [Serializable]
    public class CommonForTestOfSub : ICommonForTestOfSub {
        public string test;
    }

    public abstract class AbstractForTest : ICommonForTest {
        public int abstractIntValue;
    }

    public class AbstractForTestA : AbstractForTest {
        public float abstractFloatValue;
    }

    public class AbstractForTestB : AbstractForTest {
        public string abstractStringValue;
    }
}
