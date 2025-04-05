using Cysharp.Threading.Tasks;
using Infrastructure.AssetManagement;
using UnityEngine;
using Zenject;

namespace Gameplay.Clusters.Factory
{
    public class WordsContainerFactory : IWordsContainerFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;

        private string path = "";

        public WordsContainerFactory(IAssetProvider assetProvider, IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _instantiator = instantiator;
        }

        public async UniTask<WordsContainer> CreateWordsContainer(Transform parent)
        {
            Cluster prefab = await _assetProvider.Load<Cluster>(path);
            var cluster = _instantiator.InstantiatePrefab(prefab, parent);

            return cluster.GetComponent<WordsContainer>();
        }
    }
}