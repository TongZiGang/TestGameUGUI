using System.Collections.Generic;
using UnityEngine;

namespace CreatGame.UI
{
    public class UIManager : Singleton<UIManager>
    {
        private Dictionary<UILayer,GameObject> m_UILayers;
        private Dictionary<UILayer, Queue<UIViewBase>> m_Windows;
        /// <summary>
        /// 
        /// </summary>
        public UIManager()
        {
            m_UILayers = new Dictionary<UILayer, GameObject>();
            m_Windows = new Dictionary<UILayer, Queue<UIViewBase>>();
            
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
            
            view.InitView();
            return view;
        }
    }
}