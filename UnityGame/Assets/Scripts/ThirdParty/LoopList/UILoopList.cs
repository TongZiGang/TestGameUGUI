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
    [RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
    [DisallowMultipleComponent]
    public class UILoopList : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
    {
        /// <summary>
        /// 
        /// </summary>
        public LoopListItemRenderer ListItemRenderer;

        /// <summary>
        /// 
        /// </summary>
        private Stack<Transform> itemPool = new Stack<Transform>();
        /// <summary>
        /// 预制件
        /// </summary>
        public GameObject itemPrefab;
        public GameObject GetObject(int index)
        {
            if (itemPool.Count == 0)
            {
                return Instantiate(itemPrefab);
            }
            Transform candidate = itemPool.Pop();
            candidate.gameObject.SetActive(true);
            return candidate.gameObject;
        }

        public void ReturnObject(Transform trans)
        {
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            itemPool.Push(trans);;
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
        private void Awake()
        {
            ScrollRect = GetComponent<LoopScrollRect>();
            ScrollRect.prefabSource = this;
            ScrollRect.dataSource = this;
        }

        public int ItemCount
        {
            get => ScrollRect.totalCount;
            set
            {
                ScrollRect.totalCount = value;
                ScrollRect.RefreshCells();
            }
        }

        public void ScrollToItem(int index)
        {
            ScrollRect?.ScrollToCell(index - 1,100.0f);
        }
    }
}