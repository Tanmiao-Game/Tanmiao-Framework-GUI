using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Akatsuki.Framework.GUI.Editor {
    /// <summary>
    /// 拖拽元素
    /// </summary>
    public class DragAndDropElement : VisualElement
    {
        public Action<UnityEngine.Object[]> onDropEvent;
        public string label;

        public new class UxmlFactory : UxmlFactory<DragAndDropElement, UxmlTraits> { }

        public DragAndDropElement() : this("Drag Target To Here") {}
        public DragAndDropElement(string label) {
            this.BuildFrameboxStyle(2);
            this.BuildDarkBackgroundColor();
            style.alignItems = Align.Center;
            style.justifyContent = Justify.Center;
            style.height = 50f;
            this.label = label;

            // 添加一个提示文本
            Add(new Label(label));

            // 注册拖拽事件
            RegisterCallback<DragEnterEvent>(OnDragEnter);
            RegisterCallback<DragLeaveEvent>(OnDragLeave);
            RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
            RegisterCallback<DragPerformEvent>(OnDragPerform);
        }

        private void OnDragEnter(DragEnterEvent evt)
        {
            this.BuildSelectedBackgroundColor();
        }

        private void OnDragLeave(DragLeaveEvent evt)
        {
            this.BuildDarkBackgroundColor();
        }

        private void OnDragUpdated(DragUpdatedEvent evt)
        {
            // 修改拖拽时的可视效果
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }

        private void OnDragPerform(DragPerformEvent evt)
        {
            // 接受拖拽的对象
            DragAndDrop.AcceptDrag();

            // // 示例：打印所有拖拽的对象名称
            // foreach (var obj in DragAndDrop.objectReferences)
            // {
            //     Debug.Log("拖拽对象：" + obj.name);
            // }
            onDropEvent?.Invoke(DragAndDrop.objectReferences);

            // 恢复背景色
            this.BuildDarkBackgroundColor();
        }
    }
}