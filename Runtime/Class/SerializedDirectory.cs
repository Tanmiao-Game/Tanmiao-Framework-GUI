using System;
using System.IO;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// 可序列化文件夹
    /// </summary>
    [Serializable]
    public class SerializedDirectory : ISerializationCallbackReceiver {

        /// <summary>
        /// 项目内路径
        /// </summary>
        [field: SerializeField]
        public string ProjectPath { get; private set; }

        /// <summary>
        /// 文件夹完整路径
        /// </summary>
        public string FullPath => GetFullAssetsPath(ProjectPath);

#if UNITY_EDITOR
        /// <summary>
        /// 编辑器下是否展开，默认 true，仅编辑器使用
        /// </summary>
        [SerializeField]
        [HideInInspector]
        internal bool foldout = true;
#endif

        public SerializedDirectory(string path) {
            ProjectPath = path;
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize() { }

        public static implicit operator string(SerializedDirectory sdy) {
            return sdy.ProjectPath;
        }

        public static implicit operator SerializedDirectory(string path) {
            return new SerializedDirectory(path);
        }

        #region File
        /// <summary>
        /// 获得资源路径
        /// </summary>
        /// <param name="path">
        ///     例：C://Demo/Assets/Res/Textures/Test.png => Assets/Res/Textures/Test.png
        /// </param>
        /// <returns></returns>
        public static string GetRelativeAssetsPath(string path) {
            return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
        }

        /// <summary>
        /// 获得详细路径
        /// </summary>
        /// <param name="path">
        ///     例：Assets/Res/Textures/Test.png => C://Demo/Assets/Res/Textures/Test.png
        /// </param>
        /// <returns></returns>
        public static string GetFullAssetsPath(string path) {
            return Path.Combine(Application.dataPath, path.Replace("Assets/", ""));
        }
        #endregion
    }
}