using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Levels
{
    public interface ILevelSessionService
    {
        void SetUp(Transform clustersParent, Transform wordsParent);
        UniTask Run();
        void CleanUp();
    }
}