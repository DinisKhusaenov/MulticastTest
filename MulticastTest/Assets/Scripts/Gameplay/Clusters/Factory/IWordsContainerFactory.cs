using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Clusters.Factory
{
    public interface IWordsContainerFactory
    {
        UniTask<WordsContainer> CreateWordsContainer(Transform parent);
    }
}