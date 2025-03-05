using UnityEngine;

namespace Akatsuki.Framework.GUI.Test {
    public class Test5 : MonoBehaviour {
        [Layer]
        public int layerIntValue;
        [Layer]
        public string layerStrValue;

        [SortingLayer]
        public int sortingLayerIntValue;
        [SortingLayer]
        public string sortingLayerStrValue;
    }
}
