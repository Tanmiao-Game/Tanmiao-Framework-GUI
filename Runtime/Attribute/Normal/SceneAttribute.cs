using System;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// Scene name or index (not path), default get all scenes in project and show operations
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SceneAttribute : PropertyAttribute {
        /// <summary>
        /// Is build scene only, else find all scene in assets
        /// </summary>
        public bool BuildSceneOnly { get; private set; }

        /// <summary>
        /// Show operation buttons
        /// </summary>
        public bool ShowOperator { get; private set; }

        public SceneAttribute() : this(false, true) {}
        public SceneAttribute(bool buildSceneOnly) : this(buildSceneOnly, true) {}
        public SceneAttribute(bool buildSceneOnly, bool showOperator) {
            BuildSceneOnly = buildSceneOnly;
            ShowOperator = showOperator;
        }
    }
}