using Cysharp.Threading.Tasks;
using Infrastructure.AssetManagement;
using UnityEngine;
using Zenject;

namespace Gameplay.Clusters.Factory
{
    public class ClusterFactory : IClusterFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;

        public ClusterFactory(IAssetProvider assetProvider, IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _instantiator = instantiator;
        }

        public async UniTask<Cluster> CreateCluster(Transform parent, string letters)
        {
            Cluster prefab = await _assetProvider.Load<Cluster>(GetPathByClusterLength(letters.Length));
            var cluster = _instantiator.InstantiatePrefab(prefab, parent);
            cluster.GetComponent<Cluster>().Initialize(letters);

            return cluster.GetComponent<Cluster>();
        }

        private string GetPathByClusterLength(int length)
        {
            return "123";
        }
    }
}