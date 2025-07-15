using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class AddressableUpdater : MonoBehaviour
{
    public string remoteCatalogUrl;  // CDN catalog 路径

    public Action<float> OnProgressChanged;     // 进度更新回调 0~1
    public Action<float> OnSpeedChanged;        // 下载速度更新 (KB/s)
    public Action<float> OnDownloadSizeFetched; // 总下载大小回调 (MB)
    public Action OnCompleted;                  // 下载完成

    private float lastDownloadedBytes = 0;
    private float updateCheckInterval = 0.5f;

    void Start()
    {
        StartCoroutine(UpdateAddressables());
    }

    IEnumerator UpdateAddressables()
    {
        // 1. 初始化 Addressables
        var initHandle = Addressables.InitializeAsync();
        yield return initHandle;

        // 2. 更新 Catalog
        var catalogHandle = Addressables.UpdateCatalogs();
        yield return catalogHandle;

        List<IResourceLocator> catalogs = catalogHandle.Result;

        // 3. 获取资源下载大小
        var sizeHandle = Addressables.GetDownloadSizeAsync(Addressables.ResourceManager.ResourceProviders);
        yield return sizeHandle;

        long totalDownloadSize = sizeHandle.Result;

        if (totalDownloadSize <= 0)
        {
            Debug.Log("无需更新");
            OnCompleted?.Invoke();
            yield break;
        }

        OnDownloadSizeFetched?.Invoke(totalDownloadSize / (1024f * 1024f)); // MB

        // 4. 开始下载所有资源
        var downloadHandle = Addressables.DownloadDependenciesAsync(Addressables.ResourceManager.ResourceProviders, true);
        StartCoroutine(TrackDownloadProgress(downloadHandle, totalDownloadSize));
        yield return downloadHandle;

        if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("热更完成！");
            OnCompleted?.Invoke();
        }
        else
        {
            Debug.LogError("热更失败！");
        }
    }

    IEnumerator TrackDownloadProgress(AsyncOperationHandle handle, long totalSize)
    {
        float lastTime = Time.realtimeSinceStartup;
        float lastDownloaded = 0;

        while (!handle.IsDone)
        {
            float percent = handle.PercentComplete;
            OnProgressChanged?.Invoke(percent);

            float currentTime = Time.realtimeSinceStartup;
            float elapsed = currentTime - lastTime;

            if (elapsed >= updateCheckInterval)
            {
                float currentDownloaded = percent * totalSize;
                float speed = (currentDownloaded - lastDownloaded) / elapsed / 1024f; // KB/s

                OnSpeedChanged?.Invoke(speed);

                lastTime = currentTime;
                lastDownloaded = currentDownloaded;
            }

            yield return null;
        }

        OnProgressChanged?.Invoke(1f);
        OnSpeedChanged?.Invoke(0);
    }
}
