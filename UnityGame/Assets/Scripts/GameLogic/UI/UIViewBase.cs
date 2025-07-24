using UnityEngine;

namespace CreatGame.UI
{
    public class UIViewBase : UILoader
    {
        /// <summary>
        /// 预制件的Addressables地址
        /// 由导出代码自己添加
        /// </summary>
        public virtual string PrefabPath { get; set; }
        /// <summary>
        /// 窗口预制件
        /// </summary>
        protected GameObject m_ViewObject;
        /// <summary>
        /// 导出脚本
        /// </summary>
        protected UIViewExport m_ViewExport;
        /// <summary>
        /// 是否加载完成
        /// </summary>
        public bool IsPreLoad = false;
        /// <summary>
        /// 加载窗口的时候需要预先加载的东西
        /// </summary>
        public virtual void PreLoad(GameObject viewObject)
        {
            m_ViewObject = viewObject;
            m_ViewExport = viewObject.GetComponent<UIViewExport>();
            IsPreLoad = true;
        }
        /// <summary>
        /// 初始化界面
        /// </summary>
        public virtual void InitView()
        {
            
        }
        /// <summary>
        /// 关闭窗口的时候的调用
        /// </summary>
        public virtual void CloseView()
        {
            DisposeGameObjectCache();
            GameObject.Destroy(m_ViewObject);
        }
        
        protected GameObject GetGameObject(string name)
        {
            for (int i = 0; i < m_ViewExport.entries.Count; i++)
            {
                if (m_ViewExport.entries[i].key == name)
                {
                    return m_ViewExport.entries[i].prefab;
                }
            }

            return null;
        }
    }
}