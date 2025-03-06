using UnityEngine;

namespace Akatsuki.Framework.GUI.Test {
    public class Test5 : MonoBehaviour {
        [Layer]
        public int layerIntValue;
        [Layer]
        public string layerStrValue;

        [Space]
        [SortingLayer]
        public int sortingLayerIntValue;
        [SortingLayer]
        public string sortingLayerStrValue;

        [Space]
        [Tag]
        public string tagValue;

        [Space]
        [Scene]
        public string sceneName;
        [Scene]
        public int sceneIndex;
    }
}
