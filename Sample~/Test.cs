using UnityEngine;
using Akatsuki.Framework.GUI;
using UnityEngine.UIElements;

public class Test : MonoBehaviour {
    public Animator animator;

    [AnimatorParam("animator", AnimatorControllerParameterType.Bool)]
    public int animationHash;

    [AnimatorParam("animator", AnimatorControllerParameterType.Trigger)]
    public string animationParam;

    [AnimatorParam("animator")]
    public float animatorFloat;

    // test for property
    [field: HelpBox("this is property", HelpBoxMessageType.Error)]
    [field: SerializeField]
    public int TestPropertyIntValue { get; private set; }

    [Method]
    public void TestMethod() {
        Debug.Log("this is test method");
    }

    // if method have parameters
    // inspector will have framebox style to pack visual elements
    [Method("Test parameter method")]
    public void TestMethod(GameObject obj, Animator animator, string str) {
        Debug.Log($"value1 {obj}, value2 {animator}, value3 {str}");
    }
}