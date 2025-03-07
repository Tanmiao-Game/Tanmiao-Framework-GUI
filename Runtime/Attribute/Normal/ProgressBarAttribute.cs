using System;
using System.Drawing;
using UnityEngine;

namespace Akatsuki.Framework.GUI {
    /// <summary>
    /// Progress bar for slider value
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ProgressBarAttribute : PropertyAttribute {
        public float Min { get; private set; }
        public float Max { get; private set; }
        public KnownColor Color { get; private set; }

        public ProgressBarAttribute(float min, float max) : this(min, max, KnownColor.SkyBlue) {}
        public ProgressBarAttribute(float min, float max, KnownColor color) {
            Min = min;
            Max = max;
            Color = color;
        }
    }
}