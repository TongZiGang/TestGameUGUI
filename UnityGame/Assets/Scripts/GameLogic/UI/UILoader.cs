using UnityEngine;
using CreatGame.AssetBundle;
using System.Collections.Generic;
using UnityEngine.U2D;

namespace CreatGame.UI
{
    /// <summary>
    /// ui上的资源加载器
    /// </summary>
    public class UILoader
    {
        private List<GameObject> m_GameObjectCache = new List<GameObject>();
        private List<Sprite> m_SpriteCache = new List<Sprite>();
        /// <summary>
        /// 同步资源加载
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject LoadGameObject(string name)
        {
            var gameObj = AssetBundleManager.Instance.LoadGameObject(name);
            if (gameObj == null)
            {
                return null;
            }
            
            m_GameObjectCache.Add(gameObj);

            return gameObj;
        }
        /// <summary>
        /// 异步加载预制件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        public void LoadGameObjectAsync(string name, System.Action<GameObject> callback)
        {
            AssetBundleManager.Instance.LoadGameObjectAsync(name, (obj) =>
            {
                if (obj != null)
                {
                    m_GameObjectCache.Add(obj);
                }
                callback(obj);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="atlasName"></param>
        /// <param name="spriteName"></param>
        /// <returns></returns>
        public Sprite LoadSprite(string atlasName, string spriteName)
        {
            var spriteAtlas = AssetBundleManager.Instance.LoadSpriteAtlas(atlasName);
            if (spriteAtlas == null)
            {
                return null;
            }
            
            return spriteAtlas.GetSprite(spriteName);
        }

        public void DisposeGameObjectCache()
        {
            var count = m_GameObjectCache.Count;
            for (int i = 0; i < count; i++)
            {
                var obj = m_GameObjectCache[0];
                m_GameObjectCache.RemoveAt(0);
                GameObject.Destroy(obj);
            }
        }
    }
}