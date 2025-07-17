using System.IO;
using System.Net.Mime;
using Luban;
using UnityEngine;

namespace CreatGame
{
    /// <summary>
    /// 表格
    /// </summary>
    public class ConfigManager : Singleton<ConfigManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly cfg.Tables tables;
        /// <summary>
        /// 
        /// </summary>
        public cfg.Tables Tables => tables;

        public ConfigManager()
        {
            var tablesCtor = typeof(cfg.Tables).GetConstructors()[0];
            // 根据cfg.Tables的构造函数的Loader的返回值类型决定使用json还是ByteBuf Loader
            System.Delegate loader = new System.Func<string, ByteBuf>(LoadByteBuf);
            tables = (cfg.Tables)tablesCtor.Invoke(new object[] {loader});
        }

        private static ByteBuf LoadByteBuf(string file)
        {
            return new ByteBuf(File.ReadAllBytes($"{Application.dataPath}/AssetBundle/Config/{file}.bytes"));
        }
    }
}