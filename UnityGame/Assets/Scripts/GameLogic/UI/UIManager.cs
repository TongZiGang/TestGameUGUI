using System.Collections.Generic;

namespace CreatGame.UI
{
    public class UIManager : Singleton<UIManager>
    {
        private Dictionary<UILayer, Queue<UIViewBase>> m_Windows;
        /// <summary>
        /// 
        /// </summary>
        public UIManager()
        {
        }
    }
}