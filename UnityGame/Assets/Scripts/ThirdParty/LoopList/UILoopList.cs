using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CreatGame.UI
{
    /// <summary>
    /// item提供者
    /// </summary>
    public delegate string LoopListItemProvider(int index);
    /// <summary>
    /// item渲染函数
    /// </summary>
    public delegate void LoopListItemRenderer(GameObject item,int index);
    public class UILoopList : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
    {
        /// <summary>
        /// 
        /// </summary>
        public LoopListItemProvider ListItemProvider;
        /// <summary>
        /// 
        /// </summary>
        public LoopListItemRenderer ListItemRenderer;
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, Stack<GameObject>> itemPool = new Dictionary<string, Stack<GameObject>>();
        /// <summary>
        /// 预制件
        /// </summary>
        public GameObject itemPrefab;
        public GameObject GetObject(int index)
        {
            string itemPath = String.Empty;
            if (ListItemProvider != null)
            {
                itemPath = ListItemProvider(index);
            }
            else
            {
                itemPath = "Default";
            }

            if (itemPool.TryGetValue(itemPath, out Stack<GameObject> itemList) == false)
            {
                itemList = new Stack<GameObject>();
                itemPool.Add(itemPath, itemList);
            }

            if (itemList.Count == 0)
            {
                if (ListItemProvider == null)
                {
                    if (itemPrefab != null)
                    {
                        itemList.Push(Instantiate(itemPrefab));
                    }
                    else
                    {
                        Debug.LogError("ItemPrefab is null and ListItemProvider == null");
                        return null;
                    }
                }
                else
                {
                    var item = AssetBundle.AssetBundleManager.Instance.LoadGameObject(itemPath);
                    var export = item.GetComponent<UIViewBase>();
                    if (export == null)
                    {
                        Debug.LogError("预制件没有绑定导出代码的UIViewBase");
                    }

                    export.PreLoad(item);
                    itemList.Push(item);
                }
            }

            return itemList.Pop();
        }

        public void ReturnObject(Transform trans)
        {
            var export = trans.GetComponent<UIViewBase>();
            if (export != null)
            {
                if (itemPool.TryGetValue(export.PrefabPath, out Stack<GameObject> itemList) ==false)
                {
                    itemList = itemPool["Default"];
                }
                
                itemList.Push(trans.gameObject);
            }
            else
            {
                Debug.LogError("返回对象池失败");
            }
        }

        public void ProvideData(Transform transform, int idx)
        {
            if (ListItemRenderer == null)
            {
                Debug.LogError("ListItemRenderer is null");
                return;
            }
            ListItemRenderer(transform.gameObject, idx);
        }

        /// <summary>
        /// 
        /// </summary>
        public LoopScrollRect ScrollRect { get; private set; }

        private void Start()
        {
            ScrollRect = GetComponent<LoopScrollRect>();
            ScrollRect.prefabSource = this;
            ScrollRect.dataSource = this;
        }

        public int ItemCount
        {
            get => ScrollRect.totalCount;
            set => ScrollRect.totalCount = value;
        }

        public void RefillCells()
        {
            ScrollRect?.RefillCells();
        }
        
    }
}