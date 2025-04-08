using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Extensions;
using Infrastructure.Services.LogService;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Infrastructure.AssetManagement
{
  public class AssetDownloadService : IAssetDownloadService
  {
    private readonly IAssetDownloadReporter _downloadReporter;
    private readonly ILogService _logService;
    private List<IResourceLocator> _catalogsLocators;
    private long _downloadSize;

    public AssetDownloadService(IAssetDownloadReporter downloadReporter, ILogService logService)
    {
      _downloadReporter = downloadReporter;
      _logService = logService;
    }
    
    public async UniTask InitializeDownloadDataAsync()
    {
      await Addressables.InitializeAsync().ToUniTask();
      await UpdateCatalogsAsync();
      await UpdateDownloadSizeAsync();
    }
    
    public float GetDownloadSizeMb() => 
      SizeToMb(_downloadSize);

    public async UniTask UpdateContentAsync()
    {
      if (_catalogsLocators == null) 
        await UpdateCatalogsAsync();
      
      IList<IResourceLocation> locations = await RefreshResourceLocations(_catalogsLocators);
      if(locations.IsNullOrEmpty())
        return;

      try
      {
        await DownloadContentWithPreciseProgress(locations);
      }
      catch (Exception e)
      {
        _logService.LogError(e);
      }
    }

    private async UniTask DownloadContentWithPreciseProgress(IList<IResourceLocation> locations)
    {
      AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(locations);
      
      while (!downloadHandle.IsDone && downloadHandle.IsValid())
      {
        await UniTask.Delay(100);
        _downloadReporter.Report(downloadHandle.GetDownloadStatus().Percent);
      }
      
      _downloadReporter.Report(1);
      if (downloadHandle.Status == AsyncOperationStatus.Failed) 
        _logService.LogError("Error while downloading catalog dependencies");

      if(downloadHandle.IsValid())
        Addressables.Release(downloadHandle);
      
      _downloadReporter.Reset();
    }

    private async UniTask UpdateCatalogsAsync()
    {
      List<string> catalogsToUpdate = await Addressables.CheckForCatalogUpdates().ToUniTask();
      if (catalogsToUpdate.IsNullOrEmpty())
      {
        _catalogsLocators = Addressables.ResourceLocators.ToList();
        return;
      }

      _catalogsLocators = await Addressables.UpdateCatalogs(catalogsToUpdate).ToUniTask();
    }

    private async UniTask UpdateDownloadSizeAsync()
    {
      IList<IResourceLocation> locations = await RefreshResourceLocations(_catalogsLocators);

      if(locations.IsNullOrEmpty())
        return;

      _downloadSize = await Addressables
        .GetDownloadSizeAsync(locations)
        .ToUniTask();
    }

    private async UniTask<IList<IResourceLocation>> RefreshResourceLocations(IEnumerable<IResourceLocator> locators)
    {
      IEnumerable<object> keysToCheck = locators.SelectMany(x => x.Keys);

      return await Addressables
        .LoadResourceLocationsAsync(keysToCheck, Addressables.MergeMode.Union)
        .ToUniTask();
    }

    private static float SizeToMb(long downloadSize) => downloadSize * 1f / 1048576;
  }
}