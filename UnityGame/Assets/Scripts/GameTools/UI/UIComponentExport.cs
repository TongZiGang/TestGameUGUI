using System;
using UnityEngine;
using System.Collections.Generic;

namespace CreatGame.UI
{
    public class UIComponentExport : MonoBehaviour
    {
        [Serializable]
        public class UIEntry
        {
            public string key;
            public GameObject prefab;
            public string selectedComponentName; // 存储选择的组件类型名
        }
        /// <summary>
        /// 
        /// </summary>
        public List<UIEntry> entries = new List<UIEntry>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
}