using System;

namespace Infrastructure.AssetManagement
{
  public interface IAssetDownloadReporter : IProgress<float>
  {
    float Progress { get; }
    event Action ProgressUpdated;
    void Report(float value);
    void Reset();
  }
}