using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Clusters.Factory
{
    public interface IClustersContainerFactory
    {
        UniTask<ClustersClusterContainer> CreateClustersContainer(Transform parent);
    }
}