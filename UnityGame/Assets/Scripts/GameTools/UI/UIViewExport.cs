using System;
using UnityEngine;
using System.Collections.Generic;

public class UIViewExport : MonoBehaviour
{
    [Serializable]
    public class UIEntry
    {
        public string key;
        public GameObject prefab;
        public string selectedComponentName; // 存储选择的组件类型名
    }

    public List<UIEntry> entries = new List<UIEntry>();
    
    public GameObject GetGameObject(string name)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i].key == name)
            {
                return entries[i].prefab;
            }
        }

        return null;
    }
}
