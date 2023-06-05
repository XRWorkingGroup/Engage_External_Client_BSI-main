using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Engage.UI.Editor
{
    public static class GuiTools
    {
        public static void DisplayText(string title, string content)
        {
            var window = EditorWindow.CreateWindow<TextViewer>(title);
            window.Content = content;
            window.ShowUtility();
        }

        public static string ToIdString(this int id) => id < 0 ? "-" : id.ToString();
        public static string ToIdString(this int? id) => id.ToString() ?? "-";

        public static void DrawButton(string label, Action action, bool? enabled = null, params GUILayoutOption[] options)
        {
            DrawButton(new GUIContent(label), action, enabled, options);
        }

        public static void DrawButton(GUIContent label, Action action, bool? enabled = null, params GUILayoutOption[] options)
        {
            var guiEnabled = GUI.enabled;

            if (enabled.HasValue)
            {
                GUI.enabled = enabled.Value;
            }

            if (GUILayout.Button(label, options))
            {
                action?.Invoke();
            }

            GUI.enabled = guiEnabled;
        }

        public static void DrawButton(string label, Action action = null, params GUILayoutOption[] options)
        {
            DrawButton(new GUIContent(label), action, options);
        }

        public static void DrawButton(GUIContent label, Action action = null, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(label, options))
            {
                action?.Invoke();
            }
        }

        public static void DrawColumnHeader(string label, Action action = null, params GUILayoutOption[] options)
        {
            DrawColumnHeader(new GUIContent(label), action, options);
        }

        public static void DrawColumnHeader(GUIContent label, Action action = null, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(label, EditorStyles.boldLabel, options))
            {
                action?.Invoke();
            }
        }

        public static void DrawProperty(string label, string value, Color color)
        {
            DrawProperty(new GUIContent(label), value, color);
        }

        public static void DrawProperty(GUIContent label, string value, Color color)
        {
            Color wasColor = GUI.contentColor;
            GUI.contentColor = color;

            EditorGUILayout.LabelField(label, value);

            GUI.contentColor = wasColor;
        }

        public static string BrowseFolderPath(string title, string startPath)
        {
            var buildPath = new System.IO.DirectoryInfo(startPath);

            if (!buildPath.Exists)
            {
                buildPath.Create();
            }

            var path = EditorUtility.OpenFolderPanel(
                title,
                buildPath.Parent?.FullName ?? buildPath.Name,
                buildPath.Name
                );

            return string.IsNullOrEmpty(path) ? startPath : path;
        }

        public static string FolderSelectionField(string label, string startPath)
        {
            EditorGUILayout.LabelField(label);

            using (var bundlePathPanel = new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.SelectableLabel(startPath);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(30));

                if (GUI.Button(new Rect(bundlePathPanel.rect.x + bundlePathPanel.rect.width - 30, bundlePathPanel.rect.y + (bundlePathPanel.rect.height * 0.5f) - 10, 20, 20), new GUIContent(Labels.Ellipsis)))
                {
                    return GuiTools.BrowseFolderPath(label, startPath);
                }
                else
                {
                    return startPath;
                }
            }
        }

        public class EnabledScope : IDisposable
        {
            private bool wasEnabled;

            public EnabledScope(bool enabled = false)
            {
                wasEnabled = GUI.enabled;
                GUI.enabled = enabled;
            }

            public void Dispose()
            {
                GUI.enabled = wasEnabled;
            }
        }

        public class ColorScope : IDisposable
        {
            private Color wasColor;
            private Color wasBackgroundColor;

            public ColorScope(Color? color = null, Color? backgroundColor = null)
            {
                wasColor = GUI.contentColor;
                wasBackgroundColor = GUI.backgroundColor;

                if (color.HasValue)
                    GUI.contentColor = color.Value;

                if (backgroundColor.HasValue)
                    GUI.backgroundColor = backgroundColor.Value;
            }

            public void Dispose()
            {
                GUI.contentColor = wasColor;
                GUI.backgroundColor = wasBackgroundColor;
            }
        }

        public class ScrollArea : IDisposable
        {
            public EditorGUILayout.HorizontalScope ViewportScope { get; private set; }
            public EditorGUILayout.ScrollViewScope ScrollViewScope { get; private set; }
            public Vector2 ScrollPosition { get; set; }
            public Rect Viewport { get; private set; }

            public ScrollArea(Vector2 scrollPosition, params GUILayoutOption[] options) : this(scrollPosition, null, options) {}

            public ScrollArea(Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options)
            {
                ViewportScope = new EditorGUILayout.HorizontalScope();
                ScrollViewScope = style == null ? new EditorGUILayout.ScrollViewScope(scrollPosition, options)
                    : new EditorGUILayout.ScrollViewScope(scrollPosition, style, options);

                ScrollPosition = ScrollViewScope.scrollPosition;
                Viewport = new Rect(ScrollPosition, ViewportScope.rect.size);
            }

            public void Dispose()
            {
                ScrollViewScope.Dispose();
                ViewportScope.Dispose();
            }
        }


        #region Extensions
        public static string ToPretty(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var chars = input.Trim('_').ToCharArray();
            var output = new List<char>(chars.Length);
            var lastChar = '_';

            foreach (char c in chars)
            {
                if (c == '_')
                {
                    output.Add(' ');
                }
                else if (lastChar == '_')
                {
                    output.Add(char.ToUpper(c));
                }
                else if (char.IsUpper(c) && char.IsLower(lastChar))
                {
                    output.Add(' ');
                    output.Add(c);
                }
                else
                {
                    output.Add(c);
                }

                lastChar = c;
            }

            return new string(output.ToArray());
        }

        #endregion
    }
}
