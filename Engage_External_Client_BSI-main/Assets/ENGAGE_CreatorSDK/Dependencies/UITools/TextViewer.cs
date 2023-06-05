using UnityEditor;
using UnityEngine;

namespace Engage.UI.Editor
{
    public class TextViewer : EditorWindow
    {
        private Vector2 scrollPos;

        public string Content { get; set; }

        private void OnGUI()
        {
            using (var scrollarea = new GuiTools.ScrollArea(scrollPos))
            {
                scrollPos = scrollarea.ScrollPosition;

                EditorGUILayout.TextArea(Content);
            }

            if (GUILayout.Button("Done"))
            {
                Close();
            }
        }
    }
}
