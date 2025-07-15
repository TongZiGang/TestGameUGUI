namespace CreatGame
{
    public class Singleton<T> where T : class, new()
    {
        private static T _instance;
        private static readonly object _lock = new object();
        
        [UnityEngine.Scripting.Preserve]
        protected Singleton()
        {
        }

        /// <summary>
        /// 单例对象（线程安全，双重锁定）
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }

                return _instance;
            }
        }
    }
}
