using System;
using System.Collections.Generic;
using UnityEngine;
using CreatGame.AssetBundle;

namespace CreatGame.UI
{
    public class UIManager : Singleton<UIManager>
    {
        private GameObject m_UIRoot;
        private Dictionary<UILayer,GameObject> m_UILayers;
        private Dictionary<UILayer, Queue<UIViewBase>> m_Windows;
        /// <summary>
        /// 
        /// </summary>
        public UIManager()
        {
            m_UIRoot = GameObject.Find("UIRoot");
            InitLayer();
            m_Windows = new Dictionary<UILayer, Queue<UIViewBase>>();
        }
        /// <summary>
        /// 初始化layer层的预制件
        /// </summary>
        private void InitLayer()
        {
            if (m_UIRoot == null)
            {
                return;
            }

            m_UILayers = new Dictionary<UILayer, GameObject>();

            foreach (var layer in Enum.GetValues(typeof(UILayer)))
            {
                var layerObj = m_UIRoot.transform.Find(Enum.GetName(typeof(UILayer), layer)).gameObject;
                layerObj.transform.SetParent(m_UIRoot.transform);
                layerObj.transform.localScale = Vector3.one;
                layerObj.transform.localPosition = Vector3.zero;
                layerObj.layer = LayerMask.NameToLayer("UI");
                m_UILayers.Add((UILayer)layer,layerObj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public UIViewBase OpenView<T>(UILayer layer) where T : UIViewBase , new()
        {
            var view = new T();
            //加载预制件
            AssetBundleManager.Instance.LoadGameObjectAsync(view.PrefabPath, (obj) =>
            {
                if (obj == null)
                {
                    Debug.LogError($"窗口加载失败{typeof(T).Name}");
                    return;
                }
                
                view.PreLoad(obj);
                if (m_UILayers.TryGetValue(layer, out var uiLayer))
                {
                    obj.transform.SetParent(uiLayer.transform);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale = Vector3.one;
                }
                view.InitView();

                if (m_Windows.ContainsKey(layer) == false)
                {
                    m_Windows.Add(layer, new Queue<UIViewBase>());
                }
            
                m_Windows[layer].Enqueue(view);
            });

            return view;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer"></param>
        public void CloseView(UILayer layer)
        {
            if (m_Windows.ContainsKey(layer))
            {
                var view = m_Windows[layer].Dequeue();
                view.CloseView();
            }
        }
    }
}