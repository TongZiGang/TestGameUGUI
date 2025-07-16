using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;


[CustomEditor(typeof(UIExportTool))]
public class UIExportToolEditor : Editor
{
    private ReorderableList reorderableList;

    private void OnEnable()
    {
        reorderableList = new ReorderableList(
            serializedObject,
            serializedObject.FindProperty("entries"),
            true, true, true, true);

        reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "UI Prefab Entries");
        };

        reorderableList.elementHeightCallback = (int index) =>
        {
            return EditorGUIUtility.singleLineHeight + 6f;
        };

        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            // drawElementCallback 中的内容
            SerializedProperty keyProp = element.FindPropertyRelative("key");
            SerializedProperty prefabProp = element.FindPropertyRelative("prefab");
            SerializedProperty selectedNameProp = element.FindPropertyRelative("selectedComponentName");

            GameObject prefab = prefabProp.objectReferenceValue as GameObject;

// 查找组件类型
            List<string> uguiComponentNames = new List<string>();
            if (prefab != null)
            {
                uguiComponentNames = prefab.GetComponents<Component>()
                    .Where(c => c != null && IsUGUIComponent(c))
                    .Select(c => c.GetType().Name)
                    .Distinct()
                    .ToList();
            }

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float padding = 4f;
            float thirdWidth = (rect.width - 2 * padding) / 3f;

            Rect keyRect = new Rect(rect.x, rect.y + 2, thirdWidth, lineHeight);
            Rect prefabRect = new Rect(rect.x + thirdWidth + padding, rect.y + 2, thirdWidth, lineHeight);
            Rect popupRect = new Rect(rect.x + 2 * (thirdWidth + padding), rect.y + 2, thirdWidth, lineHeight);

// key 字段
            EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none);

// prefab 字段
            EditorGUI.PropertyField(prefabRect, prefabProp, GUIContent.none);

// 下拉框
            if (uguiComponentNames.Count > 0)
            {
                int selectedIndex = Mathf.Max(0, uguiComponentNames.IndexOf(selectedNameProp.stringValue));
                selectedIndex = EditorGUI.Popup(popupRect, selectedIndex, uguiComponentNames.ToArray());
                selectedNameProp.stringValue = uguiComponentNames[selectedIndex];
            }
            else
            {
                EditorGUI.LabelField(popupRect, "无UGUI组件");
            }

        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
    
    /// <summary>
    /// 判断是否是常见UGUI组件（你可以自行扩展）
    /// </summary>
    private bool IsUGUIComponent(Component component)
    {
        return component is Graphic // 基类，包含 Image、Text、RawImage
               || component is Button
               || component is Toggle
               || component is Slider
               || component is ScrollRect
               || component is Dropdown
               || component is InputField;
    }
}