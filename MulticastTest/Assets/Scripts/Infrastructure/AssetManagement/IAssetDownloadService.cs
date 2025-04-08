using Cysharp.Threading.Tasks;

namespace Infrastructure.AssetManagement
{
  public interface IAssetDownloadService
  {
    UniTask InitializeDownloadDataAsync();
    float GetDownloadSizeMb();
    UniTask UpdateContentAsync();
  }
}