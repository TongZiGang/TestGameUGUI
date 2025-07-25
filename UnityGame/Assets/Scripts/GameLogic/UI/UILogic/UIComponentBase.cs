using UnityEngine;

namespace CreatGame.UI
{
    /// <summary>
    /// 通用的导出预制件的基类
    /// </summary>
    public class UIComponentBase
    {
        /// <summary>
        /// 窗口预制件
        /// </summary>
        protected GameObject m_ComponentObject;
        /// <summary>
        /// 导出脚本
        /// </summary>
        protected UIViewExport m_ViewExport;
        /// <summary>
        /// 是否加载完成
        /// </summary>
        public bool IsPreLoad = false;
        public virtual void PreLoad(GameObject obj)
        {
            m_ComponentObject = obj;
            m_ViewExport = obj.GetComponent<UIViewExport>();
            IsPreLoad = true;
        }
    }
}