namespace CreatGame.UI
{
    public class UIViewBase
    {
        /// <summary>
        /// 预制件的路径
        /// </summary>
        public virtual string PrefabPath { get; set; }
        /// <summary>
        /// 加载窗口的时候需要预先加载的东西
        /// </summary>
        public virtual void PreLoad()
        {
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
            
        }
    }
}