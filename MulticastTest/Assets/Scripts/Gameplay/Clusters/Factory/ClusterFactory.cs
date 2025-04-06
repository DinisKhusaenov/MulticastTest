using System;
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
            GameObject prefab = await _assetProvider.Load<GameObject>(GetPathByClusterLength(letters.Length));
            var cluster = _instantiator.InstantiatePrefab(prefab, parent);
            cluster.GetComponent<Cluster>().Initialize(letters);

            return cluster.GetComponent<Cluster>();
        }

        private string GetPathByClusterLength(int length)
        {
            switch (length)
            {
                case 2:
                    return AssetPath.ClusterTwoLetters;
                
                case 3:
                    return AssetPath.ClusterThreeLetters;
                
                case 4:
                    return AssetPath.ClusterFourLetters;
                
                default:
                    throw new Exception($"Cluster by length {length} does not exist");
            }
        }
    }
}