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
    }

    private void InitReorderableList()
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
        if (reorderableList == null)
        {
            InitReorderableList();
        }
        reorderableList?.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("导出代码"))
        {
            ExportCode();
        }
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
    /// <summary>
    /// 导出代码
    /// </summary>
    private void ExportCode()
    {
        string savePath = Application.dataPath + "/Scripts/GameLogic/Export/UGUI/";
        if (string.IsNullOrEmpty(savePath))
            return;

        string className = $"UI{target.GameObject().name}";
        string filename = System.IO.Path.Combine(savePath, $"{className}.cs");
        
        var sb = new System.Text.StringBuilder();
        
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine("using UnityEngine.UI;");
        sb.AppendLine();
        sb.AppendLine("namespace CreatGame.UI");
        sb.AppendLine("{");
        sb.AppendLine($"    public class {className} : UIViewBase");
        sb.AppendLine("    {");

        // 字段定义

        for (int i = 0; i < reorderableList.count; i++)
        {
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(i);
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// ");
            sb.AppendLine("        /// </summary>");

            sb.AppendLine($"        public {element.FindPropertyRelative("selectedComponentName").stringValue} {element.FindPropertyRelative("key").stringValue}");
        }
        sb.AppendLine("        public override void PreLoad(GameObject view)");
        sb.AppendLine("        {");
        sb.AppendLine("            base.PreLoad(view);");
        sb.AppendLine();
        for (int i = 0; i < reorderableList.count; i++)
        {
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(i);
            var filedName = element.FindPropertyRelative("key").stringValue;
            var typeName = element.FindPropertyRelative("selectedComponentName").stringValue;
            sb.AppendLine($"            {filedName} = GetGameObject(nameof({filedName})).GetComponent<{typeName}>();");
        }
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        Debug.Log(sb.ToString());
        System.IO.File.WriteAllText(filename, sb.ToString());
        Debug.Log($"✅ 导出成功：{filename}");

        AssetDatabase.Refresh();
    }
}