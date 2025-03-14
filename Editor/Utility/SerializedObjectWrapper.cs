using UnityEngine;

namespace Akatsuki.Framework.GUI.Editor {
    /// <summary>
    /// 用于封装任意对象，使其能被 SerializedObject 处理
    /// </summary>
    public class SerializedObjectWrapper : ScriptableObject {
        [SerializeReference]
        public object data;

        /// <summary>
        /// 创建一个新的 Wrapper 并赋值
        /// </summary>
        public static SerializedObjectWrapper Create(object target) {
            var wrapper = ScriptableObject.CreateInstance<SerializedObjectWrapper>();
            wrapper.data = target;
            return wrapper;
        }
    }
}
