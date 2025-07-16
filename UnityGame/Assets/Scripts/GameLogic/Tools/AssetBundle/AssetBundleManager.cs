using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace CreatGame.AssetBundle
{
    public class AssetBundleManager : Singleton<AssetBundleManager>
    {
        /// <summary>
        /// 需要初始化Addressble系统
        /// </summary>
        public AssetBundleManager()
        {
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
            }
        }
    }
}