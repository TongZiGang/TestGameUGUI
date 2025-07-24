using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.U2D;

namespace CreatGame.AssetBundle
{
    public class AssetBundleManager : Singleton<AssetBundleManager>
    {
        private class AssetBundleData
        {
            public string assetBundleName;
            public GameObject assetBundle;
        }
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, AssetBundleData> m_AssetBundles;
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, SpriteAtlas> m_SpriteAtlasCache;
        /// <summary>
        /// 是否初始化完成
        /// </summary>
        public bool IsInitializeAsync;
        /// <summary>
        /// 需要初始化Addressble系统
        /// </summary>
        public AssetBundleManager()
        {
            IsInitializeAsync = false;
            m_AssetBundles = new Dictionary<string, AssetBundleData>();
            m_SpriteAtlasCache = new Dictionary<string, SpriteAtlas>();
            
            
            Addressables.InitializeAsync();
        }

        public void Initialize()
        {
            Addressables.InitializeAsync().Completed += OnInitializeCompleted;
        }
        /// <summary>
        /// 初始化信息回调
        /// </summary>
        private void OnInitializeCompleted(AsyncOperationHandle<IResourceLocator> operationHandle)
        {
            if (operationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Addressables initialization succeeded");
#region MyRegion
                // IResourceLocator locator = operationHandle.Result;
                // foreach (object locatorKey in locator.Keys)
                // {
                //     locator.Locate(locatorKey,typeof(UnityEngine.Object),out IList<IResourceLocation> locations);
                //     if (locations == null)
                //     {
                //         continue;
                //     }
                //     
                //     foreach (var location in locations)
                //     {
                //         string key = location.PrimaryKey;
                //         Addressables.LoadAssetAsync<UnityEngine.Object>(key).Completed += (handle) =>
                //         {
                //             if (handle.Status == AsyncOperationStatus.Succeeded)
                //             {
                //                 Debug.Log("Addressables load asset succeeded");
                //                 var asset = handle.Result;
                //                 GameObject.Instantiate(asset);
                //             }
                //         };
                //     }
                // }
#endregion

                IsInitializeAsync = true;
            }
        }
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="callback"></param>
        public void LoadGameObjectAsync(string assetBundleName, Action<GameObject> callback)
        {
            if (m_AssetBundles.TryGetValue(assetBundleName, out var bundle))
            {
                callback.Invoke(GameObject.Instantiate(bundle.assetBundle));
                return;
            }

            Addressables.LoadAssetAsync<GameObject>(assetBundleName).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var assetData = CacheAssetBundles(assetBundleName, handle.Result);
                    Addressables.Release(handle);
                    callback?.Invoke(GameObject.Instantiate(assetData.assetBundle));
                }
                else
                {
                    Debug.Log($"assetBundleName = {assetBundleName}   加载失败    Status = {handle.Status}");
                    callback?.Invoke(null);
                }
            };
        }
        /// <summary>
        /// 同步等待加载资源
        /// </summary>
        /// <param name="assetBundleName"></param>
        public GameObject LoadGameObject(string assetBundleName)
        {
            if (m_AssetBundles.TryGetValue(assetBundleName, out var bundle))
            {
                return GameObject.Instantiate(bundle.assetBundle);
            }

            var handle = Addressables.LoadAssetAsync<GameObject>(assetBundleName);
            handle.WaitForCompletion();
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                bundle = CacheAssetBundles(assetBundleName, handle.Result);
                Addressables.Release(handle);
                
                return GameObject.Instantiate(bundle.assetBundle);
            }

            Debug.LogError("资源加载失败");
            return null;
        }
        /// <summary>
        /// 缓存加载出来的资源
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="bundle"></param>
        /// <returns></returns>
        private AssetBundleData CacheAssetBundles(string bundleName, GameObject bundle = null)
        {
            var data = new AssetBundleData(){assetBundleName = bundleName, assetBundle = bundle};
            m_AssetBundles.Add(bundleName, data);
            return data;
        }
        /// <summary>
        /// 同步加载图集
        /// </summary>
        /// <param name="atlasName"></param>
        /// <returns></returns>
        public SpriteAtlas LoadSpriteAtlas(string atlasName)
        {
            if (m_SpriteAtlasCache.TryGetValue(atlasName, out var atlas))
            {
                return atlas;
            }

            var handle = Addressables.LoadAssetAsync<SpriteAtlas>(atlasName);
            handle.WaitForCompletion();
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                atlas = handle.Result;
                Addressables.Release(handle);
                m_SpriteAtlasCache.Add(atlasName, atlas);
                return atlas;
            }
            
            Debug.LogError($"图集加载失败  atlasName = {atlasName}  Status = {handle.Status}");
            return null;
        }
        /// <summary>
        /// 异步加载图集
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="callback"></param>
        public void LoadSpriteAsync(string atlasName, Action<SpriteAtlas> callback)
        {
            if (m_SpriteAtlasCache.TryGetValue(atlasName, out var atlas))
            {
                callback.Invoke(atlas);
                return;
            }
            Addressables.LoadAssetAsync<SpriteAtlas>(atlasName).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    atlas = handle.Result;
                    Addressables.Release(handle);
                    m_SpriteAtlasCache.Add(atlasName, atlas);
                }
                else
                {
                    Debug.LogError($"图集加载失败  atlasName = {atlasName}  Status = {handle.Status}");
                    callback?.Invoke(null);
                }
            };
        }
        /// <summary>
        /// 释放所有的资源
        /// </summary>
        public void ReleaseAll()
        {
            foreach (var mAssetBundle in m_AssetBundles)
            {
                GameObject.Destroy(mAssetBundle.Value.assetBundle);
            }
            m_AssetBundles.Clear();
        }
    }
}