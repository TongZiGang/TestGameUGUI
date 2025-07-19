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

        public List<UIEntry> entries = new List<UIEntry>();
    }
}