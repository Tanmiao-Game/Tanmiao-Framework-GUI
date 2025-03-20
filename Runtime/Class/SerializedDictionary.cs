using System;
using System.Collections.Generic;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// 序列化字典
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();
        [SerializeField]
        private List<TValue> vals = new List<TValue>();

        public void OnBeforeSerialize() {
            // 清空旧数据
            keys.Clear();
            vals.Clear();

            // 存储键值对
            foreach (var kvp in this) {
                keys.Add(kvp.Key);
                vals.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize() {
            // 清空并恢复数据
            this.Clear();
            if (keys.Count != vals.Count) {
                Debug.LogError($"SerializedDictionary: Keys ({keys.Count}) and Values ({vals.Count}) count mismatch!");
                return;
            }

            for (int i = 0; i < keys.Count; i++) {
                this[keys[i]] = vals[i];
            }
        }
    }
}